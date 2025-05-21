using System;

namespace Qns
{
    public class Program
    {
//-----------------------------------------Question 1-------------------------------------
        public static void GreetName(string name)
        {
            Console.WriteLine($"Hello {name}, Welcome aboard!!");
        }
        public static void Qn1()
        {
            Console.WriteLine("Enter the Name : ");
            string? name = Console.ReadLine();
            GreetName(name);
        }
//-----------------------------------------Question 2-----------------------------------------
        public static void GetInput(out int firstNo, out int secondNo)
        {
            Console.WriteLine("Enter the FirstNo : ");
            firstNo = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter the SecondNo : ");
            secondNo = Convert.ToInt32(Console.ReadLine());
        }
        public static void LargestNo(int firstNo, int secondNo)
        {
            if (firstNo < secondNo)
            {
                Console.WriteLine($"{secondNo} is the largest");
            }
            else if (firstNo > secondNo)
            {
                Console.WriteLine($"{firstNo} is the largest");
            }
            else
            {
                Console.WriteLine("Both are Equal");
            }
        }
        public static void Qn2()
        {
            int firstNo;
            int secondNo;
            GetInput(out firstNo, out secondNo);
            LargestNo(firstNo, secondNo);
        }
//--------------------------------------Question 3------------------------------------------
        public static void Operations(int firstNo, int secondNo)
        {
            double num1 = Convert.ToDouble(firstNo);
            double num2 = Convert.ToDouble(secondNo);
            Console.Write("Enter operation (+, -, *, /): ");
            char operation = Console.ReadLine()[0];

            double result = 0;
            bool valid = true;

            switch (operation)
            {
                case '+': result = num1 + num2; break;
                case '-': result = num1 - num2; break;
                case '*': result = num1 * num2; break;
                case '/': result = num2 != 0 ? num1 / num2 : double.NaN; break;
                default: valid = false; Console.WriteLine("Invalid operation."); break;
            }

            if (valid)
                Console.WriteLine($"Result: {result}");
        }
        public static void Qn3()
        {
            int firstNo;
            int secondNo;
            GetInput(out firstNo, out secondNo);
            Operations(firstNo, secondNo);
        }
//---------------------------------------Question 4--------------------------------------------------
        public static void GetStringInput(out string uname, out string passwd)
        {
            Console.WriteLine("Enter the UserName : ");
            uname = Console.ReadLine();
            Console.WriteLine("Enter the Password : ");
            passwd = Console.ReadLine();
        }
        public static void CheckUser(string uname, string passwd)
        {
            if (string.IsNullOrEmpty(uname) || string.IsNullOrEmpty(passwd))
            {
                Console.WriteLine("No Valid Input!");
                return;
            }
            else
            {
                int i = 3;
                while (i > 0)
                {
                    if (uname == "Admin" && passwd == "pass")
                    {
                        Console.WriteLine("Congrats, You are the One!!");
                        return;
                    }
                    else
                    {
                        i -= 1;
                        if (i == 0)
                        {
                            Console.WriteLine("Attempts Over!!");
                            return;
                        }
                        Console.WriteLine($"Wrong Input! {i} attempts left!");
                        Console.WriteLine("Enter the UserName : ");
                        uname = Console.ReadLine();
                        Console.WriteLine("Enter the Password : ");
                        passwd = Console.ReadLine();
                    }
                }
            }
        }
        public static void Qn4()
        {
            string uname;
            string passwd;
            GetStringInput(out uname, out passwd);
            Console.WriteLine("Checking the User!!");
            CheckUser(uname, passwd);
        }
//-----------------------------------------Question 5----------------------------------------
        public static void Get10Inputs(out List<int> Arr)
        {
            Arr = new List<int>();
            Console.WriteLine("Enter 10 numbers:");
            for (int i = 0; i < 10; i++)
            {
                Console.Write($"Number {i + 1}: ");
                int num = Convert.ToInt32(Console.ReadLine());
                Arr.Add(num);
            }
        }
        public static void DivisibleBySeven(List<int> Arr)
        {
            int count = 0;
            foreach (int i in Arr)
            {
                if (i % 7 == 0)
                {
                    count++;
                }
            }
            Console.WriteLine($"Total numbers divisible by 7: {count}");
        }
        public static void Qn5()
        {
            List<int> Arr = new List<int>();
            Get10Inputs(out Arr);
            DivisibleBySeven(Arr);
        }
//-----------------------------------------Question 6----------------------------------------
        static void CountFrequency(int[] arr)
        {
            Dictionary<int, int> frequencyMap = new Dictionary<int, int>();

            foreach (int num in arr)
            {
                if (frequencyMap.TryGetValue(num, out int value))
                    frequencyMap[num] = ++value;
                else
                    frequencyMap[num] = 1;
            }

            foreach (var entry in frequencyMap)
            {
                Console.WriteLine($"{entry.Key} occurs {entry.Value} times");
            }
        }
        public static void Qn6()
        {
            int[] numbers = { 1, 2, 2, 3, 4, 4, 4 };
            CountFrequency(numbers);
        }
//---------------------------------Question 7------------------------------------------------
        public static void RotateLeftByOne(int[] arr)
        {
            if (arr == null || arr.Length == 0) return;

            int first = arr[0];
            for (int i = 0; i < arr.Length - 1; i++)
            {
                arr[i] = arr[i + 1];
            }
            arr[arr.Length - 1] = first;

            Console.WriteLine("Array after left rotation:");
            foreach (var item in arr)
            {
                Console.Write(item + " ");
            }
            Console.WriteLine();
        }
        public static void Qn7()
        {
            int[] arr = { 10, 20, 30, 40, 50 };
            RotateLeftByOne(arr);
        }
//---------------------------------Question 8------------------------------------------------
        public static int[] MergeArrays(int[] arr1, int[] arr2)
        {
            int[] merged = new int[arr1.Length + arr2.Length];
            for (int i = 0; i < arr1.Length; i++)
            {
                merged[i] = arr1[i];
            }
            for (int i = 0; i < arr2.Length; i++)
            {
                merged[arr1.Length + i] = arr2[i];
            }
            return merged;
        }
        public static void Qn8()
        {
            int[] arr1 = { 1, 3, 5 };
            int[] arr2 = { 2, 4, 6 };
            int[] merged = MergeArrays(arr1, arr2);

            Console.WriteLine("Merged array:");
            foreach (var item in merged)
            {
                Console.Write(item + " ");
            }
            Console.WriteLine();
        }
//---------------------------------Question 9------------------------------------------------
        public static void Qn9()
        {
            const string secret = "GAME";
            int attempts = 0;
            while (true)
            {
                Console.Write("Enter your 4-letter guess: ");
                string? guess = Console.ReadLine();
                if (string.IsNullOrEmpty(guess) || guess.Length != 4)
                {
                    Console.WriteLine("Please enter a valid 4-letter word.");
                    continue;
                }

                attempts++;
                int bulls = 0, cows = 0;
                bool[] secretUsed = new bool[4];
                bool[] guessUsed = new bool[4];
                for (int i = 0; i < 4; i++)
                {
                    if (char.ToUpper(guess[i]) == secret[i])
                    {
                        bulls++;
                        secretUsed[i] = true;
                        guessUsed[i] = true;
                    }
                }
                for (int i = 0; i < 4; i++)
                {
                    if (guessUsed[i]) continue;
                    for (int j = 0; j < 4; j++)
                    {
                        if (!secretUsed[j] && char.ToUpper(guess[i]) == secret[j])
                        {
                            cows++;
                            secretUsed[j] = true;
                            break;
                        }
                    }
                }

                Console.WriteLine($"{bulls} Bulls, {cows} Cows");

                if (bulls == 4)
                {
                    Console.WriteLine($"Congratulations! You guessed the word in {attempts} attempts.");
                    break;
                }
            }
        }
//---------------------------------Question 10------------------------------------------------
        public static void Qn10()
        {
            int[] row = new int[9];
            Console.WriteLine("Enter 9 numbers for the Sudoku row (separated by spaces):");
            string? input = Console.ReadLine();
            string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length != 9)
            {
                Console.WriteLine("Invalid input. Please enter exactly 9 numbers.");
                return;
            }
            for (int i = 0; i < 9; i++)
            {
                if (!int.TryParse(parts[i], out row[i]) || row[i] < 1 || row[i] > 9)
                {
                    Console.WriteLine("Invalid input. All numbers must be between 1 and 9.");
                    return;
                }
            }
            bool isValid = true;
            for (int i = 0; i < 9; i++)
            {
                for (int j = i + 1; j < 9; j++)
                {
                    if (row[i] == row[j])
                    {
                        isValid = false;
                        break;
                    }
                }
                if (!isValid) break;
            }

            if (isValid)
                Console.WriteLine("The row is valid.");
            else
                Console.WriteLine("The row is invalid.");
        }
//---------------------------------Question 11------------------------------------------------
        public static void Qn11()
        {
            int[,] board = new int[9, 9];
            Console.WriteLine("Enter the Sudoku board, one row per line (9 numbers separated by spaces):");
            for (int row = 0; row < 9; row++)
            {
                Console.Write($"Row {row + 1}: ");
                string? input = Console.ReadLine();
                string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length != 9)
                {
                    Console.WriteLine("Invalid input. Each row must have exactly 9 numbers.");
                    return;
                }

                for (int col = 0; col < 9; col++)
                {
                    if (!int.TryParse(parts[col], out board[row, col]) || board[row, col] < 1 || board[row, col] > 9)
                    {
                        Console.WriteLine("Invalid input. All numbers must be between 1 and 9.");
                        return;
                    }
                }
            }
            bool allRowsValid = true;
            for (int row = 0; row < 9; row++)
            {
                bool[] seen = new bool[10];
                for (int col = 0; col < 9; col++)
                {
                    int num = board[row, col];
                    if (seen[num])
                    {
                        allRowsValid = false;
                        Console.WriteLine($"Row {row + 1} is invalid (duplicate number {num}).");
                        break;
                    }
                    seen[num] = true;
                }
            }

            if (allRowsValid)
                Console.WriteLine("All rows are valid.");
            else
                Console.WriteLine("Some rows are invalid.");
        }
//---------------------------------Question 12------------------------------------------------
        public static string Encrypt(string message, int shift)
        {
            char[] encrypted = new char[message.Length];
            for (int i = 0; i < message.Length; i++)
            {
                char c = message[i];
                if (c >= 'a' && c <= 'z')
                {
                    encrypted[i] = (char)('a' + (c - 'a' + shift) % 26);
                }
                else
                {
                    encrypted[i] = c;
                }
            }
            return new string(encrypted);
        }

        public static string Decrypt(string encrypted, int shift)
        {
            char[] decrypted = new char[encrypted.Length];
            for (int i = 0; i < encrypted.Length; i++)
            {
                char c = encrypted[i];
                if (c >= 'a' && c <= 'z')
                {
                    decrypted[i] = (char)('a' + (c - 'a' - shift + 26) % 26);
                }
                else
                {
                    decrypted[i] = c;
                }
            }
            return new string(decrypted);
        }
        public static void Qn12()
        {
            Console.Write("Enter a message (lowercase letters only): ");
            string? message = Console.ReadLine();
            Console.Write("Enter shift value: ");
            int shift = int.Parse(Console.ReadLine() ?? "3");

            string encrypted = Encrypt(message, shift);
            string decrypted = Decrypt(encrypted, shift);

            Console.WriteLine($"Encrypted: {encrypted}");
            Console.WriteLine($"Decrypted: {decrypted}");
        }
        public static void Main()
        {
            Qn1();
            Qn2();
            Qn3();
            Qn4();
            Qn5();
            Qn6();
            Qn7();
            Qn8();
            Qn9();
            Qn10();
            Qn11();
            Qn12();
        }
    }
}