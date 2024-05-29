using RPG.Saving;
using System.Collections;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        private const string defaultSaveFile = "save";
        [SerializeField] private float fadeInTime = 0.2f;

        private void Awake()
        {
            StartCoroutine(LoadLastScene());
        }

        private IEnumerator LoadLastScene()
        {
            Fader fader = FindFirstObjectByType<Fader>();
            fader.FadeOutImmediate();
            yield return StartCoroutine(GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile));
            yield return StartCoroutine(fader.FadeIn(fadeInTime));
        }

        void Update()
        {
            if (Input.GetKeyUp(KeyCode.L))
            {
                Load();
            }

            if (Input.GetKeyUp(KeyCode.S))
            {
                Save();
            }

            if (Input.GetKeyUp(KeyCode.Delete))
            {
                Delete();
            }
        }

        public void Load()
        {
            GetComponent<SavingSystem>().Load(defaultSaveFile);
        }

        public void Save()
        {
            GetComponent<SavingSystem>().Save(defaultSaveFile);
        }

        public void Delete()
        {
            GetComponent<SavingSystem>().Delete(defaultSaveFile);
        }
    }
}