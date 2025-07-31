using UnityEngine;

namespace Core.UI
{
    public class UIManager : MonoBehaviour
    {
        public PlayerStatsUI PlayerStats { get; private set; }
        public EnemyInfoUI EnemyInfo { get; private set; }
        public TurnInfoUI TurnInfo { get; private set; }
        public TextAreaUI TextArea{ get; private set; }

        private void Awake()
        {
            PlayerStats = GetComponent<PlayerStatsUI>();
            EnemyInfo = GetComponent<EnemyInfoUI>();
            TurnInfo = GetComponent<TurnInfoUI>();
            TextArea = GetComponentInChildren<TextAreaUI>();
            
            GameManager.instance.uiManager = this;
        }
    }
}
