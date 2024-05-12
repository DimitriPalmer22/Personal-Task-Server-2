using Task_Server_2.DebugLogger;
using Task_Server_2.ServerTasks;
using Task_Server_2.ServerTasks.ActivationConditions;
using Task_Server_2.ServerTasks.HelperServerTasks;
using Task_Server_2.ServerTasks.ServerTaskEventArgs;

namespace Basketball_Newsletter;

public sealed class BasketballNewsletterProject : ServerTaskProject
{
    private static BasketballNewsletterProject _instance;

    /// <summary>
    /// A reference to the recurring CheckForUpdateTask that will be used to check for updates.
    /// </summary>
    private readonly CheckForUpdateTask _checkForUpdateTask;

    /// <summary>
    /// A reference to the recurring CheckForUpdateTask that will be used to check for updates.
    /// </summary>
    public CheckForUpdateTask CheckForUpdateTask => _checkForUpdateTask;

    public static BasketballNewsletterProject Instance
    {
        get
        {
            if (_instance == null)
                _instance = new BasketballNewsletterProject();
            return _instance;
        }
    }

    private BasketballNewsletterProject()
    {
        // Create a new CheckForUpdateTask that will run once
        _checkForUpdateTask = new CheckForUpdateTask(
            new SimpleActivationCondition()
        );

        var newsletterCheckTasks = new ServerTask[]
        {
            // Create a new EnsureVenvTask that will run once
            new EnsureVenvTask(
                "Ensure Python Environment",
                new SimpleActivationCondition()
            ),

            _checkForUpdateTask,
        };

        // Add both tasks to a server task group that will run repeatedly
        var groupTask = new ServerTaskGroup(
            "Basketball Newsletter Tasks",
            new RecurringActivationCondition(
                DateTime.Now,
                new TimeSpan(days: 0, hours: 0, minutes: 0, seconds: 2, milliseconds: 0)
            ),
            ServerTaskGroupType.Sequential,
            newsletterCheckTasks
        );

        // Add the CheckForUpdateTask to the project
        EnqueueTask(_checkForUpdateTask);
        EnqueueTask(groupTask);
    }
}