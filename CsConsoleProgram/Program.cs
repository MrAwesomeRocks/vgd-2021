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

Enter your choice here: ";

        // Private vars
        private static double s_previousAnswer;
        private static double s_savedAnswer;

        static void Main(string[] args)
        {
            // Print start message
            Console.WriteLine(START_MESSAGE + "\n");

            // Main loop
            while (true)
            {
                // Prompt for name
                string name = "";
                while (name == "")
                {
                    Console.Write("Please enter your name: ");
                    name = Console.ReadLine();
                }

                // Welcome message
                Console.WriteLine($"\nHello {name}! Welcome to ConCalc, the console-based calculator.");

                // Action loop
                while (true)
                {
                    // Pick an action
                    Console.Write(ACTION_MESSAGE);
                    string action = Console.ReadLine();

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
                Console.WriteLine($"Invalid number!");
                return;
            }

            double secondNumber;
            try
            {
                secondNumber = s_promptForNumber("second");
            }
            catch (FormatException)
            {
                Console.WriteLine($"Invalid number!");
                return;
            }

            Console.Write("Would you like to perform (a)ddition, (s)ubtraction, (m)ultiplication, or (d)ivision: ");
            string operation = Console.ReadLine();

            // Validation
            if (operation.Length == 0)
            {
                Console.WriteLine("You must specify an operation!");
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
                Console.WriteLine($"\nResult: {s_previousAnswer}\n");
            }
            catch (ArithmeticException e)
            {
                Console.WriteLine($"Invalid operation!\nError: {e.Message}");
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
                Console.WriteLine($"Invalid number!");
                return;
            }

            double secondNumber;
            try
            {
                secondNumber = s_promptForNumber("second");
            }
            catch (FormatException)
            {
                Console.WriteLine($"Invalid number!");
                return;
            }

            Console.Write("Would you like to find the (g)reater or (l)esser number: ");
            string operation = Console.ReadLine();

            // Validation
            if (operation.Length == 0)
            {
                Console.WriteLine("You must specify an operation!");
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
                Console.WriteLine($"\nResult: {s_previousAnswer}\n");
            }
            catch (ArithmeticException e)
            {
                Console.WriteLine($"Invalid operation!\nError: {e.Message}");
            }
        }

        private static double s_promptForNumber(string name)
        {
            Console.Write($"Please enter the {name} number or ans for the saved answer, {s_savedAnswer}: ");
            string raw = Console.ReadLine();

            double result;
            try
            {
                result = Convert.ToDouble(raw);
            }
            catch (FormatException)
            {
                if (raw.Length == 0 || (raw.Length == 3 && raw.Substring(0, 3) == "ans"))
                {
                    result = s_savedAnswer;
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
