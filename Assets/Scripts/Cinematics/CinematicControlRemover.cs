using RPG.Control;
using RPG.Core;
using System;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicControlRemover : MonoBehaviour
    {
        private const string PLAYER = "Player";

        private PlayableDirector playableDirector;
        private GameObject player;

        private void Start()
        {
            playableDirector = GetComponent<PlayableDirector>();
            player = GameObject.FindWithTag(PLAYER);

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
            player.GetComponent<PlayerController>().enabled = true;
        }

        public void DisableControl(PlayableDirector director)
        {         
            player.GetComponent<ActionScheduler>().CancelCurrentAction();
            player.GetComponent<PlayerController>().enabled = false;
        }
    }
}