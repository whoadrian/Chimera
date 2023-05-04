using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Chimera
{
    public class GameState : MonoBehaviour
    {
        private static GameState _instance;
        public static GameState Instance
        {
            get
            {
                if (_instance == null)
                {
                    const string name = "GameState";
                    var go = GameObject.Find(name);
                    
                    if (go == null)
                    {
                        go = new GameObject(name);
                        _instance = go.AddComponent<GameState>();
                        DontDestroyOnLoad(go);
                    }
                    else
                    {
                        _instance = go.GetComponent<GameState>();
                        DontDestroyOnLoad(go);
                    }
                }
                
                return _instance;
            }
        }
        
        public enum State
        {
            None = -1,
            MainMenu,
            Playing,
            Paused,
            Exit
        }

        private State _currentState = State.None;
        public State CurrentState
        {
            set
            {
                if (_currentState == value)
                {
                    return;
                }
                
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

        private void Start()
        {
            if (_currentState == State.None)
            {
                CurrentState = State.MainMenu;
            }
        }

        private void Update()
        {
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
