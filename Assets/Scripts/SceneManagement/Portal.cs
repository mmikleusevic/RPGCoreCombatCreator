using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        private const string PLAYER = "Player";

        [SerializeField] int sceneToLoad = -1;

        private bool isTriggered = false;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == PLAYER && !isTriggered)
            {
                isTriggered = true;
                SceneManager.LoadScene(sceneToLoad);
            }
        }
    }
}

