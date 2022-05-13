using UnityEngine;

namespace Models.Interfaces
{
    public interface ISnapshot
    {
        public Color[,] CellColors { get; }
    }
}