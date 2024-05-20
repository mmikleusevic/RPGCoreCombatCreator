using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        private enum DestinationIdentifier
        {
            A, B
        }

        private const string PLAYER = "Player";

        [SerializeField] private int sceneToLoad = -1;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private DestinationIdentifier destinationIdentifier;
        [SerializeField] private float fadeInTime;
        [SerializeField] private float fadeOutTime;
        [SerializeField] private float fadeWaitTime;

        private void Awake()
        {
            transform.parent = null;
        }

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

            Fader fader = FindFirstObjectByType<Fader>();

            yield return fader.FadeOut(fadeOutTime);

            SavingWrapper savingWrapper = FindFirstObjectByType<SavingWrapper>();
            savingWrapper.Save();

            yield return SceneManager.LoadSceneAsync(sceneToLoad);

            savingWrapper.Load();

            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);

            savingWrapper.Save();

            yield return new WaitForSeconds(fadeWaitTime);
            yield return fader.FadeIn(fadeInTime);
        }

        private Portal GetOtherPortal()
        {
            Portal[] portals = FindObjectsByType<Portal>(FindObjectsSortMode.None);
            foreach (Portal portal in portals)
            {
                if (portal == this || portal.destinationIdentifier == destinationIdentifier) continue;

                return portal;
            }

            return null;
        }

        private void UpdatePlayer(Portal portal)
        {
            GameObject player = GameObject.FindWithTag(PLAYER);

            NavMeshAgent agent = player.GetComponent<NavMeshAgent>();
            agent.enabled = false;
            agent.Warp(portal.spawnPoint.position);
            player.transform.rotation = portal.spawnPoint.rotation;
            agent.enabled = true;
        }
    }
}

