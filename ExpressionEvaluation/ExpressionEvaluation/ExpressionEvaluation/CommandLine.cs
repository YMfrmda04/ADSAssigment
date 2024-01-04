using System;
namespace ExpressionEvaluation
{
	public class CommandLine
	{
        private Input input;
		private MANAGER manager;

        public CommandLine()
		{
            input = new Input();
			manager = new MANAGER();
        }

		public void Start()
		{
			Console.Write(" Please enter you expressions: ");
            string? a = input.InputType<String>();

            manager.printer(a);

            /*
            manager.printer("5+3*2");          //  11
            manager.printer("(7-4)/2+5");      //  6.5
            manager.printer("8/(4-2)");        //  4
            manager.printer("10-(2+1)*2");     //  4
            manager.printer("3 * 5+(8/2)");    // 19
            manager.printer("-3 * 5+(8/2)");   // -11 

            manager.printer("5 and 0");        // false
            manager.printer("5 or 0");         // true
            manager.printer("5 not 0");        // true

            manager.printer("5 < 0");          // false 
            manager.printer("5 <= 0");         // false 
            manager.printer("5 > 0");          // true
            manager.printer("5 >= 0");         // true
            manager.printer("5 = 0");          // flase
            manager.printer("5 != 0");         // true
            */

            /*
            manager.printer("1 and 1");          // True
            manager.printer("1 or 2 and 3");     // True
            manager.printer("1 and 2 and 3");     // True
            manager.printer("0 and 1");          // False
            manager.printer("1 or 0 and 1");     // True
            manager.printer("true and false");   // False
            manager.printer("true or false");    // True
            manager.printer("1 and 0 or 1");      // True
            manager.printer("1 or 2 or 3");       // True
            manager.printer("0 or 0 or 0");       // False
            
            manager.printer("A > 5 and B < 10");  // depend on a and b values
            */

        }
    }
}

