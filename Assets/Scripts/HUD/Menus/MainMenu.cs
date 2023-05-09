using UnityEngine;
using UnityEngine.UI;

namespace Chimera
{
    public class MainMenu : MonoBehaviour
    {
        public Button continueButton;
        
        private void Start()
        {
            continueButton.interactable = GameConfig.Level > 0;
            SetGameState(GameState.State.MainMenu);
        }

        public void OnContinueClicked()
        {
            SetGameState(GameState.State.Playing);
        }
        
        public void OnNewGameClicked()
        {
            GameConfig.Level = 0;
            SetGameState(GameState.State.Playing);
        }
        
        public void OnQuitClicked()
        {
            SetGameState(GameState.State.Exit);
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