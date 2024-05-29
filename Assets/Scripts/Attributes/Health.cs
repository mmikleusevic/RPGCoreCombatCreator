using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        private const string DIE = "die";

        [SerializeField] private float healthPoints = 100f;
        [SerializeField] private Animator animator;
        [SerializeField] private ActionScheduler actionScheduler;

        private bool isDead = false;

        private void Start()
        {
            healthPoints = GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            healthPoints = Mathf.Max(0, healthPoints - damage);

            if (healthPoints == 0 && !isDead)
            {
                Die();
                AwardExperience(instigator);
            }

            print(healthPoints);
        }


        public float GetPercentage()
        {
            return healthPoints / GetComponent<BaseStats>().GetStat(Stat.Health) * 100;
        }

        private void Die()
        {
            if (isDead) return;

            isDead = true;
            animator.SetTrigger(DIE);
            actionScheduler.CancelCurrentAction();
        }

        private void AwardExperience(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();

            if (experience == null) return;

            experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
        }

        public object CaptureState()
        {
            return healthPoints;
        }

        public void RestoreState(object state)
        {
            float healthPoints = (float)state;

            this.healthPoints = healthPoints;

            if (this.healthPoints == 0)
            {
                Die();
            }
        }
    }
}