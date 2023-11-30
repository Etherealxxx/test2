using Newtonsoft.Json;

namespace speed
{
    class MainMenu
    {
        protected static List<User> Users;
        public static void Main()
        {
            if (!File.Exists("stats.json"))
            {
                File.Create("stats.json");
            }
            Users = JsonConvert.DeserializeObject<List<User>>(File.ReadAllText("stats.json"));
            if (Users == null)
                Users = new List<User>();
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.White;
                if (Users.Count > 1)
                {
                    SortUsers();
                }
                static string Player()
                {
                    Console.WriteLine("Введите имя для таблицы рекордов:");
                    string name = Console.ReadLine()
                        ?? throw new ArgumentException("Ошибка при введении имени");
                    return name;
                }
                SpeedTestManager.Registration();
                File.WriteAllText("stats.json", JsonConvert.SerializeObject(Users));
            }
        }
        protected static bool UserExist(string name)
        {
            foreach (var user in Users)
            {
                if (user.Name == name)
                    return true;
            }
            return false;
        }
        protected static void SortUsers()
        {
            Users = (List<User>)Users.OrderByDescending(x => x.Id);
        }
    }
    class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Result { get; set; }
        public User(int id, string name, int result)
        {
            Id = id;
            Name = name;
            Result = result;
        }

        public string Data()
        {
            return "Имя: " + Name + "Символов в минуту: " + Result;
        }
    }
    class SpeedTestManager : MainMenu
    {
        private static bool testInPtogress = false;
        private static string text = "Пипипупу тутуту очень глупый текст ничего не работает помогите пожалуйста.";
        private static int ind = 0, time = 0;
        public static void Registration()
        {
            Console.Clear();
            Console.WriteLine("Введите имя:");
            string name = Console.ReadLine();
            if (name == "")
            {
                Registration();
            }
            if (!UserExist(name))
            {
                Start(name);
            }
            else Registration();
        }
        public static void Start(string name)
        {
            ConsoleKeyInfo ch;
            do
            {
                Console.Clear();
                Console.WriteLine(text);
                Console.WriteLine("Как только будете готовы - нажмите Enter");
                ch = Console.ReadKey(true);
            } while (ch.Key != ConsoleKey.Enter);
            testInPtogress = true;
            new Thread(Timer).Start();
            while (testInPtogress)
            {
                ch = Console.ReadKey(true);
                char chr = text[ind];
                if (ch.KeyChar.ToString() == chr.ToString())
                {
                    int sop = ind / 120, pos = ind % 120;
                    Console.SetCursorPosition(ind % 120, ind / 120);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(ch.KeyChar);
                    ind++;
                    if (ind - 1 == text.Length)
                        testInPtogress = false;
                }

            }
            Console.SetCursorPosition(0, 20);
            Console.WriteLine("Тест окончен");
            Console.ReadKey(true);
            Users.Add(new User(Users.Count + 1, name, ind / (60 - time) * 60));

        }
        private static void Timer()
        {
            time = 60;
            do 
            {
                Console.SetCursorPosition(20, 15);
                Console.WriteLine("    ");
                Console.SetCursorPosition(20, 15);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(time == 60 ? "1:00" : $"0:" + time);
                Thread.Sleep(1000);
                time--;
                if (time == 0) testInPtogress = false;
            } while (testInPtogress) ;
            
        }
    }
}
