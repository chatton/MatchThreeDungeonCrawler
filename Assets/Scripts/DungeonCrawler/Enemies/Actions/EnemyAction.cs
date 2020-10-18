using System.Collections;
using DungeonCrawler.PlayerCharacter;
using DungeonCrawler.UI.ActionPreviews;
using UnityEngine;

namespace DungeonCrawler.Enemies.Actions
{
    public abstract class EnemyAction : ScriptableObject
    {
        [Tooltip("The game object that appears above the enemy the turn before they will use the action")]
        public ActionPreview actionPreviewPrefab;

        public abstract IEnumerator Use(Player player, Enemy enemy);
    }
}