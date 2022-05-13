using UnityEngine;

namespace Models.Interfaces
{
    public interface IDatabaseAdapter
    {
        public void Save(string filepath, Color[,] cellColors);
        public Color[,] Load(string filepath);
    }
}