using Task_Server_2.DebugLogger;
using Task_Server_2.DebugLogger.LogOutput;
using Task_Server_2.ServerTasks;

namespace Personal_Task_Server;

public static class Program
{
    public static void Main(string[] args)
    {
        // Change where the log messages are outputted
        DebugLog.Instance.AddLogOutput(new FileOutput("Personal Task Server Log.txt"));
        
        // Create a new server task project
        var serverTaskProject = new PersonalServerTaskProject();
        
        // Start the server task manager
        ServerTaskManager.Instance.Start(serverTaskProject);
        
        // Run the project
        serverTaskProject.Start();
        
        // Stop the server task manager
        ServerTaskManager.Instance.Stop();
    }
}