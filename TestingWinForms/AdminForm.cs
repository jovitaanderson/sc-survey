using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestingWinForms
{
    public partial class AdminForm : Form
    {
        public AdminForm()
        {
            InitializeComponent();
        }

        private void AdminForm_Load(object sender, EventArgs e)
        {
            // Customize the appearance of the TabControl for vertical tabs
            tabControl.Alignment = TabAlignment.Left;
            tabControl.SizeMode = TabSizeMode.Fixed;
            tabControl.ItemSize = new Size(40, 120);
            tabControl.Appearance = TabAppearance.FlatButtons;

            // Add event handler for TabControl's SelectedIndexChanged event
            tabControl.SelectedIndexChanged += TabControl_SelectedIndexChanged;
            // Form_Load event or constructor
            tabControl.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabControl.DrawItem += TabControl1_DrawItem;

            
        }
        // DrawItem event handler
        private void TabControl1_DrawItem(object sender, DrawItemEventArgs e)
        {
            TabControl tabControl = (TabControl)sender;
            Graphics g = e.Graphics;
            Rectangle bounds = tabControl.GetTabRect(e.Index);

            /*if (e.Index == tabControl.SelectedIndex)
            {
                using (Brush selectedTabBrush = new SolidBrush(Color.LightBlue))
                {
                    g.FillRectangle(selectedTabBrush, bounds);
                }
            }*/

            using (Brush tabTextBrush = new SolidBrush(Color.Black))
            {
                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Center;
                stringFormat.LineAlignment = StringAlignment.Center;

                g.DrawString(tabControl.TabPages[e.Index].Text, tabControl.Font, tabTextBrush, bounds, stringFormat);
            }

        }

        private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Get the currently selected tab
            TabPage selectedTab = tabControl.SelectedTab;

            // Perform actions or update the UI based on the selected tab
            if (selectedTab == tabTable)
            {
                // Display specific content for TabPage 1
                
            }
            else if (selectedTab == tabQuestion1)
            {
                // Display specific content for TabPage 2
                
            }
            else if (selectedTab == tabQuestion2)
            {
                // Display specific content for TabPage 3
                
            }
            else if (selectedTab == tabQuestion3)
            {
                // Display specific content for TabPage 3
                
            }
            else if (selectedTab == tabDownload)
            {
                // Display specific content for TabPage 3
                
            }
            else if (selectedTab == tabAdvance)
            {
                // Display specific content for TabPage 3
                
            }
            // Add more conditions for additional tab pages as needed

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Form1 firstForm = new Form1();
            firstForm.Show();
            this.Close();
        }


        private void btnUploadImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Select Image";
                openFileDialog.Filter = "Image Files (*.png; *.jpg; *.jpeg; *.gif; *.bmp)|*.png; *.jpg; *.jpeg; *.gif; *.bmp";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedImagePath = openFileDialog.FileName;

                    // Load the selected image into the PictureBox
                    pictureBox.Image = Image.FromFile(selectedImagePath);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void labelTitle_Click(object sender, EventArgs e)
        {

        }

        private void textBoxTitle_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxXAxis_TextChanged(object sender, EventArgs e)
        {

        }

        private void labelXAxis_Click(object sender, EventArgs e)
        {

        }

        private void labelYAxis_Click(object sender, EventArgs e)
        {

        }

        private void textBoxYAxis_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
