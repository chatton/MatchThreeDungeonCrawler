using System;

namespace DungeonCrawler.PlayerCharacter
{
    public class Stamina
    {
        public event Action OnStaminaDepleted;

        private int _maxStamina;

        public Stamina(int numberOfActions)
        {
            _maxStamina = numberOfActions;
            CurrentStamina = numberOfActions;
        }

        public int CurrentStamina { get; private set; }

        public bool CanTakeAction(int staminaCost = 1)
        {
            return CurrentStamina >= staminaCost;
        }

        public void DepleteStamina(int amount = 1)
        {
            CurrentStamina -= amount;
            if (CurrentStamina <= 0)
            {
                OnStaminaDepleted?.Invoke();
            }
        }

        public void ReplenishStamina()
        {
            CurrentStamina = _maxStamina;
        }
    }
}