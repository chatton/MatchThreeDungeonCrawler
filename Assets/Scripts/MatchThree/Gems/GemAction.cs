using System;
using System.Collections;
using System.Collections.Generic;
using DungeonCrawler;
using UnityEngine;

namespace MatchThree.Gems
{
    public abstract class GemAction : ScriptableObject
    {
        public GemEffectType gemEffectType;
        private static readonly int Attack1 = Animator.StringToHash("Attack1");
        private static readonly int Hurt = Animator.StringToHash("Hurt");

        public abstract void OnMatch(Dictionary<GemEffectType, GemResult> gemMatchResults);

        public static IEnumerator PerformActions(Dictionary<GemEffectType, GemResult> gemMatchResults)
        {
            foreach (GemEffectType gemEffectType in gemMatchResults.Keys)
            {
                GemResult result = gemMatchResults[gemEffectType];
                if (!result.ActionTaken)
                {
                    continue;
                }

                switch (gemEffectType)
                {
                    case GemEffectType.Defend:
                        yield return HandleDefendAction(result);
                        break;
                    case GemEffectType.Attack:
                        yield return HandleAttackAction(result);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }


            yield return null;
        }

        private static IEnumerator HandleAttackAction(GemResult result)
        {
            Debug.Log("ATTACK");
            Animator playerAnimator = result.Player.GetComponentInChildren<Animator>();
            playerAnimator.SetTrigger(Attack1);

            yield return new WaitForSeconds(0.25f);

            Animator enemyAnimator = result.Enemy.GetComponentInChildren<Animator>();
            enemyAnimator.SetTrigger(Hurt);
            yield return null;
        }

        private static IEnumerator HandleDefendAction(GemResult result)
        {
            Debug.Log("DEFEND");
            result.Player.AddDefence(result.PlayerDefenceModification);
            yield return null;
        }
    }
}