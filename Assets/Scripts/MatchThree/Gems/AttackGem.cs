using System.Collections.Generic;
using UnityEngine;

namespace MatchThree.Gems
{
    [CreateAssetMenu(menuName = "GemActions/AttackAction")]
    public class AttackGem : GemAction
    {
        public int attackAmount = 5;

        public override void OnMatch(Dictionary<GemEffectType, GemResult> gemMatchResults)
        {
            GemResult result = gemMatchResults[GemEffectType.Attack];
            result.GemEffectType = GemEffectType.Attack;
            result.DamageAmount = attackAmount;
            result.ActionTaken = true;
        }
    }
}