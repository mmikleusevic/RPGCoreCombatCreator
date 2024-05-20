using System.Collections;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;

        private void Start()
        {
            StartCoroutine(FadeOutIn());
        }

        private IEnumerator FadeOutIn()
        {
            yield return FadeOut(3f);
            yield return FadeIn(2f);
        }

        private IEnumerator FadeOut(float time)
        {
            while (canvasGroup.alpha < 1)
            {
                canvasGroup.alpha += Time.deltaTime / time;

                yield return null;
            }
        }

        private IEnumerator FadeIn(float time)
        {
            while (canvasGroup.alpha > 0)
            {
                canvasGroup.alpha -= Time.deltaTime / time;

                yield return null;
            }
        }
    }
}
