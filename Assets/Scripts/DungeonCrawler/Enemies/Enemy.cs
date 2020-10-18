using System;
using System.Collections;
using System.Collections.Generic;
using DungeonCrawler.Enemies.Actions;
using DungeonCrawler.PlayerCharacter;
using DungeonCrawler.UI;
using DungeonCrawler.UI.ActionPreviews;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DungeonCrawler.Enemies
{
    public class Enemy : MonoBehaviour
    {
        public event Action OnTurnBeginEvent;

        [SerializeField] private Transform selectionArrow;

        [SerializeField] private Transform attackPreviewLocation;
        [SerializeField] private List<EnemyAction> allActions;


        private ActionPreview _lastAttackPreview;
        private Stats _stats;

        private EnemyAction CurrentAction { get; set; }

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

        public void SetAction(EnemyAction action)
        {
            if (action != null)
            {
                CurrentAction = action;
                if (_lastAttackPreview != null)
                {
                    Destroy(_lastAttackPreview.gameObject);
                }

                _lastAttackPreview = Instantiate(action.actionPreviewPrefab, attackPreviewLocation.transform);
                _lastAttackPreview.Init(CurrentAction, this);
            }
        }

        public EnemyAction GetActionThisTurn()
        {
            EnemyAction action = allActions[Random.Range(0, allActions.Count)];
            return action;
        }

        public IEnumerator PerformAction(Player player)
        {
            yield return CurrentAction.Use(player, this);
        }

        public void OnTurnBegin()
        {
            OnTurnBeginEvent?.Invoke();
        }
    }
}