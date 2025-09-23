using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace Models.Scenes
{
    public class SurvivalGameSceneModel
    {
        private bool _isNewBest;
        private int _maxWordIndex;
        private int _wordIndex;
        private int _healthCount;
        private int _wordsCount;
        
        private string _currentWord;
        private string _displayText;
        
        private List<char> _currentLetters;
        private List<string> _colorLetters;
        private List<bool> _stateLetters;

        private const string BestWordsKey = "SurvivalGameSceneModel.BestWords";

        public string CurrentWord => _currentWord;
        public string DisplayText => _displayText;
        public bool CanContinueGame => _healthCount > 0;
        public bool IsNewBest => _isNewBest;
        public int HealthCount => _healthCount;
        public int CurrentWords => _wordsCount;

        public int BestWords
        {
            get => PlayerPrefs.GetInt(BestWordsKey, 0);
            private set => PlayerPrefs.SetInt(BestWordsKey, value);
        }

        public SurvivalGameSceneModel()
        {
            _wordsCount = 0;
            _maxWordIndex = -1;
            _healthCount = 5;
            _maxWordIndex = WordsUtility.GetSurvivalWordsCount();
            
            SetNewWord();
        }
        
        public void SetNewWord()
        {
            _wordIndex++;

            if (_wordIndex == _maxWordIndex)
            {
                _wordIndex = 0;
            }

            _currentWord = WordsUtility.GetSurvivalWord(_wordIndex);

            _currentLetters = new List<char>(WordToLetterList(_currentWord));
            InitializeStateLetters();
            InitializeColorLetters();
            UpdateDisplayText();
        }

        public void SubtractHealth()
        {
            _healthCount--;
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
                
                TryUpdateBestWords();
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

        private void TryUpdateBestWords()
        {
            if (_wordsCount <= BestWords)
            {
                return;
            }
            
            BestWords = _wordsCount;
            _isNewBest = true;
        }
    }
}