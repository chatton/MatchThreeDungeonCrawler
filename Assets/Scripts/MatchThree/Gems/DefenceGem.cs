using System.Collections.Generic;
using UnityEngine;

namespace MatchThree.Gems
{
    [CreateAssetMenu(menuName = "GemActions/DefenceAction")]
    public class DefenceGem : GemAction
    {
        public int defenceAmount = 5;

        public override void OnMatch(Dictionary<GemEffectType, GemResult> gemMatchResults)
        {
            GemResult result = gemMatchResults[GemEffectType.Defend];
            result.GemEffectType = GemEffectType.Defend;
            result.PlayerDefenceModification += defenceAmount;
            result.ActionTaken = true;
        }
    }
}