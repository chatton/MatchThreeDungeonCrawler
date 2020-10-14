using System;
using HealthSystem;
using UnityEngine;

namespace DungeonCrawler
{
    public class Player : MonoBehaviour
    {
        public event Action<Player> OnPlayerStatsChanged;
        public event Action<Player, Enemy> OnAttack;

        public int Defence => _currentDefence;

        private int _currentDefence;

        private Health _health;


        private void Awake()
        {
            _health = GetComponent<Health>();
        }


        public void AddDefence(int defenceAmount)
        {
            _currentDefence += defenceAmount;
            OnPlayerStatsChanged?.Invoke(this);
        }

        public void Attack(Enemy enemy)
        {
            OnAttack?.Invoke(this, enemy);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                _health.Damage(10);
            }
        }
    }
}