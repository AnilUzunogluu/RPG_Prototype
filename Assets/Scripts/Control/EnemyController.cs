using System;
using RPG.Combat;
using RPG.Movement;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Control
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private float chaseDistance = 5f;
        [SerializeField] private float idleMovementSpeed = 3f;
        [SerializeField] private float aggroMovementSpeed = 4.5f;

        private GameObject _player;
        private Fighter _fighter;
        private Mover _mover;
        private Health _health;

        private NavMeshAgent _navMeshAgent;
        private Vector3 guardPosition;
        
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

            guardPosition = transform.position;
        }

        private void Update()
        {
            if (_health.IsDead) return;

            if (DistanceToPlayer() < chaseDistance && _fighter.CanAttack(_player))
            {
                    _navMeshAgent.speed = aggroMovementSpeed;
                    _fighter.SetAttackTarget(_player);
            }
            else
            {
                _navMeshAgent.speed = idleMovementSpeed;
                _mover.Move(guardPosition);
                _fighter.CancelAttack();
            }
        }
        
        private float DistanceToPlayer()
        {
            return Vector3.Distance(_player.transform.position, transform.position);

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
