using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using UnityEditor;
using System;

public static class JSONReader
{
    
    // Start is called before the first frame update
    public static List<Level> GetLevelsFromJson(TextAsset text)
    {
        // read text file
        string data = File.ReadAllText(AssetDatabase.GetAssetPath(text));

        // deserialize
        RootObject rootObj =  JsonConvert.DeserializeObject<RootObject>(data);

        // seperate words
        foreach (var item in rootObj.level)
        {
            item.wordsList = GetSeperateWordsFromString(item.wordsString, '|', ',');
        }
        
        return rootObj.level;

    }

    private static List<Word> GetSeperateWordsFromString(string wordsString, char wordSeperator, char wordPropertySeperator)
    {
        List<Word> words = new List<Word>();

        while(wordsString != null && wordsString.Length > 0)
        {
            int wordSeperatorLocation = wordsString.IndexOf(wordSeperator, StringComparison.Ordinal);
            if(wordSeperatorLocation > -1 || (wordSeperatorLocation == -1 && wordsString.Length > 0))
            {
                string word;
                bool breakNow = false;

                // get word's properties
                if (wordSeperatorLocation != -1)
                    word = wordsString.Substring(0, wordSeperatorLocation);
                else
                {
                    word = wordsString;
                    breakNow = true;
                }
                    
               
                // add word to list
                int wordPropertySeperatorLocation = word.IndexOf(wordPropertySeperator, StringComparison.Ordinal);
                int row = int.Parse(word.Substring(0, wordPropertySeperatorLocation));
                word = word.Substring(wordPropertySeperatorLocation + 1);
                
                wordPropertySeperatorLocation = word.IndexOf(wordPropertySeperator, StringComparison.Ordinal);
                int column = int.Parse(word.Substring(0, wordPropertySeperatorLocation));
                word = word.Substring(wordPropertySeperatorLocation + 1);

                wordPropertySeperatorLocation = word.IndexOf(wordPropertySeperator, StringComparison.Ordinal);
                string characters = word.Substring(0, wordPropertySeperatorLocation);
                word = word.Substring(wordPropertySeperatorLocation + 1);
                
                string boolHor = word;

                words.Add(new Word(row, column, characters, boolHor.Equals("H")));
             
                // remove word from string
                wordsString = wordsString.Substring(wordSeperatorLocation + 1);
                
                if (breakNow)
                    break; 
            }
        }

        return words;
    }

    public class RootObject 
    {
        [JsonProperty("Levels")]
        public List<Level> level { get; set; }
    }

    public class Level
    {
        [JsonProperty("Row")]
        public string row { get; set; }
        [JsonProperty("Column")]
        public string column { get; set; }
        [JsonProperty("Words")]
        public string wordsString { get; set; }

        public List<Word> wordsList { get; set; }
    }
    public class Word
    {
        public int startRow { get; set; }
        public int startColumn { get; set; }
        public string characters { get; set; }
        public bool isHorizontal { get; set; }

        public Word(int startRow, int startColumn, string word, bool isHorizontal) 
        {
            this.startRow = startRow;
            this.startColumn = startColumn;
            this.characters = word;
            this.isHorizontal = isHorizontal;
        }
        
    }
    
}
