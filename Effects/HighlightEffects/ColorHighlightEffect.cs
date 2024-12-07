using UnityEngine;
using BubbleZun;
namespace BubbleZun.Effects.HighlightEffects
{
    public class ColorHighlightEffect : MonoBehaviour, IHighlightEffect
    {
        public Color highlightColor = Color.white;
        [SerializeField] private SpriteRenderer spriteRenderer;
        private Color originalColor;

        private void Awake()
        {
            if (spriteRenderer == null) spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                originalColor = spriteRenderer.color;
            }
        }

        public void Highlight()
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.color = highlightColor;
            }
        }

        public void Unhighlight()
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.color = originalColor;
            }
        }
    } 
}