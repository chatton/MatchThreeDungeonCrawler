using System.Collections.Generic;
using System.Linq;
using MatchThree.Gems;
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

        public override bool keepWaiting => _gems.Any(gem => !gem.HasReachedTargetPosition);
    }
}