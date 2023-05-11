using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

namespace Chimera
{
    /// <summary>
    /// Manages all levels. Loads/Unloads scenes associated with the levels.
    /// </summary>
    public class LevelManager : MonoBehaviour
    {
        // Level data
        public LevelsConfig levelsConfig;

        // Current loaded level
        private Level _currentLevel;
        private Scene _currentLevelScene;
        
        private void Start()
        {
            Assert.IsTrue(levelsConfig != null && levelsConfig.levels.Count > 0);

            // Make sure the game state is set to playing
            if (GameState.Instance)
            {
                GameState.Instance.CurrentState = GameState.State.Playing;
            }

            // Load current player level
            LoadLevel(GameConfig.Level);
        }

        private void LoadLevel(int levelIndex)
        {
            // Async load
            var asyncLoader = SceneManager.LoadSceneAsync(levelsConfig.levels[levelIndex], new LoadSceneParameters(LoadSceneMode.Additive));
            asyncLoader.completed += OnLevelLoaded;
        }

        private void OnLevelLoaded(AsyncOperation asyncOp)
        {
            // Level has finished loading, set current
            _currentLevelScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
            
            // Get current level component, should be root level
            foreach (var go in _currentLevelScene.GetRootGameObjects())
            {
                _currentLevel = go.GetComponent<Level>();
                if (_currentLevel != null)
                {
                    break;
                }
            }
            
            Assert.IsTrue(_currentLevel != null);
            _currentLevel.OnLevelFinished = OnLevelFinished;
        }
        
        private void OnLevelFinished(bool winState)
        {
            if (GameState.Instance && GameState.Instance.CurrentState != GameState.State.Playing)
            {
                return;
            }
            
            _currentLevel.OnLevelFinished = null;

            // Unload current level
            SceneManager.UnloadSceneAsync(_currentLevelScene);
            _currentLevel = null;

            // Load win/lose scene
            var asyncLoader = SceneManager.LoadSceneAsync(winState ? levelsConfig.winScene : levelsConfig.loseScene, new LoadSceneParameters(LoadSceneMode.Additive));
            if (asyncLoader != null)
            {
                asyncLoader.completed += OnEndLevelSceneLoaded;
            }
        }

        private void OnEndLevelSceneLoaded(AsyncOperation asyncOp)
        {
            // Win/lose scene has loaded
            var endLevelScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
            
            // Get level menu component, should be in root
            foreach (var go in endLevelScene.GetRootGameObjects())
            {
                var menu = go.GetComponent<EndLevelMenu>();
                if (menu == null)
                {
                    continue;
                }

                // Restart button callback
                menu.RestartLevel = () =>
                {
                    SceneManager.UnloadSceneAsync(endLevelScene);
                    LoadLevel(GameConfig.Level);
                };
            }
        }
    }
}
