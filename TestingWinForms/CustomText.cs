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
        public Label ChangeTextLabel { get; private set; }

        public CustomText(Label label)
        {

            //SelectedAlignment = alignment;
            //SelectedWrap = wrap;

            ChangeTextLabel = label;

            InitializeComponent();
            InitializeControls();

            comboBoxAlignment.SelectedIndexChanged += comboBoxAlignment_SelectedIndexChanged;
            comboBoxWrap.SelectedIndexChanged += comboBoxAlignment_SelectedIndexChanged;

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

            comboBoxAlignment.SelectedIndex = GetAlignmentIndex(ChangeTextLabel.TextAlign);

            this.comboBoxWrap.Items.AddRange(new string[] {
            "NoWrap",
            "Wrap"});

            comboBoxWrap.SelectedIndex = GetWrapIndex(ChangeTextLabel.AutoSize);

            sampleLabel.Font = ChangeTextLabel.Font;
            sampleLabel.ForeColor = ChangeTextLabel.ForeColor;
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
            //SelectedAlignment = GetSelectedTextAlignment();
            //SelectedWrap = GetSelectedTextWrap();

            ChangeTextLabel = sampleLabel;



            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void comboBoxAlignment_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Retrieve the selected alignment and wrap values
            ContentAlignment selectedAlignment = GetSelectedTextAlignment();
            bool selectedWrap = GetSelectedTextWrap();

            // Update the label's properties
            sampleLabel.TextAlign = selectedAlignment;
            sampleLabel.AutoSize = selectedWrap;
        }

        private void btnChangeFont_Click(object sender, EventArgs e)
        {
            FontDialog fontDialog = new FontDialog();
            fontDialog.Font = sampleLabel.Font;
            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                sampleLabel.Font = fontDialog.Font;
            }
        }

        private void btnChangeColour_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            colorDialog.Color = sampleLabel.ForeColor;
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                // Get the selected color
                Color selectedColor = colorDialog.Color;

                // Apply the color to the font
                sampleLabel.ForeColor = selectedColor;
            }
        }
    }
}
