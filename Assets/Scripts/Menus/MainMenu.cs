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
            GameState.Instance.CurrentState = GameState.State.MainMenu;
        }

        public void OnContinueClicked()
        {
            Chimera.GameState.Instance.CurrentState = GameState.State.Playing;
        }
        
        public void OnNewGameClicked()
        {
            GameConfig.Level = 0;
            Chimera.GameState.Instance.CurrentState = GameState.State.Playing;
        }
        
        public void OnQuitClicked()
        {
            Chimera.GameState.Instance.CurrentState = GameState.State.Exit;
        }
    }
}