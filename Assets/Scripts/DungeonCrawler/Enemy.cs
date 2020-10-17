using DungeonCrawler.UI;
using UnityEngine;

namespace DungeonCrawler
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private Transform selectionArrow;

        private Player _player;


        private void Awake()
        {
            _player = FindObjectOfType<Player>();
        }

        private void OnMouseDown()
        {
            _player.SelectedEnemy = this;
            EnemySelectionIcon.DisplayIcon(selectionArrow.position);
        }
    }
}