using System.Collections;
using DungeonCrawler.PlayerCharacter;
using UnityEngine;

namespace DungeonCrawler.Enemies.Actions
{
    public abstract class EnemyAction : ScriptableObject
    {
        [Tooltip("The game object that appears above the enemy the turn before they will use the action")]
        public GameObject actionPreviewPrefab;

        public abstract IEnumerator Use(Player player, Enemy enemy);
    }
}