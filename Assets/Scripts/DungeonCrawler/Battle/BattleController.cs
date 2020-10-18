using System.Collections;
using Core.Util;
using DungeonCrawler.Enemies;
using DungeonCrawler.PlayerCharacter;
using MatchThree.Board;
using UnityEngine;

namespace DungeonCrawler.Battle
{
    public class BattleController : Singleton<BattleController>
    {
        private Enemy[] allEnemies;
        private bool battleIsOver;

        public bool IsPlayerTurn { get; private set; }

        public void EndPlayerTurn()
        {
            IsPlayerTurn = false;
        }

        private IEnumerator Start()
        {
            allEnemies = FindObjectsOfType<Enemy>();
            yield return BeginBattle();
        }

        private IEnumerator BeginBattle()
        {
            Debug.Log("Beginning Battle!");
            while (!battleIsOver)
            {
                yield return SelectEnemyActions();
                yield return TakePlayerTurn();
                yield return new WaitUntil(() => !GameBoard.Instance.MatchInProgress);
                yield return TakeEnemyTurn();
            }

            Debug.Log("Ending Battle!");
        }

        private IEnumerator TakeEnemyTurn()
        {
            foreach (Enemy enemy in allEnemies)
            {
                if (enemy != null)
                {
                    Debug.Log($"Enemy: {enemy.name} performing action!");
                    yield return enemy.GetActionThisTurn().Use(Player.Instance, enemy);
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

        private IEnumerator SelectEnemyActions()
        {
            yield return null;
        }
    }
}