using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ScaleCube : MonoBehaviour
{
    public void Scale()
    {
        transform.DOPunchScale(new Vector3(2, 2, 2), 0.5f);
    }
}
