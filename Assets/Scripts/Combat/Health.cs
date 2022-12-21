using UnityEngine;

namespace RPG.Combat
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private float baseHealth = 20f;

        private float _currentHealth;
        private Animator _animator;

        public bool IsDead { get; private set; }

        private void Start()
        {
            _currentHealth = baseHealth;
            _animator = GetComponent<Animator>();
        }

        public void TakeDamage(float damage)
        {
            _currentHealth = Mathf.Max(_currentHealth - damage, 0f);
            if (_currentHealth == 0 && !IsDead)
            {
                HandleDeath();
            }
            
            Debug.Log($"{gameObject.name} + {_currentHealth}");
        }

        private void HandleDeath()
        {
            IsDead = true;
            _animator.SetTrigger("die");
        }
    }

}