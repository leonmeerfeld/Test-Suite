using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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

namespace ut_test_suite {
    /// <summary>
    /// Interaktionslogik für ConfigurationWindow.xaml
    /// </summary>
    public partial class ConfigurationWindow : Window {


        public ConfigurationWindow() {
            InitializeComponent();
            configurationUrlTextbox.Text = readConfig("URL");
            }

        /// <summary>
        /// Check in Config if line is a Header
        /// Returns true if Lines start with #
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private bool isHeader(string line) {
            return line.Substring(0, 1) == "#";
            }

        private string path = Directory.GetCurrentDirectory() + "\\TestDir" + "\\config.txt";

        /// <summary>
        /// Change Configuration
        /// </summary>
        /// <param name="configitem"></param>
        /// <returns></returns>
        public string readConfig(String configitem) {
            string configItemvalue = "";
            configitem = "#" + configitem;

            ////Exception if config file is missing
            //if (!File.Exists(path)) {
            //    w.WriteL("Konfigurationsdatei wurde nicht gefunden.", "red");
            //    }

            //Read Document
            string[] configLines = File.ReadAllLines(path);
            for (int i = 0; i < configLines.Length; i++) {
                if ((configLines[i] == configitem)) {
                    i++;
                    int m = i;
                    while ((configLines.Length != m) && !isHeader(configLines[m])) {
                        configItemvalue += configLines[m];
                        m++;
                        }
                    }
                }
            //Option not in configuration file
            //if (configItemvalue == "") {
            //    Console.WriteLine("Die Option '" + configitem + "' wurde in der Konfigurationsdatei nicht gefunden.", "red");
            //    }
            return configItemvalue;
            }

        /// <summary>
        /// Change configuration file
        /// </summary>
        /// <param name="configitem"></param>
        /// <param name="newconfig"></param>
        public void changeConfig(String configitem, String newconfig) {
            string configOptions = File.ReadAllText(path);
            configOptions = configOptions.Replace("#" + configitem +"\r" + "\n" + readConfig(configitem), "#" + configitem + "\r" + "\n" + newconfig);
            File.WriteAllText(path, configOptions);
            }

        /// <summary>
        /// Click Event Save Button Configuration Menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void configurationSaveButton_Click(object sender, RoutedEventArgs e) {
            changeConfig("URL", configurationUrlTextbox.Text);
            Close();
            }

        /// <summary>
        /// Click Event Cancel Button Configuration Menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void configurationCancelButton_Click(object sender, RoutedEventArgs e) {
            Close();
            }
        }
    }
