using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MessageHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public TMPro.TextMeshPro PlayerListTextMeshPro;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        string text = "";
        foreach(var key in PlayerManager.s_Players.Keys.ToList())
        {
            text += PlayerManager.s_Players[key].Username+"<br>";
        }
        PlayerListTextMeshPro.text = text;
    }
}
