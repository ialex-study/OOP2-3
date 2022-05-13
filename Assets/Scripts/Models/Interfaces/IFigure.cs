using UnityEngine;

namespace Models.Interfaces
{
    public interface IFigure
    {
        public Color PaintingColor { set; }
        public Color FillingColor { set; }
        public void OnBeginDrag(int i, int j);
        public Color[,] OnDrag(Color[,] sourceCanvas, int i, int j);
    }
}