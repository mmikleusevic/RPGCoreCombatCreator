using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] AnimatorOverrideController animatorOverride = null;
        [SerializeField] private GameObject equippedPrefab = null;
        [SerializeField] private float weaponDamage = 5;
        [SerializeField] private float weaponRange = 2f;
        [SerializeField] private bool isRightHanded = true;

        public void SpawnWeapon(Transform rightHand, Transform leftHand, Animator animator)
        {
            if (equippedPrefab != null)
            {
                if (isRightHanded)
                {
                    Instantiate(equippedPrefab, rightHand);
                }
                else
                {
                    Instantiate(equippedPrefab, leftHand);
                }

            }

            if (animatorOverride != null)
            {
                animator.runtimeAnimatorController = animatorOverride;
            }
        }

        public float GetWeaponDamage()
        {
            return weaponDamage;
        }

        public float GetWeaponRange()
        {
            return weaponRange;
        }
    }
}