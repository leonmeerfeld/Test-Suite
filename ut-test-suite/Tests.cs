using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace ut_test_suite
{
    /// <summary>
    /// Object that describes tests for Databinding in the DasboardListView
    /// </summary>
    public class Tests
    {
        /// <summary>
        /// Name of the test.
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// Determines whether the test is running.
        /// </summary>
        public string status { get; set; }

        /// <summary>
        /// Determines whether the test has been run.
        /// </summary>
        [DefaultValue(0)]
        public int stage { get; set; }

        /// <summary>
        /// Determines the result of the test.
        /// </summary>
        public string result { get; set; }
    }
}
