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
            if (Input.GetMouseButtonDown(0))
            {
                if (!GameManager.instance.uiManager.TextArea.IsShowingActions)
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
                m_gm.fightManager.TryStartNextTurn();
            }
            else
            {
                m_gm.uiManager.TextArea.DisplayText();
            }
        }
    }
}