using Chimera.Combat;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Chimera
{
    /// <summary>
    /// Main Menu logic and UX flow.
    /// Main Menu scene menu callbacks.
    /// </summary>
    public class MainMenu : MonoBehaviour
    {
        // Different menus within the main menu scene are children of this component.
        public GameObject mainMenu;
        public GameObject levelSelect;
        public GameObject factionSelect;

        // Global game data
        public GameConfig gameConfig;
        
        private void Start()
        {
            // Set first menu
            mainMenu.SetActive(true);
            levelSelect.SetActive(false);
            factionSelect.SetActive(false);
            
            // Make sure game state is set to main menu
            SetGameState(GameState.State.MainMenu);
        }
        
        public void OnPlayClicked()
        {
            // Play switches to level select
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
            OnLevelSelected(0);
        }

        public void OnIslandLevelClicked()
        {
            OnLevelSelected(1);
        }

        public void OnRedFactionClicked()
        {
            OnFactionSelected(Faction.Red);
        }

        public void OnBlueFactionClicked()
        {
            OnFactionSelected(Faction.Blue);
        }
        
        private void OnLevelSelected(int levelId)
        {
            GameConfig.Level = levelId;
            
            // Level selection switches to faction selection screen
            mainMenu.SetActive(false);
            levelSelect.SetActive(false);
            factionSelect.SetActive(true);
        }

        private void OnFactionSelected(Faction faction)
        {
            gameConfig.playerFaction = faction;
#if UNITY_EDITOR
            EditorUtility.SetDirty(gameConfig);
#endif
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