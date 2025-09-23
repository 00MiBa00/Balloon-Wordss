using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
    public static class WordsUtility
    {
        [System.Serializable]
        public class WordListWrapper
        {
            public string[] words;
        }

        private static List<string> words;

        private static void LoadWords(string pathFile)
        {
            TextAsset jsonText = Resources.Load<TextAsset>(pathFile);

            if (jsonText != null)
            {
                WordListWrapper wordListWrapper = JsonUtility.FromJson<WordListWrapper>(jsonText.text);
                words = new List<string>(wordListWrapper.words);
            }
            else
            {
                Debug.LogError("File not found! Убедитесь, что файл words.json находится в папке Resources.");
            }
        }

        public static int GetSurvivalWordsCount()
        {
            if (words == null || words.Count == 0)
            {
                LoadWords("words");
            }

            if (words.Count > 0)
            {
                return words.Count;
            }
            else
            {
                Debug.LogError("Список слов не загружен или пуст.");
                return 0;
            }
        }

        public static int GetClassicWordsCount()
        {
            if (words == null || words.Count == 0)
            {
                LoadWords("classicwords");
            }

            if (words.Count > 0)
            {
                return words.Count;
            }
            else
            {
                Debug.LogError("Список слов не загружен или пуст.");
                return 0;
            }
        }
        
        public static int GetSprintWordsCount()
        {
            if (words == null || words.Count == 0)
            {
                LoadWords("sprintwords");
            }

            if (words.Count > 0)
            {
                return words.Count;
            }
            else
            {
                Debug.LogError("Список слов не загружен или пуст.");
                return 0;
            }
        }

        public static string GetSurvivalWord(int index)
        {
            if (words == null || words.Count == 0)
            {
                LoadWords("words");
            }

            if (words.Count > 0 && index >= 0 && index < words.Count)
            {
                return words[index].ToUpper();
            }
            else
            {
                Debug.LogError("Список слов не загружен или индекс выходит за пределы.");
                return null;
            }
        }

        public static string GetClassicWord(int index)
        {
            if (words == null || words.Count == 0)
            {
                LoadWords("classicwords");
            }

            if (words.Count > 0 && index >= 0 && index < words.Count)
            {
                return words[index].ToUpper();
            }
            else
            {
                Debug.LogError("Список слов не загружен или индекс выходит за пределы.");
                return null;
            }
        }

        public static string GetSprintWord(int index)
        {
            Debug.Log(words.Count);
            
            if (words == null || words.Count == 0)
            {
                LoadWords("sprintwords");
            }

            if (words.Count > 0 && index >= 0 && index < words.Count)
            {
                Debug.Log(index);
                
                return words[index].ToUpper();
            }
            else
            {
                Debug.LogError("Список слов не загружен или индекс выходит за пределы.");
                return null;
            }
        }
    }
}