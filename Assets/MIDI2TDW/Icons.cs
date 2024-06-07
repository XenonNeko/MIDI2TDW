using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Icons : MonoBehaviour
{
    [field: SerializeField]
    public Sprite[] IconSprites { get; private set; }

    [field: SerializeField]
    public Sprite NoIcon { get; private set; }
}
