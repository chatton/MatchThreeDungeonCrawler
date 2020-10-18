using System.Collections;
using DungeonCrawler.PlayerCharacter;
using UnityEngine;

namespace DungeonCrawler.Enemies.Actions
{
    public abstract class EnemyAction : ScriptableObject
    {
        [Tooltip("The icon that appears above the enemy the turn before they will use the action")]
        public Sprite actionPreview;

        public abstract IEnumerator Use(Player player, Enemy enemy);
    }
}