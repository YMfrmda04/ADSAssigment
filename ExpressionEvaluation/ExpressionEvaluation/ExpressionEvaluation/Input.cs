using System;
namespace ExpressionEvaluation
{
    public class Input

    {
        public T? InputType<T>()
        {

            T? input = (T)Convert.ChangeType(false, typeof(T));

            try
            {
                input = (T)Convert.ChangeType(Console.ReadLine(), typeof(T));
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"\n{e.Message}\n");
                Console.ResetColor();
            }
            return input;
        }
    }

}