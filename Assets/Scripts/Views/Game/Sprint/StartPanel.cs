using UnityEngine;
using UnityEngine.UI;
using Views.General;

namespace Views.Game.Sprint
{
    public class StartPanel : PanelView
    {
        [SerializeField]
        private Text _levelCounterText;
        [SerializeField] 
        private Text _wordText;
        [SerializeField] 
        private Text _timeText;

        public void SetLevelText(int value)
        {
            _levelCounterText.text = value.ToString();
        }

        public void SetWordText(string word)
        {
            _wordText.text = word;
        }

        public void SetTime(int sec)
        {
            _timeText.text = $"00:{sec}";
        }
    }
}