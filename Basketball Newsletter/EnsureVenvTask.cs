using System.Diagnostics;
using Task_Server_2.DebugLogger;
using Task_Server_2.ServerTasks;
using Task_Server_2.ServerTasks.ActivationConditions;

namespace Basketball_Newsletter;

public class EnsureVenvTask : ServerTask
{
    /// <summary>
    /// A flag that indicates whether the python environment was installed during this program's lifetime.
    /// </summary>
    private bool _isVenvInstalled;

    public EnsureVenvTask(string name, IActivationCondition activationCondition)
        : base(name, activationCondition)
    {
    }

    protected override void TaskLogic(ServerTaskManager serverTaskManager, ServerTaskProject serverTaskProject)
    {
        // First, ensure that the python environment is installed
        EnsureVenvIsInstalled();
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
                Arguments = CheckForUpdateTask.VENV_NAME,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = false,
                WorkingDirectory = CheckForUpdateTask.PYTHON_PROJECT_PATH
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
                FileName = CheckForUpdateTask.PYTHON_ENV_PATH,
                Arguments =
                    $"-m pip install -r \"{CheckForUpdateTask.PYTHON_PROJECT_PATH}/{CheckForUpdateTask.REQUIREMENTS_FILE}\"",
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