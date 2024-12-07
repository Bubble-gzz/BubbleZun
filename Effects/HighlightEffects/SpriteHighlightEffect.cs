using UnityEngine;
using BubbleZun;
namespace BubbleZun.Effects.HighlightEffects
{
    public class SpriteHighlightEffect : MonoBehaviour, IHighlightEffect
    {
        public Sprite highlightSprite;
        [SerializeField] private SpriteRenderer spriteRenderer;
        private Sprite originalSprite;

        private void Awake()
        {
            if (spriteRenderer == null) spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                originalSprite = spriteRenderer.sprite;
            }
        }

        public void Highlight()
        {
            if (spriteRenderer != null && highlightSprite != null)
            {
                spriteRenderer.sprite = highlightSprite;
            }
        }

        public void Unhighlight()
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = originalSprite;
            }
        }
    } 
}