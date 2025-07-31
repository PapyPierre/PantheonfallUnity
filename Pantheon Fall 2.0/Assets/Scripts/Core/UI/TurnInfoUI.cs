using TMPro;
using UnityEngine;

namespace Core.UI
{
    public class TurnInfoUI : MonoBehaviour
    {
        private Director m_lvlManager; 
            
        [SerializeField] private TextMeshProUGUI turnNumberText;
        [SerializeField] private GameObject[] tickIndicator;

        private void Awake()
        {
            m_lvlManager = GameManager.instance.director;
            m_lvlManager.TurnPass += OnTurnPass;
            m_lvlManager.TickExectue += OnTickExecute;
            
            foreach (GameObject go in tickIndicator)
            {
                go.SetActive(false);
            }
        }

        private void OnTurnPass(int turnNumber)
        {
            turnNumberText.text = $"Turn {turnNumber}";
            tickIndicator[turnNumber % 4].SetActive(true);
        }
        
        private void OnTickExecute()
        {
            //TODO Feedback
            
            foreach (GameObject go in tickIndicator)
            {
                go.SetActive(false);
            }
        }
    }
}