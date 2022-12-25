using UnityEngine;
using RPG.Movement;

namespace  RPG.Combat
{
    public class Fighter : MonoBehaviour
    {
        [SerializeField] private float weaponRange = 2f;
        [SerializeField] private float attackSpeed = 1f;
        [SerializeField] private float weaponDamage = 5f;

        private Health _target;
        private Mover _mover;
        private Animator _animator;

        private float _timeSinceLastAttack = Mathf.Infinity;

        private void Start()
        {
            _mover = GetComponent<Mover>();
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            _timeSinceLastAttack += Time.deltaTime;

            if (HasAvailableTarget())
            {
                MoveAndAttack();
            }
        }

        private bool HasAvailableTarget()
        {
            return _target != null && !_target.IsDead;
        }
        
        private void MoveAndAttack()
        {
            var isInAttackRange = Vector3.Distance(transform.position, _target.transform.position) <= weaponRange;

            if (isInAttackRange)
            {
                _mover.Stop();
                AttackTheTarget();
            }
            else
            {
                _mover.Move(_target.transform.position);
            }
        }
        
        private void AttackTheTarget()
        {
            transform.LookAt(_target.transform);
            if (_timeSinceLastAttack > attackSpeed)
            {
                _animator.ResetTrigger("cancelAttack");
                _animator.SetTrigger("attack");
                _timeSinceLastAttack = 0f;
            }
        }
        
        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) return false;
            return !combatTarget.GetComponent<Health>().IsDead;
        }

        public void SetAttackTarget(GameObject combatTarget)
        {
            _target = combatTarget.GetComponent<Health>();
        }

        public void CancelAttack()
        {
            _animator.SetTrigger("cancelAttack");
            _target = null;
        }
        
        //Attack Animation Event
        private void Hit()
        {
            if (_target == null) return;
            
            _target.TakeDamage(weaponDamage);
        }
    }
}
