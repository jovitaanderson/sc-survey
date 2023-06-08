using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
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

        // Helper method to prevent flickering
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams handleParams = base.CreateParams;
                handleParams.ExStyle = 0x02000000;
                return handleParams;
            }
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

                    if (values[5] != null)
                    {
                        string imagePath = Path.Combine(values[6]);
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

        private Font FontFromBinaryString(string fontData)
        {
            byte[] binaryData = Convert.FromBase64String(fontData);

            using (MemoryStream stream = new MemoryStream(binaryData))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return (Font)formatter.Deserialize(stream);
            }
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
                    if (values[6] != null)
                    {
                        // Load the font data from the CSV
                        //string fontEndSurvey = values[5]; // Assuming font data is at index 5
                        // Deserialize the font from the font data
                        //Font loadedFontEndSurvey = FontFromBinaryString(fontEndSurvey);
                        // Apply the font to the label or control of your choice
                        //labelEndMessage.Font = loadedFontEndSurvey;

                        loadContentToComponent(values, 7, labelEndMessage);
                    }
                }
            }
            // Start the timer
            timer1.Start();
        }

        void loadContentToComponent(string[] values, int ValueIndex, Label label)
        {
            string[] textProperties = values[ValueIndex].Split(';'); // Assuming text font and alignment data is at index 5
            string fontTitle = textProperties[0]; // First ; is text font

            Font loadedFontTitle = FontFromBinaryString(fontTitle);
            label.Font = loadedFontTitle;

            if (textProperties.Length > 3)
            {
                String textAlign = textProperties[1]; // Second ; is text align property
                String textWrap = textProperties[2]; // Third ; is text wrap property
                if (Enum.TryParse(textAlign, out ContentAlignment alignment))
                {
                    label.TextAlign = alignment;
                }
                else
                {
                    label.TextAlign = ContentAlignment.TopLeft; //default ContentAlignment
                }
                label.AutoSize = textWrap.Equals("true", StringComparison.OrdinalIgnoreCase);

                if (!string.IsNullOrEmpty(textProperties[3]))
                {
                    if (int.TryParse(textProperties[3].Replace("\0", ","), out int foreColorArgb))
                    {
                        Color foreColor = Color.FromArgb(foreColorArgb);
                        label.ForeColor = foreColor;
                    }
                    else
                    {
                        label.ForeColor = Color.Black; // Default forecolor if parsing fails
                    }
                }
            }
            label.Height = (int)Math.Ceiling(loadedFontTitle.GetHeight()) + Padding.Vertical;
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
