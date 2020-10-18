using System.Collections.Generic;
using DungeonCrawler.Enemies.Actions;
using DungeonCrawler.PlayerCharacter;
using DungeonCrawler.UI;
using UnityEngine;

namespace DungeonCrawler.Enemies
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private Transform selectionArrow;
        [SerializeField] private List<EnemyAction> allActions;

        private Stats _stats;

        private void Awake()
        {
            _stats = GetComponent<Stats>();
        }

        public void SelectEnemy()
        {
            EnemySelectionIcon.DisplayIcon(selectionArrow.position);
        }

        private void OnMouseDown()
        {
            Player.Instance.SelectedEnemy = this;
            SelectEnemy();
        }

        public EnemyAction GetActionThisTurn()
        {
            return allActions[0];
        }
    }
}