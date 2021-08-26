using System;

namespace CsConsoleProgram
{
    class Program
    {
        // Public vars
        public const string START_MESSAGE = @"
 ██████╗ ██████╗ ███╗   ██╗ ██████╗ █████╗ ██╗      ██████╗
██╔════╝██╔═══██╗████╗  ██║██╔════╝██╔══██╗██║     ██╔════╝
██║     ██║   ██║██╔██╗ ██║██║     ███████║██║     ██║
██║     ██║   ██║██║╚██╗██║██║     ██╔══██║██║     ██║
╚██████╗╚██████╔╝██║ ╚████║╚██████╗██║  ██║███████╗╚██████╗
 ╚═════╝ ╚═════╝ ╚═╝  ╚═══╝ ╚═════╝╚═╝  ╚═╝╚══════╝ ╚═════╝";
        public const string ACTION_MESSAGE = @"
Would you like to
  - preform an arithmetic (o)peration?
  - (c)ompare two numbers?
  - (s)ave the result of the previous computation?
  - (q)uit?
  - or (e)xit?

Enter your choice here: ";

        // Private vars
        private static double s_previousAnswer;
        private static double s_savedAnswer;
        private static string s_name;
        private static Random rand = new();

        static void Main(string[] args)
        {
            // Print start message
            Console.WriteLine(START_MESSAGE + "\n");

            // Main loop
            while (true)
            {
                // Prompt for name
                s_name = "";
                while (s_name == "")
                {
                    Console.Write("Please enter your name: ");
                    s_name = Console.ReadLine();
                }

                // Welcome message
                Console.WriteLine($"\nHello {s_name}! Welcome to ConCalc, the console-based calculator.");

                // Action loop
                bool signedIn = true;
                while (signedIn)
                {
                    // Pick an action
                    Console.Write(ACTION_MESSAGE);
                    string action = Console.ReadLine().ToLower();

                    if (action.Length > 0)
                    {
                        switch (action[0])
                        {
                            case 'o':
                                s_mathOperation();
                                break;
                            case 'c':
                                s_comparison();
                                break;
                            case 's':
                                s_savedAnswer = s_previousAnswer;
                                break;
                            case 'q':
                                signedIn = false;
                                break;
                            case 'e':
                                Environment.Exit(0);
                                break;
                            default:
                                continue;
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
            }
        }

        private static void s_mathOperation()
        {
            Console.WriteLine();

            // Get data
            double firstNumber;
            try
            {
                firstNumber = s_promptForNumber("first");
            }
            catch (FormatException)
            {
                Console.WriteLine($"Invalid number, {s_name}!");
                return;
            }

            double secondNumber;
            try
            {
                secondNumber = s_promptForNumber("second");
            }
            catch (FormatException)
            {
                Console.WriteLine($"Invalid number, {s_name}!");
                return;
            }

            Console.Write("Would you like to perform (a)ddition, (s)ubtraction, (m)ultiplication, or (d)ivision: ");
            string operation = Console.ReadLine().ToLower();

            // Validation
            if (operation.Length == 0)
            {
                Console.WriteLine("You must specify an operation, {s_name}!");
                return;
            }

            try
            {
                switch (operation[0])
                {
                    case 'a':
                        s_previousAnswer = firstNumber + secondNumber;
                        break;
                    case 's':
                        s_previousAnswer = firstNumber - secondNumber;
                        break;
                    case 'm':
                        s_previousAnswer = firstNumber * secondNumber;
                        break;
                    case 'd':
                        s_previousAnswer = firstNumber / secondNumber;
                        break;
                    default:
                        Console.WriteLine($"Invalid operation {operation}!");
                        return;
                }
                Console.WriteLine($"\nHere is your result, {s_name}: {s_previousAnswer}\n");
            }
            catch (ArithmeticException e)
            {
                Console.WriteLine($"Invalid operation, {s_name}!\nError: {e.Message}");
            }
        }

        private static void s_comparison()
        {
            Console.WriteLine();

            // Get data
            double firstNumber;
            try
            {
                firstNumber = s_promptForNumber("first");
            }
            catch (FormatException)
            {
                Console.WriteLine($"Invalid number, {s_name}!");
                return;
            }

            double secondNumber;
            try
            {
                secondNumber = s_promptForNumber("second");
            }
            catch (FormatException)
            {
                Console.WriteLine($"Invalid number, {s_name}!");
                return;
            }

            Console.Write("Would you like to find the (g)reater or (l)esser number: ");
            string operation = Console.ReadLine().ToLower();

            // Validation
            if (operation.Length == 0)
            {
                Console.WriteLine($"You must specify an operation, {s_name}!");
                return;
            }

            try
            {
                switch (operation[0])
                {
                    case 'g':
                        s_previousAnswer = firstNumber > secondNumber ? firstNumber : secondNumber;
                        break;
                    case 'l':
                        s_previousAnswer = firstNumber < secondNumber ? firstNumber : secondNumber;
                        break;
                    default:
                        Console.WriteLine($"Invalid operation {operation}!");
                        return;
                }
                Console.WriteLine($"\nHere is your result, {s_name}: {s_previousAnswer}\n");
            }
            catch (ArithmeticException e)
            {
                Console.WriteLine($"Invalid operation, {s_name}!\nError: {e.Message}");
            }
        }

        private static double s_promptForNumber(string name)
        {
            Console.Write($"Please enter the {name} number, rand for a random int, or nothing for the saved answer, {s_savedAnswer}: ");
            string raw = Console.ReadLine().ToLower();

            double result;
            try
            {
                result = Convert.ToDouble(raw);
            }
            catch (FormatException)
            {
                if (raw.Length == 0)
                {
                    result = s_savedAnswer;
                }
                else if (raw.Length == 4 && raw.Substring(0, 4) == "rand")
                {
                    // Random double in (-1000, 1000)
                    result = rand.NextDouble() * 2000 - 1000;
                }
                else
                {
                    // Bubble up the exception
                    throw;
                }
            }
            return result;
        }
    }
}
