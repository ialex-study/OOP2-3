using System;
using UnityEngine;

namespace Models
{
    public class Rectangle: Figure
    {
        private int[] _startPoint;
        
        public override void OnBeginDrag(int i, int j)
        {
            _startPoint = new[] {i, j};
        }

        public override Color[,] OnDrag(Color[,] sourceCanvas, int i, int j)
        {
            int iSign = Math.Sign(i - _startPoint[0]);
            int jSign = Math.Sign(j - _startPoint[1]);

            if (iSign != 0)
            {
                for (int k = _startPoint[0]; k != i + iSign; k += iSign)
                {
                    sourceCanvas[k, j] = sourceCanvas[k, _startPoint[1]] = _paintingColor;
                }
            }

            if (jSign != 0)
            {
                for (int k = _startPoint[1]; k != j; k += jSign)
                {
                    sourceCanvas[i, k] = sourceCanvas[_startPoint[0], k] = _paintingColor;
                }
            }

            if (iSign != 0 && jSign != 0)
            {
                for (int k = _startPoint[0] + iSign; k != i; k += iSign)
                {
                    for (int z = _startPoint[1] + jSign; z != j; z += jSign)
                    {
                        sourceCanvas[k, z] = _fillingColor;
                    }
                }
            }

            return sourceCanvas;
        }
    }
}