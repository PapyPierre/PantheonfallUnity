using TMPro;
using UnityEngine;

namespace Core.UI
{
    public class TurnInfoUI : MonoBehaviour
    {
        private FightManager m_fightManager;

        [SerializeField] private TextMeshProUGUI turnNumberText;
        [SerializeField] private GameObject[] tickIndicator;

        private void Awake()
        {
            m_fightManager = GameManager.instance.fightManager;
            m_fightManager.TurnPass += OnTurnPass;
            m_fightManager.TickExecute += OnTickExecute;
        }

        public void HideAllTurnInfos()
        {
            HideAllTickIndicators();

            turnNumberText.gameObject.SetActive(false);
        }

        private void HideAllTickIndicators()
        {
            foreach (GameObject go in tickIndicator)
            {
                go.SetActive(false);
            }
        }

        private void OnTurnPass(int turnNumber)
        {
            turnNumberText.text = $"Turn {turnNumber}";
            if (turnNumber % 4 != 0) tickIndicator[m_fightManager.TickNumber - 1].SetActive(true);
        }

        public void EnableTurnNumberText()
        {
            turnNumberText.gameObject.SetActive(true);
        }

        private void OnTickExecute()
        {
            //TODO Feedback

            HideAllTickIndicators();
        }
    }
}