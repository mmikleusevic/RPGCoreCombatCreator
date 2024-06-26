using GameDevTV.Utils;
using RPG.Attributes;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using System;
using UnityEngine;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        private const string PLAYER_TAG = "Player";

        [SerializeField] private float chaseDistance = 5f;
        [SerializeField] private float suspicionTime = 2f;
        [SerializeField] private float waypointDwellTime = 3f;
        [SerializeField] private PatrolPath patrolPath;
        [Range(0f, 1f)]
        [SerializeField] private float patrolSpeedFraction = 0.4f;
        [SerializeField] private float aggro = 5f;

        private Fighter fighter;
        private Mover mover;
        private Health health;
        private GameObject player;
        private ActionScheduler actionScheduler;

        private LazyValue<Vector3> guardPosition;
        private float timeSinceLastSawPlayer = Mathf.Infinity;
        private int currentWaypointIndex = 0;
        private float waypointTolerance = 1f;
        private float timeSinceArrivedAtWaypoint = Mathf.Infinity;
        private float timeSinceAggrevated = Mathf.Infinity;
        private float shoutDistance = 5f;

        private void Awake()
        {
            guardPosition = new LazyValue<Vector3>(GetGuardPosition);
            fighter = GetComponent<Fighter>();
            mover = GetComponent<Mover>();
            health = GetComponent<Health>();
            player = GameObject.FindWithTag(PLAYER_TAG);
            actionScheduler = GetComponent<ActionScheduler>();
        }
        private void Start()
        {
            guardPosition.ForceInit();
        }

        private Vector3 GetGuardPosition()
        {
            return transform.position;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }

        private void Update()
        {
            if (health.IsDead()) return;

            if (IsAggrevated() && fighter.CanAttack(player))
            {
                AttackBehaviour();
            }
            else if (timeSinceLastSawPlayer < suspicionTime)
            {
                SuspicionBehaviour();
            }
            else
            {
                PatrolBehaviour();
            }

            UpdateTimers();
        }

        public void Aggrevate()
        {
            timeSinceAggrevated = 0;
        }

        private void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArrivedAtWaypoint += Time.deltaTime;
            timeSinceAggrevated += Time.deltaTime;
        }

        private void PatrolBehaviour()
        {
            Vector3 nextPosition = guardPosition.value;

            if (patrolPath != null)
            {
                if (AtWaypoint())
                {
                    timeSinceArrivedAtWaypoint = 0;
                    CycleWaypoint();
                }
                nextPosition = GetCurrentWaypoint();
            }
            if (timeSinceArrivedAtWaypoint > waypointDwellTime)
            {
                mover.StartMoveAction(nextPosition, patrolSpeedFraction);
            }
        }

        private void CycleWaypoint()
        {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(currentWaypointIndex);
        }

        private bool AtWaypoint()
        {
            return Vector3.Distance(transform.position, GetCurrentWaypoint()) < waypointTolerance;
        }

        private void SuspicionBehaviour()
        {
            actionScheduler.CancelCurrentAction();
        }

        private void AttackBehaviour()
        {
            timeSinceLastSawPlayer = 0;
            fighter.Attack(player);

            AggrevateNearbyEnemies();
        }

        private void AggrevateNearbyEnemies()
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, shoutDistance, Vector3.up, 0);

            foreach (RaycastHit hit in hits)
            {
                AIController enemy = hit.collider.GetComponent<AIController>();
                if (enemy == null) continue;

                enemy.Aggrevate();
            }
        }

        private bool IsAggrevated()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            return distanceToPlayer < chaseDistance || timeSinceAggrevated < aggro;
        }
    }
}

