using Basketball_Newsletter;
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
        DebugLog.Instance.AddLogOutput(new RealTimeConsoleLogOutput());

        // Create a personal server task project
        var personalServerTaskProject = new PersonalServerTaskProject();

        // Create a new list of background server task projects
        var backgroundProjects = new ServerTaskProject[]
        {
            BasketballNewsletterProject.Instance
        };

        // Start the server task manager
        ServerTaskManager.Instance.AddProject(personalServerTaskProject);
        ServerTaskManager.Instance.Start(backgroundProjects);

        // Run the project (this will block the main thread)
        personalServerTaskProject.Start();

        // Stop the server task manager
        ServerTaskManager.Instance.Stop();
    }
}