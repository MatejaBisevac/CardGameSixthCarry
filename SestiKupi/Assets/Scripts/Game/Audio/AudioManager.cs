using SestiKupi.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Game.Audio
{
    public class AudioManager : Singleton<AudioManager>
    {
        public AudioSource BoomSoundEffect;
        public AudioSource SlideSoundEffect;


        public void PlayBoom()
        {
            BoomSoundEffect.Play();
        }

        public void PlaySlide()
        {
            SlideSoundEffect.Play();
        }
    }
}
