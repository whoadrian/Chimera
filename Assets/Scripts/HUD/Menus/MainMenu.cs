using Chimera.Combat;
using UnityEngine;

namespace Chimera
{
    public class MainMenu : MonoBehaviour
    {
        public GameObject mainMenu;
        public GameObject levelSelect;
        public GameObject factionSelect;

        public GameConfig gameConfig;
        
        private void Start()
        {
            mainMenu.SetActive(true);
            levelSelect.SetActive(false);
            factionSelect.SetActive(false);
            
            SetGameState(GameState.State.MainMenu);
        }
        
        public void OnPlayClicked()
        {
            mainMenu.SetActive(false);
            levelSelect.SetActive(true);
            factionSelect.SetActive(false);
        }
        
        public void OnQuitClicked()
        {
            SetGameState(GameState.State.Exit);
        }

        public void OnDesertLevelClicked()
        {
            GameConfig.Level = 0;
            OnLevelSelected();
        }

        public void OnIslandLevelClicked()
        {
            GameConfig.Level = 1;
            OnLevelSelected();
        }

        public void OnRedFactionClicked()
        {
            gameConfig.playerFaction = Faction.Red;
            OnFactionSelected();
        }

        public void OnBlueFactionClicked()
        {
            gameConfig.playerFaction = Faction.Blue;
            OnFactionSelected();
        }
        
        private void OnLevelSelected()
        {
            mainMenu.SetActive(false);
            levelSelect.SetActive(false);
            factionSelect.SetActive(true);
        }

        private void OnFactionSelected()
        {
            SetGameState(GameState.State.Playing);
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