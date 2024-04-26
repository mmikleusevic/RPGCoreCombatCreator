using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour
    {
        private const string PLAYER = "Player";

        private bool alreadyTriggered = false;

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.tag == PLAYER && !alreadyTriggered)
            {
                alreadyTriggered = true;
                GetComponent<PlayableDirector>().Play();
            }
        }
    }
}

