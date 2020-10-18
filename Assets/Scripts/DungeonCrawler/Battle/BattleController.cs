using System.Collections;
using Core.Util;
using DungeonCrawler.Enemies;
using DungeonCrawler.Enemies.Actions;
using DungeonCrawler.PlayerCharacter;
using MatchThree.Board;
using UnityEngine;

namespace DungeonCrawler.Battle
{
    public class BattleController : Singleton<BattleController>
    {
        private Enemy[] _allEnemies;
        private bool _battleIsOver;

        public bool IsPlayerTurn { get; private set; }

        public void EndPlayerTurn()
        {
            IsPlayerTurn = false;
        }

        private IEnumerator Start()
        {
            _allEnemies = FindObjectsOfType<Enemy>();
            yield return BeginBattle();
        }

        private IEnumerator BeginBattle()
        {
            Debug.Log("Beginning Battle!");
            while (!_battleIsOver)
            {
                BeginEnemyTurn();
                SelectEnemyActions();
                yield return TakePlayerTurn();
                yield return new WaitUntil(() => !GameBoard.Instance.MatchInProgress);
                yield return TakeEnemyTurn();
            }

            Debug.Log("Ending Battle!");
        }

        private IEnumerator TakeEnemyTurn()
        {
            foreach (Enemy enemy in _allEnemies)
            {
                if (enemy != null)
                {
                    yield return enemy.PerformAction(Player.Instance);
                    yield return new WaitForSeconds(1f);
                }
            }

            yield return null;
        }

        private IEnumerator TakePlayerTurn()
        {
            Debug.Log("Beginning Player Turn");
            IsPlayerTurn = true;
            Player.Instance.OnBeginTurn();
            while (IsPlayerTurn)
            {
                yield return null;
            }

            Debug.Log("Ending Player Turn");
        }

        private void BeginEnemyTurn()
        {
            foreach (Enemy enemy in _allEnemies)
            {
                if (enemy != null)
                {
                    enemy.OnTurnBegin();
                }
            }
        }

        private void SelectEnemyActions()
        {
            foreach (Enemy enemy in _allEnemies)
            {
                if (enemy != null)
                {
                    EnemyAction action = enemy.GetActionThisTurn();
                    enemy.SetAction(action);
                }
            }
        }
    }
}