using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Util;

namespace MatchThree
{
    public class GameBoard : MonoBehaviour
    {
        [SerializeField] private int width = 5;
        [SerializeField] private int height = 5;
        [SerializeField] private int matchNumber = 3;


        private static readonly List<Gem> NoMatches = new List<Gem>();

        // the current gem that is selected
        private Gem CurrentlySelectedGem { get; set; }

        private GemSource _gemSource;
        // private Gem[,] _gems;

        // private Dictionary<(int, int), Gem> _gemDictionary;
        // private Dictionary<Gem, (int, int)> _gemCoordDictionary;

        private MultiMap<Gem, (int, int)> _gemDict;

        private void Awake()
        {
            _gemSource = GetComponent<GemSource>();
            // _gems = new Gem[width, height];
            // _gemDictionary = new Dictionary<(int, int), Gem>();
            // _gemCoordDictionary = new Dictionary<Gem, (int, int)>();

            _gemDict = new MultiMap<Gem, (int, int)>();
        }


        private void Start()
        {
            CreateBoard();
        }


        private void CreateBoard()
        {
            // generate the board relative the position of the containing object
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Gem gem = _gemSource.GetNextGem(this);
                    gem.transform.parent = transform;
                    gem.transform.localPosition =
                        new Vector3(i * gem.transform.localScale.x, j * gem.transform.localScale.y, 0);
                    gem.name = $"gem_{i}_{j}";
                    _gemDict.Add(gem, (i, j));
                }
            }
        }

        public void SelectGem(Gem gem)
        {
            if (CurrentlySelectedGem == null)
            {
                CurrentlySelectedGem = gem;
                Debug.Log($"Set gem ${gem.name} as CurrentlySelectedGem");
                return;
            }

            if (CanSwapGems(CurrentlySelectedGem, gem))
            {
                SwapGems(CurrentlySelectedGem, gem);
                List<Gem> currentlySelectedGemMatches = CheckForMatches(CurrentlySelectedGem);
                List<Gem> matches = CheckForMatches(gem);
                MatchGems(currentlySelectedGemMatches.Union(matches).ToList());
                // CollapseColumns();
                CurrentlySelectedGem = null;
            }
            else
            {
                // animation to show gems can't perform swap
                CurrentlySelectedGem = null;
            }
        }


        // private bool ColumnIsFull(int columnIndex)
        // {
        // for (int i = 0; i < height; i++)
        // {
        // if (_gems[i, columnIndex] == null)
        // {
        // return false;
        // }
        // }

        // return true;
        // }


        // private void ShiftDown(int columnIndex)
        // {
        // for (int i = 0; i < height - 1; i++)
        // {
        // while (_gems[i, columnIndex] == null)
        // {
        // SwapGems(_gems[i, columnIndex], _gems[i + 1, columnIndex]);
        // _gems[i, columnIndex] = _gems[i + 1, columnIndex];
        // }

        // we have shifted down all of the gems, only nulls above
        // these will be filled in later!
        // if (NoGemsAbove(i, columnIndex))
        // {
        // return;
        // }
        // }
        // }

        // private bool NoGemsAbove(int row, int col)
        // {
        // for (int i = row + 1; i < height; i++)
        // {
        // if (_gems[row, col] != null)
        // {
        // return false;
        // }
        // }

        // return true;
        // }

        // private void CollapseColumn(int columnIndex)
        // {
        // while (!ColumnIsFull(columnIndex))
        // {
        // ShiftDown(columnIndex);
        // }
        // }

        // private void CollapseColumns()
        // {
        // for (int i = 0; i < width; i++)
        // {
        // Debug.Log($"Collapsing column {i}");
        // CollapseColumn(i);
        // }
        // }

        private bool SwappingGemsWouldResultInMatch(Gem gem0, Gem gem1)
        {
            // update board to see if there is a match after this move
            SwapGems(gem0, gem1);
            List<Gem> gem0Matches = CheckForMatches(gem0);
            List<Gem> gem1Matches = CheckForMatches(gem1);

            // reset the board to previous state
            SwapGems(gem0, gem1);

            // if there were any valid matches, this move was valid
            return gem0Matches.Union(gem1Matches).Distinct().Any();
        }

        private void MatchGems(List<Gem> matchedGems)
        {
            foreach (Gem gem in matchedGems)
            {
                if (gem != null)
                {
                    (int, int) gemCoords = _gemDict.Get(gem);
                    _gemDict.Add(gem, (gemCoords.Item1, gemCoords.Item2));
                    Destroy(gem.gameObject);
                }
            }
        }


        private bool GemsAreNextToOneAnother(Gem gem0, Gem gem1)
        {
            (int, int) gem0Coords = _gemDict.Get(gem0);
            (int, int) gem1Coords = _gemDict.Get(gem1);
            int xDiff = Mathf.Abs(gem0Coords.Item1 - gem1Coords.Item1);
            int yDiff = Mathf.Abs(gem0Coords.Item2 - gem1Coords.Item2);
            return (xDiff == 1 && yDiff == 0) || (xDiff == 0 && yDiff == 1);
        }

        // determine if swapping two given gems is a valid move
        private bool CanSwapGems(Gem gem0, Gem gem1)
        {
            if (gem0 == null || gem1 == null)
            {
                Debug.Log("Attempted to match a null gem!");
                return false;
            }

            if (gem0 == gem1)
            {
                Debug.Log("Attempted to match gem with itself!");
                return false;
            }

            if (!GemsAreNextToOneAnother(gem0, gem1))
            {
                Debug.Log("Attempted to match gems that were not next to each other!");
                return false;
            }

            if (!SwappingGemsWouldResultInMatch(gem0, gem1))
            {
                Debug.Log("Attempted to match gems that would not have resulted in a match!");
                return false;
            }

            return true;
        }
        //
        // private void SwapVisualPositions(Gem gem0, Gem gem1)
        // {
        //     Vector3 pos0 = gem0.transform.localPosition;
        //     Vector3 pos1 = gem1.transform.localPosition;
        //
        //     // update the in world positions of the game objects
        //     gem0.transform.localPosition = pos1;
        //     gem1.transform.localPosition = pos0;
        // }

        // Swap Gems swaps the two provided gems, validation is expected to
        // have been done before hand
        // private void SwapGems(Gem gem0, Gem gem1)
        // {
        // Vector3 pos0 = gem0.transform.localPosition;
        // Vector3 pos1 = gem1.transform.localPosition;

        // update the in world positions of the game objects
        // gem0.transform.localPosition = pos1;
        // gem1.transform.localPosition = pos0;

        // (int, int) gem0Coords = GetGemCoordinates(gem0);
        // (int, int) gem1Coords = GetGemCoordinates(gem1);

        // string tempName = gem1.name;
        // gem1.name = gem0.name;
        // gem0.name = tempName;

        // update the backing array of the new positions
        // _gems[gem0Coords.Item1, gem0Coords.Item2] = gem1;
        // _gems[gem1Coords.Item1, gem1Coords.Item2] = gem0;
        // }


        // private Gem GetGem((int, int) coords)
        // {
        // return _gemDictionary[coords];
        // }

        private void SwapGems(Gem gem0, Gem gem1)
        {
            SwapGems(_gemDict.Get(gem0), _gemDict.Get(gem1));
        }

        private void SwapGems((int, int) gem0Coords, (int, int) gem1Coords)
        {
            Gem gem0 = _gemDict.Get(gem0Coords);
            Gem gem1 = _gemDict.Get(gem1Coords);


            if (gem0 != null)
            {
                // Update visuals
            }

            if (gem1 != null)
            {
                // Update visuals
            }

            // Vector3 pos0 = gem0.transform.localPosition;
            // Vector3 pos1 = gem1.transform.localPosition;

            // update the in world positions of the game objects
            // gem0.transform.localPosition = pos1;
            // gem1.transform.localPosition = pos0;

            _gemDict.Add(gem0, gem1Coords);
            _gemDict.Add(gem1, gem0Coords);
        }


        private List<Gem> CheckForMatches(Gem gem)
        {
            List<Gem> horizontalMatches = CheckForHorizontalMatch(gem);
            horizontalMatches.AddRange(CheckForVerticalMatch(gem));
            return horizontalMatches.Distinct().ToList();
        }

        private List<Gem> CheckForVerticalMatch(Gem gem)
        {
            (int, int) gemCoords = _gemDict.Get(gem);

            HashSet<Gem> matchedGems = new HashSet<Gem>();

            int currentY = gemCoords.Item2;
            while (currentY < height)
            {
                Gem currentGem = _gemDict.Get((gemCoords.Item1, currentY));
                if (currentGem == null)
                {
                    break;
                }

                if (currentGem.MatchType != gem.MatchType)
                {
                    break;
                }

                matchedGems.Add(currentGem);
                currentY++;
            }


            currentY = gemCoords.Item2;
            while (currentY >= 0)
            {
                Gem currentGem = _gemDict.Get((gemCoords.Item1, currentY));
                if (currentGem == null)
                {
                    break;
                }

                if (currentGem.MatchType != gem.MatchType)
                {
                    break;
                }

                matchedGems.Add(currentGem);
                currentY--;
            }


            if (matchedGems.Count >= matchNumber)
            {
                return matchedGems.ToList();
            }

            return NoMatches;
        }


        private List<Gem> CheckForHorizontalMatch(Gem gem)
        {
            (int, int) gemCoords = _gemDict.Get(gem);

            HashSet<Gem> matchedGems = new HashSet<Gem>();

            int currentX = gemCoords.Item1;
            while (currentX < width)
            {
                Gem currentGem = _gemDict.Get((currentX, gemCoords.Item2));
                if (currentGem == null)
                {
                    break;
                }

                if (currentGem.MatchType != gem.MatchType)
                {
                    break;
                }

                matchedGems.Add(currentGem);
                currentX++;
            }


            currentX = gemCoords.Item1;
            while (currentX >= 0)
            {
                Gem currentGem = _gemDict.Get((currentX, gemCoords.Item2));
                if (currentGem == null)
                {
                    break;
                }

                if (currentGem.MatchType != gem.MatchType)
                {
                    break;
                }

                matchedGems.Add(currentGem);
                currentX--;
            }


            if (matchedGems.Count >= matchNumber)
            {
                return matchedGems.ToList();
            }

            return NoMatches;
        }
    }
}