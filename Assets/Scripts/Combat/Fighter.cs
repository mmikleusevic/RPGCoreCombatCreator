using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        private const string ATTACK = "attack";
        private const string STOP_ATTACK = "stopAttack";

        [SerializeField] private float weaponRange = 2f;
        [SerializeField] private float timeBetweenAttacks = 2f;
        [SerializeField] private int weaponDamage = 5;

        private Health target;
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

            if (target == null) return;
            if (target.IsDead()) return;

            if (!GetIsInRange())
            {
                mover.MoveTo(target.transform.position);
                return;
            }

            mover.Cancel();
            AttackBehaviour();
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < weaponRange;
        }

        public void Attack(CombatTarget combatTarget)
        {
            actionScheduler.StartAction(this);
            target = combatTarget.transform.GetComponent<Health>();
        }

        public void Cancel()
        {
            animator.SetTrigger(STOP_ATTACK);
            target = null;
        }

        public bool CanAttack(CombatTarget combatTarget)
        {
            if (combatTarget == null) return false;

            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();
        }

        private void AttackBehaviour()
        {
            transform.LookAt(target.transform);
            if (timeBetweenAttacks < timeSinceLastAttack)
            {
                //This will trigger Hit() event.
                animator.SetTrigger(ATTACK);
                timeSinceLastAttack = 0;
            }
        }

        //Animation Event
        private void Hit()
        {
            if (target == null) return;

            target.TakeDamage(weaponDamage);
        }
    }
}