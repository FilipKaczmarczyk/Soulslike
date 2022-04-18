using Player;
using UnityEngine;

public class PlayerStatistics : MonoBehaviour
{
    [SerializeField] private int healthLevel;
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private AnimatorHandler animatorHandler;
    
    private int _maxHealth;
    private int _currentHealth;
    
    private void Start()
    {
        _currentHealth = SetMaxHealthFromHealthLevel();
        healthBar.SetMaxHealth(_currentHealth);
    }

    private int SetMaxHealthFromHealthLevel()
    {
        _maxHealth = healthLevel * 10;
        return _maxHealth;
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        
        healthBar.SetCurrentHealth(_currentHealth);
        
        animatorHandler.PlayTargetAnimation("TakeDamage01", true);

        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            animatorHandler.PlayTargetAnimation("Death01", false);
        }
    }
}
