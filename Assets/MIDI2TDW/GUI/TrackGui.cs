using Melanchall.DryWetMidi.Common;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// A GUI for assigning Thirty Dollar Website sounds to all of the MIDI Programs in a MIDI Track.
/// </summary>
/// <remarks>
/// Note Numbers take the place of Programs in percussion tracks.
/// </remarks>
public class TrackGui : MonoBehaviour, IStackedRect
{
    [Header("UI")]
    [SerializeField]
    private ProgramMapGui programTemplate;
    [SerializeField]
    private float templateHeight;

    [SerializeField]
    private TextMeshProUGUI trackNameLabel;
    [SerializeField]
    private RectTransform rectTransform;
    [SerializeField]
    private RectTransform content;
    [SerializeField]
    private Transform arrow;

    [SerializeField]
    private Color audibleColor;
    [SerializeField]
    private Color inaudibleColor;

    [Header("Animation")]
    [SerializeField]
    private float contentHeight;

    [SerializeField]
    private float animationDuration;

    private bool isExpanded = false;
    private bool isBusy = false;
    private float animateTime;

    private IEnumerator Expand()
    {
        isBusy = true;
        animateTime = Time.time;

        while (!isExpanded)
        {
            float elapsed = Time.time - animateTime;
            float t = elapsed / animationDuration;
            arrow.localEulerAngles = new Vector3(0f, 0f, Mathf.Lerp(0f, -90f, t));
            float height = Mathf.Lerp(0f, contentHeight, t);
            content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height + 50f);
            onLayoutChanged.Invoke();
            isExpanded = t >= 1f;
            yield return null;
        }
        isBusy = false;
    }
    private IEnumerator Collapse()
    {
        isBusy = true;
        animateTime = Time.time;

        while (isExpanded)
        {
            float elapsed = Time.time - animateTime;
            float t = elapsed / animationDuration;
            arrow.localEulerAngles = new Vector3(0f, 0f, Mathf.Lerp(-90f, 0f, t));
            float height = Mathf.Lerp(contentHeight, 0f, t);
            content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height + 50f);
            onLayoutChanged.Invoke();
            isExpanded = t < 1f;
            yield return null;
        }
        isBusy = false;
    }

    public void OnClick()
    {
        if (isBusy)
        {
            return;
        }
        if (!isExpanded)
        {
            StartCoroutine(Expand());
            return;
        }

        StartCoroutine(Collapse());
    }

    private ProgramMapGui[] programMaps;

    public ProgramMapGui[] GetProgramMaps()
    {
        return programMaps;
    }

    [Header("Dependencies")]
    [SerializeField]
    private Sounds sounds;

    private MidiTrack midiTrack;
    private int trackIndex;
    public void SetTrack(MidiTrack track, int index)
    {
        midiTrack = track;
        trackIndex = index;

        trackNameLabel.text = track.name;

        if (programMaps is not null)
        {
            for (int i = 0; i < programMaps.Length; i++)
            {
                Destroy(programMaps[i].gameObject);
            }
        }

        programMaps = new ProgramMapGui[track.programs.Length];
        for (int i = 0; i < programMaps.Length; i++)
        {
            ProgramMapGui programMap = Instantiate(programTemplate, programTemplate.transform.parent);
            programMap.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, templateHeight * i, templateHeight);
            programMap.SetProgram(track.programs[i], track.isPercussion);
            programMap.SetSound(sounds.GetSilence());
            programMap.gameObject.SetActive(true);
            programMaps[i] = programMap;
        }
        contentHeight = templateHeight * programMaps.Length;
    }

    public void SendToBuzzToneConvert()
    {
        BuzzToneConvert.ConvertTrack(midiTrack);
    }

    [SerializeField]
    private TracksScreen tracksScreen;

    public void Mute()
    {
        tracksScreen.MuteTrack(trackIndex);
    }
    
    public void Solo()
    {
        tracksScreen.SoloTrack(trackIndex);
    }

    public void On()
    {
        tracksScreen.OnTrack(trackIndex);
    }

    public void SetAudible(bool value)
    {
        trackNameLabel.color = value ? audibleColor : inaudibleColor;
    }

    /// <summary>
    /// Constructs a <see cref="MappedTrack"/> using the input provided to this GUI.
    /// </summary>
    /// <returns></returns>
    public MappedTrack GetMappedTrack()
    {
        Dictionary<SevenBitNumber, TdwProgramMap> programMappings = new();

        foreach (ProgramMapGui programMap in programMaps)
        {
            programMappings.Add(programMap.GetProgram(), programMap.GetProgramMap());
        }

        MappedTrack mappedTrack = new()
        {
            midiTrack = midiTrack,
            programMappings = programMappings
        };

        return mappedTrack;
    }

    private IStackedRect.LayoutChangedCallback onLayoutChanged;

    void IStackedRect.LayoutAwake(IStackedRect.LayoutChangedCallback layoutChangedCallback)
    {
        onLayoutChanged = layoutChangedCallback;
    }

    RectTransform IStackedRect.GetRect()
    {
        return rectTransform;
    }
}
