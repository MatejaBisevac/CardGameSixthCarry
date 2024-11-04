using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuAnimator : MonoBehaviour
{
    // Start is called before the first frame update

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0.5f,0.5f,0.5f));
        //transform.DORotate(new Vector3(transform.position.x, transform.position.y, transform.position.z), 10f);    
        //transform.DORotate(new Vector3(transform.position.x, transform.position.y+20f, transform.position.z), 10f);    
    }
}
