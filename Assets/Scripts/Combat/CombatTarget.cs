using RPG.Attributes;
using RPG.Control;
using UnityEngine;

namespace RPG.Combat
{
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour, IRaycastable
    {
        public bool HandleRaycast(PlayerController callingController)
        {
            if (Input.GetMouseButton(0))
            {
                Fighter player = callingController.GetComponent<Fighter>();

                if (!callingController.GetComponent<Fighter>().CanAttack(gameObject))
                {
                    return false;
                }

                if (Input.GetMouseButton(0))
                {
                    player.Attack(gameObject);
                }

                return true;
            }

            return false;
        }
    }
}