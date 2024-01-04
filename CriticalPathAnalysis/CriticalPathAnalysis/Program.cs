using System;
using System.Collections.Generic;
using System.Linq;

namespace CriticalPathAnalysis;

class Programs
{
    static void Main()
    {
        ProjectManager manager = new ProjectManager(1);

        Commands command = new Commands();

        command.printCommands();
    }
}
