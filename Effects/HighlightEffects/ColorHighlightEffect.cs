using UnityEngine;
using BubbleZun;
using UnityEngine.UI;
namespace BubbleZun.Effects.HighlightEffects
{
    public class ColorHighlightEffect : MonoBehaviour, IHighlightEffect
    {
        public Color highlightColor = Color.white;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Image image;
        private Color originalColor;

        private void Awake()
        {
            if (spriteRenderer == null) spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            if (image == null) image = GetComponentInChildren<Image>();
            if (spriteRenderer != null)
            {
                originalColor = spriteRenderer.color;
            }
            if (image != null)
            {
                originalColor = image.color;
            }
        }

        public void Highlight()
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.color = highlightColor;
            }
            if (image != null)
            {
                image.color = highlightColor;
            }
        }

        public void Unhighlight()
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.color = originalColor;
            }
            if (image != null)
            {
                image.color = originalColor;
            }
        }
    } 
}