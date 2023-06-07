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
    public partial class CustomText : Form
    {

        public ContentAlignment SelectedAlignment { get; private set; }
        public bool SelectedWrap { get; private set; }

        public CustomText(ContentAlignment alignment, bool wrap)
        {

            SelectedAlignment = alignment;
            SelectedWrap = wrap;

            InitializeComponent();
            InitializeControls();

            // Set the passed alignment and wrap values
        }

        private void InitializeControls()
        {
            // Set the initial selected index of the ComboBox based on SelectedAlignment value
            this.comboBoxAlignment.Items.AddRange(new string[]
            {
                "Top Left",
                "Top Center",
                "Top Right",
                "Middle Left",
                "Middle Center",
                "Middle Right",
                "Bottom Left",
                "Bottom Center",
                "Bottom Right"
            });

            comboBoxAlignment.SelectedIndex = GetAlignmentIndex(SelectedAlignment);

            this.comboBoxWrap.Items.AddRange(new string[] {
            "NoWrap",
            "Wrap"});

            comboBoxWrap.SelectedIndex = GetWrapIndex(SelectedWrap);
        }


        private int GetAlignmentIndex(ContentAlignment alignment)
        {
            // Determine the index based on the alignment value
            switch (alignment)
            {
                case ContentAlignment.TopLeft:
                    return 0;
                case ContentAlignment.TopCenter:
                    return 1;
                case ContentAlignment.TopRight:
                    return 2;
                case ContentAlignment.MiddleLeft:
                    return 3;
                case ContentAlignment.MiddleCenter:
                    return 4;
                case ContentAlignment.MiddleRight:
                    return 5;
                case ContentAlignment.BottomLeft:
                    return 6;
                case ContentAlignment.BottomCenter:
                    return 7;
                case ContentAlignment.BottomRight:
                    return 8;
                default:
                    return 0;
            }
        }


        private int GetWrapIndex(bool alignment)
        {
            // Determine the index based on the alignment value
            switch (alignment)
            {
                case false:
                    return 0;
                case true:
                    return 1;
                default:
                    return 1;
            }
        }


        private ContentAlignment GetSelectedTextAlignment()
        {
            switch (comboBoxAlignment.SelectedItem.ToString())
            {
                case "Top Left":
                    return ContentAlignment.TopLeft;
                case "Top Center":
                    return ContentAlignment.TopCenter;
                case "Top Right":
                    return ContentAlignment.TopRight;
                case "Middle Left":
                    return ContentAlignment.MiddleLeft;
                case "Middle Center":
                    return ContentAlignment.MiddleCenter;
                case "Middle Right":
                    return ContentAlignment.MiddleRight;
                case "Bottom Left":
                    return ContentAlignment.BottomLeft;
                case "Bottom Center":
                    return ContentAlignment.BottomCenter;
                case "Bottom Right":
                    return ContentAlignment.BottomRight;
                default:
                    return ContentAlignment.TopLeft; // Default alignment
            }
            
        }


        private bool GetSelectedTextWrap()
        {
            switch (comboBoxWrap.SelectedItem.ToString())
            {
                case "NoWrap":
                    return false;
                case "Wrap":
                    return true;
                default:
                    return true;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            SelectedAlignment = GetSelectedTextAlignment();
            SelectedWrap = GetSelectedTextWrap();



            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
