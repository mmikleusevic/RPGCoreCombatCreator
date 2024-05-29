using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attributes
{
    public class HealthDisplay : MonoBehaviour
    {
        private const string PLAYER = "Player";

        private Text healthValueText;

        private Health health;

        private void Awake()
        {
            health = GameObject.FindWithTag(PLAYER).GetComponent<Health>();
            healthValueText = GetComponent<Text>();
        }

        private void Update()
        {
            healthValueText.text = string.Format("{0:0}/{1:0}", health.GetHealthPoints(), health.GetMaxHealthPoints());
        }
    }
}

