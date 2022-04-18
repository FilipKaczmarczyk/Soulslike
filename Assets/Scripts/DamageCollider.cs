using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    [SerializeField] private int currentWeaponDamage = 25;
    
    private Collider _damageCollider;
    
    private void Awake()
    {
        _damageCollider = GetComponent<Collider>();
        _damageCollider.gameObject.SetActive(true);
        _damageCollider.isTrigger = true;
        _damageCollider.enabled = false;
    }

    public void ToggleDamageCollider(bool state)
    {
        _damageCollider.enabled = state;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<EnemyStatistics>() != null)
        {
            var enemy = other.GetComponent<EnemyStatistics>();

            enemy.TakeDamage(currentWeaponDamage);
        }
        
        if (other.GetComponent<PlayerStatistics>() != null)
        {
            var enemy = other.GetComponent<PlayerStatistics>();

            enemy.TakeDamage(currentWeaponDamage);
        }
    }
}
