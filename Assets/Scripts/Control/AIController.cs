using UnityEngine;
using UnityEngine.AI;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        private const string PLAYER_TAG = "Player"; 

        [SerializeField] float chaseDistance = 5f;

        private NavMeshAgent agent;

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }

        private void Update()
        {
            if (DistanceToPlayer() < chaseDistance)
            {
                print(gameObject.name + " should chase");
            }
        }

        private float DistanceToPlayer()
        {
            GameObject playerGameObject = GameObject.FindWithTag(PLAYER_TAG);
            return Vector3.Distance(transform.position, playerGameObject.transform.position);
        }
    }
}

