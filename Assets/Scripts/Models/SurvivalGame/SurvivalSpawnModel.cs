using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Models.SurvivalGame
{
    public class SurvivalSpawnModel
    {
        private bool _canSpawn;
        private int _letterIndex;
        private int _balloonCount;
        private int _skinBalloonsCount;
        private int _positionCount;
        
        private List<char> _lettersInWord;
        private List<char> _otherLetters;
        private List<int> _spawnPosIndexes;

        private const float MinBalloonSize = 0.5f;
        private const float MaxBalloonSize = 1.5f;

        public bool CanSpawn
        {
            get => _canSpawn;
            set => _canSpawn = value;
        }

        public SurvivalSpawnModel(int skinCount, int posCount)
        {
            _balloonCount = 0;
            _skinBalloonsCount = skinCount;
            _positionCount = posCount;
        }

        public void SetWord(string word)
        {
            _letterIndex = 0;

            _lettersInWord = new List<char>(WordToLetterList(word));
            _otherLetters = new List<char>(GetRemainingLetters(word));
        }

        public float GetRandomSize()
        {
            float size = Random.Range(MinBalloonSize, MaxBalloonSize);

            return size;
        }

        public char GetLetter()
        {
            bool value = _balloonCount % 2 == 0;

            _balloonCount++;

            if (value)
            {
                int index = Random.Range(0, _otherLetters.Count);

                return _otherLetters[index];
            }
            else
            {
                char letter = _lettersInWord[_letterIndex];
                _letterIndex++;

                if (_letterIndex == _lettersInWord.Count)
                {
                    _letterIndex = 0;
                }

                return letter;
            }
        }

        public int GetBalloonSpriteIndex()
        {
            int index = Random.Range(0, _skinBalloonsCount);

            return index;
        }

        public int GetBalloonPositionIndex()
        {
            if (_spawnPosIndexes == null || _spawnPosIndexes.Count == 0)
            {
                FillSpawnPositionIndexes();
            }

            int index = _spawnPosIndexes[0];
            _spawnPosIndexes.RemoveAt(0);

            return index;
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
        
        private List<char> GetRemainingLetters(string word)
        {
            List<char> allLetters = new List<char>
            {
                'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
            };
            
            List<char> wordLetters = new List<char>(word.ToUpper().ToCharArray());
            
            foreach (char letter in wordLetters)
            {
                allLetters.Remove(letter);
            }

            return allLetters;
        }

        private void FillSpawnPositionIndexes()
        {
            List<int> spawnPos = new List<int>();
            _spawnPosIndexes = new List<int>();

            for (int i = 0; i < _positionCount; i++)
            {
                spawnPos.Add(i);
            }

            var random = new System.Random();
            _spawnPosIndexes.AddRange(spawnPos.OrderBy(item => random.Next()).ToList());
        }
    }
}