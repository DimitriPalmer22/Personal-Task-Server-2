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
    private const string VENV_NAME = ".venv";
    
    /// <summary>
    /// The name of the file that contains the requirements for the virtual environment.
    /// </summary>
    private const string REQUIREMENTS_FILE = "requirements.txt";

    /// <summary>
    /// The folder that contains the python project.
    /// </summary>
    private const string PYTHON_PROJECT_PATH = @"Python Basketball Newsletter";

    /// <summary>
    /// The path to the python environment that will be used to run the update script.
    /// </summary>
    private const string PYTHON_ENV_PATH = $@"{PYTHON_PROJECT_PATH}/{VENV_NAME}/bin/python3";

    /// <summary>
    /// The path to the script that will be used to check for updates.
    /// </summary>
    private const string SCRIPT_PATH = $@"{PYTHON_PROJECT_PATH}/main.py";

    /// <summary>
    /// The argument that will be passed to the script to indicate that it should check for updates.
    /// </summary>
    private const string UPDATE_FLAG = @"--check-for-update";

    /// <summary>
    /// A flag that indicates whether the python environment was installed during this program's lifetime.
    /// </summary>
    private bool _isVenvInstalled;

    public CheckForUpdateTask(IActivationCondition activationCondition)
        : base($"Basketball Newsletter Check For Update", activationCondition)
    {
    }

    protected override void TaskLogic(ServerTaskManager serverTaskManager, ServerTaskProject serverTaskProject)
    {
        // First, ensure that the python environment is installed
        EnsureVenvIsInstalled();

        // Run the update script using the python environment

        // Construct a new string that contains the path to the script and the update flag
        var argument = $"\"{SCRIPT_PATH}\" {UPDATE_FLAG}";

        // Create a new process that will run the script
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = PYTHON_ENV_PATH,
                Arguments = argument,
                UseShellExecute = true,
                RedirectStandardOutput = false,
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

    private void EnsureVenvIsInstalled()
    {
        // If the venv flag is already true, return
        if (_isVenvInstalled)
            return;

        // Set the flag to indicate that the python environment is installed
        _isVenvInstalled = true;

        // If it is not installed, install it

        // create the process to install virtualenv
        var installVirtualEnv = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "pip",
                Arguments = "install virtualenv",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = false
            }
        };

        // Start the process and wait for it to exit
        installVirtualEnv.Start();
        installVirtualEnv.WaitForExit();

        // create the process to create the virtual environment
        var createVirtualEnv = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "virtualenv",
                Arguments = VENV_NAME,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = false,
                WorkingDirectory = PYTHON_PROJECT_PATH
            }
        };

        // Start the process and wait for it to exit
        createVirtualEnv.Start();

        createVirtualEnv.WaitForExit();

        // create the process to install the required packages
        var installPackages = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = PYTHON_ENV_PATH,
                Arguments = $"-m pip install -r \"{PYTHON_PROJECT_PATH}/{REQUIREMENTS_FILE}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = false
            }
        };

        // Start the process and wait for it to exit
        installPackages.Start();
        installPackages.WaitForExit();

        // Log that the python environment has been installed
        DebugLog.Instance.Log(LogType.Normal, "Python Environment Installed");
    }
}