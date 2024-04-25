using UnityEngine;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {
        [SerializeField] private float radius = 0.3f;

        private void OnDrawGizmos()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                child.position = new Vector3(child.position.x, radius, child.position.z);
                Gizmos.DrawSphere(child.position, radius);
            }        
        }
    }
}
