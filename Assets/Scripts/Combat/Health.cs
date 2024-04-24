using UnityEngine;

namespace RPG.Combat
{
    public class Health : MonoBehaviour
    {
        private const string DIE = "die";

        [SerializeField] private float healthPoints = 100f;

        private Animator animator;
        private bool isDead = false;

        private void Start()
        {
            animator = GetComponent<Animator>();
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
        }
    }
}