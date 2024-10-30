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
            readyForGameOverlay.SetActive(false);
        }

    }
}
