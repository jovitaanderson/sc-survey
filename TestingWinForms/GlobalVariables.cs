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

        static GlobalVariables()
        {
            totalOptions = 8;
        }
    }
}
