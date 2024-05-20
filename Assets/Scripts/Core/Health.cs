using RPG.Saving;
using UnityEngine;

namespace RPG.Core
{
    public class Health : MonoBehaviour, ISaveable
    {
        private const string DIE = "die";

        [SerializeField] private float healthPoints = 100f;

        private Animator animator;
        private ActionScheduler actionScheduler;

        private bool isDead = false;

        public bool IsDead()
        {
            return isDead;
        }

        private void Start()
        {
            animator = GetComponent<Animator>();
            actionScheduler = GetComponent<ActionScheduler>();
        }

        public void TakeDamage(float damage)
        {
            healthPoints = Mathf.Max(0, healthPoints - damage);

            if (healthPoints == 0 && !isDead)
            {
                Die();
            }

            print(healthPoints);
        }

        private void Die()
        {
            if (isDead) return;

            isDead = true;
            animator.SetTrigger(DIE);
            actionScheduler.CancelCurrentAction();
        }

        public object CaptureState()
        {
            return healthPoints;
        }

        public void RestoreState(object state)
        {
            float healthPoints = (float)state;

            this.healthPoints = healthPoints;

            if (healthPoints == 0)
            {
                Die();
            }
        }
    }
}