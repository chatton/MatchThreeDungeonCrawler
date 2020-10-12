using UnityEngine;

namespace MatchThree
{
    public class Gem : MonoBehaviour
    {
        [SerializeField] private MatchType matchType;

        public MatchType MatchType => matchType;
        internal GameBoard GameBoard { get; set; }

        public void OnMouseDown()
        {
            GameBoard.SelectGem(this);
        }
    }
}