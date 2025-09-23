using TMPro;
using UnityEngine;

namespace Views.Game.Sprint
{
    public class TimerUpdater : MonoBehaviour
    {
        [SerializeField] 
        private TextMeshProUGUI _timerText;

        public void UpdateTime(int sec)
        {
            _timerText.text = $"00:{sec:00}";
        }
    }
}