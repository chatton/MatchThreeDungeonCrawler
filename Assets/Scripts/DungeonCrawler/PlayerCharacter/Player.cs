using System;
using System.Collections.Generic;
using Core.Util;
using DungeonCrawler.UI;
using HealthSystem;
using UnityEngine;

namespace DungeonCrawler.PlayerCharacter
{
    public class Player : Singleton<Player>
    {
        [SerializeField] private int startingStamina = 3;
        [SerializeField] private StaminaSphere staminaSpherePrefab;
        [SerializeField] private Transform staminaSphereStartPoint;

        private List<StaminaSphere> _staminaSpheres;


        public event Action<Player> OnPlayerStatsChanged;
        public event Action<Player, Enemy> OnAttack;

        public int Defence => _defence.CurrentDefence;
        public Enemy SelectedEnemy { get; set; }


        // Components
        private Health _health;
        private Defence _defence;

        private Stamina _stamina;

        private void Awake()
        {
            _defence = GetComponent<Defence>();
            _health = GetComponent<Health>();
            _stamina = new Stamina(startingStamina);
            _staminaSpheres = BuildStaminaSpheres();

            Enemy e = FindObjectOfType<Enemy>();
            SelectedEnemy = e;
            if (e != null)
            {
                e.SelectEnemy();
            }
        }


        private List<StaminaSphere> BuildStaminaSpheres()
        {
            List<StaminaSphere> spheres = new List<StaminaSphere>();
            for (int i = 0; i < startingStamina; i++)
            {
                StaminaSphere s = Instantiate(staminaSpherePrefab,
                    staminaSphereStartPoint.position + new Vector3(i * 0.5f, 0, 0),
                    Quaternion.identity);
                spheres.Add(s);
            }

            return spheres;
        }

        public void AddDefence(int defenceAmount)
        {
            _defence.AddDefence(defenceAmount);
            OnPlayerStatsChanged?.Invoke(this);
        }

        public bool CanTakeAction()
        {
            return _stamina.CanTakeAction();
        }

        public void DepleteStamina()
        {
            _stamina.DepleteStamina();
            for (int i = 0; i < _staminaSpheres.Count; i++)
            {
                StaminaSphere staminaSphere = _staminaSpheres[i];
                if (i < _stamina.CurrentStamina)
                {
                    staminaSphere.Replenish();
                }
                else
                {
                    staminaSphere.Deplete();
                }
            }
        }
    }
}