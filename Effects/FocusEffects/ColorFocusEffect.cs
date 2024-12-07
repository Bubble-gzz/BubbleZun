using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BubbleZun.Effects.FocusEffects
{
    public class ColorFocusEffect : MonoBehaviour, IFocusEffect
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

            public void Focus()
            {
                Debug.Log("Color Focus");
                if (spriteRenderer != null)
                {
                    spriteRenderer.color = highlightColor;
                }
            }

            public void Unfocus()
            {
                if (spriteRenderer != null)
                {
                    spriteRenderer.color = originalColor;
                }
            }
    }
}