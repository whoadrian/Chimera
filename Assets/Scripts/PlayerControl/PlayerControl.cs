using System;
using System.Collections.Generic;
using Chimera.Combat;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Chimera
{
    /// <summary>
    /// Handles the player commands, like move & attack, as well as selectable components. See ISelectable and IControllable.
    /// Draws the selection rectangle via Unity's UI
    /// </summary>
    public class PlayerControl : MonoBehaviour
    {
        // Global game data
        public GameConfig gameConfig;
        
        // Selection UI rectangle
        public RectTransform selectionRect;

        // Command on-screen radius, to avoid issuing commands when right-click + drag, which is associated with camera rotation.
        private const float _maxCommandDragRadiusSqr = 150.0f;
        
        // Position at which command button (right-click) was pressed.
        private Vector3 _commandPressPosition;

        private struct SelectableData
        {
            public Transform transform;
            public ISelectable selectable;
            public IControllable controllable;
        }

        // Selectable objects
        private bool _selectionActive = false;
        private Vector2 _startSelectionPos = Vector2.zero;
        private List<SelectableData> _selected = new();

        #region MonoBehaviour
        
        private void Start()
        {
            selectionRect.gameObject.SetActive(false);
        }

        private void Update()
        {
            // Only update when playing
            if (GameState.Instance != null && GameState.Instance.CurrentState != GameState.State.Playing)
            {
                return;
            }
            
            // Selection not active
            if (!_selectionActive)
            {
                // Selection button pressed
                if (Input.GetMouseButtonDown(0))
                {
                    _selectionActive = true;
                    _startSelectionPos = Input.mousePosition;
                    ActivateSelectionRect(_startSelectionPos);
                }
            }
            else // Selection active
            {
                // Selection button is pressed, update selection rectangle
                if (Input.GetMouseButton(0))
                {
                    UpdateSelectionRect(_startSelectionPos, Input.mousePosition);
                }
                else // Selection button is not pressed, deactivate selected rectangle and select objects within it
                {
                    _selectionActive = false;
                    DeactivateSelectionRect();
                    OnSelect(_startSelectionPos, Input.mousePosition);
                }
            }

            // Command button start
            if (Input.GetMouseButtonDown(1))
            {
                _commandPressPosition = Input.mousePosition;
            }
            // Command button released
            else if (Input.GetMouseButtonUp(1) &&
                     Vector3.SqrMagnitude(_commandPressPosition - Input.mousePosition) < _maxCommandDragRadiusSqr)
            {
                // Issue command
                OnCommand(Input.mousePosition);
            }
        }
        
        #endregion

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

        /// <summary>
        /// Selects all ISelectables within the selection rectangle
        /// </summary>
        private void OnSelect(Vector2 startPos, Vector2 endPos)
        {
            // Deselect all currently selected objects
            foreach (var s in _selected)
            {
                if (s.selectable != null)
                {
                    s.selectable.Selected = false;
                }
            }
            _selected.Clear();

            // Raycast all selectables within the selection rectangle
            var playerSelectables = RaycastSelectables(startPos, endPos, true);
            if (playerSelectables != null && playerSelectables.Count > 0)
            {
                // Select all
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

        /// <summary>
        /// Issue player command at position
        /// </summary>
        private void OnCommand(Vector2 pos)
        {
            // No units selected, nothing to do
            if (_selected.Count == 0)
            {
                return;
            }
            
            // Attack command. Raycast all selectables at this position.
            var enemySelectables = RaycastSelectables(pos, pos, false);
            if (enemySelectables != null && enemySelectables.Count > 0)
            {
                foreach (var selectable in enemySelectables)
                {
                    // Check if enemy
                    var enemyActor = (Actor)selectable.selectable;
                    if (enemyActor != null)
                    {
                        // If enemy, attack command for all selected objects
                        foreach (var selected in _selected)
                        {
                            selected.controllable?.OnAttackCommand(enemyActor);
                        }

                        return;
                    }
                }
            }

            // Move command position
            var plane = new Plane(Vector3.up, Vector3.zero);
            var posRay = UnityEngine.Camera.main.ScreenPointToRay(pos);
            if (plane.Raycast(posRay, out var enter))
            {
                // Move-to position
                var moveToPos = posRay.GetPoint(enter);
                
                // Make units align in a grid at destination (so they don't all try to go to the same exact point)
                var rows = (int)Mathf.Sqrt(_selected.Count);
                var columns = _selected.Count / rows + _selected.Count % rows;
                
                // Find closest to destination
                var minDistance = float.MaxValue;
                var closestPosition = Vector3.zero;
                foreach (var s in _selected)
                {
                    if (s.transform == null)
                    {
                        continue;
                    }
                    
                    var sqrDistance = Vector3.SqrMagnitude(moveToPos - s.transform.position);
                    if (sqrDistance < minDistance)
                    {
                        minDistance = sqrDistance;
                        closestPosition = s.transform.position;
                    }
                }
                
                // No min distance found, selectables & their transforms by now have been destroyed
                if (minDistance == float.MaxValue)
                {
                    return;
                }
                
                // Get forward and right vectors
                var forward = Vector3.Normalize(moveToPos - closestPosition);
                var right = Vector3.Cross(Vector3.up, forward);
                
                // Space between grid points
                const float gridSpacing = 2f;
                
                // Get top-left corner pos of destination grid
                var topLeftPos = moveToPos + forward * ((rows - 1) * gridSpacing * 0.5f) - right * ((columns - 1) * gridSpacing * 0.5f);
                
                // Move command for all selected objects
                int rowIndex = 0;
                int columnIndex = 0;
                for (int i = 0; i < _selected.Count; ++i)
                {
                    var destinationPos = topLeftPos - forward * (rowIndex * gridSpacing) +
                                         right * (columnIndex * gridSpacing);
                    
                    _selected[i].controllable?.OnMoveCommand(destinationPos);

                    // Increment row & index
                    rowIndex = (rowIndex + 1) % rows;
                    if (rowIndex == 0)
                    {
                        columnIndex++;
                    }
                }
            }
        }

        /// <summary>
        /// Returns all ISelectable objects within a rectangle, by constructing a box from the selection rectangle to the ground and checking for colliders within it.
        /// </summary>
        private List<SelectableData> RaycastSelectables(Vector3 startPos, Vector2 endPos, bool playerFactionMask)
        {
            // Get current camera
            var camera = UnityEngine.Camera.main;
            if (camera == null)
            {
                return null;
            }

            // Min/Max position of selection rectangle
            var minPos = Vector2.Min(startPos, endPos);
            var maxPos = Vector2.Max(startPos, endPos);

            // Selection rectangle rays
            var plane = new Plane(Vector3.up, Vector3.zero);
            var minPosRay = camera.ScreenPointToRay(minPos);
            var maxPosRay = camera.ScreenPointToRay(maxPos);

            // Check if they hit the ground
            if (!plane.Raycast(minPosRay, out var minPosEnter) || !plane.Raycast(maxPosRay, out var maxPosEnter))
            {
                return null;
            }

            // Construct the box used to check for colliders within the selection rectangle
            var maxDistance = Mathf.Max(minPosEnter, maxPosEnter) + 50;
            var centrePos = Vector2.Lerp(minPos, maxPos, 0.5f);
            var centrePosRay = camera.ScreenPointToRay(centrePos);
            var rightPosRay = camera.ScreenPointToRay(new Vector2(maxPos.x, centrePos.y));
            var topPosRay = camera.ScreenPointToRay(new Vector2(centrePos.x, maxPos.y));
            
            var boxExtents = new Vector3(Vector3.Distance(centrePosRay.origin, rightPosRay.origin),
                Vector3.Distance(centrePosRay.origin, topPosRay.origin), maxDistance * 0.5f);

            // Check for colliders
            var colliders = Physics.OverlapBox(centrePosRay.GetPoint(maxDistance * 0.5f), boxExtents,
                camera.transform.rotation, Layers.ActorLayerMask);

            // Data container for all selectables
            var selectablesList = new List<SelectableData>();

            // Check colliders
            foreach (var c in colliders)
            {
                // Get relevant components
                var selectable = c.GetComponent<ISelectable>();
                var controllable = c.GetComponent<IControllable>();
                var combatant = c.GetComponent<ICombatant>();

                // If combatant, skip if it's not the player's faction
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

                // If not selectable of controllable, skip
                if (selectable == null && controllable == null)
                {
                    continue;
                }

                // Add
                selectablesList.Add(new SelectableData() { transform = c.transform, controllable = controllable, selectable = selectable });
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

            // Draw selection rectangle debug rays
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