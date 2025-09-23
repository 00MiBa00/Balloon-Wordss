using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace Models.Scenes
{
    public class SprintGameSceneModel
    {
        private int _maxWordIndex;
        private int _wordsCount;
        private int _gameSec;
        
        private string _currentWord;
        private string _displayText;
        
        private List<char> _currentLetters;
        private List<string> _colorLetters;
        private List<bool> _stateLetters;

        private const string LevelIndexKey = "SprintGameSceneModel.LevelIndex";

        public string CurrentWord => _currentWord;
        public string DisplayText => _displayText;
        public bool HaveTime => _gameSec > 0;
        public int SecLeft => _gameSec;

        public int LevelIndex
        {
            get => PlayerPrefs.GetInt(LevelIndexKey, 1);
            private set => PlayerPrefs.SetInt(LevelIndexKey, value);
        }

        public SprintGameSceneModel()
        {
            _wordsCount = 0;
            _maxWordIndex = WordsUtility.GetSprintWordsCount();
            
            SetNewWord();
            SetTime();
        }

        public void SubtractTime()
        {
            _gameSec--;
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
        
        private void SetNewWord()
        {
            _currentWord = WordsUtility.GetSprintWord(LevelIndex);
            
            Debug.Log(_currentWord);

            _currentLetters = new List<char>(WordToLetterList(_currentWord));
            InitializeStateLetters();
            InitializeColorLetters();
            UpdateDisplayText();
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

        private void SetTime()
        {
            _gameSec = LevelIndex >= 30 ? 30 : 40 - LevelIndex;
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