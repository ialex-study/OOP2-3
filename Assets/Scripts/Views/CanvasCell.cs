using Observer;
using UnityEngine;
using ViewModels;

namespace Views
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class CanvasCell: MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;

        public void SetColor(Color color) =>
            _spriteRenderer.color = color;

        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }
}