using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public static class FileTypeAnalyzer
{
    private static bool StartsWith(this byte[] bytes, string match)
    {
        //                                            🗿
        Encoding encoding = Encoding.GetEncoding("iso-8859-1");
        byte[] ascii = encoding.GetBytes(match);
        if (bytes.Length < ascii.Length)
        {
            return false;
        }
        for (int i = 0; i < ascii.Length; i++)
        {
            // Don't care
            if (ascii[i] == (byte)'?')
            {
                continue;
            }
            if (bytes[i] != ascii[i])
            {
                return false;
            }
        }
        return true;
    }
    private static bool StartsWithAny(this byte[] bytes, params string[] matches)
    {
        foreach (string match in matches)
        {
            if (StartsWith(bytes, match))
            {
                return true;
            }
        }
        return false;
    }

    private static readonly string[] mp3Signatures = { "ÿû", "ÿó", "ÿþ", "ID3" };
    private const string wavSignature = "RIFF????WAVE";
    private const string oggSignature = "OggS";
    private const string flacSignature = "fLaC";

    public static bool IsMidiFile(string path, out string message)
    {
        byte[] bytes;
        try
        {
            bytes = File.ReadAllBytes(path);
        }
        catch (Exception e)
        {
            message = $"An exception occurred while trying to read the file:\r\n{e.Message}\r\n{e.StackTrace}";
            return false;
        }

        if (bytes.StartsWith("MThd"))
        {
            message = null;
            return true;
        }

        string guessedFileType = null;

        if (bytes.StartsWithAny(mp3Signatures))
        {
            Debug.Log("File signature matched MP3 signature.");
            guessedFileType = "MP3 (audio)";
        }
        else if (bytes.StartsWith(wavSignature))
        {
            Debug.Log("File signature matched WAV signature.");
            guessedFileType = "WAV (audio)";
        }
        else if (bytes.StartsWith(oggSignature))
        {
            Debug.Log("File signature matched OGG signature.");
            guessedFileType = "OGG (audio)";
        }
        else if (bytes.StartsWith(flacSignature))
        {
            Debug.Log("File signature matched FLAC signature.");
            guessedFileType = "FLAC (audio)";
        }

        if (!string.IsNullOrEmpty(guessedFileType))
        {
            message = $"The specified file is not a MIDI file.\r\nBased on the contents of the file, it is most likely a {guessedFileType} file.";
        }
        else
        {

            message = "The specified file is not a MIDI file.\r\n";
        }
        return false;
    }
}
