using SestiKupi.Core;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : Singleton<UIManager>
{
    public LocalSceneUI LocalUI;
    public GameObject DisconnectedText;
    public void Connect()
    {
        if(LocalUI == null)
        {
            Debug.LogError("No local UI on this scene");
            return;
        }

        string connectInput = "ConnectInput";
        if(!LocalUI.Components.TryGetValue(connectInput, out UIComponent component))
        {
            Debug.LogError(" No input component found: " + connectInput);
            return;
        }

        InputComponent input = (InputComponent)component;
        string username = input.Input.text;
        string ip = input.InputIP.text;
        NetworkManager.Instance.SetIp(ip);
        NetworkManager.Instance.Connect(username);

        //SceneManager.LoadScene("01-Level");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
