using Task_Server_2.DebugLogger;
using Task_Server_2.ServerTasks;
using Task_Server_2.ServerTasks.ActivationConditions;
using Task_Server_2.ServerTasks.HelperServerTasks;

namespace Personal_Task_Server;

public class PersonalServerTaskProject : ServerTaskProject
{
    private bool _running;

    public void Start()
    {
        // Set the running flag to true
        _running = true;

        // Run the project
        Run();
    }

    private void Run()
    {
        while (_running)
        {
            DebugLog.Instance.WriteLine($"Enter a command: ");
            var command = Console.ReadLine();

            if (command == null)
                continue;

            if (command.ToLower() == "test")
                EnqueueTask(
                    new FunctionWrapperServerTask(
                        "Test Task", new SimpleActivationCondition(),
                        () => DebugLog.Instance.WriteLine("Test Task Ran!")
                    )
                );

            else if (command.ToLower() == "exit")
                Stop();
        }
    }

    public void Stop()
    {
        _running = false;
    }
}