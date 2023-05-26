using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingWinForms
{
    public static class GlobalVariables
    {
        public static int totalOptions { get; set; }

        public static string csvAdminQuestionsFilePath { get; set; }
        public static string csvAdminTableFilePath { get; set; }
        public static string csvAdminAdvanceFilePath { get; set; }
        public static string csvRawDataFilePath { get; set; }

        static GlobalVariables()
        {
            totalOptions = 8;
            csvAdminQuestionsFilePath = "admin/admin_questions.csv";
            csvAdminTableFilePath = "admin/admin_table.csv";
            csvAdminAdvanceFilePath = "admin/admin_advance.csv";
            csvRawDataFilePath = "ForDevelopersOnly/raw_data.csv";
        }
    }
}
