using System.Collections;
using DungeonCrawler.Animations;
using DungeonCrawler.PlayerCharacter;
using UnityEngine;

namespace DungeonCrawler.Enemies.Actions
{
    [CreateAssetMenu(fileName = "New Attack Action", menuName = "Enemies/Actions/New Attack Action")]
    public class AttackAction : EnemyAction
    {
        public int damageAmount = 10;


        public override IEnumerator Use(Player player, Enemy enemy)
        {
            Stats enemyStats = enemy.GetComponent<Stats>();
            int totalDamageAmount = enemyStats.attackPower + damageAmount;

            EnemyAnimationController eac = enemy.GetComponent<EnemyAnimationController>();
            eac.Attack();

            yield return new WaitForSeconds(1f);

            player.Damage(totalDamageAmount);
            yield return null;
        }
    }
}