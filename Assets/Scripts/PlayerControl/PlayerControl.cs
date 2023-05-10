using System;
using System.Collections.Generic;
using Chimera.Combat;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Chimera
{
    public class PlayerControl : MonoBehaviour
    {
        public GameConfig gameConfig;
        public RectTransform selectionRect;

        private const float _maxCommandDragRadiusSqr = 150.0f;
        private Vector3 _commandPressPosition;

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

            if (Input.GetMouseButtonDown(1))
            {
                _commandPressPosition = Input.mousePosition;
            }
            else if (Input.GetMouseButtonUp(1) &&
                     Vector3.SqrMagnitude(_commandPressPosition - Input.mousePosition) < _maxCommandDragRadiusSqr)
            {
                OnCommand(Input.mousePosition);
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

            var playerSelectables = RaycastSelectables(startPos, endPos, true);
            if (playerSelectables != null && playerSelectables.Count > 0)
            {
                foreach (var s in playerSelectables)
                {
                    if (s.selectable != null)
                    {
                        s.selectable.Selected = true;
                    }
                }

                _selected.AddRange(playerSelectables);
            }
        }

        private void OnCommand(Vector2 pos)
        {
            var enemySelectables = RaycastSelectables(pos, pos, false);
            if (enemySelectables != null && enemySelectables.Count > 0)
            {
                foreach (var selectable in enemySelectables)
                {
                    var enemyActor = (Actor)selectable.selectable;
                    if (enemyActor != null)
                    {
                        foreach (var selected in _selected)
                        {
                            selected.controllable?.OnAttackCommand(enemyActor);
                        }

                        return;
                    }
                }
            }

            var plane = new Plane(Vector3.up, Vector3.zero);
            var posRay = UnityEngine.Camera.main.ScreenPointToRay(pos);

            if (plane.Raycast(posRay, out var enter))
            {
                var moveToPos = posRay.GetPoint(enter);
                foreach (var selected in _selected)
                {
                    selected.controllable?.OnMoveCommand(moveToPos);
                }
            }
        }

        private List<SelectableData> RaycastSelectables(Vector3 startPos, Vector2 endPos, bool playerFactionMask)
        {
            var camera = UnityEngine.Camera.main;
            if (camera == null)
            {
                return null;
            }

            var minPos = Vector2.Min(startPos, endPos);
            var maxPos = Vector2.Max(startPos, endPos);

            var plane = new Plane(Vector3.up, Vector3.zero);
            var minPosRay = camera.ScreenPointToRay(minPos);
            var maxPosRay = camera.ScreenPointToRay(maxPos);

            if (!plane.Raycast(minPosRay, out var minPosEnter) || !plane.Raycast(maxPosRay, out var maxPosEnter))
            {
                return null;
            }

            var maxDistance = Mathf.Max(minPosEnter, maxPosEnter) + 50;
            var centrePos = Vector2.Lerp(minPos, maxPos, 0.5f);
            var centrePosRay = camera.ScreenPointToRay(centrePos);
            var rightPosRay = camera.ScreenPointToRay(new Vector2(maxPos.x, centrePos.y));
            var topPosRay = camera.ScreenPointToRay(new Vector2(centrePos.x, maxPos.y));
            
            var boxExtents = new Vector3(Vector3.Distance(centrePosRay.origin, rightPosRay.origin),
                Vector3.Distance(centrePosRay.origin, topPosRay.origin), maxDistance * 0.5f);

            var colliders = Physics.OverlapBox(centrePosRay.GetPoint(maxDistance * 0.5f), boxExtents,
                camera.transform.rotation, Layers.ActorLayerMask);

            var selectablesList = new List<SelectableData>();

            foreach (var c in colliders)
            {
                var selectable = c.GetComponent<ISelectable>();
                var controllable = c.GetComponent<IControllable>();
                var combatant = c.GetComponent<ICombatant>();

                if (combatant != null)
                {
                    if (playerFactionMask && combatant.Faction != gameConfig.playerFaction)
                    {
                        continue;
                    }
                    else if (!playerFactionMask && combatant.Faction == gameConfig.playerFaction)
                    {
                        continue;
                    }
                }

                if (selectable == null && controllable == null)
                {
                    continue;
                }

                selectablesList.Add(new SelectableData() { controllable = controllable, selectable = selectable });
            }

            return selectablesList;
        }

#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            var camera = UnityEngine.Camera.main;
            if (camera == null)
            {
                return;
            }

            if (Input.GetMouseButton(0))
            {
                var minPos = Vector3.Min(_startSelectionPos, Input.mousePosition);
                var maxPos = Vector3.Max(_startSelectionPos, Input.mousePosition);
                
                var centrePosRay = camera.ScreenPointToRay(Vector2.Lerp(minPos, maxPos, 0.5f));
                var topLeftRay = camera.ScreenPointToRay(new Vector2(minPos.x, maxPos.y));
                var bottomLeftRay = camera.ScreenPointToRay(new Vector2(minPos.x, minPos.y));
                var topRightRay = camera.ScreenPointToRay(new Vector2(maxPos.x, maxPos.y));
                var bottomRightRay = camera.ScreenPointToRay(new Vector2(maxPos.x, minPos.y));
                
                Gizmos.DrawLine(centrePosRay.origin, centrePosRay.GetPoint(100));
                Gizmos.DrawLine(topLeftRay.origin, topLeftRay.GetPoint(100));
                Gizmos.DrawLine(bottomLeftRay.origin, bottomLeftRay.GetPoint(100));
                Gizmos.DrawLine(topRightRay.origin, topRightRay.GetPoint(100));
                Gizmos.DrawLine(bottomRightRay.origin, bottomRightRay.GetPoint(100));
            }
        }

#endif
    }
}