using UnityEngine;
using TMPro;
using BubbleZun;
namespace BubbleZun.Effects.HighlightEffects
{
    public class TextHighlightEffect : MonoBehaviour, IHighlightEffect
    {
        public Color highlightColor = Color.white;
        
        [SerializeField] private TMP_Text textMeshPro;
        private Color originalTextColor;

        private void Awake()
        {
            if (textMeshPro == null) textMeshPro = GetComponentInChildren<TMP_Text>();
            if (textMeshPro != null)
            {
                originalTextColor = textMeshPro.color;
            }
        }

        public void Highlight()
        {
            if (textMeshPro != null)
            {
                textMeshPro.color = highlightColor;
            }
        }

        public void Unhighlight()
        {
            if (textMeshPro != null)
            {
                textMeshPro.color = originalTextColor;
            }
        } 
    }
}