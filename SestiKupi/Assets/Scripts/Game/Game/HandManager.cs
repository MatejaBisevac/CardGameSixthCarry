using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    // Start is called before the first frame update
    public bool CardsShown = false;
    private Vector3 initialPosition;
    private bool lift = true;
    void Awake()
    {
        initialPosition = transform.localPosition;
    }

    void OnMouseDown()
    {
        if(lift)
        {
            var x = initialPosition.x;
            var y = initialPosition.y;
            var z = initialPosition.z;
            var newVector = new Vector3(x, y, z + 1f);
            transform.DOMove(newVector, 0.15f);
        }
        else
        {
            transform.DOMove(initialPosition, 0.15f);
        }

        lift = !lift;
        CardsShown = !CardsShown;
    }
}
