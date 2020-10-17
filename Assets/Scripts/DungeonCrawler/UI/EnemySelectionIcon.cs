using Core.Util;
using UnityEngine;

namespace DungeonCrawler.UI
{
    public class EnemySelectionIcon : Singleton<EnemySelectionIcon>
    {
        private static SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _spriteRenderer.enabled = false;
        }

        public static void DisplayIcon(Vector3 pos)
        {
            _spriteRenderer.enabled = true;
            Instance.gameObject.transform.position = pos;
        }
    }
}