using System;
namespace CriticalPathAnalysis
{
	public class Commands
	{
        public void printCommands()
        {
            while (true)
            {
                Console.Write(@"Please select one of the options below.

1) Calculate how long the project will take to complete without delay.
2) Calculate the critical path(s) in the project.
3) Generate project management data: earliest start time, latest start time, and slack time.
9) Exit

Please enter option: ");

                inputCommands();
            }
        }

        private void inputCommands()
        {
            byte userInput = Convert.ToByte(Console.ReadLine());

            ProjectManager project = new ProjectManager(1);
            project.populateGraph();

            switch (userInput)
            {
                case 1:
                    project.printProjectDuration();
                    break;

                case 2:
                    project.printCriticalPaths();
                    break;

                case 3:
                    project.printActivityData();
                    break;

                case 9:
                    return;


                default:
                    Console.WriteLine("\nOption is outside of range.\nPlease a option from the following menu.");
                    break;
            }
        }
    }
}

