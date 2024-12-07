using UnityEngine;
using BubbleZun;
namespace BubbleZun.Effects.HighlightEffects
{
    public class BorderHighlightEffect : MonoBehaviour, IHighlightEffect
    {
        public Color borderColor = Color.white;
        public float borderWidth = 1f;

        [SerializeField] private SpriteRenderer spriteRenderer;
        private Material originalMaterial;
        private Material outlineMaterial;

        private void Awake()
        {
            if (spriteRenderer == null) spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                originalMaterial = spriteRenderer.material;
                
                // 创建outline材质
                outlineMaterial = new Material(Shader.Find("Custom/SpriteOutline"));
                outlineMaterial.SetColor("_OutlineColor", borderColor);
                outlineMaterial.SetFloat("_OutlineWidth", 0);
            }
        }

        private void OnDestroy()
        {
            // 清理创建的材质
            if (outlineMaterial != null)
            {
                Destroy(outlineMaterial);
            }
        }

        public void Highlight()
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.material = outlineMaterial;
                outlineMaterial.SetFloat("_OutlineWidth", borderWidth);
                outlineMaterial.SetColor("_OutlineColor", borderColor);
            }
        }

        public void Unhighlight()
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.material = originalMaterial;
            }
        }
    } 
}