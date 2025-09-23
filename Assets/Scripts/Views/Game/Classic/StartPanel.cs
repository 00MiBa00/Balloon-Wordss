using UnityEngine;
using UnityEngine.UI;
using Views.General;

namespace Views.Game.Classic
{
    public class StartPanel : PanelView
    {
        [SerializeField]
        private Text _levelCounterText;
        [SerializeField] 
        private Text _wordText;

        public void SetLevelText(int value)
        {
            _levelCounterText.text = value.ToString();
        }

        public void SetWordText(string word)
        {
            _wordText.text = word;
        }
    }
}