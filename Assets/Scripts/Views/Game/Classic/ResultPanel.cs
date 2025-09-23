using UnityEngine;
using UnityEngine.UI;
using Views.General;

namespace Views.Game.Classic
{
    public class ResultPanel : PanelView
    {
        [SerializeField] 
        private Text _levelText;

        public void SetLevelText(int value)
        {
            _levelText.text = value.ToString();
        }
    }
}