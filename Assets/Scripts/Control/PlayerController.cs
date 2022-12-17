using RPG.Combat;
using UnityEngine;
using RPG.Movement;
using Vector3 = UnityEngine.Vector3;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        private Mover _mover;
        private Camera _mainCamera;
        private Fighter _fighter;

        private void Start()
        {
            _mover = GetComponent<Mover>();
            _fighter = GetComponent<Fighter>();
            _mainCamera = Camera.main;  
        }

        private void Update()
        {
            if (InteractWithCombat()) return;
            if (InteractWithMovement()) return;
        }

        private bool InteractWithCombat()
        {
            var hits = Physics.RaycastAll(RayOnMousePosition());

            for (int i = 0; i < hits.Length; i++)
            {
                var target = hits[i].transform.GetComponent<CombatTarget>();
                if (target == null) continue;

                if (Input.GetMouseButtonDown(0))
                {
                    _fighter.Attack(target);
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
    }
}
