using System.Collections;
using UnityEngine;
using RPG.Movement;

namespace  RPG.Combat
{
    public class Fighter : MonoBehaviour
    {
        [SerializeField] private float weaponRange = 2f;
        [SerializeField] private float attackSpeed = 1f;
        [SerializeField] private float weaponDamage = 5f;

        private Transform _target;
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
            
            if (_target == null) return;

            var isInAttackRange = Vector3.Distance(transform.position, _target.position) <= weaponRange;
            
            if (isInAttackRange)
            {
                _mover.Stop();
                AttackBehavior();
            }
            else
            {
                _mover.Move(_target.position);
            }
        }

        public void Attack(CombatTarget combatTarget)
        {
            _target = combatTarget.transform;
        }

        public void CancelAttack()
        {
            _target = null;
        }

        private void AttackBehavior()
        {
            if (timeSinceLastAttack > attackSpeed)
            {
                _animator.SetTrigger("attack");
                timeSinceLastAttack = 0f;
            }
        }

        //Attack Animation Event
        private void Hit()
        {
            _target.GetComponent<Health>().TakeDamage(weaponDamage);
        }

    }
}
