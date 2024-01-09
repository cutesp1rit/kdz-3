using LIBRARY;
using System.ComponentModel;

namespace APP
{
    // Максимов Тимофей, БПИ236-1, Вариант: 3
    // Считываемый файл должен быть в папке с экзешником!!
    // Оставил вам для тестирования файлик с названием "foryou"
    // По заданию у меня слова состоят только из русских и латинских букв (без запятых и прочих знаков)
    // Несмотря на то что мне кажется это не необходимым (так как и так будет работать красиво), я все равно его сделал
    // Но вы можете убрать это условие, тогда в словах будут присутствовать любые знаки (кроме пробелов и ";", очевидно)
    // Такое условие будет помечено как HERE (оно проверяется в методе Reading)
    internal class Program
    {
        /// <summary>
        /// Метод для чтение файла по переданному пути
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string[] Reading(string path)
        {
            // переменная для посчета строк в файле
            int kolStr = File.ReadAllText(path).Split('\n').Length;
            string[] massivStrings = new string[kolStr];
            // чтение файла и проверка данных
            using (StreamReader sr = new StreamReader(path))
            {
                int indexMassiv = 0;
                string line = sr.ReadLine(); // считывание первой строки
                if (line == null) // ловля ошибки (файл пуст)
                {
                    throw new ArgumentNullException();
                }
                while (line != null) // цикл для считывания всех строк
                {
                    for (int i = 0; i < line.Length; i++)
                    {
                        // проверка на то чтобы в строке были только латинские буквы, русские буквы, пробелы и ";". В ином случае вывожу такие данные, как некорректные
                        if (!((line[i] >= 'a' && line[i] <= 'z') || (line[i] >= 'A' && line[i] <= 'Z') || line[i] == ' ' || line[i] == ';' || (line[i] >= 'а' && line[i] <= 'я') || (line[i] >= 'А' && line[i] <= 'Я')))
                        {
                            throw new ArgumentNullException();
                        }
                    }

                    if (line.Length != 0)
                    {
                        // !!!!!!!!!!!!! HERE !!!!!!!!!!!!!
                        // если НЕ пустая строка заканчивается не на ';', то выбрасываем сообщение о некорректных данных
                        if (line[^1]!=';')
                        {
                            throw new ArgumentNullException();
                        }
                    }
                    massivStrings[indexMassiv] = line;
                    indexMassiv++;
                    line = sr.ReadLine();
                }
            }
            return massivStrings;
        }
        /// <summary>
        /// Подсчет НЕ пустых строк по переданному массиву
        /// </summary>
        /// <param name="strings"></param>
        /// <returns></returns>
        public static int AmountOfNotEmptyStrings(string[] strings)
        {
            int k = 0;
            foreach (string someString in strings)
            {
                if (someString == null)
                {
                    continue;
                }
                if (someString.Length != 0)
                {
                    k += 1;
                }
            }
            return k;
        }
        /// <summary>
        /// Метод для чтения и записи полученных данных/предложений
        /// </summary>
        /// <param name="allSentences"></param>
        public static void PrintAndWritingSentences(MyStrings[] allSentences)
        {
            int kolSentences = 0;
            for (int i = 0; i < allSentences.Length; i++)
            {
                string[] sentences = allSentences[i].Sentences;
                string[] acro = allSentences[i].ACRO;
                for (int j = 0; j < sentences.Length; j++)
                {
                    Console.WriteLine($"Предложение: \"{sentences[j]}\", его аббревиатура: \"{acro[j]}\"");
                    kolSentences++;
                }
            }
            string[] massivForWriting = new string[kolSentences];
            kolSentences = 0;
            for (int i = 0; i < allSentences.Length; i++)
            {
                string[] sentences = allSentences[i].Sentences;
                string[] acro = allSentences[i].ACRO;
                for (int j = 0; j < sentences.Length; j++)
                {
                    massivForWriting[kolSentences] = $"Предложение: \"{sentences[j]}\", его аббревиатура: \"{acro[j]}\"\n";
                    kolSentences++;
                }
            }

            WriteFile(massivForWriting);
        }
        /// <summary>
        /// Метод для записи данных в файл
        /// </summary>
        /// <param name="massivOfSentences"></param>
        public static void WriteFile(string[] massivOfSentences)
        {
            Console.WriteLine("Введите имя файла (без разрешения): ");
            while (true)
            {
                string name = Console.ReadLine();
                if (name == null || name.Length == 0) // Если пользователь не ввел имя, то запускаем цикл заново.
                {
                    Console.WriteLine("Вы не ввели название файла, пожалуйста, повторите ввод:");
                    continue;
                }
                string path = name + ".txt"; // создание пути для нового файла или поиска старого
                if (File.Exists(path))
                {
                    bool flag = true;
                    while (flag)
                    {
                        Console.WriteLine("Такой файл уже существует, потому определите дальнейшие действия с полученными данными:");
                        Console.WriteLine("\t1. Я хочу дописать данные.");
                        Console.WriteLine("\t2. Я хочу перезаписать данные.");
                        string var = Console.ReadLine();
                        switch (var)
                        {
                            case "1":
                                AddToFile(path, massivOfSentences);
                                return;
                            case "2":
                                flag = false;
                                break;
                            default:
                                Console.WriteLine("Введенное значение может быть от 1 до 2, как выбор пункта для запуска действия, повторите попытку.");
                                break;
                        }

                    }
                }
                try
                {
                    // создание файла и заполнение его данными
                    using (StreamWriter sw = File.CreateText(path))
                    {
                        for (int i = 0; i < massivOfSentences.Length; i++)
                        {
                            sw.Write(massivOfSentences[i]);
                        }
                    }

                    Console.WriteLine("Данные записаны успешно!");
                    break;
                }
                catch (IOException ex) // поимка одного из исключений
                {
                    Console.WriteLine("Введено некорректное название файла. Повторите попытку:");
                    continue;
                }
                catch (Exception ex) // поимка остальных
                {
                    Console.WriteLine("Возникла непредвиденная ошибка, повторите попытку:");
                    continue;
                }
            }
        }
        /// <summary>
        /// Метод, в случае если пользователь захочет ДОписать данные
        /// </summary>
        /// <param name="path"></param>
        /// <param name="massivOfSentences"></param>
        public static void AddToFile(string path, string[] massivOfSentences)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(path, true))
                {
                    for (int i = 0; i < massivOfSentences.Length; i++)
                    {
                        sw.Write(massivOfSentences[i]);
                    }
                }
                Console.WriteLine("Данные записаны успешно!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Возникла непредвиденная ошибка, повторите попытку. Перезапустите программу и выберете другой файл.");
            }
        }
        static void Main(string[] args)
        {
            do
            {
                Console.WriteLine("Введите имя файла, у которого разделитель предложений ';' (разрешение не указывайте): ");
                // Console.Write("а также его предложения состоят лишь из латинских и русских букв, пробелов: ");
                string path;
                string[] fileStrings;
                while (true)
                {
                    try
                    {
                        path = "." + Path.DirectorySeparatorChar + Console.ReadLine() + ".txt"; // назначение пути
                        fileStrings = Reading(path); // массив строк из файла
                        Console.WriteLine("Файл успешно считан.");
                        break;
                    }
                    catch (ArgumentNullException e)
                    {
                        Console.WriteLine("Файл отсутствует или его структура не соответствуют варианту. Повторите попытку: ");
                    }
                    catch (ArgumentException e)
                    {
                        Console.WriteLine("Возникла ошибка при открытии файла, повторите попытку: ");
                    }
                    catch (IOException e)
                    {
                        Console.WriteLine("Введено некорректное название файла или он находится не в текущей директории, повторите попытку: ");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Возникла непредвиденная ошибка, повторите попытку: ");
                    }
                }

                MyStrings[] allSentences = new MyStrings[AmountOfNotEmptyStrings(fileStrings)];
                int indexOfSentence = 0;
                for (int i = 0; i < fileStrings.Length; i++)
                {
                    if (fileStrings[i] == null)
                    {
                        continue;
                    }
                    if (fileStrings[i].Length != 0)
                    {
                        // заполнение массив объектов объектами с помощью объектов 
                        allSentences[indexOfSentence] = new MyStrings(fileStrings[i], ';');
                        indexOfSentence++;
                    }
                }
                PrintAndWritingSentences(allSentences);
                Console.WriteLine("Если хотите выйти из программы, то нажмите ECS, в ином случае она начнется заново...");
            } while (Console.ReadKey().Key != ConsoleKey.Escape);
        }
    }
}