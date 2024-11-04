using DG.Tweening;
using SestiKupi.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAnimator : Singleton<CameraAnimator>
{
    // Start is called before the first frame update
    Vector3 originalPosition;
    public GameObject newCameraPosition;

    void Start()
    {
        originalPosition = transform.position;
        GoToSecondPosition();

    }

    public void ReturnToOriginalPosition()
    {
        transform.DOMove(originalPosition, 2f);
    }

    public void GoToSecondPosition()
    {
        transform.DOMove(newCameraPosition.transform.position, 2f);
    }
}
