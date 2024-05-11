using Task_Server_2.ServerTasks;
using Task_Server_2.ServerTasks.ActivationConditions;

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
        // Create a new CheckForUpdateTask that will run every minute
        _checkForUpdateTask = new CheckForUpdateTask(
            new RecurringActivationCondition(
                DateTime.Now,
                new TimeSpan(days: 0, hours: 0, minutes: 1, seconds: 0))
        );

        // Add the CheckForUpdateTask to the project
        EnqueueTask(_checkForUpdateTask);
    }
}