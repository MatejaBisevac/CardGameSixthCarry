using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RowBoxBehaviour : MonoBehaviour
{
    // Start is called before the first frame updat
    public int Column;
    public float LiftAmount;

    [Header("Audio Source")]
    public AudioSource audioSource;

    private Vector3 originalPosition;
    private Vector3 liftedPosition;

    void Awake()
    {
        originalPosition = transform.position;
        liftedPosition = new Vector3(transform.position.x, transform.position.y + LiftAmount, transform.position.z);
    }

    void OnMouseEnter()
    {
        LiftObjectAmount();
    }

    void OnMouseExit()
    {
        DeLiftObjectAmount();
    }

    void OnMouseDown()
    {
        audioSource.Play();
        transform.position = originalPosition;
        GameManager.Instance.TryChooseStackFromRow(Column);
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
