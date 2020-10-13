using System.Collections;
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
        private MultiMap<Gem, (int, int)> _gemDict;

        private void Awake()
        {
            _gemSource = GetComponent<GemSource>();
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
                    Gem gem = _gemSource.GetNextGem();
                    gem.transform.parent = transform;
                    gem.transform.localPosition =
                        new Vector3(i * gem.transform.localScale.x, j * gem.transform.localScale.y, 0);
                    gem.name = $"gem_{i}_{j}";
                    _gemDict.Add(gem, (i, j));
                }
            }
        }


        private IEnumerator SelectGemRoutine(Gem gem)
        {
            if (CurrentlySelectedGem == null)
            {
                CurrentlySelectedGem = gem;
                Debug.Log($"Set gem ${gem.name} as CurrentlySelectedGem");
                yield break;
            }

            if (CanSwapGems(CurrentlySelectedGem, gem))
            {
                SwapGems(CurrentlySelectedGem, gem);
                List<Gem> currentlySelectedGemMatches = CheckForMatches(CurrentlySelectedGem);
                List<Gem> matches = CheckForMatches(gem);
                yield return MatchGems(currentlySelectedGemMatches.Union(matches).ToList());
                yield return new WaitForSeconds(0.2f);
                CollapseColumns();
                FillBoard();
                CurrentlySelectedGem = null;
            }
            else
            {
                // animation to show gems can't perform swap
                CurrentlySelectedGem = null;
            }
        }

        public void SelectGem(Gem gem)
        {
            StartCoroutine(SelectGemRoutine(gem));
        }


        private void CollapseColumns()
        {
            for (int i = 0; i < width; i++)
            {
                CollapseColumn(i);
            }
        }


        private Gem FindFirstActiveGemAbove(int startRow, int column)
        {
            // we look vertically for a non disabled gem
            for (int i = startRow + 1; i < height; i++)
            {
                Gem gem = _gemDict.Get((column, i));
                if (gem.gameObject.activeSelf)
                {
                    // we found the next gem above us, we can swap with it
                    // so it falls down
                    return gem;
                }
            }

            return null;
        }

        private void CollapseColumn(int columnIndex)
        {
            for (int i = 0; i < height; i++)
            {
                // yield return new WaitForSeconds(1f);
                // Gem gem = _gemDict.Get((i, columnIndex));
                Gem gem = _gemDict.Get((columnIndex, i));

                // there is a gap in the column. We can look up for the first
                // non null gem to fill this position with
                if (!gem.gameObject.activeSelf)
                {
                    Gem gemAbove = FindFirstActiveGemAbove(i, columnIndex);
                    if (gemAbove != null)
                    {
                        SwapGems(gem, gemAbove);
                    }
                }
            }
        }

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

        private IEnumerator MatchGems(List<Gem> matchedGems)
        {
            yield return new WaitForSeconds(0.5f);
            foreach (Gem gem in matchedGems)
            {
                (int, int) gemCoords = _gemDict.Get(gem);
                _gemDict.Add(gem, (gemCoords.Item1, gemCoords.Item2));
                gem.gameObject.SetActive(false);
            }

            yield return null;
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

        private void SwapGems(Gem gem0, Gem gem1)
        {
            SwapGems(_gemDict.Get(gem0), _gemDict.Get(gem1));
        }

        private void SwapGems((int, int) gem0Coords, (int, int) gem1Coords)
        {
            Gem gem0 = _gemDict.Get(gem0Coords);
            Gem gem1 = _gemDict.Get(gem1Coords);

            gem0.UpdatePosition(gem0Coords, gem1Coords);
            gem1.UpdatePosition(gem1Coords, gem0Coords);

            _gemDict.Add(gem0, gem1Coords);
            _gemDict.Add(gem1, gem0Coords);
        }


        private void FillBoard()
        {
            List<Gem> disabledGems = new List<Gem>();
            foreach (Gem gem in _gemDict)
            {
                (int, int) coords = _gemDict.Get(gem);
                if (!gem.gameObject.activeSelf && coords.Item1 >= 0 && coords.Item1 < width && coords.Item2 >= 0 &&
                    coords.Item2 < width)
                {
                    disabledGems.Add(gem);
                }
            }

            foreach (Gem gem in disabledGems)
            {
                Gem newGem = _gemSource.GetNextGem();
                (int, int) cords = _gemDict.Get(gem);
                newGem.transform.parent = transform;
                newGem.SetPosition((cords.Item1, cords.Item2 + height));
                _gemDict.Add(newGem, (cords.Item1, cords.Item2 + height));
                SwapGems(newGem, gem);
            }
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
                if (!currentGem.gameObject.activeSelf)
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
                if (!currentGem.gameObject.activeSelf)
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
                if (!currentGem.gameObject.activeSelf)
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
                if (!currentGem.gameObject.activeSelf)
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