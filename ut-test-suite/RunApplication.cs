using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.ComponentModel;

namespace ut_test_suite
{
    class RunApplication
    {
        Process proc = new Process();
        BackgroundWorker runApplication = new BackgroundWorker();

        int? exitCode = null;
        bool testRunning = false;

        public void Run(string fileName)
        {
            runApplication.DoWork += RunApplication_DoWork;
            runApplication.RunWorkerCompleted += RunApplication_RunWorkerCompleted;
            runApplication.WorkerReportsProgress = true;
            runApplication.RunWorkerAsync(fileName);
        }

        private void RunApplication_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            testRunning = false;
        }

        private void RunApplication_DoWork(object sender, DoWorkEventArgs e)
        {
            if (!testRunning)
            {
                ProcessStartInfo start = new ProcessStartInfo();

                //start.Arguments = "cmd args";

                start.FileName = e.Argument as string;

                using (Process proc = Process.Start(start))
                {
                    testRunning = true;
                    proc.WaitForExit();
                    exitCode = proc.ExitCode;
                }
            }
        }

        public string RetrieveExitCode()
        {
            string result = "ERROR ()";

            switch(exitCode)
            {
                case 0:
                    result = "Erfolgreich";
                    break;

                case 1:
                    result = "Test fehlgeschlagen";
                    break;

                default:
                    result = "ERROR (unbekannter Exitcode)";
                    break;
            }

            exitCode = null;
            return result;
        }

        public IList<string> RunningProcesses()
        {
            IList<string> runningProcessesList = new List<string>();

            Process[] processes = Process.GetProcesses();

            foreach (Process proc in processes)
                runningProcessesList.Add(proc.ProcessName);

            return runningProcessesList;
        }
    }
}
