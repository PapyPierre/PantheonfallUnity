using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public class TextAreaUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textTMP;
        [SerializeField] private Button[] actionsButtons;
        [SerializeField] private TextMeshProUGUI[] actionsButtonsTmp;
        
        public void OnActionBtnPressed(int actionIndex)
        {
             GameManager.instance.fightManager.Player.DoAction(actionIndex);
        }
        
        public void DisplayAvailableActions()
        {
            
        }
        
        private void EnableActionBtn(int index, string actionText)
        {
            actionsButtons[index].gameObject.SetActive(true);
            actionsButtonsTmp[index].text = actionText;
        }
    }
}
