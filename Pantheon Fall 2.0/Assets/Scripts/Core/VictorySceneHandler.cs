using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    public class VictorySceneHandler : MonoBehaviour
    {
        public void BackToMainMenu()
        {
            SceneManager.LoadScene("MainMenuScene");
        }
    }
}