using UnityEngine;

namespace Core
{
    public class InputHandler : MonoBehaviour
    {
        private GameManager m_gm;

        private void Start()
        {
            m_gm = GameManager.instance;
            m_gm.inputHandler = this;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && m_gm.gameIsOn)
            {
                if (!GameManager.instance.uiManager.TextArea.IsShowingActionsOrLoot)
                {
                    Interact();
                }
            }
        }

        private void Interact()
        {
            m_gm.uiManager.TextArea.HideCurrentlyDisplayedText();

            if (m_gm.uiManager.TextArea.QueueIsEmpty())
            {
                m_gm.fightManager.TrySetNextEnemy();
            }
            else
            {
                m_gm.uiManager.TextArea.DisplayText();
            }
        }
    }
}