using System;
using System.Collections.Generic;
using Core.Entity;
using Core.Entity.Ability;
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
        
        public bool IsShowingActions { get; private set; }

        private void Start()
        {
            m_gm = GameManager.instance;
            
            textTMP.text = string.Empty;
            
            ShowIntroText();
        }

        public bool QueueIsEmpty() => m_textToDisplay.Count <= 0;

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
            Debug.Log($"OnActionBtnPressed: {actionIndex}");
            m_gm.fightManager.Player.DoAction(actionIndex);
        }

        public void DisplayActions()
        {
            IsShowingActions = true;
            
            HideCurrentlyDisplayedText();

            actionsButtons[0].gameObject.SetActive(true);
            actionsButtons[1].interactable = true;
            actionsButtonsTmp[0].text = "Use an ability";

            actionsButtons[1].gameObject.SetActive(true);
            actionsButtons[1].interactable = false;
            actionsButtonsTmp[1].text = "Use an item";
        }

        public void DisplayAbilities()
        {
            HideActions();
            
            IsShowingActions = true;
            
            for (int i = 0; i < m_gm.fightManager.Player.AvailableAbilities.Count; i++)
            {
                EAbilities ability = m_gm.fightManager.Player.AvailableAbilities[i];

                actionsButtons[i].gameObject.SetActive(true);
                actionsButtons[i].interactable = true;
                actionsButtonsTmp[i].text = ability.ToString();
            }
        }

        public void DisplayItems() {}

        public void HideActions()
        {
            foreach (Button btn in actionsButtons)
            {
                btn.gameObject.SetActive(false);
            }

            IsShowingActions = false;
        }

        public void AddTextToDisplayQueue(TextToDisplay text)
        {
            m_textToDisplay.Enqueue(text);
        }

        public void DisplayText()
        {
            m_textCurrentlyDisplayed = m_textToDisplay.Dequeue();
            textTMP.text = m_textCurrentlyDisplayed.text;
            Debug.Log(m_textCurrentlyDisplayed.text);
            m_textCurrentlyDisplayed.FeedbackOnRead?.Invoke();
        }

        public void HideCurrentlyDisplayedText()
        {
            if (m_textCurrentlyDisplayed == null) return;
            
            textTMP.text = string.Empty;

            if (m_textCurrentlyDisplayed.isFinalIntroText)
            {
                m_textCurrentlyDisplayed = null;
                m_gm.director.IntroTextFinished.Invoke();
                return;
            }
            
            m_textCurrentlyDisplayed = null;
        }
    }
}