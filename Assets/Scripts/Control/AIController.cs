using RPG.Combat;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        private const string PLAYER_TAG = "Player"; 

        [SerializeField] float chaseDistance = 5f;

        private NavMeshAgent agent;
        private Fighter fighter;
        private GameObject player;

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            fighter = GetComponent<Fighter>();
            player = GameObject.FindWithTag(PLAYER_TAG);
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }

        private void Update()
        {
            if (InChaseRange() && fighter.CanAttack(player))
            {
                agent.isStopped = false;

                if (DistanceToPlayer() < agent.stoppingDistance)
                {
                    agent.isStopped = true;
                    fighter.Attack(player);
                }
            }
            else
            {
                agent.isStopped = true;
                fighter.Cancel();
            }
        }

        private bool InChaseRange()
        {
            return DistanceToPlayer() < chaseDistance;
        }

        private float DistanceToPlayer()
        {
            return Vector3.Distance(transform.position, player.transform.position);
        }
    }
}

