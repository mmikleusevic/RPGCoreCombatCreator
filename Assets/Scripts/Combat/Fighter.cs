using GameDevTV.Utils;
using RPG.Attributes;
using RPG.Core;
using RPG.Movement;
using RPG.Saving;
using RPG.Stats;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable, IModifierProvider
    {
        private const string ATTACK = "attack";
        private const string STOP_ATTACK = "stopAttack";

        [SerializeField] private float timeBetweenAttacks = 2f;
        [SerializeField] private Transform leftHandTransform = null;
        [SerializeField] private Transform rightHandTransform = null;
        [SerializeField] private WeaponConfig defaultWeapon = null;

        private Health target;
        private Mover mover;
        private ActionScheduler actionScheduler;
        private LazyValue<WeaponConfig> currentWeapon;

        private float timeSinceLastAttack = Mathf.Infinity;

        private void Awake()
        {
            currentWeapon = new LazyValue<WeaponConfig>(SetupDefaultWeapon);
            mover = GetComponent<Mover>();
            actionScheduler = GetComponent<ActionScheduler>();
        }

        private void Start()
        {
            currentWeapon.ForceInit();
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (target == null) return;
            if (target.IsDead()) return;

            if (!GetIsInRange())
            {
                mover.MoveTo(target.transform.position, 1f);
                return;
            }

            mover.Cancel();
            AttackBehaviour();
        }

        private WeaponConfig SetupDefaultWeapon()
        {
            AttachWeapon(defaultWeapon);
            return defaultWeapon;
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < currentWeapon.value.GetWeaponRange();
        }

        public void Attack(GameObject combatTarget)
        {
            actionScheduler.StartAction(this);
            target = combatTarget.transform.GetComponent<Health>();
        }

        public bool CanAttack(GameObject combatTarget)
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
                TriggerAttack();
                timeSinceLastAttack = 0;
            }
        }

        private void TriggerAttack()
        {
            Animator animator = GetComponent<Animator>();

            animator.ResetTrigger(STOP_ATTACK);
            animator.SetTrigger(ATTACK);
        }

        //Animation Event
        private void Hit()
        {
            if (target == null) return;

            float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);

            if (currentWeapon.value.HasProjectile())
            {
                currentWeapon.value.LaunchProjectile(rightHandTransform, leftHandTransform, target, gameObject, damage);
            }
            else
            {
                target.TakeDamage(gameObject, damage);
            }
        }

        private void Shoot()
        {
            Hit();
        }

        public void Cancel()
        {
            TriggerStopAttack();
            target = null;
            mover.Cancel();
        }

        private void TriggerStopAttack()
        {
            Animator animator = GetComponent<Animator>();

            animator.SetTrigger(STOP_ATTACK);
            animator.ResetTrigger(ATTACK);
        }

        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return currentWeapon.value.GetWeaponDamage();
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return currentWeapon.value.GetPercentageBonus();
            }
        }

        public void EquipWeapon(WeaponConfig weapon)
        {
            currentWeapon.value = weapon;
            AttachWeapon(weapon);
        }

        private void AttachWeapon(WeaponConfig weapon)
        {
            weapon.SpawnWeapon(rightHandTransform, leftHandTransform, GetComponent<Animator>());
        }

        public Health GetTarget()
        {
            return target;
        }

        public object CaptureState()
        {
            return currentWeapon.value.name;
        }

        public void RestoreState(object state)
        {
            WeaponConfig weapon = (WeaponConfig)Resources.Load((string)state);
            EquipWeapon(weapon);
        }
    }
}