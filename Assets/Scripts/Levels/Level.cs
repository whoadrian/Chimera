using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Chimera
{
    /// <summary>
    /// Level data, should be one per level scene. Keeps track of all actors and triggers the win/lose state.
    /// Register and unregister your actors via the public methods.
    /// </summary>
    public class Level : MonoBehaviour
    {
        public GameConfig gameConfig;
        
        // Level boundaries, used for camera
        public Transform minLevelBoundary;
        public Transform maxLevelBoundary;
        
        // Triggered when level has finished, bool parameter is true when player won
        public Action<bool> OnLevelFinished;

        // Actors
        public const int InvalidActorId = -1;
        private static int _actorIdCounter = 0;
        private Dictionary<int, Actor> _actors = new();
        
        public static Level Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("Level instance already exists!");
                return;
            }
            
            Instance = this;
        }

        /// <summary>
        /// Register an actor to this level. Returns a unique id.
        /// </summary>
        public int RegisterActor(Actor actor)
        {
            _actorIdCounter++;
            _actors.Add(_actorIdCounter, actor);
            return _actorIdCounter;
        }

        /// <summary>
        /// Unregister an actor from this level by its unique id.
        /// </summary>
        public void UnregisterActor(int actorId)
        {
            if (_actors.ContainsKey(actorId))
            {
                _actors.Remove(actorId);
            }

            CheckWinState();
        }

        /// <summary>
        /// Checks if player has won, or lost, or the level is still ongoing.
        /// </summary>
        private void CheckWinState()
        {
            int enemyCount = 0;
            int playerCount = 0;

            foreach (var a in _actors)
            {
                if (a.Value == null || a.Value.config.excludeFromWinLoseState)
                {
                    continue;
                }

                if (a.Value.faction == gameConfig.playerFaction)
                {
                    playerCount++;
                }
                else
                {
                    enemyCount++;
                }
            }

            if (playerCount == 0)
            {
                OnLevelFinished?.Invoke(false);
            }
            else if (enemyCount == 0)
            {
                OnLevelFinished?.Invoke(true);
            }
        }

#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            // Draw level boundaries in editor
            if (minLevelBoundary && maxLevelBoundary)
            {
                var min = Vector3.Min(minLevelBoundary.position, maxLevelBoundary.position);
                var max = Vector3.Max(minLevelBoundary.position, maxLevelBoundary.position);
                var a = min;
                a.x += max.x - min.x;
                var b = max;
                b.x -= max.x - min.x;
                
                Handles.color = Color.yellow;
                Handles.DrawLine(min, a);
                Handles.DrawLine(a, max);
                Handles.DrawLine(max, b);
                Handles.DrawLine(b, min);
            }
        }

#endif
    }
}