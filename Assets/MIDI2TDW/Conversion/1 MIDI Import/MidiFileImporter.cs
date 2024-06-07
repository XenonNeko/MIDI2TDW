using Melanchall.DryWetMidi.Common;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

public static class MidiFileImporter
{
    public static string debugPath;
    public static bool debugMidiImport = true;

    private const int CH_PERCUSSION = 9;

    private static void PrintControlChangeEvent(ControlChangeEvent controlChangeEvent, StringBuilder stringBuilder)
    {
        switch (controlChangeEvent.ControlNumber)
        {
            case 7:
                stringBuilder.AppendLine(
                    "[ControlChangeEvent (ChannelVolume)] " +
                    $"Channel: {controlChangeEvent.Channel}, " +
                    $"ControlValue: {controlChangeEvent.ControlValue}");
                break;
            default:
                stringBuilder.AppendLine(
                    "[ControlChangeEvent] " +
                    $"Channel: {controlChangeEvent.Channel}, " +
                    $"ControlValue: {controlChangeEvent.ControlValue}");
                break;
        }
    }

    private static void PrintTimedEvent(TimedEvent timedEvent, StringBuilder stringBuilder)
    {
        switch (timedEvent.Event)
        {
            case SequenceTrackNameEvent sequenceTrackName:
                stringBuilder.AppendLine(
                    "[SequenceTrackNameEvent] "
                    + $"Text: \"{sequenceTrackName.Text}\"");
                break;
            case ProgramChangeEvent programChange:
                stringBuilder.AppendLine(
                    "[ProgramChangeEvent] "
                    + $"Channel: {programChange.Channel}, "
                    + $"ProgramNumber: {programChange.ProgramNumber}");
                break;
            case ControlChangeEvent controlChange:
                PrintControlChangeEvent(controlChange, stringBuilder);
                break;
        }
    }

    private static void Print(ITimedObject timedObject, StringBuilder stringBuilder, TempoMap tempoMap = null)
    {
        if (!debugMidiImport)
        {
            return;
        }
        switch (timedObject)
        {
            case Note note:
                MetricTimeSpan timeMetric = note.TimeAs<MetricTimeSpan>(tempoMap);
                stringBuilder.AppendLine(
                    "[Note] " +
                    $"Channel: {note.Channel}, " +
                    $"NoteNumber: {note.NoteNumber}, " +
                    $"Velocity: {note.Velocity}, " +
                    $"μs: {timeMetric.TotalMicroseconds}");
                break;
            case TimedEvent timedEvent:
                PrintTimedEvent(timedEvent, stringBuilder);
                break;
        }
    }

    private static List<MidiTrack> SingleTrackFileToTracks(MidiFile file, string debugName)
    {
        StringBuilder debugLog = new();

        TempoMap tempoMap = file.GetTempoMap();
        ICollection<TimedEvent> timedEvents = file.GetTimedEvents();

        MidiTrack[] midiTracks = new MidiTrack[16];
        List<MidiSound>[] sounds = new List<MidiSound>[16];
        List<SevenBitNumber>[] programs = new List<SevenBitNumber>[16];

        SevenBitNumber[] programNumber = new SevenBitNumber[16];
        SevenBitNumber[] ccChannelVolume = new SevenBitNumber[16];

        for (int ch = 0; ch < 16; ch++)
        {
            midiTracks[ch] = new() { isPercussion = ch == CH_PERCUSSION };
            sounds[ch] = new();
            programs[ch] = new();
            ccChannelVolume[ch] = SevenBitNumber.MaxValue;
        }

        void AddNote(int channel, SevenBitNumber p_noteNumber)
        {
            // Notes are irrelevant in melody tracks
            if (channel != CH_PERCUSSION)
            {
                return;
            }
            // Don't add if it already exists
            if (programs[channel].Contains(p_noteNumber))
            {
                return;
            }
            // Only add note number to programs list if
            // this track is a percussion track and it is a new note
            programs[channel].Add(p_noteNumber);
        }

        void AddProgram(int channel, SevenBitNumber p_programNumber)
        {
            // Programs are irrelevant in percussion tracks
            if (channel == CH_PERCUSSION)
            {
                return;
            }
            // Don't add if it already exists
            if (programs[channel].Contains(p_programNumber))
            {
                return;
            }
            // Only add program number to programs list if
            // this track is a melody track and it is a new program
            programs[channel].Add(p_programNumber);
            programNumber[channel] = p_programNumber;
        }

        int channel;
        foreach (TimedEvent timedEvent in timedEvents)
        {
            Print(timedEvent, debugLog, tempoMap);

            MetricTimeSpan timeMetric = timedEvent.TimeAs<MetricTimeSpan>(tempoMap);
            switch (timedEvent.Event)
            {
                case NoteOnEvent noteOn:
                    channel = noteOn.Channel;

                    SevenBitNumber noteNumber = noteOn.NoteNumber;
                    AddNote(channel, noteNumber);
                    MidiSound sound = new()
                    {
                        programNumber = programNumber[channel],
                        noteNumber = noteNumber,
                        velocity = noteOn.Velocity,
                        channelVolume = ccChannelVolume[channel],
                        timeMicroseconds = timeMetric.TotalMicroseconds
                    };
                    sounds[channel].Add(sound);
                    break;
                case ProgramChangeEvent programChange:
                    channel = programChange.Channel;

                    AddProgram(channel, programChange.ProgramNumber);
                    break;
                case ControlChangeEvent controlChange:
                    channel = controlChange.Channel;

                    switch (controlChange.ControlNumber)
                    {
                        case 7: // ChannelVolume
                            ccChannelVolume[channel] = controlChange.ControlValue;
                            break;
                    }
                    break;
            }
        }

        List<MidiTrack> list = new();
        for (int ch = 0; ch < 16; ch++)
        {
            MidiTrack midiTrack = midiTracks[ch];

            midiTrack.name = $"MIDI Channel {ch + 1}";

            midiTrack.sounds = sounds[ch].ToArray();

            if (programs[ch].Count == 0)
            {
                programs[ch].Add(SevenBitNumber.Values[0]);
            }
            midiTrack.programs = programs[ch].ToArray();

            DebugLog("# END OF MIDI DATA", debugLog);
            DebugLog("", debugLog);
            DebugLog($"# channel: {ch + 1}, ", debugLog);
            DebugLog($"# number of sounds: {midiTrack.sounds.Length}", debugLog);
            DebugLog($"# number of programs: {midiTrack.programs.Length}", debugLog);
            DebugLog($"# isPercussion: {midiTrack.isPercussion}", debugLog);

            if (midiTrack.sounds.Length == 0)
            {
                continue;
            }

            list.Add(midiTrack);
        }

        WriteDebugLog(debugLog, $"{debugName}.fmt0");

        return list;
    }

    private static MidiTrack ChunkToTrack(MidiFile chunk, TempoMap tempoMap, string debugName)
    {
        StringBuilder debugLog = new();

        ICollection<ITimedObject> timedObjects = chunk.GetObjects(ObjectType.Note | ObjectType.TimedEvent);

        MidiTrack midiTrack = new();
        List<MidiSound> sounds = new();

        bool isPercussion;

        List<SevenBitNumber> programs = new();

        SevenBitNumber? programNumber = null;
        SevenBitNumber ccChannelVolume = SevenBitNumber.MaxValue;

        void SetChannel(FourBitNumber channel)
        {
            isPercussion = channel == CH_PERCUSSION;
            midiTrack.isPercussion = isPercussion;
        }

        void AddNote(SevenBitNumber p_noteNumber)
        {
            // Notes are irrelevant in melody tracks
            if (!isPercussion)
            {
                return;
            }
            // Don't add if it already exists
            if (programs.Contains(p_noteNumber))
            {
                return;
            }
            // Only add note number to programs list if
            // this track is a percussion track and it is a new note
            programs.Add(p_noteNumber);
        }

        void AddProgram(SevenBitNumber p_programNumber)
        {
            // Programs are irrelevant in percussion tracks
            if (isPercussion)
            {
                return;
            }
            // Don't add if it already exists
            if (programs.Contains(p_programNumber))
            {
                return;
            }
            // Only add program number to programs list if
            // this track is a melody track and it is a new program
            programs.Add(p_programNumber);
            programNumber = p_programNumber;
        }

        bool hasUnknownPrograms = false;

        foreach (ITimedObject timedObject in timedObjects)
        {
            Print(timedObject, debugLog, tempoMap);

            switch (timedObject)
            {
                case Note note:
                    SetChannel(note.Channel);
                    SevenBitNumber noteNumber = note.NoteNumber;
                    AddNote(noteNumber);
                    MetricTimeSpan timeMetric = note.TimeAs<MetricTimeSpan>(tempoMap);
                    MetricTimeSpan endTimeMetric = note.EndTimeAs<MetricTimeSpan>(tempoMap);
                    long durationMicroseconds = endTimeMetric.TotalMicroseconds - timeMetric.TotalMicroseconds;

                    if (programNumber is null)
                    {
                        hasUnknownPrograms = !isPercussion;
                        programNumber = (SevenBitNumber?)0;
                    }

                    MidiSound sound = new()
                    {
                        programNumber = programNumber.Value,
                        noteNumber = noteNumber,
                        velocity = note.Velocity,
                        channelVolume = ccChannelVolume,
                        timeMicroseconds = timeMetric.TotalMicroseconds,
                        durationMicroseconds = durationMicroseconds
                    };
                    sounds.Add(sound);
                    break;
                case TimedEvent timedEvent:
                    switch (timedEvent.Event)
                    {
                        case SequenceTrackNameEvent sequenceTrackName:
                            midiTrack.name = sequenceTrackName.Text;
                            break;
                        case ProgramChangeEvent programChange:
                            SetChannel(programChange.Channel);
                            AddProgram(programChange.ProgramNumber);
                            break;
                        case ControlChangeEvent controlChange:
                            SetChannel(controlChange.Channel);
                            switch (controlChange.ControlNumber)
                            {
                                case 7:
                                    ccChannelVolume = controlChange.ControlValue;
                                    break;
                            }
                            break;
                    }
                    break;
            }
        }
        midiTrack.sounds = sounds.ToArray();

        if (programs.Count == 0)
        {
            programs.Add(SevenBitNumber.Values[0]);
        }
        midiTrack.programs = programs.ToArray();

        DebugLog("# END OF MIDI DATA", debugLog);
        DebugLog("", debugLog);
        DebugLog($"# track name: \"{midiTrack.name}\"", debugLog);
        DebugLog($"# number of sounds: {midiTrack.sounds.Length}", debugLog);
        DebugLog($"# number of programs: {midiTrack.programs.Length}", debugLog);
        DebugLog($"# isPercussion: {midiTrack.isPercussion}", debugLog);
        DebugLog($"# hasUnknownPrograms: {hasUnknownPrograms}", debugLog);

        WriteDebugLog(debugLog, $"{debugName}.midi");

        if (midiTrack.sounds.Length == 0)
        {
            return null;
        }

        return midiTrack;
    }

    private static string SecondsToString(double totalSeconds)
    {
        int minutes = (int)Math.Floor(totalSeconds / 60.0);
        int seconds = (int)Math.Floor(totalSeconds) % 60;
        double secondsFraction = totalSeconds % 1.0;
        int milliseconds = (int)Math.Floor(secondsFraction * 1000.0);
        return $"{minutes:D2}:{seconds:D2}:{milliseconds:D3}";
    }

    private static void PrintFile(MidiFile file)
    {
        TempoMap tempoMap = file.GetTempoMap();
        ICollection<TimedEvent> timedEvents = file.GetTimedEvents();

        StringBuilder builder = new();

        foreach (TimedEvent timedEvent in timedEvents)
        {
            MetricTimeSpan timeMetric = timedEvent.TimeAs<MetricTimeSpan>(tempoMap);
            switch (timedEvent.Event)
            {
                case NoteOnEvent noteOn:
                    builder.AppendLine($"[{SecondsToString(timeMetric.TotalSeconds)}] NoteOn----------- ch:{noteOn.Channel + 1:D2} 0x{noteOn.NoteNumber:x2} 0x{noteOn.Velocity:x2}");
                    break;
                case SequenceTrackNameEvent sequenceTrackName:
                    builder.AppendLine($"[{SecondsToString(timeMetric.TotalSeconds)}] SequenceTrackName ch:?? \"{sequenceTrackName.Text}\"");
                    break;
                case ProgramChangeEvent programChange:
                    builder.AppendLine($"[{SecondsToString(timeMetric.TotalSeconds)}] ProgramChange---- ch:{programChange.Channel + 1:D2} 0x{programChange.ProgramNumber:x2}");
                    break;
                case ControlChangeEvent controlChange:
                    builder.AppendLine($"[{SecondsToString(timeMetric.TotalSeconds)}] ControlChange---- ch:{controlChange.Channel + 1:D2} 0x{controlChange.ControlNumber:x2} 0x{controlChange.ControlValue:x2}");
                    break;
            }
        }

        WriteDebugLog(builder, "drywetmidi-output");
    }

    private static void PrintChunks(MidiFile[] chunks, TempoMap tempoMap)
    {
        for (int i = 0; i < chunks.Length; i++)
        {
            MidiFile chunk = chunks[i];
            ICollection<ITimedObject> timedObjects = chunk.GetObjects(ObjectType.Note | ObjectType.TimedEvent);

            StringBuilder builder = new();

            double lastTime = 0.0;
            int nextNoteNumber = 0;

            foreach (ITimedObject timedObject in timedObjects)
            {
                switch (timedObject)
                {
                    case Note note:
                        SevenBitNumber noteNumber = note.NoteNumber;
                        MetricTimeSpan timeMetric = note.TimeAs<MetricTimeSpan>(tempoMap);
                        double time = timeMetric.TotalSeconds;
                        builder.AppendLine($"[{time - lastTime}] {nextNoteNumber:D3}");
                        nextNoteNumber = noteNumber;
                        lastTime = time;
                        break;
                }
            }

            WriteDebugLog(builder, $"drywetmidi-notes-track_{i}");
        }
    }

    public static TimeSpan diag_elapsed_midiread;
    public static TimeSpan diag_elapsed_midisplit;
    public static TimeSpan diag_elapsed_midiprocess;

    public static bool HasException { get; private set; }
    public static Exception LastException { get; private set; }

    private static void DebugLog(string message, StringBuilder log)
    {
        if (!debugMidiImport)
        {
            return;
        }
        log.AppendLine(message);
    }
    private static void WriteDebugLog(StringBuilder log, string name)
    {
        if (!debugMidiImport)
        {
            return;
        }
        Directory.CreateDirectory(debugPath);
        string path = Path.Combine(debugPath, $"{name}.log");
        File.WriteAllText(path, log.ToString());
    }

    public static MidiTrack[] TryImportMidiFile(string path, ReadingSettings readingSettings)
    {
        StringBuilder debugLog = new();

        string filename = Path.GetFileNameWithoutExtension(path);
        DebugLog($"Importing MIDI file \"{filename}\"", debugLog);

        diag_elapsed_midiread = TimeSpan.Zero;
        diag_elapsed_midisplit = TimeSpan.Zero;
        diag_elapsed_midiprocess = TimeSpan.Zero;

        MidiFile midiFile;

        Stopwatch sw = new();

        #region Load
        sw.Start();

        HasException = false;
        try
        {
            midiFile = MidiFile.Read(
                path,
                readingSettings
            );
        }
        catch (Exception e)
        {
            sw.Stop();
            diag_elapsed_midiread = sw.Elapsed;

            HasException = true;
            LastException = e;

            return null;
        }

        sw.Stop();
        diag_elapsed_midiread = sw.Elapsed;

        //PrintFile(midiFile);
        #endregion Load

        bool isFormat0 = false;
        TempoMap tempoMap = midiFile.GetTempoMap();

        #region Split
        sw.Restart();

        MidiFile[] chunks;

        SplitFileByChannelSettings splitFileByChannelSettings = new();

        SplitFileByChunksSettings splitFileByChunksSettings = new();
        splitFileByChunksSettings.Filter = chunk => chunk.ChunkId == "MTrk";

        IEnumerable<MidiFile> midiTracks;
        switch (midiFile.OriginalFormat)
        {
            case MidiFileFormat.SingleTrack:
                isFormat0 = true;
                chunks = null;

                DebugLog("File is format 0, will use custom splitter in 'midiprocess' phase", debugLog);
                //PrintChunks(chunks, tempoMap);
                break;
            case MidiFileFormat.MultiTrack:
                midiTracks = midiFile.SplitByChunks(splitFileByChunksSettings);

                // The first track is skipped for format 1, since in that format,
                // the first track only contains meta events.
                //chunks = midiTracks.Skip(1).ToArray();

                // HOWEVER, user testing has found files which violate this rule.
                // Import it anyway. If it has no notes or programs, it will be discarded anyway.
                chunks = midiTracks.ToArray();

                DebugLog("File is format 1", debugLog);
                break;
            case MidiFileFormat.MultiSequence:
                midiTracks = midiFile.SplitByChunks(splitFileByChunksSettings);

                chunks = midiTracks.ToArray();

                DebugLog("File is format 2", debugLog);
                break;
            default:
                DebugLog($"[ERROR] Unrecognized MIDI format '{midiFile.OriginalFormat}'", debugLog);
                WriteDebugLog(debugLog, filename);
                return null;
        }

        sw.Stop();
        diag_elapsed_midisplit = sw.Elapsed;
        #endregion

        #region Process
        sw.Restart();

        List<MidiTrack> tracks = new();
        if (isFormat0)
        {
            tracks = SingleTrackFileToTracks(midiFile, $"{filename}.fmt0");
        }
        else
        {
            for (int i = 0; i < chunks.Length; i++)
            {
                MidiTrack track = ChunkToTrack(chunks[i], tempoMap, $"{filename}.chunk_{i}");
                if (track is null)
                {
                    continue;
                }
                tracks.Add(track);
            }
        }

        sw.Stop();
        diag_elapsed_midiprocess = sw.Elapsed;
        #endregion

        WriteDebugLog(debugLog, filename);
        return tracks.ToArray();
    }
}
