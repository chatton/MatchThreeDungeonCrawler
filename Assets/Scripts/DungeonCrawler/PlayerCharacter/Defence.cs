using System;
using UnityEngine;

namespace DungeonCrawler.PlayerCharacter
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

        public void RemoveDefence(int defenceAmount)
        {
            CurrentDefence -= defenceAmount;
            OnDefenceChanged?.Invoke(this);
        }

        public void ResetDefence()
        {
            CurrentDefence = 0;
            OnDefenceChanged?.Invoke(this);
        }
    }
}