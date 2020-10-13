using DungeonCrawler;
using UnityEngine;

namespace MatchThree.Gems
{
    [CreateAssetMenu(menuName = "GemActions/DefenceAction")]
    public class DefenceGem : GemAction
    {
        public int defenceAmount = 5;

        public override void OnMatch(Player player)
        {
            player.AddDefence(defenceAmount);
        }
    }
}