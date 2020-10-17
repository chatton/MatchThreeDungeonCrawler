using System;
using UnityEngine;

namespace DungeonCrawler
{
    public class Defence : MonoBehaviour
    {
        public int CurrentDefence { get; private set; }

        public event Action<Defence> OnDefenceChanged;

        public void AddDefence(int defenceAmount)
        {
            CurrentDefence += defenceAmount;
            OnDefenceChanged?.Invoke(this);
        }
    }
}