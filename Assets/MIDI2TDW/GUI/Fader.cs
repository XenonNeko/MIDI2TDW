using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Fader : MonoBehaviour
{
    [SerializeField]
    private bool startHidden;

    private Graphic[] graphics;
    private float[] alphas;

    private bool dontRefetchGraphics;
    private void GetGraphics()
    {
        if (dontRefetchGraphics)
        {
            dontRefetchGraphics = false;
            return;
        }
        graphics = GetComponentsInChildren<Graphic>();
        alphas = new float[graphics.Length];
        for (int i = 0; i < graphics.Length; i++)
        {
            alphas[i] = graphics[i].color.a;
        }
    }

    public bool IsDone { get; private set; }

    private float duration;
    private float time;
    private IEnumerator FadeOutCoroutine()
    {
        while (Time.time - time < duration)
        {
            float t = (Time.time - time) / duration;
            for (int i = 0; i < graphics.Length; i++)
            {
                Color color = graphics[i].color;
                color.a = Mathf.Lerp(alphas[i], 0f, t);
                graphics[i].color = color;
            }
            yield return null;
        }
        for (int i = 0; i < graphics.Length; i++)
        {
            Color color = graphics[i].color;
            color.a = 0f;
            graphics[i].color = color;
        }
        IsDone = true;
    }

    public void FadeOut(float duration, float delay = 0f)
    {
        IsDone = false;
        this.duration = duration;
        time = Time.time + delay;
        GetGraphics();
        StartCoroutine(FadeOutCoroutine());
    }

    private IEnumerator FadeInCoroutine()
    {
        while (Time.time - time < duration)
        {
            float t = (Time.time - time) / duration;
            for (int i = 0; i < graphics.Length; i++)
            {
                Color color = graphics[i].color;
                color.a = Mathf.Lerp(0f, alphas[i], t);
                graphics[i].color = color;
            }
            yield return null;
        }
        for (int i = 0; i < graphics.Length; i++)
        {
            Color color = graphics[i].color;
            color.a = alphas[i];
            graphics[i].color = color;
        }
        IsDone = true;
    }

    public void FadeIn(float duration, float delay = 0f)
    {
        IsDone = false;
        this.duration = duration;
        time = Time.time + delay;
        GetGraphics();
        StartCoroutine(FadeInCoroutine());
    }

    private void Awake()
    {
        if (!startHidden)
        {
            return;
        }
        GetGraphics();
        dontRefetchGraphics = true;
        for (int i = 0; i < graphics.Length; i++)
        {
            Color color = graphics[i].color;
            color.a = 0f;
            graphics[i].color = color;
        }
    }
}
