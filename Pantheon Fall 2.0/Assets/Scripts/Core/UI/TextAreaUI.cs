using System;
using System.Collections.Generic;
using Core.Entity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public class TextAreaUI : MonoBehaviour
    {
        private GameManager m_gm;
        
        [Header("Text"), SerializeField] private TextMeshProUGUI textTMP;
        
        [Header("Actions"), SerializeField] private Button[] actionsButtons;
        [SerializeField] private TextMeshProUGUI[] actionsButtonsTmp;
        
        private readonly Queue<TextToDisplay> m_textToDisplay = new Queue<TextToDisplay>();
        private TextToDisplay m_textCurrentlyDisplayed;

        private void Start()
        {
            m_gm = GameManager.instance;
            HideCurrentText();
            ShowIntroText();
        }

        private void Update()
        {
            if (m_textCurrentlyDisplayed == null) return;
            if (Input.anyKeyDown) ShowNextTextInQueue();
        }

        private void ShowIntroText()
        {
            var introTextData = DataManager.GetData<TextData>("IntroText");

            foreach (TextToDisplay txt in introTextData.textsToDisplays)
            {
                AddTextToDisplayQueue(txt);
            }
        }

        public void OnActionBtnPressed(int actionIndex)
        {
            m_gm.fightManager.Player.DoAction(actionIndex);
        }
        
        public void DisplayActions()
        {
            textTMP.gameObject.SetActive(false);

            actionsButtons[0].gameObject.SetActive(true);
            actionsButtons[1].enabled = true;
            actionsButtonsTmp[0].text = "Use an ability";
            
            actionsButtons[1].gameObject.SetActive(true);
            actionsButtons[1].enabled = false;
            actionsButtonsTmp[1].text = "Use an item";

            actionsButtons[2].gameObject.SetActive(true);
            actionsButtons[1].enabled = true;
            actionsButtonsTmp[2].text = "Stand by";
        }

        public void DisplayAbilities()
        {
            HideActions();

            for (int i = 0; i < m_gm.fightManager.Player.AvailableAbilities.Count; i++)
            {
                EAbilities ability = m_gm.fightManager.Player.AvailableAbilities[i];
                
                actionsButtons[i].gameObject.SetActive(true);
                actionsButtons[i].enabled = true;
                actionsButtonsTmp[i].text = ability.ToString();
            }
        }
        
        public void DisplayItems()
        {
            
        }

        private void HideActions()
        {
            foreach (Button btn in actionsButtons)
            {
                btn.gameObject.SetActive(false);
            }
        }

        public void AddTextToDisplayQueue(TextToDisplay text)
        {
            m_textToDisplay.Enqueue(text);
            TryDisplayText();
        }
        
        private void TryDisplayText()
        {
            if (m_textCurrentlyDisplayed != null) return;

            HideActions();
            textTMP.gameObject.SetActive(true);
            textTMP.text = m_textToDisplay.Peek().text;
            Debug.Log(m_textToDisplay.Peek().text);
            m_textCurrentlyDisplayed = m_textToDisplay.Peek();
        }

        public void ShowNextTextInQueue()
        {
            HideCurrentText();
            if (m_textToDisplay.Count > 0) TryDisplayText();
        }

        private void HideCurrentText()
        {
            textTMP.text = string.Empty;
            m_textCurrentlyDisplayed = null;

            if (m_textToDisplay.Count > 0)
            {
                if (m_textToDisplay.Dequeue().isFinalIntroText)
                {
                    m_gm.director.IntroTextFinished.Invoke();
                }
            }
            
            if (m_textToDisplay.Count <= 0) DisplayActions();
        }
    }
}