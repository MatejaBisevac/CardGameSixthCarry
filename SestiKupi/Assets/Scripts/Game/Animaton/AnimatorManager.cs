using DG.Tweening;
using SestiKupi.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : MonoBehaviour 
{
    public AudioSource clickSound;
    public float LiftAmount;
    private Vector3 originalPosition;
    private Vector3 liftedPosition;
    void Start()
    {
        originalPosition = transform.position;
        liftedPosition = new Vector3(transform.position.x, transform.position.y + LiftAmount, transform.position.z);
    }
    void OnMouseEnter()
    {
        LiftObjectAmount();
    }
    void OnMouseExit() {
        DeLiftObjectAmount();
    }

    void OnMouseDown()
    {
        transform.position = originalPosition;
        clickSound.Play();
    }

    void OnMouseUp()
    {
        LiftObjectAmount();
    }

    private void LiftObjectAmount()
    {
        transform.DOMove(liftedPosition, 0.15f);
    }

    private void DeLiftObjectAmount()
    {
        transform.DOMove(originalPosition, 0.15f);
    }
}
