using System;
using System.Collections.Generic;
using UnityEngine;

namespace Chimera
{
    public class Level : MonoBehaviour
    {
        public GameConfig gameConfig;
        public Action<bool> OnLevelFinished;
        
        public static Level Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        public static int InvalidActorId = -1;
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
                if (a.Value == null)
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
    }
}