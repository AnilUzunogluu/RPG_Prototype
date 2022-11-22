using System;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    private Camera _mainCamera; 
    private NavMeshAgent _navMeshAgent;

    private const int GROUND_LAYER = 1 << 6;

    private void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _mainCamera = Camera.main;
    }


    private void Update()
    {
        MoveToTargetLocation();
    }

    private Ray RayOnMousePosition()
    {
        return _mainCamera.ScreenPointToRay(Input.mousePosition);
    }

    private Vector3 HitPositionOfRay()
    {
        return Physics.Raycast(RayOnMousePosition(), out var hit, 1000f, GROUND_LAYER) ? hit.point : transform.position;
    }

    private void MoveToTargetLocation()
    {
        if (Input.GetMouseButton(1))
        {
            _navMeshAgent.destination = HitPositionOfRay();
        }
    }
}
