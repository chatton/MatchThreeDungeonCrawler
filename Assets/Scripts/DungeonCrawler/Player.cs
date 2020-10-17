using System;
using HealthSystem;
using UnityEngine;

namespace DungeonCrawler
{
    public class Player : MonoBehaviour
    {
        public event Action<Player> OnPlayerStatsChanged;
        public event Action<Player, Enemy> OnAttack;

        public int Defence => _defence.CurrentDefence;
        public Enemy SelectedEnemy { get; set; }

        private Health _health;
        private Defence _defence;

        private void Awake()
        {
            _defence = GetComponent<Defence>();
            _health = GetComponent<Health>();
        }


        public void AddDefence(int defenceAmount)
        {
            _defence.AddDefence(defenceAmount);
            OnPlayerStatsChanged?.Invoke(this);
        }
    }
}