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

        public ThankYouScreen()
        {
            InitializeComponent();
            DoubleBuffered = true;

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
            if (File.Exists(GlobalVariables.csvAdminAdvanceFilePath))
            {
                string[] lines;
                while (true)
                {
                    try
                    {
                        lines = File.ReadAllLines(GlobalVariables.csvAdminAdvanceFilePath);
                        break;
                    }
                    catch (IOException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }

                if (lines.Length > 0)
                {
                    string[] values = lines[lines.Length - 1].Split(',');

                    if (values[4] != null)
                    {
                        string imagePath = Path.Combine(values[4]);
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
            TableForm form1 = new TableForm();
            form1.Show();
            this.Close();
        }

        private void ThankYouScreen_Load(object sender, EventArgs e)
        {
            if (File.Exists(GlobalVariables.csvAdminAdvanceFilePath))
            {
                string[] lines;
                while (true)
                {
                    try
                    {
                        lines = File.ReadAllLines(GlobalVariables.csvAdminAdvanceFilePath);
                        break;
                    }
                    catch (IOException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                if (lines.Length > 0)
                {
                    string[] values = lines[lines.Length - 1].Split(',');
                    if (values[2] != null)
                    {
                        labelEndMessage.Text = values[2];

                        labelEndMessage.Location = new Point((this.ClientSize.Width - labelEndMessage.Width) / 2, (this.ClientSize.Height - labelEndMessage.Height) / 2);
                        btnMain.Location = new Point((this.ClientSize.Width - btnMain.Width) / 2, labelEndMessage.Bottom + 20);

                    }
                }
            }
            // Start the timer
            timer1.Start();
        }

        private void btnMain_Click(object sender, EventArgs e)
        {
            TableForm form1 = new TableForm();
            form1.Show();
            this.Close();
        }

        private void labelEndMessage_Click(object sender, EventArgs e)
        {

        }
    }
}
