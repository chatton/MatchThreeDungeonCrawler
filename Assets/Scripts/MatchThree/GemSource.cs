using System.Collections.Generic;
using UnityEngine;

namespace MatchThree
{
    public class GemSource : MonoBehaviour
    {
        [SerializeField] private List<Gem> gemPrefabs;

        public Gem GetNextGem(GameBoard gameBoard)
        {
            Gem gem = Instantiate(gemPrefabs[Random.Range(0, gemPrefabs.Count)]);
            gem.GameBoard = gameBoard;
            return gem;
        }
    }
}