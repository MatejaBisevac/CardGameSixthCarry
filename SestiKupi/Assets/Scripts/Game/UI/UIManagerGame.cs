using SestiKupi.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Game.UI
{
    public class UIManagerGame : Singleton<UIManagerGame>
    {
        public GameObject readyForGameOverlay;

        public void OnReadyClick()
        {
            PlayerManager.Instance.LocalPlayerReadyToPlay();
            //readyForGameOverlay.SetActive(false);
        }

        public void ReceiveCanPlay()
        {
            CameraAnimator.Instance.ReturnToOriginalPosition();
            readyForGameOverlay.SetActive(false);
            GameManager.Instance.LocalPlayerWaiting = false;
        }

        public void QuitGame()
        {
            Debug.Log("APP QUIT");
            Application.Quit();
        }

    }
}
