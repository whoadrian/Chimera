using Chimera.Combat;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

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