using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationManager : MonoBehaviour
{
    //0 - EXIT ; 1 - SETTINGS; 2 - GAME RULES 3- CLOSE
    public int ButtonType;

    [Header("MENU ITEM GAME OBJECTS")]
    public GameObject SettingsGameObj;
    public GameObject GameRulesGameObj;

    [Header("NAVIGATION ITEM OBJECT")]
    public GameObject CloseGameObj;

    private static bool toggleSettings;
    private static bool toggleGameRules;
    // Start is called before the first frame update
    void Start()
    {
        toggleSettings = false;
        toggleGameRules = false;
    }

    void Awake()
    {
       
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseDown()
    {
        switch(ButtonType) {
            case 0: //EXIT
                {
                    Application.Quit();
                    break;
                }
            case 1: // SETTINGS
                {
                    CloseGameRules();
                    if(toggleSettings)
                    {
                        CloseSettings();
                        HideCloseMenu();
                    }
                    else
                    {
                        ShowSettings();
                        ShowCloseMenu();
                    }
                    break;
                }
            case 2: // GAME RULES
                {
                    CloseSettings();
                    if(toggleGameRules)
                    {
                        CloseGameRules();
                        HideCloseMenu();
                    }
                    else
                    {
                        ShowGameRules();
                        ShowCloseMenu();
                    }
                    break;
                }
            case 3:
                {
                    CloseSettings();
                    CloseGameRules();
                    HideCloseMenu();
                    break;
                }
    }
}
    private void CloseSettings()
    {
        toggleSettings = false;
        SettingsGameObj.SetActive(false);
    }

    private void CloseGameRules()
    {
        toggleGameRules = false;
        GameRulesGameObj.SetActive(false);
    }

    private void ShowSettings()
    {
        toggleSettings = true;
        SettingsGameObj.SetActive(true);
    }

    private void ShowGameRules()
    {
        toggleGameRules = true;
        GameRulesGameObj.SetActive(true);
    }
    private void ShowCloseMenu()
    {
        CloseGameObj.SetActive(true);
    }

    private void HideCloseMenu()
    {
        CloseGameObj.SetActive(false);
    }
}
