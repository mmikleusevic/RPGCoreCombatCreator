using UnityEngine;

namespace RPG.Attributes
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Health health;
        [SerializeField] private RectTransform foreground;
        [SerializeField] private Canvas rootCanvas;

        private void Update()
        {
            float healthNormalized = health.GetFraction();

            if (Mathf.Approximately(healthNormalized, 1) || Mathf.Approximately(healthNormalized,0))
            {
                rootCanvas.enabled = false;
            }

            rootCanvas.enabled = true;
            foreground.localScale = new Vector3(healthNormalized, 1, 1);
        }
    }
}
