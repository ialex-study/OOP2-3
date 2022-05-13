using Models.Interfaces;
using UnityEngine;

namespace Models
{
    public class Snapshot : ISnapshot
    {
        public Color[,] CellColors { get; private set; }

        public Snapshot(Color[,] colors) =>
            CellColors = colors;
    }
}