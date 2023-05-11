using System;
using UnityEngine;

namespace Chimera
{
    /// <summary>
    /// End level scene menu callbacks.
    /// </summary>
    public class EndLevelMenu : MonoBehaviour
    {
        public Action RestartLevel;

        public void OnRestartClicked()
        {
            RestartLevel?.Invoke();
        }

        public void OnMainMenuClicked()
        {
            if (GameState.Instance)
            {
                GameState.Instance.CurrentState = GameState.State.MainMenu;
            }
        }
    }
}