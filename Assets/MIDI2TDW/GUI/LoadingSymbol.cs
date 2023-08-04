using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingSymbol : MonoBehaviour
{
    [SerializeField]
    private Transform[] transforms;
    private Vector3[] origins;

    [SerializeField]
    private AnimationCurve curve;
    [SerializeField]
    private float amplitude;
    [SerializeField]
    private float interval;
    [SerializeField]
    private float duration;

    private int numTransforms;
    private float loopInterval;

    private void Awake()
    {
        numTransforms = transforms.Length;
        origins = new Vector3[numTransforms];
        for (int i = 0; i < numTransforms; i++)
        {
            origins[i] = transforms[i].localPosition;
        }
        loopInterval = interval * numTransforms;
    }

    private void Animate()
    {
        for (int i = 0; i < numTransforms; i++)
        {
            float offset = i * interval;
            float elapsed = Time.time - offset;
            elapsed = Mathf.Max(Mathf.Repeat(elapsed, loopInterval), 0f);
            float t = elapsed / duration;
            t = curve.Evaluate(t);
            float yOffset = t * amplitude;
            transforms[i].localPosition = origins[i] + new Vector3(0f, yOffset, 0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Animate();
    }
}
