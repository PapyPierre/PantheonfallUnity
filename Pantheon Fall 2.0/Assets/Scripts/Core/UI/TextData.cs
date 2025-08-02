using System;
using UnityEngine;

namespace Core.UI
{
    [CreateAssetMenu(fileName = "Text Data", menuName = "ScriptableObjects/Text Data", order = 1)]
    public class TextData : ScriptableObject
    {
        public TextToDisplay[] textsToDisplays;
    }

    [Serializable]
    public class TextToDisplay
    {
        [TextArea] public string text;
        public bool isFinalIntroText;
        public Action FeedbackOnRead;

        public TextToDisplay(string text, Action feedbackOnRead = null)
        {
            this.text = text;
            FeedbackOnRead = feedbackOnRead;
        }
    }
}