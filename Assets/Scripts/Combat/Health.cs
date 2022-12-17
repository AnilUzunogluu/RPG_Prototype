using Unity.Mathematics;
using UnityEngine;

namespace RPG.Combat
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private float baseHealth = 20f;

        private float _currentHealth;

        private void Start()
        {
            _currentHealth = baseHealth;
        }

        public void TakeDamage(float damage)
        {
            _currentHealth = Mathf.Max(_currentHealth - damage, 0f);
            
            Debug.Log($"{gameObject.name} + {_currentHealth}");
        }
    }

}