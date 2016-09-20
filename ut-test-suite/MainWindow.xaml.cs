using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.IO;
using System.ComponentModel;

namespace ut_test_suite
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml.
    /// </summary>
    public partial class MainWindow : Window
    {
        //Class variables:
        Tests t = new Tests();
        OpenFileDialog oFD = new OpenFileDialog();
        RunApplication rA = new RunApplication();
        Logs l = new Logs();

        BackgroundWorker updateTestStatus = new BackgroundWorker();

        string currentPath = System.AppDomain.CurrentDomain.BaseDirectory;

        public MainWindow()
        {
            InitializeComponent();
            updateTestStatus.DoWork += UpdateTestStatus_DoWork;
            updateTestStatus.ProgressChanged += UpdateTestStatus_ProgressChanged;
            updateTestStatus.RunWorkerCompleted += UpdateTestStatus_RunWorkerCompleted;
            updateTestStatus.WorkerReportsProgress = true;
            updateTestStatus.RunWorkerAsync();
        }

        private void UpdateTestStatus_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            dashboardListView.Items.Refresh();
        }

        private void UpdateTestStatus_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            updateTestStatus.RunWorkerAsync();
        }

        private void UpdateTestStatus_DoWork(object sender, DoWorkEventArgs e)
        {
            updateTestStatus = sender as BackgroundWorker;

            while(!updateTestStatus.CancellationPending)
            {
                bool testIsRunning = false;

                //Check which tests are currently running so that they can be marked as such in the dashboardListView.
                foreach (Tests item in dashboardListView.Items)
                {
                    foreach (string runningProcess in rA.RunningProcesses())
                    {
                        if (item.name == runningProcess + ".exe")
                        {
                            testIsRunning = true;
                            break;
                        }
                    }

                    if (testIsRunning)
                    {
                        item.status = "läuft";
                        item.stage = 1;
                        System.Threading.Thread.Sleep(500);
                        updateTestStatus.ReportProgress(50);
                        testIsRunning = false;
                    }
                    else
                    {
                        if(item.stage == 1)
                        {
                            string result = rA.RetrieveExitCode();
                            item.result = result;
                            item.status = "beendet";
                            item.stage = 2;
                        }

                        System.Threading.Thread.Sleep(500);
                        updateTestStatus.ReportProgress(50);
                    }
                }
            }
        }

        private void ListView_Loaded(object sender, RoutedEventArgs e)
        {
            AddSavedTestsToDashboard();
            MainWindowBtnControl();
        }
        
        /// <summary>
        /// Adds a string to DashboardListView. Makes sure no duplicates and non executable files are displayed.
        /// </summary>
        /// <param name="entry"></param>
        private void AddToListView(string entry, ListView targetListView)
        {
            entry = entry.Split('\\')[entry.Split('\\').Count() - 1];

            foreach(Tests entryInListView in targetListView.Items)
            {
                if (entryInListView.name == entry)
                    return;
            }

            if(entry.IndexOf(".exe") != -1)
                targetListView.Items.Add(new Tests() { name = entry });
        }

        private void AddTestBtn_Click(object sender, RoutedEventArgs e)
        {
            
            oFD.Multiselect = true;

            if (oFD.ShowDialog() == true)
            {
                for (int i = 0; i < oFD.FileNames.Count(); i++)
                {
                    File.Copy(oFD.FileNames[i], currentPath + "TestDir\\" + oFD.FileNames[i].Split('\\')[oFD.FileNames[i].Split('\\').Count() - 1], true);

                    AddToListView(oFD.FileNames[i], dashboardListView);
                }
            }
        }

        /// <summary>
        /// Adds executables in the /TestDir/ directory to DasboardListView.
        /// </summary>
        private void AddSavedTestsToDashboard()
        {
            Directory.CreateDirectory(currentPath + "TestDir\\");

            foreach(string fileInTestDir in Directory.GetFiles(currentPath + "TestDir\\"))
            {
                if (fileInTestDir.Contains(".exe"))
                    AddToListView(fileInTestDir, dashboardListView);
            }
        }

        private void RemoveTestBtn_Click(object sender, RoutedEventArgs e)
        {
            Tests itemToDelete = (Tests)dashboardListView.SelectedItems[0];

            File.Delete(currentPath + "TestDir\\" + itemToDelete.name);
            dashboardListView.Items.Remove(dashboardListView.SelectedItems[0]);
            dashboardListView.Items.Refresh();
        }

        private void RunTestBtn_Click(object sender, RoutedEventArgs e)
        {
            Tests testToRun = (Tests)dashboardListView.SelectedItem;
            rA.Run(currentPath + "TestDir\\" + testToRun.name);
        }

        /// <summary>
        /// Controls the buttons on the MainWindow.
        /// </summary>
        public void MainWindowBtnControl()
        {
            if (dashboardListView.SelectedItems.Count < 1)
            {
                removeTestBtn.IsEnabled = false;
                runTestBtn.IsEnabled = false;
            }
            else
            {
                removeTestBtn.IsEnabled = true;
                runTestBtn.IsEnabled = true;
            }
        }

        private void ListView_Focus(object sender, RoutedEventArgs e)
        {
            MainWindowBtnControl();
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MainWindowBtnControl();
        }

        private void ShowLatestLog_Click(object sender, RoutedEventArgs e)
        {
            Tests testtoRetrieveLatestLogFrom = (Tests)dashboardListView.SelectedItem;

            LogResult lR = new LogResult(l.getLatestLog(testtoRetrieveLatestLogFrom.name));
            lR.Show();
        }

        private void Configuration_Click(object sender, RoutedEventArgs e) {
            ConfigurationWindow cW = new ConfigurationWindow();
            cW.Show();
            }
        }
}
