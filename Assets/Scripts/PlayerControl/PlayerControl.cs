using System;
using UnityEngine;

namespace Chimera
{
    public class PlayerControl : MonoBehaviour
    {
        public RectTransform selectionRect;

        private bool _selectionActive = false;
        private Vector2 _startSelectionPos = Vector2.zero;

        private void Start()
        {
            selectionRect.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (!_selectionActive)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    _selectionActive = true;
                    _startSelectionPos = Input.mousePosition;
                    ActivateSelectionRect(_startSelectionPos);
                }
            }
            else
            {
                if (Input.GetMouseButton(0))
                {
                    UpdateSelectionRect(_startSelectionPos, Input.mousePosition);
                }
                else
                {
                    _selectionActive = false;
                    OnSelect(_startSelectionPos, Input.mousePosition);
                    DeactivateSelectionRect();
                }
            }
        }

        private void ActivateSelectionRect(Vector2 startPos)
        {
            selectionRect.gameObject.SetActive(true);
            selectionRect.sizeDelta = Vector2.zero;
            selectionRect.anchoredPosition = startPos;
        }

        private void DeactivateSelectionRect()
        {
            selectionRect.gameObject.SetActive(false);
            selectionRect.sizeDelta = Vector2.zero;
        }

        private void UpdateSelectionRect(Vector2 startPos, Vector2 currentPos)
        {
            var minPos = Vector2.Min(startPos, currentPos);
            var maxPos = Vector2.Max(startPos, currentPos);

            selectionRect.sizeDelta = new Vector2(maxPos.x - minPos.x, maxPos.y - minPos.y);
            selectionRect.anchoredPosition = minPos;
        }

        private void OnSelect(Vector2 startPos, Vector2 endPos)
        {
            
        }
    }
}