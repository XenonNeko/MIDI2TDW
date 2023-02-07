using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AppLoadScreen : MonoBehaviour
{
    [SerializeField]
    private App app;
    [SerializeField]
    private Sounds sounds;

    [SerializeField]
    private TextMeshProUGUI loadingMessage;
    [SerializeField]
    private Fader loadingFader;
    [SerializeField]
    private Fader splashFader;

    private bool waitingForSoundLoad;
    public void Open()
    {
        loadingMessage.text = "Loading, please wait...";
        waitingForSoundLoad = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (waitingForSoundLoad)
        {
            if (sounds.HasError)
            {
                waitingForSoundLoad = false;
                loadingMessage.text = "Something went wrong.";
                loadingFader.FadeOut(.5f, 0f);
            }
            if (sounds.IsLoaded)
            {
                waitingForSoundLoad = false;
                loadingMessage.text = "Done!";
                loadingFader.FadeOut(0.7f, 0f);
                splashFader.FadeIn(0.7f, 0.7f);
            }
        }
    }
}
