using System.Collections.Generic;
using System.Linq;
using MatchThree;
using UnityEngine;

namespace Util
{
    public class WaitForGemsToReachDestination : CustomYieldInstruction
    {
        private readonly IEnumerable<Gem> _gems;

        public WaitForGemsToReachDestination(IEnumerable<Gem> gems)
        {
            _gems = gems;
        }

        public WaitForGemsToReachDestination(Gem gem)
        {
            _gems = new List<Gem> {gem};
        }

        public override bool keepWaiting => _gems.Any(gem => !gem.HasReachedTargetPosition);
    }
}