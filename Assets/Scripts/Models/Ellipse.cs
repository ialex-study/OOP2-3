using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Models
{
    public class Ellipse: Figure
    {
        private int[] _startPoint;
        
        public override void OnBeginDrag(int i, int j)
        {
            _startPoint = new[] {i, j};
        }

        public override Color[,] OnDrag(Color[,] sourceCanvas, int i, int j)
        {
            int a = Math.Abs(i - _startPoint[0]);
            int b = Math.Abs(j - _startPoint[1]);

            DrawEllipse(sourceCanvas, CalculatePoints(a, b), _paintingColor);


            for (int k = a - 1; k >= 0; k--)
            {
                for (int z = b - 1; z >= 0; z--)
                {
                    DrawEllipse(sourceCanvas, CalculatePoints(k, z), _fillingColor);
                }
            }

            return sourceCanvas;
        }

        private void DrawEllipse(Color[,] sourceCanvas, List<int[]> points, Color paintingColor)
        {
            foreach (var point in points)
            {
                int x = _startPoint[0] - point[0];
                int y = _startPoint[1] - point[1];

                if (
                    x < 0 ||
                    x >= sourceCanvas.Length ||
                    y < 0 ||
                    y >= sourceCanvas.Length
                )
                    continue;

                sourceCanvas[x, y] = paintingColor;
            }
        }

        private List<int[]> CalculatePoints(int a, int b)
        {
            List<int[]> result = new List<int[]>();

            for (int degree = 0; degree <= 360; degree++)
            {
                switch (degree)
                {
                    case 0:
                    case 90:
                    case 180:
                    case 270:
                    case 360:
                        continue;
                }

                double radians = GetRadians(degree);

                result.Add(new[] {(int) (a * Math.Cos(radians)), (int) (b * Math.Sin(radians))});
            }

            return result;
        }

        private double GetRadians(int degree) =>
            Math.PI * degree / 180;
    }
}