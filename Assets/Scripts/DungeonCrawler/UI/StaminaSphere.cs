using UnityEngine;

namespace DungeonCrawler.UI
{
    public class StaminaSphere : MonoBehaviour
    {
        [SerializeField] private Sprite depletedSprite;
        [SerializeField] private Sprite unusedSprite;


        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _spriteRenderer.sprite = unusedSprite;
        }

        public void Replenish()
        {
            _spriteRenderer.sprite = unusedSprite;
        }

        public void Deplete()
        {
            _spriteRenderer.sprite = depletedSprite;
        }
    }
}