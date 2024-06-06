using UnityEngine;

namespace RPG.Attributes
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Health health;
        [SerializeField] private RectTransform foreground;
        private void Update()
        {
            float healthNormalized = health.GetFraction();

            foreground.localScale = new Vector3(healthNormalized, 1, 1);
        }
    }
}
