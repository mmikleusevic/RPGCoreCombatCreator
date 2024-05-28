using System;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] ProgressionCharacterClass[] characterClasses;

        public float GetStat(Stat stats , CharacterClass characterClass, int level)
        {
            foreach (ProgressionCharacterClass progCharacterClass in characterClasses)
            {
                if (progCharacterClass.characterClass != characterClass) continue;

                foreach (ProgressionStat progressionStat in progCharacterClass.progressionStat)
                {
                    if (progressionStat.stat != stats) continue;

                    if (progressionStat.levels.Length < level) continue;

                    return progressionStat.levels[level - 1];
                }
            }

            return 0;
        }

        [Serializable]
        public class ProgressionCharacterClass
        {
            public CharacterClass characterClass;
            public ProgressionStat[] progressionStat;
            //public float[] health;
        }

        [Serializable]
        public class ProgressionStat
        {
            public Stat stat;
            public float[] levels;
        }
    }
}
