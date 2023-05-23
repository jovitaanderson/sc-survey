using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestingWinForms
{
    public partial class ThankYouScreen : Form
    {
        private string csvAdminAdvanceFilePath = "admin_advance.csv";

        public ThankYouScreen()
        {
            InitializeComponent();

            FormBorderStyle = FormBorderStyle.None; // Remove the border
            WindowState = FormWindowState.Maximized; // Maximize the window

            //Display Background Image
            string imagePath = LoadBackgroundImageFromCSV();

            if (!string.IsNullOrEmpty(imagePath) && File.Exists(imagePath))
            {
                // Set the background image of the Windows Forms application
                this.BackgroundImage = Image.FromFile(imagePath);

                // Adjust the background image display settings
                this.BackgroundImageLayout = ImageLayout.Stretch;
            }

            // Subscribe to the Timer's Tick event
            timer1.Tick += Timer1_Tick;
        }

        private string LoadBackgroundImageFromCSV()
        {
            if (File.Exists(csvAdminAdvanceFilePath))
            {
                string[] lines = File.ReadAllLines(csvAdminAdvanceFilePath);
                if (lines.Length > 0)
                {
                    string[] values = lines[lines.Length - 1].Split(',');

                    if (values[3] != null)
                    {
                        string imagePath = Path.Combine(values[3]);
                        return imagePath;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            // Stop the timer
            timer1.Stop();

            // Close the Thank You screen and show the home screen (Form1)
            Form1 form1 = new Form1();
            form1.Show();
            this.Close();
        }

        private void ThankYouScreen_Load(object sender, EventArgs e)
        {
            if (File.Exists(csvAdminAdvanceFilePath))
            {
                string[] lines = File.ReadAllLines(csvAdminAdvanceFilePath);
                if (lines.Length > 0)
                {
                    string[] values = lines[lines.Length - 1].Split(',');
                    if (values[2] != null)
                    {
                        labelEndMessage.Text = values[2];

                    }
                }
            }
            // Start the timer
            timer1.Start();
        }

        private void btnMain_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Close();
        }

        
    }
}
