using UnityEngine;
using UnityEngine.UI;

namespace RPG.Stats
{
    public class LevelDisplay : MonoBehaviour
    {
        private const string PLAYER = "Player";

        private Text levelValueText;

        private BaseStats baseStats;

        private void Awake()
        {
            baseStats = GameObject.FindWithTag(PLAYER).GetComponent<BaseStats>();
            levelValueText = GetComponent<Text>();
        }

        private void Update()
        {
            levelValueText.text = baseStats.GetLevel().ToString();
        }
    }
}

