namespace TestingWinForms
{
    partial class Form2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.optionARadioButton = new System.Windows.Forms.RadioButton();
            this.optionBRadioButton = new System.Windows.Forms.RadioButton();
            this.optionCRadioButton = new System.Windows.Forms.RadioButton();
            this.questionLabel = new System.Windows.Forms.Label();
            this.submitButton = new System.Windows.Forms.Button();
            this.optionACheckBox = new System.Windows.Forms.CheckBox();
            this.optionBCheckBox = new System.Windows.Forms.CheckBox();
            this.optionCCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // optionARadioButton
            // 
            this.optionARadioButton.AutoSize = true;
            this.optionARadioButton.Location = new System.Drawing.Point(226, 185);
            this.optionARadioButton.Name = "optionARadioButton";
            this.optionARadioButton.Size = new System.Drawing.Size(126, 24);
            this.optionARadioButton.TabIndex = 1;
            this.optionARadioButton.TabStop = true;
            this.optionARadioButton.Text = "radioButton1";
            this.optionARadioButton.UseVisualStyleBackColor = true;
            // 
            // optionBRadioButton
            // 
            this.optionBRadioButton.AutoSize = true;
            this.optionBRadioButton.Location = new System.Drawing.Point(226, 216);
            this.optionBRadioButton.Name = "optionBRadioButton";
            this.optionBRadioButton.Size = new System.Drawing.Size(126, 24);
            this.optionBRadioButton.TabIndex = 2;
            this.optionBRadioButton.TabStop = true;
            this.optionBRadioButton.Text = "radioButton2";
            this.optionBRadioButton.UseVisualStyleBackColor = true;
            // 
            // optionCRadioButton
            // 
            this.optionCRadioButton.AutoSize = true;
            this.optionCRadioButton.Location = new System.Drawing.Point(226, 247);
            this.optionCRadioButton.Name = "optionCRadioButton";
            this.optionCRadioButton.Size = new System.Drawing.Size(126, 24);
            this.optionCRadioButton.TabIndex = 3;
            this.optionCRadioButton.TabStop = true;
            this.optionCRadioButton.Text = "radioButton3";
            this.optionCRadioButton.UseVisualStyleBackColor = true;
            // 
            // questionLabel
            // 
            this.questionLabel.AutoSize = true;
            this.questionLabel.Location = new System.Drawing.Point(226, 130);
            this.questionLabel.Name = "questionLabel";
            this.questionLabel.Size = new System.Drawing.Size(51, 20);
            this.questionLabel.TabIndex = 4;
            this.questionLabel.Text = "label1";
            // 
            // submitButton
            // 
            this.submitButton.Location = new System.Drawing.Point(630, 352);
            this.submitButton.Name = "submitButton";
            this.submitButton.Size = new System.Drawing.Size(75, 23);
            this.submitButton.TabIndex = 5;
            this.submitButton.Text = "button1";
            this.submitButton.UseVisualStyleBackColor = true;
            this.submitButton.Click += new System.EventHandler(this.submitButton_Click);
            // 
            // optionACheckBox
            // 
            this.optionACheckBox.AutoSize = true;
            this.optionACheckBox.Location = new System.Drawing.Point(415, 184);
            this.optionACheckBox.Name = "optionACheckBox";
            this.optionACheckBox.Size = new System.Drawing.Size(113, 24);
            this.optionACheckBox.TabIndex = 6;
            this.optionACheckBox.Text = "checkBox1";
            this.optionACheckBox.UseVisualStyleBackColor = true;
            // 
            // optionBCheckBox
            // 
            this.optionBCheckBox.AutoSize = true;
            this.optionBCheckBox.Location = new System.Drawing.Point(415, 216);
            this.optionBCheckBox.Name = "optionBCheckBox";
            this.optionBCheckBox.Size = new System.Drawing.Size(113, 24);
            this.optionBCheckBox.TabIndex = 7;
            this.optionBCheckBox.Text = "checkBox2";
            this.optionBCheckBox.UseVisualStyleBackColor = true;
            // 
            // optionCCheckBox
            // 
            this.optionCCheckBox.AutoSize = true;
            this.optionCCheckBox.Location = new System.Drawing.Point(415, 246);
            this.optionCCheckBox.Name = "optionCCheckBox";
            this.optionCCheckBox.Size = new System.Drawing.Size(113, 24);
            this.optionCCheckBox.TabIndex = 8;
            this.optionCCheckBox.Text = "checkBox3";
            this.optionCCheckBox.UseVisualStyleBackColor = true;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.optionCCheckBox);
            this.Controls.Add(this.optionBCheckBox);
            this.Controls.Add(this.optionACheckBox);
            this.Controls.Add(this.submitButton);
            this.Controls.Add(this.questionLabel);
            this.Controls.Add(this.optionCRadioButton);
            this.Controls.Add(this.optionBRadioButton);
            this.Controls.Add(this.optionARadioButton);
            this.Name = "Form2";
            this.Text = "Form2";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.RadioButton optionARadioButton;
        private System.Windows.Forms.RadioButton optionBRadioButton;
        private System.Windows.Forms.RadioButton optionCRadioButton;
        private System.Windows.Forms.Label questionLabel;
        private System.Windows.Forms.Button submitButton;
        private System.Windows.Forms.CheckBox optionACheckBox;
        private System.Windows.Forms.CheckBox optionBCheckBox;
        private System.Windows.Forms.CheckBox optionCCheckBox;
    }
}