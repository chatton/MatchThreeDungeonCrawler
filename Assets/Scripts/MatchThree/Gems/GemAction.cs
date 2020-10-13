using DungeonCrawler;
using UnityEngine;

namespace MatchThree.Gems
{
    public abstract class GemAction : ScriptableObject
    {
        public abstract void OnMatch(Player player);
    }
}