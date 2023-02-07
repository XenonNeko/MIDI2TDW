using Melanchall.DryWetMidi.Common;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgramMapGui : MonoBehaviour
{
    [Header("UI")]
    [SerializeField]
    private RectTransform rectTransform;
    [SerializeField]
    private float iconSize;
    [SerializeField]
    private Image iconImage;
    [SerializeField]
    private RectTransform iconRect;

    [Header("Labels")]
    [SerializeField]
    private TextMeshProUGUI programLabel;
    [SerializeField]
    private TextMeshProUGUI soundNameLabel;
    [SerializeField]
    private TextMeshProUGUI soundOriginLabel;

    private SevenBitNumber midiProgram;
    private bool isPercussion;
    public void SetProgram(SevenBitNumber program, bool isPercussion)
    {
        midiProgram = program;
        this.isPercussion = isPercussion;
        programLabel.text = $"{program + 1}: {(isPercussion ? MidiUtil.percussion[program] : MidiUtil.instruments[program])}";
    }
    public SevenBitNumber GetProgram()
    {
        return midiProgram;
    }
    public bool IsPercussion()
    {
        return isPercussion;
    }

    public void SetInsetAndSizeFromParentEdge(RectTransform.Edge edge, float inset, float size)
    {
        rectTransform.SetInsetAndSizeFromParentEdge(edge, inset, size);
    }

    private void SetIcon(Sprite icon)
    {
        iconImage.sprite = icon;
        Rect rect = icon.rect;
        if (rect.width >= rect.height)
        {
            float scale = iconSize / rect.width;
            iconRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, iconSize);
            iconRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, rect.height * scale);
        }
        else
        {
            float scale = iconSize / rect.height;
            iconRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, rect.width * scale);
            iconRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, iconSize);
        }
    }

    [Header("Dependencies")]
    [SerializeField]
    private SoundSelect soundSelect;

    public void ChangeSound()
    {
        soundSelect.ChangeSound(this);
    }

    [SerializeField]
    private Tuner tuner;

    private TdwSound sound;
    private bool useMidiTuning;
    private float pitchOffset;

    public TdwProgramMap GetProgramMap()
    {
        return new() { sound = sound, useMidiTuning = useMidiTuning, pitchOffset = pitchOffset };
    }

    public void SetSound(TdwSound sound)
    {
        this.sound = sound;
        soundNameLabel.text = sound.name;
        soundOriginLabel.text = sound.origin is null ? string.Empty : $"({sound.origin})";
        SetIcon(sound.icon);
        if (sound.hasPitch)
        {
            tuner.SetTuningMode(Tuner.TuningMode.MIDI);
            tuner.SetPitch(-sound.midiPitch);
            pitchOffset = -sound.midiPitch;
            return;
        }
        tuner.SetTuningMode(Tuner.TuningMode.Constant);
        tuner.SetPitch(0f);
        pitchOffset = 0f;
    }

    public void UseMidiTuning(bool value)
    {
        useMidiTuning = value;
    }

    public void SetPitch(float pitch)
    {
        pitchOffset = pitch;
    }
}
