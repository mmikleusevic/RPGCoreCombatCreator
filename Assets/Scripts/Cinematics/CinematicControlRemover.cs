using System;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicControlRemover : MonoBehaviour
    {
        private PlayableDirector playableDirector;
        private void Start()
        {
            playableDirector = GetComponent<PlayableDirector>();

            playableDirector.played += DisableControl;
            playableDirector.stopped += EnableControl;
        }

        private void OnDisable()
        {
            playableDirector.played -= DisableControl;
            playableDirector.stopped -= EnableControl;
        }

        private void EnableControl(PlayableDirector director)
        {
            print("EnableControl");
        }

        public void DisableControl(PlayableDirector director)
        {
            print("DisableControl");
        }
    }
}