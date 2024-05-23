using System.Collections;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour
    {
        private const string PLAYER = "Player";

        [SerializeField] private Weapon weapon = null;
        [SerializeField] private float respawnTime = 10f;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == PLAYER)
            {
                other.GetComponent<Fighter>().EquipWeapon(weapon);

                StartCoroutine(HideForSeconds(respawnTime));
            }
        }

        private IEnumerator HideForSeconds(float seconds)
        {
            ShowPickup(false);
            yield return new WaitForSeconds(seconds);
            ShowPickup(true);
        }


        private void ShowPickup(bool shouldShow)
        {
            GetComponent<Collider>().enabled = shouldShow;

            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(shouldShow);
            }
        }
    }
}