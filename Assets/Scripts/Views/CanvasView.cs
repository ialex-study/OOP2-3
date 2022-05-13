using System;
using System.Collections.Generic;
using System.Linq;
using Observer;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using ViewModels;

namespace Views
{
    public class CanvasView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IObserver
    {
        [SerializeField] private int canvasSize = 100;
        [SerializeField] private ColorController paintingColorController;
        [SerializeField] private TMP_Dropdown figuresDropdown;
        [SerializeField] private ColorController fillingColorController;
        [SerializeField] private TMP_InputField fileInputField;

        private CanvasVM _canvasVM;
        private Dictionary<string, int> _cellKeys = new Dictionary<string, int>();

        private SortedDictionary<string, CanvasCell> _canvasCells =
            new SortedDictionary<string, CanvasCell>(new CellsComparer());

        public void OnBeginDrag(PointerEventData eventData)
        {
            (int i, int j) = GetKeyByEventDataPosition(eventData.position);

            _canvasVM.OnBeginDrag(i, j);
        }

        public void OnDrag(PointerEventData eventData)
        {
            (int i, int j) = GetKeyByEventDataPosition(eventData.position);

            _canvasVM.OnDrag(i, j);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            (int i, int j) = GetKeyByEventDataPosition(eventData.position);

            _canvasVM.OnEndDrag(i, j);
        }

        public void OnPaintingTypeSelect(string type)
        {
            figuresDropdown.SetValueWithoutNotify(0);
            _canvasVM.SetPaintingType(type);
        }

        public void Clear()
        {
            _canvasVM.Clear();
        }

        public void Undo()
        {
            _canvasVM.Undo();
        }

        public void Redo()
        {
            _canvasVM.Redo();
        }

        public void Save()
        {
            _canvasVM.Save(fileInputField.text);
        }

        public void Load()
        {
            _canvasVM.Load(fileInputField.text);
        }

        public void OnFigureDropdownSelect()
        {
            int index = figuresDropdown.value;
            if (index == 0)
                return;
            
            _canvasVM.SetPaintingType(index - 1);
        }

        void IObserver.Update()
        {
            UpdateCellsState();
        }

        private void Start()
        {
            foreach (var cell in GetComponentsInChildren<CanvasCell>())
            {
                string key = GetKeyByPosition(cell.transform.position);

                _canvasCells[key] = cell;
            }

            int counter = 0;
            foreach (var pair in _canvasCells)
            {
                _cellKeys[pair.Key] = counter;

                counter++;
            }

            _canvasVM = new CanvasVM(canvasSize);
            _canvasVM.Subscribe(this);

            paintingColorController.OnColorChanged += color => _canvasVM.PaintingColor = color;
            fillingColorController.OnColorChanged += color => _canvasVM.FillingColor = color;

            UpdateFiguresDropdown();
        }

        private void UpdateCellsState()
        {
            foreach (var pair in _canvasCells)
            {
                (int i, int j) = GetIndexByKey(pair.Key);

                pair.Value.SetColor(
                    _canvasVM[i, j]
                );
            }
        }

        private string GetKeyByPosition(Vector2 position) =>
            $"{(int) Math.Round(position.x * 100)}, {(int) Math.Round(position.y * 100)}";

        private (int, int) GetIndexByKey(string key)
        {
            int keyIndex = _cellKeys[key];

            return (keyIndex / canvasSize, keyIndex % canvasSize);
        }

        private (int, int) GetKeyByEventDataPosition(Vector3 position)
        {
            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(position) * 10;
            worldPosition = new Vector2(Mathf.Round(worldPosition.x) + 0.5f, Mathf.Round(worldPosition.y) + 0.5f) / 10;

            return GetIndexByKey(GetKeyByPosition(worldPosition));
        }

        private class CellsComparer : IComparer<string>
        {
            public int Compare(string key1, string key2)
            {
                if (key2 is null)
                    return 1;
                if (key1 is null)
                    return -1;

                int[] key1Values = (from value in key1.Split(',')
                    select int.Parse(value.Trim())).ToArray();
                int[] key2Values = (from value in key2.Split(',')
                    select int.Parse(value.Trim())).ToArray();

                int iDelta = key1Values[0] - key2Values[0];

                if (iDelta != 0)
                    return iDelta;

                return key1Values[1] - key2Values[1];
            }
        }

        private void UpdateFiguresDropdown()
        {
            figuresDropdown.ClearOptions();
            figuresDropdown.AddOptions(new List<string> {"Plugin figures..."});
            figuresDropdown.AddOptions(_canvasVM.Figures);
        }
    }
}