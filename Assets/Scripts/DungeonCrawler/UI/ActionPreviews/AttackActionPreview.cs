using DungeonCrawler.Enemies;
using DungeonCrawler.Enemies.Actions;
using TMPro;
using UnityEngine;

namespace DungeonCrawler.UI.ActionPreviews
{
    public class AttackActionPreview : ActionPreview
    {
        [SerializeField] private TextMeshPro text;

        public override void Init(EnemyAction action, Enemy enemy)
        {
            AttackAction attackAction = (AttackAction) action;
            Stats enemyStats = enemy.GetComponent<Stats>();
            int totalDamageAmount = enemyStats.attackPower + attackAction.damageAmount;
            text.text = totalDamageAmount.ToString();
        }
    }
}