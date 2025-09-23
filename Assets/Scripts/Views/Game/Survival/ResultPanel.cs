using UnityEngine;
using UnityEngine.UI;

using TMPro;
using Views.General;

namespace Views.Game.Survival
{
    public class ResultPanel : PanelView
    {
        [SerializeField] 
        private TextMeshProUGUI _description;
        [SerializeField] 
        private Text _bestScoreText;
        [SerializeField]
        private Text _currentScoreText;

        public void SetDescription(bool value)
        {
            string description = value ? "CONGRATULATIONS!" : "MAYBE NEXT TIME!";

            _description.text = description;
        }

        public void SetBestWords(int value)
        {
            _bestScoreText.text = value.ToString();
        }

        public void SetCurrentWords(int value)
        {
            _currentScoreText.text = value.ToString();
        }
    }
}