using Models.Interfaces;
using UnityEngine;

namespace Models
{
    public abstract class Figure: IFigure
    {
        protected Color _paintingColor;
        protected Color _fillingColor;

        public Color PaintingColor
        {
            set => _paintingColor = value;
        }

        public Color FillingColor
        {
            set => _fillingColor = value;
        }

        public abstract void OnBeginDrag(int i, int j);
        public abstract Color[,] OnDrag(Color[,] sourceCanvas, int i, int j);
    }
}