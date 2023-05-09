using System;
using UnityEngine;

namespace Chimera
{
    public class OverlayMenu : MonoBehaviour
    {
        private void Start()
        {
            SetGameState(GameState.State.Paused);
        }

        public void OnResumeClicked()
        {
            SetGameState(GameState.State.Playing);
        }

        public void OnQuitClicked()
        {
            SetGameState(GameState.State.MainMenu);
        }

        private void SetGameState(GameState.State state)
        {
            if (GameState.Instance)
            {
                GameState.Instance.CurrentState = state;
            }
        }
    }
}