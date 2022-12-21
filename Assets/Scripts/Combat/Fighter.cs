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

        private float timeSinceLastAttack;


        private void Start()
        {
            _mover = GetComponent<Mover>();
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (_target == null || _target.IsDead) return;
            
            var isInAttackRange = Vector3.Distance(transform.position, _target.transform.position) <= weaponRange;
            
            if (isInAttackRange)
            {
                _mover.Stop();
                AttackBehavior();
            }
            else
            {
                _mover.Move(_target.transform.position);
            }
        }

        public bool CanAttack(CombatTarget combatTarget)
        {
            if (combatTarget == null) return false;
            return !combatTarget.GetComponent<Health>().IsDead;
        }

        public void SetAttackTarget(CombatTarget combatTarget)
        {
            _target = combatTarget.GetComponent<Health>();
        }

        public void CancelAttack()
        {
            _animator.SetTrigger("cancelAttack");
            _target = null;
        }

        private void AttackBehavior()
        {
            transform.LookAt(_target.transform);
            if (timeSinceLastAttack > attackSpeed)
            {
                _animator.ResetTrigger("cancelAttack");
                _animator.SetTrigger("attack");
                timeSinceLastAttack = 0f;
            }
        }

        //Attack Animation Event
        private void Hit()
        {
            if (_target == null) return;
            
            _target.TakeDamage(weaponDamage);
        }

    }
}
