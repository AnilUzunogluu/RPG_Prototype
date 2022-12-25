using UnityEngine;

namespace RPG.Core
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private float cameraRotationSpeed = 25;

        private void Update()
        {
            RotateCamera();
        }

        private void LateUpdate()
        {
            transform.position = target.position;
        }

        private void RotateCamera()
        {
            if (Input.GetKey(KeyCode.E))
            {
                transform.RotateAround(target.position, Vector3.up, cameraRotationSpeed * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.Q))
            {
                transform.RotateAround(target.position, Vector3.up, -cameraRotationSpeed * Time.deltaTime);
            }
        }
    }
}
