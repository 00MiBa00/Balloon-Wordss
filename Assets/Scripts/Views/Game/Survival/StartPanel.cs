using UnityEngine;
using UnityEngine.UI;
using Views.General;

namespace Views.Game.Survival
{
    public class StartPanel : PanelView
    {
        [SerializeField]
        private Text _bestWords;

        public void SetBestWords(int value)
        {
            _bestWords.text = value.ToString();
        }
    }
}