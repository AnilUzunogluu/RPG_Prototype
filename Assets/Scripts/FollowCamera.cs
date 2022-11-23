using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private Transform target;

    private void Update()
    {
        RotateCamera();
    }

    void LateUpdate()
    {
        transform.position = target.position;
    }

    private void RotateCamera()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            transform.RotateAround(target.position, Vector3.up, 90);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            transform.RotateAround(target.position, Vector3.up, -90);

        }
    }
}