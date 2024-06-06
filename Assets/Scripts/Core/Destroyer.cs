using UnityEngine;

namespace RPG.Core
{
    public class Destroyer : MonoBehaviour
    {
        [SerializeField] private GameObject parent;

        public void DestroyTarget()
        {
            Destroy(parent);
        }
    }
}
