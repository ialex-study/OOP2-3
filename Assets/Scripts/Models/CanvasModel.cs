using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using Models.Enums;
using Models.Interfaces;
using Observer;
using UnityEngine;

namespace Models
{
    public class CanvasModel: IObservable
    {
        private readonly int _canvasSize;
        private Color[,] _canvasArea;
        private Color[,] _rawCanvasArea;
        private Color _paintingColor = Color.black;
        private Color _fillingColor = Color.white;
        private IFigure _paintingFigure;
        private readonly List<IFigure> _figures = new List<IFigure>(); 
        private readonly Notifier _notifier = new Notifier();
        private readonly Stack<Snapshot> _snapshots = new Stack<Snapshot>();
        private readonly Stack<Snapshot> _undidSnapshots = new Stack<Snapshot>();
        private readonly string _baseFilePath;
        private readonly string _pluginsPath;
        

        public int CanvasSize => _canvasSize;
        public List<IFigure> Figures => _figures;
        public Color this[int i, int j] => _canvasArea[i, j];

        public Color PaintingColor
        {
            get => _paintingColor;
            set
            {
                _paintingColor = value;
                _paintingFigure.PaintingColor = _paintingColor;
            }
        }

        public Color FillingColor
        {
            get => _fillingColor;
            set
            {
                _fillingColor = value;
                _paintingFigure.FillingColor = _fillingColor;
            }
        }

        public CanvasModel(int canvasSize)
        {
            // _baseFilePath = AppDomain.CurrentDomain.BaseDirectory;
            _baseFilePath = "D:\\OOP2\\";
            _pluginsPath = "D:\\OOP2\\Plugins";
            
            UpdatePluginsList(_pluginsPath);
                
            _canvasSize = canvasSize;

            _canvasArea = new Color[canvasSize, canvasSize];

            Clear();

            _paintingFigure = new Brush();
            SetFigureColors();
        }

        public void Subscribe(IObserver observer)
        {
            _notifier.Subscribe(observer);
        }

        public void Unsubscribe(IObserver observer)
        {
            _notifier.Unsubscribe(observer);
        }

        public void Clear()
        {
            for (int i = 0; i < _canvasSize; i++)
            {
                for (int j = 0; j < _canvasSize; j++)
                {
                    _canvasArea[i, j] = FillingColor;
                }
            }
            
            MakeSnapshot(_canvasArea);
            _notifier.Notify();
        }

        public void SetPaintingType(PaintingType paintingType)
        {
            switch (paintingType)
            {
                case PaintingType.Brush:
                    _paintingFigure = new Brush();
                    break;
                case PaintingType.Ellipse:
                    _paintingFigure = new Ellipse();
                    break;
                case PaintingType.Polygon:
                    _paintingFigure = new Polygon();
                    break;
                case PaintingType.Rectangle:
                    _paintingFigure = new Rectangle();
                    break;
            }

            SetFigureColors();
        }

        public void SetPaintingType(int figureIndex)
        {
            _paintingFigure = _figures[figureIndex];
            SetFigureColors();
        }

        public void OnBeginDrag(int i, int j)
        {
            _rawCanvasArea = (Color[,]) _canvasArea.Clone();
            MakeSnapshot(_rawCanvasArea);
            _paintingFigure.OnBeginDrag(i, j);
        }

        public void OnDrag(int i, int j)
        {
            _canvasArea = _paintingFigure.OnDrag((Color[,]) _rawCanvasArea.Clone(), i, j);
            
            _notifier.Notify();
        }

        public void OnEndDrag(int i, int j)
        {
            _canvasArea = _paintingFigure.OnDrag((Color[,]) _rawCanvasArea.Clone(), i, j);
            
            _notifier.Notify();
        }

        public void Undo()
        {
            if (_snapshots.Count == 0)
                return;

            if(_snapshots.Count != 1)
                _undidSnapshots.Push(new Snapshot((Color[,]) _canvasArea.Clone()));
            Snapshot snapshot = _snapshots.Pop();

            _canvasArea = (Color[,]) snapshot.CellColors.Clone();
            _notifier.Notify();
        }

        public void Redo()
        {
            if (_undidSnapshots.Count == 0)
                return;
            
            _snapshots.Push(new Snapshot((Color[,]) _canvasArea.Clone()));
            Snapshot snapshot = _undidSnapshots.Pop();

            _canvasArea = (Color[,]) snapshot.CellColors.Clone();
            _notifier.Notify();
        }

        public void Save(string filename)
        {
            Database.GetDatabase().Save(_baseFilePath + filename, _canvasArea);
        }

        public void Load(string filename)
        {
            _canvasArea = Database.GetDatabase().Load(_baseFilePath + filename);
            _notifier.Notify();
        }

        private void MakeSnapshot(Color[,] colors)
        {
            _snapshots.Push(new Snapshot(colors));
        }

        private void UpdatePluginsList(string pluginsPath)
        {
            string[] libs = Directory.GetFiles(pluginsPath, "*.dll");

            foreach (var lib in libs)
            {
                var assembly = Assembly.LoadFile(lib);

                foreach (Type type in assembly.GetTypes())
                {
                    try
                    {
                        _figures.Add((IFigure) Activator.CreateInstance(type));
                    }
                    catch (InvalidCastException)
                    {
                    }
                }
            }
        }

        private void SetFigureColors()
        {
            _paintingFigure.PaintingColor = PaintingColor;
            _paintingFigure.FillingColor = FillingColor;
        }
    }
}