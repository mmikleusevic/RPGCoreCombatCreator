using UnityEngine;

namespace RPG.Combat
{
    public class CombatTarget : MonoBehaviour
    {
        private Health health;

        private void Start()
        {
            health = GetComponent<Health>();
        }

        public void Attack(float damage)
        {
            health.TakeDamage(damage);
        }
    }
}