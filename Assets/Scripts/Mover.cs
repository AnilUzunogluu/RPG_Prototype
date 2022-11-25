using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour
{
    
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;


    private void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }


    private void Update()
    {
        UpdateLocomotionAnimation();
    }

    public void Move(Vector3 destination)
    {
        _navMeshAgent.destination = destination;
    }

    private void UpdateLocomotionAnimation()
    {
        var globalVelocity = _navMeshAgent.velocity;
        var localVelocity = transform.InverseTransformDirection(globalVelocity);
        _animator.SetFloat("forwardSpeed", localVelocity.z);
    }
}
