using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace RecipesCore.Processors
{
    // The source of stopwords in file stop_words.json is https://github.com/stopwords-iso/stopwords-en
    public class StopWords
    {
        private HashSet<string> _wordsSet;

        public StopWords(string file)
        {
            if (!File.Exists(file))
                throw new FileNotFoundException("File " + file + " does not exists");
            var jsonContent = File.ReadAllText(file);
            var stopWordsFromJson = JsonConvert.DeserializeObject<List<string>>(jsonContent);//JObject.Parse(jsonContent);
            _wordsSet = new HashSet<string>(stopWordsFromJson);
        }

        public bool IsStopWord(string word)
        {
            return _wordsSet.Contains(word);
        }
    }
}