using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.DamageText
{
    public class DamageTextSpawner : MonoBehaviour
    {
        [SerializeField] private DamageText damageText;

        void Start()
        {
            Spawn(100);
        }

        public void Spawn(float damage)
        {
            DamageText damageTextInstance = Instantiate(damageText, transform);
        }
    }
}
