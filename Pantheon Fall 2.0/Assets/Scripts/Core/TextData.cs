using System;
using UnityEngine;

namespace Core
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

        public TextToDisplay(string text)
        {
            this.text = text;
        }
    }
}