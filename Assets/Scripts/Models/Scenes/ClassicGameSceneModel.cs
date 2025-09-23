using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace Models.Scenes
{
    public class ClassicGameSceneModel
    {
        private int _maxWordIndex;
        private int _wordsCount;
        
        private string _currentWord;
        private string _displayText;
        
        private List<char> _currentLetters;
        private List<string> _colorLetters;
        private List<bool> _stateLetters;

        private const string LevelIndexKey = "ClassicGameSceneModel.LevelIndex";

        public string CurrentWord => _currentWord;
        public string DisplayText => _displayText;

        public int LevelIndex
        {
            get => PlayerPrefs.GetInt(LevelIndexKey, 1);
            private set => PlayerPrefs.SetInt(LevelIndexKey, value);
        }

        public ClassicGameSceneModel()
        {
            _wordsCount = 0;
            _maxWordIndex = WordsUtility.GetClassicWordsCount();
            
            SetNewWord();
        }
        
        public void SetNewWord()
        {
            _currentWord = WordsUtility.GetClassicWord(LevelIndex);

            _currentLetters = new List<char>(WordToLetterList(_currentWord));
            InitializeStateLetters();
            InitializeColorLetters();
            UpdateDisplayText();
        }

        public void UpdateLevel()
        {
            LevelIndex++;

            if (LevelIndex == _maxWordIndex)
            {
                LevelIndex = 0;
            }
        }

        public bool TryChangeLetterState(char letter)
        {
            if (_currentLetters.Contains(letter))
            {
                List<int> indexes = new List<int>(FindLetterIndices(letter));

                foreach (var index in indexes)
                {
                    if (_stateLetters[index])
                    {
                        return false;
                    }

                    _stateLetters[index] = true;
                }

                InitializeColorLetters();
                UpdateDisplayText();

                return true;
            }

            return false;
        }

        public bool IsEndRound()
        {
            bool value = !_stateLetters.Contains(false);

            if (value)
            {
                _wordsCount++;
            }

            return value;
        }
        
        private List<char> WordToLetterList(string word)
        {
            List<char> letterList = new List<char>();
            
            foreach (char letter in word.ToCharArray())
            {
                letterList.Add(letter);
            }

            return letterList;
        }

        private List<int> FindLetterIndices(char letter)
        {
            List<int> indices = new List<int>();

            for (int i = 0; i < _currentWord.Length; i++)
            {
                if (_currentWord[i] == letter)
                {
                    indices.Add(i);
                }
            }

            return indices;
        }

        private void InitializeColorLetters()
        {
            _colorLetters = new List<string>();
            
            for (int i = 0; i < _stateLetters.Count; i++)
            {
                string letterColor = _stateLetters[i]
                    ? $"<color=yellow>{_currentLetters[i]}</color>"
                    : $"<color=white>{_currentLetters[i]}</color>";
                
                _colorLetters.Add(letterColor);
            }
        }

        private void InitializeStateLetters()
        {
            _stateLetters = new List<bool>();
            
            for (int i = 0; i < _currentLetters.Count; i++)
            {
                _stateLetters.Add(false);
            }
        }

        private void UpdateDisplayText()
        {
            _displayText = "";
            
            foreach (var letter in _colorLetters)
            {
                _displayText += letter;
            }
        }
    }
}