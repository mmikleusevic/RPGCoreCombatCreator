using RPG.Saving;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour, ISaveable
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
        public object CaptureState()
        {
            return alreadyTriggered;
        }

        public void RestoreState(object state)
        {
            bool isTriggered = (bool)state;

            alreadyTriggered = isTriggered;
        }
    }
}

