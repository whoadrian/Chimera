using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

namespace Chimera
{
    public class LevelManager : MonoBehaviour
    {
        public LevelsConfig levelsConfig;

        private Level _currentLevel;
        private Scene _currentLevelScene;
        
        private void Start()
        {
            Assert.IsTrue(levelsConfig != null && levelsConfig.levels.Count > 0);
            GameState.Instance.CurrentState = GameState.State.Playing;
            LoadLevel(GameConfig.Level);
        }

        private void LoadLevel(int levelIndex)
        {
            var asyncLoader = SceneManager.LoadSceneAsync(levelsConfig.levels[levelIndex].name, new LoadSceneParameters(LoadSceneMode.Additive));
            asyncLoader.completed += OnLevelLoaded;
        }

        private void OnLevelLoaded(AsyncOperation asyncOp)
        {
            _currentLevelScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
            
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
            SceneManager.UnloadSceneAsync(_currentLevelScene);
            _currentLevel = null;

            var asyncLoader = SceneManager.LoadSceneAsync(winState ? levelsConfig.winScene.name : levelsConfig.loseScene.name, new LoadSceneParameters(LoadSceneMode.Additive));
            asyncLoader.completed += OnEndLevelSceneLoaded;
        }

        private void OnEndLevelSceneLoaded(AsyncOperation asyncOp)
        {
            var endLevelScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
            
            foreach (var go in _currentLevelScene.GetRootGameObjects())
            {
                var menu = go.GetComponent<EndLevelMenu>();
                if (menu == null)
                {
                    continue;
                }

                menu.RestartLevel = () =>
                {
                    SceneManager.UnloadSceneAsync(endLevelScene);
                    LoadLevel(GameConfig.Level);
                };

                menu.LoadNextLevel = () =>
                {
                    SceneManager.UnloadSceneAsync(endLevelScene);
                    
                    var levelIndex = GameConfig.Level + 1;
                    if (levelIndex >= levelsConfig.levels.Count)
                    {
                        levelIndex = 0;
                    }

                    GameConfig.Level = levelIndex;
                    LoadLevel(levelIndex);
                };
            }
        }
    }
}
