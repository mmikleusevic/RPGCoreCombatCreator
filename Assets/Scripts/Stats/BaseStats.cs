using GameDevTV.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        public event Action OnLevelUp;

        [Range(1, 99)]
        [SerializeField] private GameObject levelUpParticleEffect = null;
        [SerializeField] Progression progression = null;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] private int startingLevel = 1;
        [SerializeField] private bool shouldUseModifiers = false;

        private Experience experience;
        private LazyValue<int> currentLevel;

        private void Awake()
        {
            currentLevel = new LazyValue<int>(CalculateLevel);
            experience = GetComponent<Experience>();
        }

        private void Start()
        {
            currentLevel.ForceInit();
        }

        private void OnEnable()
        {
            if (experience != null)
            {
                experience.OnExperienceGained += UpdateLevel;
            }
        }

        private void OnDisable()
        {
            if (experience != null)
            {
                experience.OnExperienceGained -= UpdateLevel;
            }
        }

        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();
            if (newLevel > currentLevel.value)
            {
                currentLevel.value = newLevel;
                LevelUpEffect();
                OnLevelUp?.Invoke();
            }
        }

        private void LevelUpEffect()
        {
            Instantiate(levelUpParticleEffect, transform);
        }

        public float GetStat(Stat stat)
        {
            return (GetBaseStat(stat) + GetAdditiveModifier(stat)) * (1 + GetPercentageModifier(stat) / 100);
        }

        private float GetBaseStat(Stat stat)
        {
            return progression.GetStat(stat, characterClass, GetLevel());
        }

        public int GetLevel()
        {
            return currentLevel.value;
        }

        private float GetAdditiveModifier(Stat stat)
        {
            if (!shouldUseModifiers) return 0;

            float sum = 0;

            IModifierProvider[] providers = GetComponents<IModifierProvider>();
            foreach (IModifierProvider provider in providers)
            {
                IEnumerable<float> additiveModifiers = provider.GetAdditiveModifiers(stat);
                foreach (float modifiers in additiveModifiers)
                {
                    sum += modifiers;
                }
            }

            return sum;
        }

        private float GetPercentageModifier(Stat stat)
        {
            if (!shouldUseModifiers) return 0;

            float sum = 0;

            IModifierProvider[] providers = GetComponents<IModifierProvider>();
            foreach (IModifierProvider provider in providers)
            {
                IEnumerable<float> additiveModifiers = provider.GetPercentageModifiers(stat);
                foreach (float modifiers in additiveModifiers)
                {
                    sum += modifiers;
                }
            }

            return sum;
        }

        private int CalculateLevel()
        {
            if (experience == null) return startingLevel;

            float currentXP = experience.GetExperiencePoints();
            int penultimateLevel = progression.GetLevels(Stat.ExperienceToLevelUp, characterClass);

            for (int level = 1; level < penultimateLevel; level++)
            {
                float XPToLevelUp = progression.GetStat(Stat.ExperienceToLevelUp, characterClass, level);
                if (XPToLevelUp > currentXP)
                {
                    return level;
                }
            }

            return penultimateLevel + 1;
        }
    }
}
