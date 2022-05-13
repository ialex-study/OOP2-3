using System.Collections.Generic;
using System.Linq;
using Models;
using Models.Enums;
using Observer;
using UnityEngine;

namespace ViewModels
{
    public class CanvasVM: IObservable, IObserver
    {
        private CanvasModel _canvasModel;
        private Notifier _notifier;

        public Color this[int i, int j] => _canvasModel[i, j];

        public List<string> Figures => (from figure in _canvasModel.Figures
            select figure.GetType().Name).ToList();

        public Color PaintingColor
        {
            set => _canvasModel.PaintingColor = value;
        }

        public Color FillingColor
        {
            set => _canvasModel.FillingColor = value;
        }

        public CanvasVM(int canvasSize)
        {
            _canvasModel = new CanvasModel(canvasSize);
            _canvasModel.Subscribe(this);
            
            _notifier = new Notifier();
        }

        public void Subscribe(IObserver observer)
        {
            _notifier.Subscribe(observer);
        }

        public void Unsubscribe(IObserver observer)
        {
            _notifier.Unsubscribe(observer);
        }

        public void Update()
        {
            _notifier.Notify();
        }

        public void SetPaintingType(string type)
        {
            PaintingType paintingType = PaintingType.Brush;

            switch (type)
            {
                case "Brush":
                    paintingType = PaintingType.Brush;
                    break;
                case "Ellipse":
                    paintingType = PaintingType.Ellipse;
                    break;
                case "Rectangle":
                    paintingType = PaintingType.Rectangle;
                    break;
                case "Polygon":
                    paintingType = PaintingType.Polygon;
                    break;
            }
            
            _canvasModel.SetPaintingType(paintingType);
        }

        public void SetPaintingType(int figureIndex)
        {
            _canvasModel.SetPaintingType(figureIndex);
        }

        public void Clear()
        {
            _canvasModel.Clear();
        }

        public void OnBeginDrag(int i, int j)
        {
            _canvasModel.OnBeginDrag(i, j);
        }

        public void OnDrag(int i, int j)
        {
            _canvasModel.OnDrag(i, j);
        }

        public void OnEndDrag(int i, int j)
        {
            _canvasModel.OnEndDrag(i, j);
        }

        public void Undo()
        {
            _canvasModel.Undo();
        }

        public void Redo()
        {
            _canvasModel.Redo();
        }

        public void Save(string filename)
        {
            _canvasModel.Save(filename);
        }

        public void Load(string filename)
        {
            _canvasModel.Load(filename);
        }
    }
}