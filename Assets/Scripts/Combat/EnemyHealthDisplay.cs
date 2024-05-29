using RPG.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        private const string PLAYER = "Player";

        private Text healthValueText;

        private Fighter fighter;

        private void Awake()
        {
            fighter = GameObject.FindWithTag(PLAYER).GetComponent<Fighter>();
            healthValueText = GetComponent<Text>();
        }

        private void Update()
        {
            Health health = fighter.GetTarget();

            if (health == null)
            {
                healthValueText.text = "N/A";
            }
            else
            {
                healthValueText.text = string.Format("{0:0}/{1:0}", health.GetHealthPoints(), health.GetMaxHealthPoints());
            }
        }
    }
}

