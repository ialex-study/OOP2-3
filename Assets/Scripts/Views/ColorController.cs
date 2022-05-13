using System;
using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    public class ColorController : MonoBehaviour
    {

        [SerializeField] private Slider redSlider;
        [SerializeField] private Slider greenSlider;
        [SerializeField] private Slider blueSlider;
        [SerializeField] private SpriteRenderer resultColor;

        public event Action<Color> OnColorChanged;

        public void OnColorChange()
        {
            Color color = new Color(redSlider.value, greenSlider.value, blueSlider.value);

            resultColor.color = color;
            OnColorChanged?.Invoke(color);
        }
    }
}