using System.Diagnostics;
using Task_Server_2.DebugLogger;
using Task_Server_2.ServerTasks;
using Task_Server_2.ServerTasks.ActivationConditions;

namespace Basketball_Newsletter;

public class CheckForUpdateTask : ServerTask
{
    /// <summary>
    /// The name of the folder that contains the python environment.
    /// </summary>
    internal const string VENV_NAME = ".venv";

    /// <summary>
    /// The name of the file that contains the requirements for the virtual environment.
    /// </summary>
    internal const string REQUIREMENTS_FILE = "requirements.txt";

    /// <summary>
    /// The folder that contains the python project.
    /// </summary>
    internal const string PYTHON_PROJECT_PATH = @"Python Basketball Newsletter";

    /// <summary>
    /// The path to the python environment that will be used to run the update script.
    /// </summary>
    internal const string PYTHON_ENV_PATH = $@"{PYTHON_PROJECT_PATH}/{VENV_NAME}/bin/python3";

    /// <summary>
    /// The path to the script that will be used to check for updates.
    /// </summary>
    private const string SCRIPT_PATH = $@"{PYTHON_PROJECT_PATH}/main.py";

    /// <summary>
    /// The argument that will be passed to the script to indicate that it should check for updates.
    /// </summary>
    private const string UPDATE_FLAG = @"--check-for-update";

    public CheckForUpdateTask(IActivationCondition activationCondition)
        : base($"Basketball Newsletter Check For Update", activationCondition)
    {
    }

    protected override void TaskLogic(ServerTaskManager serverTaskManager, ServerTaskProject serverTaskProject)
    {
        // Run the update script using the python environment

        // Construct a new string that contains the path to the script and the update flag
        const string argument = $"\"{SCRIPT_PATH}\" {UPDATE_FLAG}";

        // Create a new process that will run the script
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = PYTHON_ENV_PATH,
                Arguments = argument,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = false
            }
        };

        // Start the process
        process.Start();

        // Wait for the process to exit
        process.WaitForExit();

        // Log the exit code of the process
        DebugLog.Instance.WriteLine($"Exit Code: {process.ExitCode}");
    }
}