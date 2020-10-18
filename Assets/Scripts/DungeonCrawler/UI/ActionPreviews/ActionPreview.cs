using DungeonCrawler.Enemies;
using DungeonCrawler.Enemies.Actions;
using UnityEngine;

namespace DungeonCrawler.UI.ActionPreviews
{
    public abstract class ActionPreview : MonoBehaviour
    {
        public abstract void Init(EnemyAction action, Enemy enemy);
    }
}