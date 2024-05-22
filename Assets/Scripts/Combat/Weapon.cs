using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] AnimatorOverrideController animatorOverride = null;
        [SerializeField] private GameObject weaponPrefab = null;

        public void SpawnWeapon(Transform handTransform, Animator animator)
        {
            Instantiate(weaponPrefab, handTransform);
            animator.runtimeAnimatorController = animatorOverride;
        }
    }
}