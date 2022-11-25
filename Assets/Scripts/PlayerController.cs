using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Mover _mover;
    private Camera _mainCamera; 
    
    private const int GROUND_LAYER = 1 << 6;

    private void Start()
    {
        _mover = GetComponent<Mover>();
        _mainCamera = Camera.main;

    }

    private void Update()
    {
        if (Input.GetMouseButton(1))
        {
            _mover.Move(TargetPosition());
        }
    }
    
    
    private Ray RayOnMousePosition()
    {
        return _mainCamera.ScreenPointToRay(Input.mousePosition);
    }

    private Vector3 TargetPosition()
    {
        return Physics.Raycast(RayOnMousePosition(), out var hit, 1000f, GROUND_LAYER) ? hit.point : transform.position;
    }
}
