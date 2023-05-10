using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Chimera
{
    public class Level : MonoBehaviour
    {
        public GameConfig gameConfig;
        public Transform minLevelBoundary;
        public Transform maxLevelBoundary;
        public Action<bool> OnLevelFinished;

        public static Level Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        public const int InvalidActorId = -1;
        private static int _actorIdCounter = 0;
        private Dictionary<int, Actor> _actors = new();

        public int RegisterActor(Actor actor)
        {
            _actorIdCounter++;
            _actors.Add(_actorIdCounter, actor);
            return _actorIdCounter;
        }

        public void UnregisterActor(int actorId)
        {
            if (_actors.ContainsKey(actorId))
            {
                _actors.Remove(actorId);
            }

            CheckWinState();
        }

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