using System;
using UnityEngine;
using RPG.Movement;
using RPG.Combat;


namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        private Mover _mover;
        private Camera _mainCamera;
        private Fighter _fighter;
        private Health _health;

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
            _mover = GetComponent<Mover>();
            _fighter = GetComponent<Fighter>();
            _health = GetComponent<Health>();
            _mainCamera = Camera.main;  
        }

        private void Update()
        {
            if (_health.IsDead) return;
            
            if (InteractWithCombatTarget()) return;
            if (InteractWithMovement()) return;
        }

        private bool InteractWithCombatTarget()
        {
            var hits = Physics.RaycastAll(RayOnMousePosition());

            for (int i = 0; i < hits.Length; i++)
            {
                var target = hits[i].transform.GetComponent<CombatTarget>();
                if (target == null) continue;
                
                if (!_fighter.CanAttack(target.gameObject)) continue;

                if (Input.GetMouseButtonDown(0))
                {
                    _fighter.SetAttackTarget(target.gameObject);
                }
                
                return true;
            }
            
            return false;
        }

        private bool InteractWithMovement()
        {
            if (!Physics.Raycast(RayOnMousePosition(), out var hit, 1000f)) return false;
            
            if (Input.GetMouseButton(1))
            {
                _fighter.CancelAttack();
                MoveToDestination(hit.point);
            }

            return true;
        }

        private void MoveToDestination(Vector3 destination)
        {
            _mover.Move(destination);
        }

        private Ray RayOnMousePosition()
        {
            return _mainCamera.ScreenPointToRay(Input.mousePosition);
        }

        private void StopAllBehavior()
        {
            _fighter.CancelAttack();
            _mover.Stop();
        }
    }
}
