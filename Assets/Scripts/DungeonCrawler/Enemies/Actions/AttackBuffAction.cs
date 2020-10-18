using System.Collections;
using DungeonCrawler.PlayerCharacter;
using UnityEngine;

namespace DungeonCrawler.Enemies.Actions
{
    [CreateAssetMenu(fileName = "New Attack Buff Action", menuName = "Enemies/Actions/New Attack Buff Action")]
    public class AttackBuffAction : EnemyAction
    {
        public int attackPower = 5;
        public int turnDuration = 3;


        private int _counter;
        private Enemy _enemy;


        public override IEnumerator Use(Player player, Enemy enemy)
        {
            _enemy = enemy;
            _counter = 0;
            
            Stats stats = enemy.GetComponent<Stats>();
            stats.attackPower += attackPower;


            enemy.OnTurnBeginEvent += DebuffAfterNumberOfTurns;

            yield return null;
        }

        private void DebuffAfterNumberOfTurns()
        {
            _counter++;
            if (_counter < turnDuration)
            {
                return;
            }

            _enemy.GetComponent<Stats>().attackPower -= attackPower;
            _enemy.OnTurnBeginEvent -= DebuffAfterNumberOfTurns;
        }
    }
}