using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ut_test_suite
{
    /// <summary>
    /// Class that interacts with test logs.
    /// </summary>
    class Logs
    {
        private static string currentPath = System.AppDomain.CurrentDomain.BaseDirectory;

        public string name { get; set; }
        public string text { get; set; }
        public DateTime date { get; set; }

        public IList<Logs> RetrieveLogData()
        {
            IList<Logs> logData = new List<Logs>();

            foreach (string fileInTestDir in Directory.GetFiles(currentPath + "TestDir\\"))
            {
                if (fileInTestDir.Contains(".autolog"))
                {
                    string date = fileInTestDir.Split(' ')[fileInTestDir.Split(' ').Count() - 2].Replace(".", "/");
                    string time = fileInTestDir.Split(' ')[fileInTestDir.Split(' ').Count() - 1].Split(new string[] { ".a" }, StringSplitOptions.None)[0].Replace(".", ":");

                    DateTime testDate = Convert.ToDateTime(date + " " + time);

                    logData.Add(new Logs() { name = fileInTestDir, text = File.ReadAllText(fileInTestDir), date = testDate });
                }
            }

            return logData;
        }

        //Example Name:
        //C:\Users\meerfeld\Source\Repos\ut-test-suite\ut-test-suite\bin\Debug\TestDir\porta-test 24.08.2016 10.19.14.autolog

        public string getLatestLog(string testName)
        {
            string latestLog = null;

            IList<Logs> logData = RetrieveLogData();

            IList<Logs> logsFromFittingTest = new List<Logs>();

            //Loops through logs to find logs with the corresponding names
            foreach (Logs logs in logData)
            {
                string logToCompare = logs.name.Split(new string[] { "\\" }, StringSplitOptions.None)[logs.name.Split(new string[] { "\\" }, StringSplitOptions.None).Count() - 1];
                logToCompare = logToCompare.Split(' ')[0];
                string testNameToCompare = currentPath + "TestDir\\" + testName.Split(new string[] { ".e" }, StringSplitOptions.None)[0];
                testNameToCompare = testNameToCompare.Split(new string[] { "\\" }, StringSplitOptions.None)[testNameToCompare.Split(new string[] { "\\" }, StringSplitOptions.None).Count() - 1];

                if (logToCompare == testNameToCompare)
                    logsFromFittingTest.Add(new Logs() { name = logs.name, text = logs.text, date = logs.date });
            }

            if(logsFromFittingTest.Count() > 0)
            {
                DateTime latestDate = logsFromFittingTest[0].date;

                //Loops through logs with the fitting name and gets the latest
                for (int i = 0; i < logsFromFittingTest.Count() - 1; i++)
                {
                    if (DateTime.Compare(latestDate, logsFromFittingTest[i + 1].date) < 0)
                    {
                        latestDate = logsFromFittingTest[i + 1].date;
                        latestLog = logsFromFittingTest[i + 1].text;
                    }
                }
            }

            return latestLog;
        }
    }
}