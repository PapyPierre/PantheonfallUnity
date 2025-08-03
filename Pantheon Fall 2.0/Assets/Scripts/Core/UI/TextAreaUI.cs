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

        public bool IsShowingActionsOrLoot { get; set; }

        private void Awake()
        {
            m_gm = GameManager.instance;
            textTMP.text = string.Empty;
        }

        private void Start()
        {
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
            
            DisplayText();
        }

        public void OnActionBtnPressed(int actionIndex)
        {
            Debug.Log($"OnActionBtnPressed: {actionIndex}");
            m_gm.fightManager.Player.DoAction(actionIndex);
        }

        public void DisplayActions()
        {
            IsShowingActionsOrLoot = true;

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

            IsShowingActionsOrLoot = true;

            for (int i = 0; i < m_gm.fightManager.Player.AvailableAbilities.Count; i++)
            {
                EAbilities ability = m_gm.fightManager.Player.AvailableAbilities[i];

                AbilityData abilityData = DataManager.GetData<AbilityData>(ability.ToString());

                actionsButtons[i].gameObject.SetActive(true);
                actionsButtons[i].interactable =
                    abilityData.manaCost <= m_gm.fightManager.Player.CurrentStats.currentMana;
                
                actionsButtonsTmp[i].text = abilityData.manaCost == 0 ?
                    $"{abilityData.abilityName}" : 
                    $"{abilityData.abilityName} ({abilityData.manaCost} MP)";
            }
        }

        public void DisplayItems() {}

        public void HideActions()
        {
            foreach (Button btn in actionsButtons)
            {
                btn.gameObject.SetActive(false);
            }

            IsShowingActionsOrLoot = false;
        }

        public void AddTextToDisplayQueue(TextToDisplay text)
        {
            m_textToDisplay.Enqueue(text);
            Debug.Log($"Enqueue: {text.text}");
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