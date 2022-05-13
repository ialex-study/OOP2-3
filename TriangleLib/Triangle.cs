using System;
using System.Collections.Generic;
using Models;
using UnityEngine;


public class Triangle : Figure
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

        if (a == 0 && b == 0)
            return sourceCanvas;

        double centerAngle = CalculateAngle(_startPoint, new[] {i, j});
        double angle1 = centerAngle + Math.PI / 6;
        double angle2 = centerAngle - Math.PI / 6;

        int[] point2 =
            {(int) (a * Math.Cos(angle1)) + _startPoint[0], (int) (b * Math.Sin(angle1)) + _startPoint[1]};
        int[] point3 =
            {(int) (a * Math.Cos(angle2)) + _startPoint[0], (int) (b * Math.Sin(angle2)) + _startPoint[1]};

        BuildStraight(CalculateStraight(_startPoint, point2), sourceCanvas, _paintingColor);
        BuildStraight(CalculateStraight(_startPoint, point3), sourceCanvas, _paintingColor);
        BuildStraight(CalculateStraight(point2, point3), sourceCanvas, _paintingColor);

        return sourceCanvas;
    }

    private List<int[]> CalculateStraight(int[] startPoint, int[] endPoint)
    {
        List<int[]> result = new List<int[]>();

        int x1 = startPoint[0];
        int y1 = startPoint[1];
        int x2 = endPoint[0];
        int y2 = endPoint[1];

        int xSign = Math.Sign(x2 - x1);

        float b = x2 != x1 ? (x2 * y1 - y2 * x1) / (x2 - x1) : 0;
        float k = (y1 - b) / x1;

        for (int x = x1; x != x2; x += xSign)
        {
            result.Add(new[] {x, (int) (k * x + b)});
        }

        return result;
    }

    private void BuildStraight(List<int[]> points, Color[,] sourceCanvas, Color color)
    {
        foreach (var point in points)
        {
            int x = point[0];
            int y = point[1];

            if (
                x < 0 ||
                x >= sourceCanvas.Length ||
                y < 0 ||
                y >= sourceCanvas.Length
            )
                continue;

            sourceCanvas[x, y] = color;
        }
    }

    private double CalculateAngle(int[] coordinatesStart, int[] point)
    {
        int x1 = coordinatesStart[0];
        int y1 = coordinatesStart[1];
        int x2 = point[0];
        int y2 = point[1];

        if (x1 == x2)
        {
            return y1 > y2 ? 2 * Math.PI : Math.PI;
        }

        if (y1 == y2)
        {
            return x1 > x2 ? Math.PI * 3 / 2 : 0;
        }

        return Math.Atan2((double) point[1] - coordinatesStart[1], (double) point[0] - coordinatesStart[0]);
    }
}