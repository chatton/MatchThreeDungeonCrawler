using System;
using System.Collections.Generic;
using Core.Util;
using DungeonCrawler.Animations;
using DungeonCrawler.Battle;
using DungeonCrawler.Enemies;
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
        public event Action<Stamina> OnStaminaChanged;

        public event Action<Player> OnPlayerStatsChanged;

        public int Defence => _defence.CurrentDefence;
        public Enemy SelectedEnemy { get; set; }


        // Components
        private Health _health;
        private Defence _defence;
        private Stats _stats;
        private PlayerAnimationController _playerAnimationController;

        private Stamina _stamina;

        private void Awake()
        {
            _stats = GetComponent<Stats>();
            _defence = GetComponent<Defence>();
            _health = GetComponent<Health>();
            _playerAnimationController = GetComponent<PlayerAnimationController>();
            _stamina = new Stamina(startingStamina);
            _stamina.OnStaminaDepleted += BattleController.Instance.EndPlayerTurn;
            _staminaSpheres = BuildStaminaSpheres();
        }

        private void Start()
        {
            SelectFirstEnemy();
        }

        private void SelectFirstEnemy()
        {
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
            OnStaminaChanged?.Invoke(_stamina);
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

        public void OnBeginTurn()
        {
            _stamina.ReplenishStamina();
            OnStaminaChanged?.Invoke(_stamina);
            foreach (StaminaSphere staminaSphere in _staminaSpheres)
            {
                staminaSphere.Replenish();
            }

            _defence.ResetDefence();
        }

        public void Damage(int totalDamageAmount)
        {
            _playerAnimationController.ReceiveDamage(totalDamageAmount);


            int damageAfterDefence = totalDamageAmount - Defence;

            // damage done to health
            if (damageAfterDefence > 0)
            {
                // we've lost all defence
                _defence.ResetDefence();
                // health takes the remainder of the damage
                _health.Damage(damageAfterDefence);
            }
            else
            {
                // we blocked all damage
                _defence.RemoveDefence(totalDamageAmount);
            }

            OnPlayerStatsChanged?.Invoke(this);
        }
    }
}