using UnityEngine;
using TMPro;

namespace Views.Game
{
    public class WordDisplay : MonoBehaviour
    {
        [SerializeField] 
        private TextMeshProUGUI _text;

        public void UpdateWord(string word)
        {
            _text.text = word;
        }
    }
}