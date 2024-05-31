using RPG.Saving;
using System.Collections;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        private const string defaultSaveFile = "save";
        [SerializeField] private float fadeInTime = 0.2f;

        private SavingSystem savingSystem;

        private void Awake()
        {
            savingSystem = GetComponent<SavingSystem>();
            StartCoroutine(LoadLastScene());
        }

        private IEnumerator LoadLastScene()
        {
            yield return StartCoroutine(savingSystem.LoadLastScene(defaultSaveFile));
            Fader fader = FindFirstObjectByType<Fader>();
            fader.FadeOutImmediate();
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
            savingSystem.Load(defaultSaveFile);
        }

        public void Save()
        {
            savingSystem.Save(defaultSaveFile);
        }

        public void Delete()
        {
            savingSystem.Delete(defaultSaveFile);
        }
    }
}