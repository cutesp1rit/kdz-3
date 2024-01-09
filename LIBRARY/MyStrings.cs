using System.Text;

namespace LIBRARY
{
    public class MyStrings
    {
        string[] _sentences;
        public MyStrings(string str, char ch) {
            if (str == null || ch == null)
            {
                _sentences = new string[0];
                return;
            }
            string[] tmpMassiv = str.Split(';');
            // невозможен исход, чтобы в конструктор поступала строка без ";" (так как я проверяю файл на такую корректность), однако если кто-то
            // захочет использовать библиотеку класса отдельно, то увидит данное сообщение, так как в ином случае может выброситься исключение
            if (tmpMassiv.Length == 1)
            {
                Console.WriteLine("То что вы пытаетесь сейчас сделать - нелегально. Подкорректируйте код в конструкторе MyStrings.");
                _sentences = new string[0];
                return;
            }
            // срез нужен, чтобы не учитывать пустое предложение, которое стоит после последней ;
            _sentences = tmpMassiv[..^1];
        }
        public MyStrings() { 
            _sentences = new string[0];
        }
        public string[] Sentences
        {
            get { return _sentences; }
        }

        /// <summary>
        /// Свойство для получения всех аббревиатур всех предложений
        /// </summary>
        public string[] ACRO
        {
            get
            {
                string[] abbreviation = new string[_sentences.Length];
                for (int i=0; i < _sentences.Length; i++)
                {
                    string[] words = _sentences[i].Split(' ');
                    StringBuilder someAbb = new StringBuilder();
                    foreach (string word in words)
                    {
                        // если есть пробелы в конце/начале или подряд, то не учитываем их как отдельные слова, так как они пустые
                        if (word.Length > 0)
                        {
                            someAbb.Append(word[0]);
                        }
                    }
                    abbreviation[i] = someAbb.ToString().ToUpper();
                }
                return abbreviation;
            }
        }
    }
}