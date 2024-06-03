using GameDevTV.Utils;
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

        private BaseStats baseStats;
        private LazyValue<float> healthPoints;
        private bool isDead = false;

        private void Awake()
        {
            healthPoints = new LazyValue<float>(GetInitialHealth);
            baseStats = GetComponent<BaseStats>();
        }

        private void Start()
        {
            healthPoints.ForceInit();
        }

        private float GetInitialHealth()
        {
            return baseStats.GetStat(Stat.Health);
        }

        private void OnEnable()
        {
            baseStats.OnLevelUp += RegenerateHealth;
        }

        private void OnDisable()
        {
            baseStats.OnLevelUp -= RegenerateHealth;
        }

        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            healthPoints.value = Mathf.Max(0, healthPoints.value - damage);

            if (healthPoints.value == 0 && !isDead)
            {
                Die();
                AwardExperience(instigator);
            }

            print(healthPoints);
        }

        public float GetHealthPoints()
        {
            return healthPoints.value;
        }

        public float GetMaxHealthPoints()
        {
            return baseStats.GetStat(Stat.Health);
        }


        public float GetPercentage()
        {
            return healthPoints.value / baseStats.GetStat(Stat.Health) * 100;
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

            experience.GainExperience(baseStats.GetStat(Stat.ExperienceReward));
        }

        private void RegenerateHealth()
        {
            float regenHealthPoints = baseStats.GetStat(Stat.Health) * regenerationPercentage / 100;
            healthPoints.value = Mathf.Max(healthPoints.value, regenHealthPoints);
        }

        public object CaptureState()
        {
            return healthPoints;
        }

        public void RestoreState(object state)
        {
            float healthPoints = (float)state;

            this.healthPoints.value = healthPoints;

            if (this.healthPoints.value == 0)
            {
                Die();
            }
        }
    }
}