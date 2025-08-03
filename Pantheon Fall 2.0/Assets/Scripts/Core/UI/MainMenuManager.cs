using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.UI
{
    public class MainMenuManager : MonoBehaviour
    {
        public void OnPlayBtnPressed()
        {
            Debug.Log("OnPlayBtnPressed"); 
            SceneManager.LoadScene("GameScene");
        }
        
        public void OnQuitBtnPressed()
        {
            Debug.Log("OnQuitBtnPressed");
            Application.Quit();
        }
    }
}
