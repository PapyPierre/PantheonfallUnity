using TMPro;
using UnityEngine;

namespace Core.UI
{
    public class TurnInfoUI : MonoBehaviour
    {
        private FightManager m_fightManager;

        [SerializeField] private TextMeshProUGUI turnNumberText;

        private void Awake()
        {
            m_fightManager = GameManager.instance.fightManager;
            m_fightManager.TurnPass += OnTurnPass;
        }

        public void HideAllTurnInfos()
        {
            turnNumberText.gameObject.SetActive(false);
        }

        private void OnTurnPass(int turnNumber)
        {
            turnNumberText.text = $"Turn {turnNumber}";
        }

        public void EnableTurnNumberText()
        {
            turnNumberText.gameObject.SetActive(true);
        }
    }
}