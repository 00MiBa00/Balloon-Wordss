using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Views.General;

namespace Views.Game.Sprint
{
    public class ResultPanel : PanelView
    {
        [SerializeField] 
        private Text _levelText;
        [SerializeField] 
        private Text _timerText;
        [SerializeField] 
        private TextMeshProUGUI _descriptionText;
        [SerializeField] 
        private List<Color> _descriptionColors;
        [SerializeField] 
        private List<Sprite> _nextOrRestartBtnSprites;

        [SerializeField] private List<Color> _timerColors;

        public void SetLevelText(int value)
        {
            _levelText.text = value.ToString();
        }

        public void SetNextOrRestartBtnState(bool value)
        {
            base.Btns[1].image.sprite = value ? _nextOrRestartBtnSprites[0] : _nextOrRestartBtnSprites[1];
        }

        public void SetDescription(bool value)
        {
            string description = value ? "CONGRATULATIONS!" : "MAYBE NEXT TIME!";

            _descriptionText.color = value ? _descriptionColors[0] : _descriptionColors[1];

            _descriptionText.text = description;
        }

        public void SetTimerState(bool value, int sec)
        {
            _timerText.text = $"00:{sec:00}";
            _timerText.color = value ? _timerColors[0] : _timerColors[1];
        }
    }
}