using DungeonCrawler.PlayerCharacter;
using DungeonCrawler.UI;
using UnityEngine;

namespace DungeonCrawler
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private Transform selectionArrow;


        private Player _player;


        public void SelectEnemy()
        {
            EnemySelectionIcon.DisplayIcon(selectionArrow.position);
        }

        private void Awake()
        {
            _player = FindObjectOfType<Player>();
        }

        private void OnMouseDown()
        {
            _player.SelectedEnemy = this;
            SelectEnemy();
        }
    }
}