using HealthSystem;
using TMPro;
using UnityEngine;

namespace DungeonCrawler.UI
{
    public class HealthBarUI : MonoBehaviour
    {
        [SerializeField] private Transform healthBar;
        [SerializeField] private TextMeshPro healthText;

        private void Start()
        {
            Health health = GetComponentInParent<Health>();
            health.OnDamaged += UpdateHealth;
            health.OnHealed += UpdateHealth;
            health.OnDeath += UpdateHealth;
            UpdateHealth(health);
        }

        private void UpdateHealth(Health health)
        {
            healthBar.localScale = new Vector3((float) health.CurrentHp / health.MaxHp, 1, 1);
            healthText.text = $"{health.CurrentHp}/{health.MaxHp}";
        }
    }
}