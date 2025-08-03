using System.Collections.Generic;
using Core.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;
        
        [HideInInspector] public FightManager fightManager;
        [HideInInspector] public UIManager uiManager;
        [HideInInspector] public Director director;
        [HideInInspector] public InputHandler inputHandler;

        private readonly List<GameObject> m_dontDestroyOnLoadObjects = new();
        
        [HideInInspector] public bool gameIsOn;
        
        #region Initialization
        
        private async void Awake()
        {
            if (instance is null)
            {
                instance = this;
                AddToDontDestroyOnLoad(gameObject);
            }
            else Debug.LogWarning("More than one game manager in scene.");

            SetObjsToDontDestroyOnLoad();
            
            Application.targetFrameRate = 120;

            await SceneManager.LoadSceneAsync("MainMenuScene");
        }

        public void AddToDontDestroyOnLoad(GameObject go) => m_dontDestroyOnLoadObjects.Add(go);

        private void SetObjsToDontDestroyOnLoad()
        {
            foreach (GameObject go in m_dontDestroyOnLoadObjects)
            {
                DontDestroyOnLoad(go);
            }
        }
        
        #endregion
    }
}