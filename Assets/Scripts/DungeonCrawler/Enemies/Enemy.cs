using DungeonCrawler.PlayerCharacter;
using DungeonCrawler.UI;
using UnityEngine;

namespace DungeonCrawler.Enemies
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private Transform selectionArrow;

        private Stats _stats;

        private void Awake()
        {
            _stats = GetComponent<Stats>();
        }

        public void SelectEnemy()
        {
            EnemySelectionIcon.DisplayIcon(selectionArrow.position);
        }

        private void OnMouseDown()
        {
            Player.Instance.SelectedEnemy = this;
            SelectEnemy();
        }
    }
}