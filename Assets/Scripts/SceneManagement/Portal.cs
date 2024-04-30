using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        private const string PLAYER = "Player";

        [SerializeField] int sceneToLoad = -1;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == PLAYER)
            {
                StartCoroutine(Transition());
            }
        }

        private IEnumerator Transition()
        {
            DontDestroyOnLoad(this);
            yield return SceneManager.LoadSceneAsync(sceneToLoad);
            print("Scene loaded");
            Destroy(gameObject);
        }
    }
}

