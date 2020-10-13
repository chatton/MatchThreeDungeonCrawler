using System;
using UnityEngine;

namespace DungeonCrawler
{
    public class Player : MonoBehaviour
    {
        public event Action<Player> OnPlayerStatsChanged;
        public event Action<Player, Enemy> OnAttack;

        public int Defence => _currentDefence;

        private int _currentDefence;


        public void AddDefence(int defenceAmount)
        {
            _currentDefence += defenceAmount;
            OnPlayerStatsChanged?.Invoke(this);
        }

        public void Attack(Enemy enemy)
        {
            OnAttack?.Invoke(this, enemy);
            
        }
    }
}