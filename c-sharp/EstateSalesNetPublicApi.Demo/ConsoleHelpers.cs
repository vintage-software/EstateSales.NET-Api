﻿using System;

namespace EstateSalesNetPublicApi.Demo
{
    public static class ConsoleHelpers
    {
        public static T Prompt<T>(string prompt)
        {
            Console.Write($"{prompt}: ");

            string input = Console.ReadLine();

            if (typeof(T).IsEnum)
            {
                return (T)Enum.Parse(typeof(T), input);
            }
            else
            {
                return (T)Convert.ChangeType(input, typeof(T));
            }
        }

        public static void Pause(bool exiting = false)
        {
            Console.WriteLine($"Press any key to {(exiting ? "exit" : "continue")}...");
            Console.ReadKey();
        }
    }
}
