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
            GameState.Instance.CurrentState = GameState.State.Playing;
            LoadLevel(GameConfig.Level);
        }

        private void LoadLevel(int levelIndex)
        {
            Assert.IsTrue(levelsConfig != null && levelsConfig.levels.Count > 0);
            
            SceneManager.LoadScene(levelsConfig.levels[levelIndex].name, LoadSceneMode.Additive);
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

            if (winState)
            {
                SceneManager.LoadScene(levelsConfig.winScene.name, LoadSceneMode.Additive);
            }
            else
            {
                SceneManager.LoadScene(levelsConfig.loseScene.name, LoadSceneMode.Additive);
            }
            
            var scene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
            foreach (var go in _currentLevelScene.GetRootGameObjects())
            {
                var menu = go.GetComponent<EndLevelMenu>();
                if (menu == null)
                {
                    continue;
                }

                menu.RestartLevel = () =>
                {
                    LoadLevel(GameConfig.Level);
                };

                menu.LoadNextLevel = () =>
                {
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
