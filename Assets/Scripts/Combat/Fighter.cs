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

            if (!GetIsInRange())
            {
                mover.MoveTo(targetTransform.position);
                return;
            }

            mover.Stop();
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, targetTransform.position) < weaponRange;
        }

        public void Attack(CombatTarget combatTarget)
        {
            targetTransform = combatTarget.transform;
        }

        public void Cancel()
        {
            targetTransform = null;
        }
    }
}