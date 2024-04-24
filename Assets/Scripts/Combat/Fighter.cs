using RPG.Movement;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour
    {
        [SerializeField] private float weaponRange = 2f;

        private Transform targetTransform;
        private Mover mover;

        private void Start()
        {
            mover = GetComponent<Mover>();
        }

        private void Update()
        {
            if (targetTransform == null) return;

            bool isInRange = Vector3.Distance(transform.position, targetTransform.position) < weaponRange;

            if (!isInRange)
            {
                mover.MoveTo(targetTransform.position);
            }
            else
            {
                mover.Stop();
            }
        }

        public void Attack(CombatTarget combatTarget)
        {
            targetTransform = combatTarget.transform;
        }
    }
}