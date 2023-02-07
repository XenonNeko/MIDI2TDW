In this folder, you will find the following files:

"default-sounds.json"
    This file contains the default Thirty Dollar Website sounds to use for each MIDI instrument:
    
    (root)
     ├ programs (object):
     │  │  The 128 MIDI programs (instruments).
     │  │
     │  └ (key): The MIDI program number in hexadecimal prefixed with "0x".
     │     │
     │     ├ name (string): The human-readable name of the MIDI program.
     │     │
     │     └ tdw (string):
     │         The sound to use for this MIDI program, represented by its text form (as seen in Thirty Dollar Website files).
     │         Note that the text forms of several characters are emojis.
     │
     └ percussion (object):
        │  The 47 standardized MIDI percussion notes.
        │
        └ (key) The MIDI note number in decimal.
           │
           ├ name (string): The human-readable name of the percussion sound.
           │
           ├ tdw (string):
           │   The sound to use for this MIDI program, represented by its text form (as seen in Thirty Dollar Website files).
           │   Note that the text forms of several characters are emojis.
           │
           └ pitch (number) [optional]:
               If present, the pitch to be applied to the sound on the Thirty Dollar Website.

"readingsettings.json"
    This file contains the settings for the MIDI file reader.
    For more information about the settings contained within, consult "readingsettings-help.txt".
    MIDI2TDW uses the DryWetMidi library. See the "about/drywetmidi" folder for more information.

"readingsettings-help.txt"
    This file contains documentation for the settings found in "readingsettings.json".
    If you're getting an error when trying to load a MIDI file,
    check to see if the error message matches an error described by one of the policies in this file.
    If so, try changing the value for that policy in "readingsettings.json".
    The most important information in an error message is typically at the top.
    Look for the word "Exception" near the top of the error message.
    
"README.txt"
    See "README.txt" for more information.

"settings.json"
    This file contains the following user-customizable settings:
    
    (root)
     └ doVolumeActions (number) [EXPERIMENTAL]:
         If you set this to any number greater than 0, your exported Thirty Dollar Website songs will be
         generated with volume actions which attempt to replicate the volume information from the source MIDI file.
         This feature is experimental, hence why it is disabled by default.