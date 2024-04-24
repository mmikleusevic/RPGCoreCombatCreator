using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        private const string ATTACK = "attack";

        [SerializeField] private float weaponRange = 2f;
        [SerializeField] private float timeBetweenAttacks = 2f;

        private Transform targetTransform;
        private Mover mover;
        private ActionScheduler actionScheduler;
        private Animator animator;

        private float timeSinceLastAttack;

        private void Start()
        {
            mover = GetComponent<Mover>();
            actionScheduler = GetComponent<ActionScheduler>();
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (targetTransform == null) return;

            if (!GetIsInRange())
            {
                mover.MoveTo(targetTransform.position);
                return;
            }

            mover.Cancel();
            AttackBehaviour();
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, targetTransform.position) < weaponRange;
        }

        public void Attack(CombatTarget combatTarget)
        {
            actionScheduler.StartAction(this);
            targetTransform = combatTarget.transform;
        }

        public void Cancel()
        {
            targetTransform = null;
        }

        //Animation Event
        private void AttackBehaviour()
        {
            if (timeBetweenAttacks < timeSinceLastAttack)
            {
                animator.SetTrigger(ATTACK);
                timeSinceLastAttack = 0;
            }
        }
    }
}