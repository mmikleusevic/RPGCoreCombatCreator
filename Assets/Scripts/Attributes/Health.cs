using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        private const string DIE = "die";

        [SerializeField] float regenerationPercentage = 70f;
        [SerializeField] private Animator animator;
        [SerializeField] private ActionScheduler actionScheduler;

        private float healthPoints = -1f;
        private bool isDead = false;

        private void Start()
        {
            BaseStats baseStats = GetComponent<BaseStats>();
            baseStats.OnLevelUp += RegenerateHealth;

            if (healthPoints < 0)
            {
                healthPoints = baseStats.GetStat(Stat.Health);
            }
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

        private void RegenerateHealth()
        {
            float regenHealthPoints = GetComponent<BaseStats>().GetStat(Stat.Health) * regenerationPercentage / 100;
            healthPoints = Mathf.Max(healthPoints, regenHealthPoints);
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