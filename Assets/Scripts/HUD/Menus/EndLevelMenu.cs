using System;
using UnityEngine;

namespace Chimera
{
    public class EndLevelMenu : MonoBehaviour
    {
        public Action RestartLevel;
        public Action LoadNextLevel;

        public void OnRestartClicked()
        {
            RestartLevel?.Invoke();
        }

        public void OnLoadNextClicked()
        {
            LoadNextLevel?.Invoke();
        }

        public void OnExitClicked()
        {
            if (GameState.Instance)
            {
                GameState.Instance.CurrentState = GameState.State.MainMenu;
            }
        }
    }
}