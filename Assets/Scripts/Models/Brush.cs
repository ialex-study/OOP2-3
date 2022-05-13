using System.Collections.Generic;
using UnityEngine;

namespace Models
{
    public class Brush: Figure
    {
        private readonly List<int[]> _points = new List<int[]>();
        
        public override void OnBeginDrag(int i, int j)
        {
            _points.Clear();
            _points.Add(new[] {i, j});
        }

        public override Color[,] OnDrag(Color[,] sourceCanvas, int i, int j)
        {
            _points.Add(new[] {i, j});
            sourceCanvas = DrawPoints(sourceCanvas);

            return sourceCanvas;
        }

        private Color[,] DrawPoints(Color[,] sourceCanvas)
        {
            foreach (var point in _points)
            {
                sourceCanvas[point[0], point[1]] = _paintingColor;
            }

            return sourceCanvas;
        }
    }
}