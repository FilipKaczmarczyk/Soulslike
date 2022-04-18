using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    [SerializeField] private int damage = 25;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerStatistics>() == null) return;

        var player = other.GetComponent<PlayerStatistics>();
        player.TakeDamage(damage);
    }
}
