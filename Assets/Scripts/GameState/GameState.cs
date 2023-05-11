using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Chimera
{
    /// <summary>
    /// Manages the global game state, between MainMenu, Playing, Paused, etc.
    /// Loads and Unloads the scenes associated with these states.
    /// </summary>
    public class GameState : MonoBehaviour
    {
        // Global instance
        public static GameState Instance { get; private set; }
        
        // Game states. Index points to the respective scenes in the Build Scenes list
        public enum State
        {
            None = -1,
            MainMenu,
            Playing,
            Paused,
            Exit
        }

        // Current state setter / getter
        private State _currentState = State.None;
        public State CurrentState
        {
            set
            {
                // Already in this state
                if (_currentState == value)
                {
                    return;
                }
                
                // Quit state
                if (value == State.Exit)
                {
                    Application.Quit();
                    return;
                }

                switch (value)
                {
                    case State.MainMenu:
                        if (SceneManager.GetActiveScene().buildIndex != (int)value)
                        {
                            SceneManager.LoadScene((int)value, LoadSceneMode.Single);
                        }
                        Time.timeScale = 1;
                        break;
                    
                    case State.Playing:
                        if (_currentState == State.Paused)
                        {
                            SceneManager.UnloadSceneAsync((int)State.Paused);
                        }
                        else
                        {
                            if (SceneManager.GetActiveScene().buildIndex != (int)value)
                            {
                                SceneManager.LoadScene((int)value, LoadSceneMode.Single);
                            }
                        }
                        Time.timeScale = 1;
                        break;
                    
                    case State.Paused:
                        if (SceneManager.GetActiveScene().buildIndex != (int)value)
                        {
                            SceneManager.LoadScene((int)value, LoadSceneMode.Additive);
                        }
                        Time.timeScale = 0;
                        break;
                    
                    default:
                        throw new NotImplementedException($"Game State not implemented: {value}");
                }
                
                _currentState = value;
            }
        }

        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            // Start with main menu
            if (_currentState == State.None)
            {
                CurrentState = State.MainMenu;
            }
        }

        private void Update()
        {
            // Escape key pauses a running game
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                switch (_currentState)
                {
                    case State.Playing:
                        CurrentState = State.Paused;
                        break;
                    
                    case State.Paused:
                        CurrentState = State.Playing;
                        break;
                }
            }
        }
    }
}
