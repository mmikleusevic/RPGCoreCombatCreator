using RPG.Combat;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour
    {
        private const string PLAYER = "Player";

        [SerializeField] private Weapon weapon = null;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == PLAYER)
            {
                other.GetComponent<Fighter>().EquipWeapon(weapon);

                Destroy(gameObject);
            }
        }
    }
}