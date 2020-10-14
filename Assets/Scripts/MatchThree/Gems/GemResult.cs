using DungeonCrawler;

namespace MatchThree.Gems
{
    public class GemResult
    {
        public bool ActionTaken { get; set; }
        public Player Player { get; set; }
        public GemEffectType GemEffectType { get; set; }
        public int PlayerDefenceModification { get; set; }
        public Enemy Enemy { get; set; }
        public int DamageAmount { get; set; }
    }
}