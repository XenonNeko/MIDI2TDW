using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tuner : MonoBehaviour
{
    [SerializeField]
    private ProgramMapGui programMap;

    [SerializeField]
    private GameObject constBtn;
    [SerializeField]
    private GameObject midiBtn;

    [SerializeField]
    private GameObject arithBtns;

    [SerializeField]
    private TMP_InputField pitchInput;

    public enum TuningMode
    {
        Constant,
        MIDI
    }

    public void SetTuningMode(int mode)
    {
        SetTuningMode((TuningMode)mode);
    }

    public void SetTuningMode(TuningMode mode)
    {
        programMap.UseMidiTuning(mode == TuningMode.MIDI);
        switch (mode)
        {
            case TuningMode.Constant:
                constBtn.SetActive(true);
                midiBtn.SetActive(false);
                arithBtns.SetActive(false);
                break;
            case TuningMode.MIDI:
                constBtn.SetActive(false);
                midiBtn.SetActive(true);
                arithBtns.SetActive(true);
                break;
        }
    }

    public void OnPitchChanged(string text)
    {
        if (float.TryParse(text, out float value))
        {
            programMap.SetPitch(value);
            return;
        }
        programMap.SetPitch(0f);
    }

    public void SetPitch(float tuning)
    {
        string stringified = tuning.ToString();
        pitchInput.SetTextWithoutNotify(stringified.Substring(0, Math.Min(7, stringified.Length)));
    }
}
