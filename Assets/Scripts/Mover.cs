using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour
{
    [SerializeField] private Transform target;

    private void Update()
    {
        NavMeshAgent navMeshAgent = GetComponent<NavMeshAgent>();

        navMeshAgent.destination = target.position;
    }
}
