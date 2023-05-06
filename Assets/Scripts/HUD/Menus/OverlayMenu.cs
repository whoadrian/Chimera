using System;
using UnityEngine;

namespace Chimera
{
    public class OverlayMenu : MonoBehaviour
    {
        private void Start()
        {
            GameState.Instance.CurrentState = GameState.State.Paused;
        }

        public void OnResumeClicked()
        {
            GameState.Instance.CurrentState = GameState.State.Playing;
        }

        public void OnQuitClicked()
        {
            GameState.Instance.CurrentState = GameState.State.MainMenu;
        }
    }
}