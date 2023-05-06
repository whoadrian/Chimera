using System.Collections.Generic;
using UnityEngine;

namespace Chimera
{
    public class PlayerControl : MonoBehaviour
    {
        public RectTransform selectionRect;

        private struct SelectableData
        {
            public ISelectable selectable;
            public IControllable controllable;
        }
        
        private bool _selectionActive = false;
        private Vector2 _startSelectionPos = Vector2.zero;
        private List<SelectableData> _selected = new();

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
                    DeactivateSelectionRect();
                    OnSelect(_startSelectionPos, Input.mousePosition);
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
            foreach (var s in _selected)
            {
                if (s.selectable != null)
                {
                    s.selectable.Selected = false;
                }
            }
            _selected.Clear();
            
            var camera = UnityEngine.Camera.main;
            if (camera == null)
            {
                return;
            }

            var minPos = Vector2.Min(startPos, endPos) - Vector2.one;
            var maxPos = Vector2.Max(startPos, endPos) + Vector2.one;

            var plane = new Plane(Vector3.up, Vector3.zero);
            var cornerRays = new Ray[]
            {
                camera.ScreenPointToRay(minPos),
                camera.ScreenPointToRay(maxPos),
                camera.ScreenPointToRay(new Vector2(minPos.x, maxPos.y)),
                camera.ScreenPointToRay(new Vector2(minPos.y, maxPos.x))
            };

            float maxDistance = -1;
            foreach (var ray in cornerRays)
            {
                if (!plane.Raycast(ray, out var enter))
                {
                    return;
                }

                maxDistance = Mathf.Max(maxDistance, enter);
            }

            if (maxDistance <= 0)
            {
                return;
            }

            var centrePos = Vector2.Lerp(minPos, maxPos, 0.5f);
            var centreRay = camera.ScreenPointToRay(centrePos);
            var centreRayOrigin = centreRay.origin;
            var maxPosRayOrigin = cornerRays[1].origin;
            var boxExtents = new Vector3(maxPosRayOrigin.x - centreRayOrigin.x, maxPosRayOrigin.y - centreRayOrigin.y, maxDistance * 0.5f);
            var colliders = Physics.OverlapBox(centreRay.GetPoint(maxDistance * 0.5f), boxExtents,
                camera.transform.rotation, Layers.ActorLayerMask);

            foreach (var c in colliders)
            {
                var selectable = c.GetComponent<ISelectable>();
                var controllable = c.GetComponent<IControllable>();

                if (selectable == null && controllable == null)
                {
                    continue;
                }

                if (selectable != null)
                {
                    selectable.Selected = true;
                }
                
                _selected.Add(new SelectableData() {controllable = controllable, selectable = selectable});
            }
        }
    }
}