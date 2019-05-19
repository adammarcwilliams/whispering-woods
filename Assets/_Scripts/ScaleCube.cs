using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ScaleCube : MonoBehaviour
{
    float[] samples = new float[20000];
    public void Scale()
    {
        DOTween.Kill(transform);
        transform.DOPunchScale(new Vector3(2, 2, 2), 0.5f);
    }
}
