using System;
using UnityEngine;

namespace DungeonCrawler
{
    public class Player : MonoBehaviour
    {
        public event Action<Player> OnPlayerStatsChanged;

        public int Defence => _currentDefence;

        private int _currentDefence;


        public void AddDefence(int defenceAmount)
        {
            _currentDefence += defenceAmount;
            OnPlayerStatsChanged?.Invoke(this);
        }
    }
}