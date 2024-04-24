using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour
    {
        private const string FORWARD_SPEED = "forwardSpeed";

        [SerializeField] private Animator animator;

        private NavMeshAgent agent;

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            UpdateAnimator();
        }

        public void MoveTo(Vector3 destination)
        {
            agent.destination = destination;
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = agent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;

            animator.SetFloat(FORWARD_SPEED, speed);
        }
    }
}