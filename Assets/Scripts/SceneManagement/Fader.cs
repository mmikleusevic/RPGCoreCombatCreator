using System.Collections;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;

        private Coroutine currentActiveFade;

        public void FadeOutImmediate()
        {
            canvasGroup.alpha = 1f;
        }

        private IEnumerator Fade(float target, float time)
        {
            if (currentActiveFade != null) StopCoroutine(currentActiveFade);

            currentActiveFade = StartCoroutine(FadeRoutine(target, time));
            yield return currentActiveFade;
        }

        private IEnumerator FadeRoutine(float target, float time)
        {
            while (!Mathf.Approximately(canvasGroup.alpha, target))
            {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, target, Time.deltaTime / time);
                yield return null;
            }
        }

        public IEnumerator FadeOut(float time)
        {
            return Fade(1f, time);
        }

        public IEnumerator FadeIn(float time)
        {
            return Fade(0f, time);
        }
    }
}
