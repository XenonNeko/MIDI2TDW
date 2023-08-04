using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class SoundSelect : MonoBehaviour
{
    [SerializeField]
    private SoundButton buttonTemplate;
    [SerializeField]
    private Sounds sounds;
    [SerializeField]
    private TextMeshProUGUI header;
    [SerializeField]
    private float size;
    [SerializeField]
    private int columns;
    [SerializeField]
    private int rows;

    public void DrawButtons()
    {
        TdwSound[] tdwSounds = sounds.GetTDWSounds();
        int column = 0;
        int row = 0;
        for (int i = 0; i < tdwSounds.Length; i++)
        {
            SoundButton soundButton = Instantiate(buttonTemplate, buttonTemplate.transform.parent);
            soundButton.SetSound(tdwSounds[i]);
            soundButton.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, column * size, size);
            soundButton.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, row * size, size);
            soundButton.gameObject.SetActive(true);
            column++;
            if (column == columns)
            {
                column = 0;
                row++;
            }
        }
    }

    private ProgramMapGui programMap;
    public void ChangeSound(ProgramMapGui requestingMap)
    {
        programMap = requestingMap;
        gameObject.SetActive(true);
    }

    private readonly List<TdwSound> hoveredSounds = new();
    private void HoveredSoundChanged()
    {
        if (hoveredSounds.Count == 0)
        {
            header.text = "Select a Sound";
            return;
        }
        TdwSound hoveredSound = hoveredSounds.First();
        if (hoveredSound is null)
        {
            return;
        }
        header.text = $@"<color=#00FF64>{hoveredSound.name}</color>{(hoveredSound.origin is null ? string.Empty : $" <color=#1B9C50>({hoveredSound.origin})</color>")}";
    }
    public void HoverSound(TdwSound sound)
    {
        hoveredSounds.Add(sound);
        HoveredSoundChanged();
    }
    public void StopHoverSound(TdwSound sound)
    {
        hoveredSounds.Remove(sound);
        HoveredSoundChanged();
    }

    public void SoundSelected(TdwSound sound)
    {
        hoveredSounds.Clear();
        programMap.SetSound(sound);
        gameObject.SetActive(false);
    }
}
