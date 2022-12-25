using System;
using RPG.Combat;
using RPG.Movement;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Control
{
    public class EnemyController : MonoBehaviour
    {
        [Header("Patrolling")]
        [SerializeField] private PatrolPath patrolPath;
        [SerializeField] private float idleMovementSpeed = 3f;
        [SerializeField] private float waypointTolerance = 1f;

        [Header("Attacking")]
        [SerializeField] private float chaseDistance = 5f;
        [SerializeField] private float aggroMovementSpeed = 4.5f;
        [SerializeField] private float suspicionTime = 4f;

        private GameObject _player;
        private Fighter _fighter;
        private Mover _mover;
        private Health _health;

        private NavMeshAgent _navMeshAgent;
        private Vector3 _guardPosition;
        private int currentWaypointIndex = 0;
        private float timeSinceLastSpottedPlayer = Mathf.Infinity;

        private void OnEnable()
        {
            _health.OnDeath += StopAllBehavior;
        }

        private void OnDisable()
        {
            _health.OnDeath -= StopAllBehavior;
        }

        private void Awake()
        {
            _player = GameObject.FindWithTag("Player");
            _mover = GetComponent<Mover>();
            _fighter = GetComponent<Fighter>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _health = GetComponent<Health>();

            _guardPosition = transform.position;
        }

        private void Update()
        {
            if (_health.IsDead) return;

            if (DistanceToPlayer() < chaseDistance && _fighter.CanAttack(_player))
            {
                timeSinceLastSpottedPlayer = 0;
                AttackState();
            }
            else if (timeSinceLastSpottedPlayer < suspicionTime)
            {
                StopAllBehavior();
            }
            else
            {
                PatrolState();
            }

            timeSinceLastSpottedPlayer += Time.deltaTime;
        }

        private float DistanceToPlayer()
        {
            return Vector3.Distance(_player.transform.position, transform.position);
        }
        
        private void AttackState()
        {
            _navMeshAgent.speed = aggroMovementSpeed;
            _fighter.SetAttackTarget(_player);
        }

        private void PatrolState()
        {
            _navMeshAgent.speed = idleMovementSpeed;
            var nextPosition = _guardPosition;

            if (patrolPath != null)
            {
                if (AtCurrentWaypoint())
                {
                    CycleWaypoints();
                }

                nextPosition = GetCurrentWaypoint();
            }
            
            _mover.Move(nextPosition);
        }

        private bool AtCurrentWaypoint()
        {
            return Vector3.Distance(transform.position, GetCurrentWaypoint()) < waypointTolerance;
        }
        
        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(currentWaypointIndex);
        }

        private  void CycleWaypoints()
        {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }
        
        private void StopAllBehavior()
        {
            _fighter.CancelAttack();
            _mover.Stop();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}
