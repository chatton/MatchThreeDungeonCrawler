using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MatchThree
{
    public class GemSource : MonoBehaviour
    {
        [SerializeField] private List<Gem> gemPrefabs;

        private Queue<Gem> _gems;
        private GameBoard _gameBoard;

        private void Awake()
        {
            _gems = new Queue<Gem>();
            _gameBoard = GetComponent<GameBoard>();
            for (int i = 0; i < 100; i++)
            {
                Gem gem = Instantiate(gemPrefabs[Random.Range(0, gemPrefabs.Count)]);
                gem.GameBoard = _gameBoard;
                gem.GemSource = this;
                gem.gameObject.SetActive(false);
            }
        }


        public Gem GetNextGem()
        {
            Gem gem = _gems.Dequeue();
            gem.gameObject.SetActive(true);
            return gem;
        }

        public void ReturnToPool(Gem gem)
        {
            Debug.Log($"Gem {gem.name} is being returned");
            _gems.Enqueue(gem);
        }
    }
}