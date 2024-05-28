using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attributes
{
    public class ExperienceDisplay : MonoBehaviour
    {
        private const string PLAYER = "Player";

        private Text experienceValueText;

        private Experience experience;

        private void Awake()
        {
            experience = GameObject.FindWithTag(PLAYER).GetComponent<Experience>();
            experienceValueText = GetComponent<Text>();
        }

        private void Update()
        {
            experienceValueText.text = experience.GetExperiencePoints().ToString();
        }
    }
}

