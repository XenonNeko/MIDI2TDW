public static class MidiUtil
{
    public static string[] instruments = new string[128]
    {
        "Acoustic Grand Piano",
        "Bright Acoustic",
        "Electric Grand",
        "Honky-Tonk",
        "Electric Piano 1",
        "Electric Piano 2",
        "Harpsichord",
        "Clav",
        "Celesta",
        "Glockenspiel",
        "Music Box",
        "Vibraphone",
        "Marimba",
        "Xylophone",
        "Tubular Bells",
        "Dulcimer",
        "Drawbar Organ",
        "Percussive Organ",
        "Rock Organ",
        "Church Organ",
        "Reed Organ",
        "Accordion",
        "Harmonica",
        "Tango Accordion",
        "Acoustic Guitar (nylon)",
        "Acoustic Guitar (steel)",
        "Electric Guitar (jazz)",
        "Electric Guitar (clean)",
        "Electric Guitar (muted)",
        "Overdriven Guitar",
        "Distortion Guitar",
        "Guitar Harmonics",
        "Acoustic Bass",
        "Electric Bass (finger)",
        "Electric Bass (pick)",
        "Fretless Bass",
        "Slap Bass 1",
        "Slap Bass 2",
        "Synth Bass 1",
        "Synth Bass 2",
        "Violin",
        "Viola",
        "Cello",
        "Contrabass",
        "Tremolo Strings",
        "Pizzicato Strings",
        "Orchestral Strings",
        "Timpani",
        "String Ensemble 1",
        "String Ensemble 2",
        "Synth Strings 1",
        "Synth Strings 2",
        "Choir Aahs",
        "Voice Oohs",
        "Synth Voice",
        "Orchestra Hit",
        "Trumpet",
        "Trombone",
        "Tuba",
        "Muted Trumpet",
        "French Horn",
        "Brass Section",
        "Synth Brass 1",
        "Synth Brass 2",
        "Soprano Sax",
        "Alto Sax",
        "Tenor Sax",
        "Baritone Sax",
        "Oboe",
        "English Horn",
        "Bassoon",
        "Clarinet",
        "Piccolo",
        "Flute",
        "Recorder",
        "Pan Flute",
        "Blown Bottle",
        "Skakuhachi",
        "Whistle",
        "Ocarina",
        "LEAD_1",
        "LEAD_2",
        "LEAD_3",
        "LEAD_4",
        "LEAD_5",
        "LEAD_6",
        "LEAD_7",
        "LEAD_8",
        "PAD_1",
        "PAD_2",
        "PAD_3",
        "PAD_4",
        "PAD_5",
        "PAD_6",
        "PAD_7",
        "PAD_8",
        "FX_1",
        "FX_2",
        "FX_3",
        "FX_4",
        "FX_5",
        "FX_6",
        "FX_7",
        "FX_8",
        "Sitar",
        "Banjo",
        "Shamisen",
        "Koto",
        "Kalimba",
        "Bagpipe",
        "Fiddle",
        "Shanai",
        "Tinkle Bell",
        "Agogo",
        "Steel Drums",
        "Woodblock",
        "Taiko Drum",
        "Melodic Tom",
        "Synth Drum",
        "Reverse Cymbal",
        "Guitar Fret Noise",
        "Breath Noise",
        "Seashore",
        "Bird Tweet",
        "Telephone Ring",
        "Helicopter",
        "Applause",
        "Gunshot",
    };

    public static string[] percussion = new string[128]
    {
      //"PERCUSSION"
        "(MIDI 0)",
        "(MIDI 1)",
        "(MIDI 2)",
        "(MIDI 3)",
        "(MIDI 4)",
        "(MIDI 5)",
        "(MIDI 6)",
        "(MIDI 7)",
        "(MIDI 8)",
        "(MIDI 9)",
        "(MIDI 10)",
        "(MIDI 11)",
        "(MIDI 12)",
        "(MIDI 13)",
        "(MIDI 14)",
        "(MIDI 15)",
        "(MIDI 16)",
        "(MIDI 17)",
        "(MIDI 18)",
        "(MIDI 19)",
        "(MIDI 20)",
        "(MIDI 21)",
        "(MIDI 22)",
        "(MIDI 23)",
        "(MIDI 24)",
        "(MIDI 25)",
        "(MIDI 26)",
        "(MIDI 27)",
        "(MIDI 28)",
        "(MIDI 29)",
        "(MIDI 30)",
        "(MIDI 31)",
        "(MIDI 32)",
        "(MIDI 33)",
        "(MIDI 34)",
        "Acoustic Bass Drum",
        "Bass Drum",
        "Side Kick",
        "Acoustic Snare",
        "Hand Clap",
        "Electric Snare",
        "Low Floor Tom",
        "Closed High Hat",
        "High Floor Tom",
        "Pedal High Hat",
        "Low Tom",
        "Open High Hat",
        "Low Mid Tom",
        "High Mid Tom",
        "Crash Cymbal",
        "High Tom",
        "Ride Cymbal",
        "Chinese Cymbal",
        "Ride Bell",
        "Tambourine",
        "Splash Cymbal",
        "Cow Bell",
        "Crash Cymbal 2",
        "Vibrastrap",
        "Ride Cymbal",
        "High Bongo",
        "Low Bongo",
        "Mute High Conga",
        "Open High Conga",
        "Low Conga",
        "High Timbale",
        "Low Timbale",
        "High Agogo",
        "Low Agogo",
        "Cabasa",
        "Maracas",
        "Short Whistle",
        "Long Whistle",
        "Short Guiro",
        "Long Guiro",
        "Claves",
        "High Wood Block",
        "Low Wood Block",
        "Mute Cuica",
        "Open Cuica",
        "Mute Triangle",
        "Open Triangle",
        "(MIDI 82)",
        "(MIDI 83)",
        "(MIDI 84)",
        "(MIDI 85)",
        "(MIDI 86)",
        "(MIDI 87)",
        "(MIDI 88)",
        "(MIDI 89)",
        "(MIDI 90)",
        "(MIDI 91)",
        "(MIDI 92)",
        "(MIDI 93)",
        "(MIDI 94)",
        "(MIDI 95)",
        "(MIDI 96)",
        "(MIDI 97)",
        "(MIDI 98)",
        "(MIDI 99)",
        "(MIDI 100)",
        "(MIDI 101)",
        "(MIDI 102)",
        "(MIDI 103)",
        "(MIDI 104)",
        "(MIDI 105)",
        "(MIDI 106)",
        "(MIDI 107)",
        "(MIDI 108)",
        "(MIDI 109)",
        "(MIDI 110)",
        "(MIDI 111)",
        "(MIDI 112)",
        "(MIDI 113)",
        "(MIDI 114)",
        "(MIDI 115)",
        "(MIDI 116)",
        "(MIDI 117)",
        "(MIDI 118)",
        "(MIDI 119)",
        "(MIDI 120)",
        "(MIDI 121)",
        "(MIDI 122)",
        "(MIDI 123)",
        "(MIDI 124)",
        "(MIDI 125)",
        "(MIDI 126)",
        "(MIDI 127)",
    };

    public static string[][] default_instrument_mappings = new string[128][]
    {
        /*"ACST_GRAND",*/ new string[] { "rh_piano" },
        /*"BRGHT_ACST",*/ new string[] { "rh_piano" },
        /*"ELEC_GRAND",*/ new string[] { "rh_piano" },
        /*"HONKY_TONK",*/ new string[] { "rh_piano" },
        /*"EL_PIANO_1",*/ new string[] { "mc_pling" },
        /*"EL_PIANO_2",*/ new string[] { "mc_pling" },
        /*"HARPSICHOR",*/ new string[] { "mc_banjo" },
        /*"CLAV      ",*/ new string[] { "mc_banjo" },
        /*"CELESTA   ",*/ new string[] { "mp_marimba" },
        /*"GLOCKENSPI",*/ new string[] { "mp_glockenspiel" },
        /*"MUSIC_BOX ",*/ new string[] { "mp_marimba", "mp_glockenspiel" },
        /*"VIBRAPHONE",*/ new string[] { "mp_marimba" },
        /*"MARIMBA   ",*/ new string[] { "mp_marimba", "mc_xylophone" },
        /*"XYLOPHONE ",*/ new string[] { "mc_xylophone", "mp_marimba" },
        /*"TUBL_BELLS",*/ new string[] { "mc_villagebell", "tacobell" },
        /*"DULCIMER  ",*/ new string[] { "mc_banjo" },
        /*"DRWB_ORGAN",*/ new string[] { "mp_organ" },
        /*"PERC_ORGAN",*/ new string[] { "mp_organ" },
        /*"ROCK_ORGAN",*/ new string[] { "mp_organ" },
        /*"CHRC_ORGAN",*/ new string[] { "mp_organ" },
        /*"REED_ORGAN",*/ new string[] { "mp_organ" },
        /*"ACCORDION ",*/ new string[] { "mc_bit", "mp_squarelead" },
        /*"HARMONICA ",*/ new string[] { "mc_bit", "mp_squarelead" },
        /*"TNG_ACCRDN",*/ new string[] { "mc_bit", "mp_squarelead" },
        /*"GUITAR_NYL",*/ new string[] { "mp_guitar" },
        /*"GUITAR_STL",*/ new string[] { "mp_guitar" },
        /*"E_GTR_JAZZ",*/ new string[] { "mp_guitar" },
        /*"E_GTR_CLN ",*/ new string[] { "mp_guitar" },
        /*"E_GTR_MTD ",*/ new string[] { "mp_guitar" },
        /*"OVD_GUITAR",*/ new string[] { "rd_guitar" },
        /*"DST_GUITAR",*/ new string[] { "rd_guitar" },
        /*"GT_HARMNCS",*/ new string[] { "mp_squarelead" },
        /*"ACST_BASS ",*/ new string[] { "mc_bass" },
        /*"E_BASS_FNG",*/ new string[] { "mc_bass" },
        /*"E_BASS_PCK",*/ new string[] { "mc_bass" },
        /*"FRTLS_BASS",*/ new string[] { "mc_bass" },
        /*"SLP_BASS_1",*/ new string[] { "bwomp", "mc_bass" },
        /*"SLP_BASS_2",*/ new string[] { "bwomp", "mc_bass" },
        /*"SYN_BASS_1",*/ new string[] { "bwomp", "mc_bass" },
        /*"SYN_BASS_2",*/ new string[] { "bwomp", "mc_bass" },
        /*"VIOLIN    ",*/ new string[] { },
        /*"VIOLA     ",*/ new string[] { },
        /*"CELLO     ",*/ new string[] { },
        /*"CONTRABASS",*/ new string[] { "mc_bass" },
        /*"TREM_STRGS",*/ new string[] { "mp_strings" },
        /*"PZCT_STRGS",*/ new string[] { "mc_bass" },
        /*"HARP      ",*/ new string[] { "mc_harp" },
        /*"TIMPANI   ",*/ new string[] { "pan" },
        /*"STR_ENSM_1",*/ new string[] { "mp_strings", "error" },
        /*"STR_ENSM_2",*/ new string[] { "mp_strings", "error" },
        /*"SYN_STR_1 ",*/ new string[] { "mp_strings" },
        /*"SYN_STR_2 ",*/ new string[] { "mp_strings" },
        /*"CHOIR_AAHS",*/ new string[] { "rh_chorus" },
        /*"VOICE_OOHS",*/ new string[] { "fnf_oh" },
        /*"SYN_VOICE ",*/ new string[] { "fnf_ah" },
        /*"ORCHST_HIT",*/ new string[] { "mp_strings", "error" },
        /*"TRUMPET   ",*/ new string[] { "trumpet", "mp_trumpet", "hoenn" },
        /*"TROMBONE  ",*/ new string[] { "trumpet", "mp_trumpet", "hoenn" },
        /*"TUBA      ",*/ new string[] { "trumpet", "mp_trumpet", "hoenn" },
        /*"MT_TRUMPET",*/ new string[] { "trumpet", "mp_trumpet", "hoenn" },
        /*"FRNCH_HORN",*/ new string[] { "mp_trumpet" },
        /*"BRASS_SCTN",*/ new string[] { "mp_trumpet" },
        /*"SYNBRASS_1",*/ new string[] { "mp_trumpet" },
        /*"SYNBRASS_2",*/ new string[] { "mp_trumpet" },
        /*"SPRNO_SAX ",*/ new string[] { "hoenn" },
        /*"ALTO_SAX  ",*/ new string[] { "hoenn" },
        /*"TENOR_SAX ",*/ new string[] { "hoenn" },
        /*"BRTN_SAX  ",*/ new string[] { "hoenn" },
        /*"OBOE      ",*/ new string[] { },
        /*"ENGLS_HORN",*/ new string[] { },
        /*"BASSOON   ",*/ new string[] { },
        /*"CLARINET  ",*/ new string[] { "mp_squarelead" },
        /*"PICCOLO   ",*/ new string[] { },
        /*"FLUTE     ",*/ new string[] { },
        /*"RECORDER  ",*/ new string[] { },
        /*"PAN_FLUTE ",*/ new string[] { },
        /*"BLOWN_BTTL",*/ new string[] { },
        /*"SKAKUHACHI",*/ new string[] { },
        /*"WHISTLE   ",*/ new string[] { },
        /*"OCARINA   ",*/ new string[] { "mc_bit", "rd_tonk" },
        /*"LEAD_1    ",*/ new string[] { "mp_squarelead", "mc_bit" },
        /*"LEAD_2    ",*/ new string[] { },
        /*"LEAD_3    ",*/ new string[] { },
        /*"LEAD_4    ",*/ new string[] { },
        /*"LEAD_5    ",*/ new string[] { },
        /*"LEAD_6    ",*/ new string[] { },
        /*"LEAD_7    ",*/ new string[] { },
        /*"LEAD_8    ",*/ new string[] { },
        /*"PAD_1     ",*/ new string[] { },
        /*"PAD_2     ",*/ new string[] { },
        /*"PAD_3     ",*/ new string[] { },
        /*"PAD_4     ",*/ new string[] { },
        /*"PAD_5     ",*/ new string[] { },
        /*"PAD_6     ",*/ new string[] { },
        /*"PAD_7     ",*/ new string[] { },
        /*"PAD_8     ",*/ new string[] { },
        /*"FX_1      ",*/ new string[] { },
        /*"FX_2      ",*/ new string[] { },
        /*"FX_3      ",*/ new string[] { },
        /*"FX_4      ",*/ new string[] { },
        /*"FX_5      ",*/ new string[] { },
        /*"FX_6      ",*/ new string[] { },
        /*"FX_7      ",*/ new string[] { },
        /*"FX_8      ",*/ new string[] { },
        /*"SITAR     ",*/ new string[] { "mc_banjo" },
        /*"BANJO     ",*/ new string[] { "mc_banjo" },
        /*"SHAMISEN  ",*/ new string[] { "mc_banjo" },
        /*"KOTO      ",*/ new string[] { "mc_banjo" },
        /*"KALIMBA   ",*/ new string[] { },
        /*"BAGPIPE   ",*/ new string[] { },
        /*"FIDDLE    ",*/ new string[] { },
        /*"SHANAI    ",*/ new string[] { },
        /*"TINKL_BELL",*/ new string[] { "mc_chime" },
        /*"AGOGO     ",*/ new string[] { "pan" },
        /*"STL_DRUMS ",*/ new string[] { "smw_steeldrum" },
        /*"WOODBLOCK ",*/ new string[] { "mc_hat" },
        /*"TAIKO_DRUM",*/ new string[] { "rd_tom" },
        /*"MLDC_TOM  ",*/ new string[] { "rd_tom" },
        /*"SYNTH_DRUM",*/ new string[] { "rd_tom" },
        /*"RVS_CYMBAL",*/ new string[] { "adofai_cymbal" },
        /*"GT_FRET_NZ",*/ new string[] { "mp_bit" },
        /*"BREATH_NZ ",*/ new string[] { },
        /*"SEASHORE  ",*/ new string[] { },
        /*"BIRD_TWEET",*/ new string[] { },
        /*"TELEPHONE ",*/ new string[] { "skyline" },
        /*"HELICOPTER",*/ new string[] { },
        /*"APPLAUSE  ",*/ new string[] { },
        /*"GUNSHOT   ",*/ new string[] { "gun" },
    };

    public static string[][] default_percussion_mappings = new string[128][]
    {
        /*"  (MIDI 0)",*/ new string[] { },
        /*"  (MIDI 1)",*/ new string[] { },
        /*"  (MIDI 2)",*/ new string[] { },
        /*"  (MIDI 3)",*/ new string[] { },
        /*"  (MIDI 4)",*/ new string[] { },
        /*"  (MIDI 5)",*/ new string[] { },
        /*"  (MIDI 6)",*/ new string[] { },
        /*"  (MIDI 7)",*/ new string[] { },
        /*"  (MIDI 8)",*/ new string[] { },
        /*"  (MIDI 9)",*/ new string[] { },
        /*" (MIDI 10)",*/ new string[] { },
        /*" (MIDI 11)",*/ new string[] { },
        /*" (MIDI 12)",*/ new string[] { },
        /*" (MIDI 13)",*/ new string[] { },
        /*" (MIDI 14)",*/ new string[] { },
        /*" (MIDI 15)",*/ new string[] { },
        /*" (MIDI 16)",*/ new string[] { },
        /*" (MIDI 17)",*/ new string[] { },
        /*" (MIDI 18)",*/ new string[] { },
        /*" (MIDI 19)",*/ new string[] { },
        /*" (MIDI 20)",*/ new string[] { },
        /*" (MIDI 21)",*/ new string[] { },
        /*" (MIDI 22)",*/ new string[] { },
        /*" (MIDI 23)",*/ new string[] { },
        /*" (MIDI 24)",*/ new string[] { },
        /*" (MIDI 25)",*/ new string[] { },
        /*" (MIDI 26)",*/ new string[] { },
        /*" (MIDI 27)",*/ new string[] { },
        /*" (MIDI 28)",*/ new string[] { },
        /*" (MIDI 29)",*/ new string[] { },
        /*" (MIDI 30)",*/ new string[] { },
        /*" (MIDI 31)",*/ new string[] { },
        /*" (MIDI 32)",*/ new string[] { },
        /*" (MIDI 33)",*/ new string[] { },
        /*" (MIDI 34)",*/ new string[] { },
        /*"ACS_BASS_D",*/ new string[] { "rd_kick", "adofai_kick", "rh_karate_hit" },
        /*"BASS_DRUM ",*/ new string[] { "rd_kick", "adofai_kick", "rh_karate_hit" },
        /*"SIDE_KICK ",*/ new string[] { "rd_stick", "mc_hat" },
        /*"ACST_SNARE",*/ new string[] { "rd_snare", "rh_karate_hit", "mc_snare" },
        /*"HAND_CLAP ",*/ new string[] { "clap", "rd_snare", "rh_karate_hit" },
        /*"ELEC_SNARE",*/ new string[] { "rd_kick", "adofai_kick", "rh_karate_hit" },
        /*"LOW_FL_TOM",*/ new string[] { "rd_tom" },
        /*"CLOSED_HH ",*/ new string[] { "mc_click", "rd_shaker" },
        /*"HI_FL_TOM ",*/ new string[] { "rd_tom" },
        /*"PEDAL_HH  ",*/ new string[] { "rd_shaker", "mc_hat" },
        /*"LOW_TOM   ",*/ new string[] { "rd_tom" },
        /*"OPEN_HH   ",*/ new string[] { "adofai_cymbal", "rd_ride" },
        /*"LO_MID_TOM",*/ new string[] { "rd_tom" },
        /*"HI_MID_TOM",*/ new string[] { "rd_tom" },
        /*"CSH_CYMBAL",*/ new string[] { "adofai_cymbal", "rd_ride" },
        /*"HIGH_TOM  ",*/ new string[] { "rd_tom" },
        /*"RD_CYMBAL ",*/ new string[] { "rd_ride" },
        /*"CHN_CYMBAL",*/ new string[] { "adofai_cymbal" },
        /*"RIDE_BELL ",*/ new string[] { "rd_ride", "rd_ride" },
        /*"TAMBOURINE",*/ new string[] { "rd_shaker" },
        /*"SPL_CYMBAL",*/ new string[] { "adofai_cymbal" },
        /*"COW_BELL  ",*/ new string[] { "rh_cowbell" },
        /*"C_CYMBAL_2",*/ new string[] { "adofai_cymbal", "rd_ride" },
        /*"VIBRASTRAP",*/ new string[] { },
        /*"R_CYMBAL_2",*/ new string[] { "rd_ride", "adofai_cymbal" },
        /*"HIGH_BONGO",*/ new string[] { "rd_tom" },
        /*"LOW_BONGO ",*/ new string[] { "rd_tom" },
        /*"MT_H_CONGA",*/ new string[] { },
        /*"OP_H_CONGA",*/ new string[] { },
        /*"LOW_CONGA ",*/ new string[] { },
        /*"HI_TIMBALE",*/ new string[] { },
        /*"LO_TIMBALE",*/ new string[] { },
        /*"HIGH_AGOGO",*/ new string[] { "pan" },
        /*"LOW_AGOGO ",*/ new string[] { "pan" },
        /*"CABASA    ",*/ new string[] { },
        /*"MARACAS   ",*/ new string[] { },
        /*"SH_WHISTLE",*/ new string[] { },
        /*"LN_WHISTLE",*/ new string[] { },
        /*"SHRT_GUIRO",*/ new string[] { "pan" },
        /*"LONG_GUIRO",*/ new string[] { "pan" },
        /*"CLAVES    ",*/ new string[] { },
        /*"H_WD_BLOCK",*/ new string[] { },
        /*"L_WD_BLOCK",*/ new string[] { },
        /*"MUTE_CUICA",*/ new string[] { },
        /*"OPEN_CUICA",*/ new string[] { },
        /*"M_TRIANGLE",*/ new string[] { },
        /*"O_TRIANGLE",*/ new string[] { "mp_glockenspiel" },
        /*" (MIDI 82)",*/ new string[] { },
        /*" (MIDI 83)",*/ new string[] { },
        /*" (MIDI 84)",*/ new string[] { },
        /*" (MIDI 85)",*/ new string[] { },
        /*" (MIDI 86)",*/ new string[] { },
        /*" (MIDI 87)",*/ new string[] { },
        /*" (MIDI 88)",*/ new string[] { },
        /*" (MIDI 89)",*/ new string[] { },
        /*" (MIDI 90)",*/ new string[] { },
        /*" (MIDI 91)",*/ new string[] { },
        /*" (MIDI 92)",*/ new string[] { },
        /*" (MIDI 93)",*/ new string[] { },
        /*" (MIDI 94)",*/ new string[] { },
        /*" (MIDI 95)",*/ new string[] { },
        /*" (MIDI 96)",*/ new string[] { },
        /*" (MIDI 97)",*/ new string[] { },
        /*" (MIDI 98)",*/ new string[] { },
        /*" (MIDI 99)",*/ new string[] { },
        /*"(MIDI 100)",*/ new string[] { },
        /*"(MIDI 101)",*/ new string[] { },
        /*"(MIDI 102)",*/ new string[] { },
        /*"(MIDI 103)",*/ new string[] { },
        /*"(MIDI 104)",*/ new string[] { },
        /*"(MIDI 105)",*/ new string[] { },
        /*"(MIDI 106)",*/ new string[] { },
        /*"(MIDI 107)",*/ new string[] { },
        /*"(MIDI 108)",*/ new string[] { },
        /*"(MIDI 109)",*/ new string[] { },
        /*"(MIDI 110)",*/ new string[] { },
        /*"(MIDI 111)",*/ new string[] { },
        /*"(MIDI 112)",*/ new string[] { },
        /*"(MIDI 113)",*/ new string[] { },
        /*"(MIDI 114)",*/ new string[] { },
        /*"(MIDI 115)",*/ new string[] { },
        /*"(MIDI 116)",*/ new string[] { },
        /*"(MIDI 117)",*/ new string[] { },
        /*"(MIDI 118)",*/ new string[] { },
        /*"(MIDI 119)",*/ new string[] { },
        /*"(MIDI 120)",*/ new string[] { },
        /*"(MIDI 121)",*/ new string[] { },
        /*"(MIDI 122)",*/ new string[] { },
        /*"(MIDI 123)",*/ new string[] { },
        /*"(MIDI 124)",*/ new string[] { },
        /*"(MIDI 125)",*/ new string[] { },
        /*"(MIDI 126)",*/ new string[] { },
        /*"(MIDI 127)",*/ new string[] { },
    };
}