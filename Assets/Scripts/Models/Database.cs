using System.IO;
using Models.Interfaces;
using Newtonsoft.Json;
using UnityEngine;

namespace Models
{
    public class Database : IDatabaseAdapter
    {
        private static IDatabaseAdapter _database;
        
        public static IDatabaseAdapter GetDatabase() =>
            _database ??= new Database();

        public void Save(string filepath, Color[,] cellColors)
        {
            (float, float, float, float)[,] colors = new (float, float, float, float)[cellColors.GetLength(0), cellColors.GetLength(1)];
            for (int i = 0; i < cellColors.GetLength(0); i++)
            {
                for (int j = 0; j < cellColors.GetLength(1); j++)
                {
                    Color color = cellColors[i, j];
                    colors[i,j] = (color.r, color.g, color.b, color.a);
                }
            }
            
            using StreamWriter file = new StreamWriter(filepath + ".json");
            string json = JsonConvert.SerializeObject(colors);
            file.Write(json);
        }

        public Color[,] Load(string filepath)
        {
            using StreamReader file = new StreamReader(filepath + ".json");
            (float, float, float, float)[,] colors = JsonConvert.DeserializeObject<(float, float, float, float)[,]>(file.ReadToEnd());

            Color[,] result = new Color[colors.GetLength(0), colors.GetLength(1)];

            for (int i = 0; i < colors.GetLength(0); i++)
            {
                for (int j = 0; j < colors.GetLength(1); j++)
                {
                    var color = colors[i, j];
                    result[i, j] = new Color(color.Item1, color.Item2, color.Item3, color.Item4);
                }
            }

            return result;
        }
        
        private Database()
        {}
    }
}