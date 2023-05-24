
namespace TestingWinForms
{
    partial class AdminForm
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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabTable = new System.Windows.Forms.TabPage();
            this.textBoxYAxis = new System.Windows.Forms.TextBox();
            this.textBoxXAxis = new System.Windows.Forms.TextBox();
            this.textBoxTitle = new System.Windows.Forms.TextBox();
            this.labelYAxis = new System.Windows.Forms.Label();
            this.labelXAxis = new System.Windows.Forms.Label();
            this.labelTitle = new System.Windows.Forms.Label();
            this.tabQuestion1 = new System.Windows.Forms.TabPage();
            this.textBoxA15 = new System.Windows.Forms.TextBox();
            this.textBoxA14 = new System.Windows.Forms.TextBox();
            this.textBoxA13 = new System.Windows.Forms.TextBox();
            this.textBoxA12 = new System.Windows.Forms.TextBox();
            this.textBoxA11 = new System.Windows.Forms.TextBox();
            this.textBoxQ1 = new System.Windows.Forms.TextBox();
            this.labelA15 = new System.Windows.Forms.Label();
            this.labelA14 = new System.Windows.Forms.Label();
            this.labelA13 = new System.Windows.Forms.Label();
            this.labelA12 = new System.Windows.Forms.Label();
            this.labelA11 = new System.Windows.Forms.Label();
            this.labelQ1 = new System.Windows.Forms.Label();
            this.tabQuestion2 = new System.Windows.Forms.TabPage();
            this.textBoxA25 = new System.Windows.Forms.TextBox();
            this.textBoxA24 = new System.Windows.Forms.TextBox();
            this.textBoxA23 = new System.Windows.Forms.TextBox();
            this.textBoxA22 = new System.Windows.Forms.TextBox();
            this.textBoxA21 = new System.Windows.Forms.TextBox();
            this.textBoxQ2 = new System.Windows.Forms.TextBox();
            this.labelA25 = new System.Windows.Forms.Label();
            this.labelA24 = new System.Windows.Forms.Label();
            this.labelA23 = new System.Windows.Forms.Label();
            this.labelA22 = new System.Windows.Forms.Label();
            this.labelA21 = new System.Windows.Forms.Label();
            this.labelQ2 = new System.Windows.Forms.Label();
            this.tabQuestion3 = new System.Windows.Forms.TabPage();
            this.textBoxA35 = new System.Windows.Forms.TextBox();
            this.textBoxA34 = new System.Windows.Forms.TextBox();
            this.textBoxA33 = new System.Windows.Forms.TextBox();
            this.textBoxA32 = new System.Windows.Forms.TextBox();
            this.textBoxA31 = new System.Windows.Forms.TextBox();
            this.textBoxQ3 = new System.Windows.Forms.TextBox();
            this.labelA35 = new System.Windows.Forms.Label();
            this.labelA34 = new System.Windows.Forms.Label();
            this.labelA33 = new System.Windows.Forms.Label();
            this.labelA32 = new System.Windows.Forms.Label();
            this.labelA31 = new System.Windows.Forms.Label();
            this.labelQ3 = new System.Windows.Forms.Label();
            this.tabDownload = new System.Windows.Forms.TabPage();
            this.btnDownload = new System.Windows.Forms.Button();
            this.dateTimePickerEndDate = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerStartDate = new System.Windows.Forms.DateTimePicker();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.tabAdvance = new System.Windows.Forms.TabPage();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.btnUploadImage = new System.Windows.Forms.Button();
            this.textBoxEndMessage = new System.Windows.Forms.TextBox();
            this.comboBoxRandomQns = new System.Windows.Forms.ComboBox();
            this.label17 = new System.Windows.Forms.Label();
            this.textBoxTimeOut = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.labelTimeOut = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.btnEndApp = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.tabControl.SuspendLayout();
            this.tabTable.SuspendLayout();
            this.tabQuestion1.SuspendLayout();
            this.tabQuestion2.SuspendLayout();
            this.tabQuestion3.SuspendLayout();
            this.tabDownload.SuspendLayout();
            this.tabAdvance.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Alignment = System.Windows.Forms.TabAlignment.Left;
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.tabTable);
            this.tabControl.Controls.Add(this.tabDownload);
            this.tabControl.Controls.Add(this.tabAdvance);
            this.tabControl.Controls.Add(this.tabQuestion1);
            this.tabControl.Controls.Add(this.tabQuestion2);
            this.tabControl.Controls.Add(this.tabQuestion3);
            this.tabControl.Location = new System.Drawing.Point(170, 128);
            this.tabControl.Margin = new System.Windows.Forms.Padding(20);
            this.tabControl.Multiline = true;
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(400, 167);
            this.tabControl.TabIndex = 0;
            // 
            // tabTable
            // 
            this.tabTable.Controls.Add(this.textBoxYAxis);
            this.tabTable.Controls.Add(this.textBoxXAxis);
            this.tabTable.Controls.Add(this.textBoxTitle);
            this.tabTable.Controls.Add(this.labelYAxis);
            this.tabTable.Controls.Add(this.labelXAxis);
            this.tabTable.Controls.Add(this.labelTitle);
            this.tabTable.Location = new System.Drawing.Point(46, 4);
            this.tabTable.Name = "tabTable";
            this.tabTable.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tabTable.Size = new System.Drawing.Size(350, 282);
            this.tabTable.TabIndex = 0;
            this.tabTable.Text = "Table";
            this.tabTable.UseVisualStyleBackColor = true;
            this.tabTable.Click += new System.EventHandler(this.tabTable_Click);
            // 
            // textBoxYAxis
            // 
            this.textBoxYAxis.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxYAxis.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxYAxis.Location = new System.Drawing.Point(194, 130);
            this.textBoxYAxis.Name = "textBoxYAxis";
            this.textBoxYAxis.Size = new System.Drawing.Size(150, 38);
            this.textBoxYAxis.TabIndex = 5;
            // 
            // textBoxXAxis
            // 
            this.textBoxXAxis.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxXAxis.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxXAxis.Location = new System.Drawing.Point(194, 66);
            this.textBoxXAxis.Name = "textBoxXAxis";
            this.textBoxXAxis.Size = new System.Drawing.Size(150, 38);
            this.textBoxXAxis.TabIndex = 4;
            // 
            // textBoxTitle
            // 
            this.textBoxTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxTitle.Location = new System.Drawing.Point(194, 3);
            this.textBoxTitle.Name = "textBoxTitle";
            this.textBoxTitle.Size = new System.Drawing.Size(150, 38);
            this.textBoxTitle.TabIndex = 3;
            // 
            // labelYAxis
            // 
            this.labelYAxis.AutoSize = true;
            this.labelYAxis.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelYAxis.Location = new System.Drawing.Point(3, 130);
            this.labelYAxis.Name = "labelYAxis";
            this.labelYAxis.Size = new System.Drawing.Size(185, 32);
            this.labelYAxis.TabIndex = 2;
            this.labelYAxis.Text = "Y Axis Name:";
            // 
            // labelXAxis
            // 
            this.labelXAxis.AutoSize = true;
            this.labelXAxis.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelXAxis.Location = new System.Drawing.Point(3, 66);
            this.labelXAxis.Name = "labelXAxis";
            this.labelXAxis.Size = new System.Drawing.Size(185, 32);
            this.labelXAxis.TabIndex = 1;
            this.labelXAxis.Text = "X Axis Name:";
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTitle.Location = new System.Drawing.Point(3, 3);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(157, 32);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "Table Title:";
            // 
            // tabQuestion1
            // 
            this.tabQuestion1.Controls.Add(this.textBoxA15);
            this.tabQuestion1.Controls.Add(this.textBoxA14);
            this.tabQuestion1.Controls.Add(this.textBoxA13);
            this.tabQuestion1.Controls.Add(this.textBoxA12);
            this.tabQuestion1.Controls.Add(this.textBoxA11);
            this.tabQuestion1.Controls.Add(this.textBoxQ1);
            this.tabQuestion1.Controls.Add(this.labelA15);
            this.tabQuestion1.Controls.Add(this.labelA14);
            this.tabQuestion1.Controls.Add(this.labelA13);
            this.tabQuestion1.Controls.Add(this.labelA12);
            this.tabQuestion1.Controls.Add(this.labelA11);
            this.tabQuestion1.Controls.Add(this.labelQ1);
            this.tabQuestion1.Location = new System.Drawing.Point(67, 4);
            this.tabQuestion1.Name = "tabQuestion1";
            this.tabQuestion1.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tabQuestion1.Size = new System.Drawing.Size(329, 159);
            this.tabQuestion1.TabIndex = 1;
            this.tabQuestion1.Text = "Question 1";
            this.tabQuestion1.UseVisualStyleBackColor = true;
            // 
            // textBoxA15
            // 
            this.textBoxA15.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxA15.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxA15.Location = new System.Drawing.Point(196, 244);
            this.textBoxA15.Name = "textBoxA15";
            this.textBoxA15.Size = new System.Drawing.Size(106, 38);
            this.textBoxA15.TabIndex = 11;
            // 
            // textBoxA14
            // 
            this.textBoxA14.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxA14.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxA14.Location = new System.Drawing.Point(196, 200);
            this.textBoxA14.Name = "textBoxA14";
            this.textBoxA14.Size = new System.Drawing.Size(106, 38);
            this.textBoxA14.TabIndex = 10;
            // 
            // textBoxA13
            // 
            this.textBoxA13.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxA13.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxA13.Location = new System.Drawing.Point(196, 152);
            this.textBoxA13.Name = "textBoxA13";
            this.textBoxA13.Size = new System.Drawing.Size(106, 38);
            this.textBoxA13.TabIndex = 9;
            // 
            // textBoxA12
            // 
            this.textBoxA12.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxA12.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxA12.Location = new System.Drawing.Point(196, 108);
            this.textBoxA12.Name = "textBoxA12";
            this.textBoxA12.Size = new System.Drawing.Size(106, 38);
            this.textBoxA12.TabIndex = 8;
            // 
            // textBoxA11
            // 
            this.textBoxA11.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxA11.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxA11.Location = new System.Drawing.Point(196, 59);
            this.textBoxA11.Name = "textBoxA11";
            this.textBoxA11.Size = new System.Drawing.Size(106, 38);
            this.textBoxA11.TabIndex = 7;
            // 
            // textBoxQ1
            // 
            this.textBoxQ1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxQ1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxQ1.Location = new System.Drawing.Point(196, 9);
            this.textBoxQ1.Name = "textBoxQ1";
            this.textBoxQ1.Size = new System.Drawing.Size(106, 38);
            this.textBoxQ1.TabIndex = 6;
            // 
            // labelA15
            // 
            this.labelA15.AutoSize = true;
            this.labelA15.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelA15.Location = new System.Drawing.Point(37, 244);
            this.labelA15.Name = "labelA15";
            this.labelA15.Size = new System.Drawing.Size(129, 32);
            this.labelA15.TabIndex = 5;
            this.labelA15.Text = "answer 5";
            // 
            // labelA14
            // 
            this.labelA14.AutoSize = true;
            this.labelA14.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelA14.Location = new System.Drawing.Point(37, 200);
            this.labelA14.Name = "labelA14";
            this.labelA14.Size = new System.Drawing.Size(129, 32);
            this.labelA14.TabIndex = 4;
            this.labelA14.Text = "answer 4";
            // 
            // labelA13
            // 
            this.labelA13.AutoSize = true;
            this.labelA13.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelA13.Location = new System.Drawing.Point(37, 155);
            this.labelA13.Name = "labelA13";
            this.labelA13.Size = new System.Drawing.Size(129, 32);
            this.labelA13.TabIndex = 3;
            this.labelA13.Text = "answer 3";
            // 
            // labelA12
            // 
            this.labelA12.AutoSize = true;
            this.labelA12.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelA12.Location = new System.Drawing.Point(37, 108);
            this.labelA12.Name = "labelA12";
            this.labelA12.Size = new System.Drawing.Size(129, 32);
            this.labelA12.TabIndex = 2;
            this.labelA12.Text = "answer 2";
            // 
            // labelA11
            // 
            this.labelA11.AutoSize = true;
            this.labelA11.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelA11.Location = new System.Drawing.Point(37, 59);
            this.labelA11.Name = "labelA11";
            this.labelA11.Size = new System.Drawing.Size(129, 32);
            this.labelA11.TabIndex = 1;
            this.labelA11.Text = "answer 1";
            // 
            // labelQ1
            // 
            this.labelQ1.AutoSize = true;
            this.labelQ1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelQ1.Location = new System.Drawing.Point(5, 12);
            this.labelQ1.Name = "labelQ1";
            this.labelQ1.Size = new System.Drawing.Size(153, 32);
            this.labelQ1.TabIndex = 0;
            this.labelQ1.Text = "Question 1";
            // 
            // tabQuestion2
            // 
            this.tabQuestion2.Controls.Add(this.textBoxA25);
            this.tabQuestion2.Controls.Add(this.textBoxA24);
            this.tabQuestion2.Controls.Add(this.textBoxA23);
            this.tabQuestion2.Controls.Add(this.textBoxA22);
            this.tabQuestion2.Controls.Add(this.textBoxA21);
            this.tabQuestion2.Controls.Add(this.textBoxQ2);
            this.tabQuestion2.Controls.Add(this.labelA25);
            this.tabQuestion2.Controls.Add(this.labelA24);
            this.tabQuestion2.Controls.Add(this.labelA23);
            this.tabQuestion2.Controls.Add(this.labelA22);
            this.tabQuestion2.Controls.Add(this.labelA21);
            this.tabQuestion2.Controls.Add(this.labelQ2);
            this.tabQuestion2.Location = new System.Drawing.Point(46, 4);
            this.tabQuestion2.Name = "tabQuestion2";
            this.tabQuestion2.Size = new System.Drawing.Size(350, 282);
            this.tabQuestion2.TabIndex = 2;
            this.tabQuestion2.Text = "Question 2";
            this.tabQuestion2.UseVisualStyleBackColor = true;
            // 
            // textBoxA25
            // 
            this.textBoxA25.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxA25.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxA25.Location = new System.Drawing.Point(169, 238);
            this.textBoxA25.Name = "textBoxA25";
            this.textBoxA25.Size = new System.Drawing.Size(178, 38);
            this.textBoxA25.TabIndex = 22;
            // 
            // textBoxA24
            // 
            this.textBoxA24.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxA24.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxA24.Location = new System.Drawing.Point(169, 194);
            this.textBoxA24.Name = "textBoxA24";
            this.textBoxA24.Size = new System.Drawing.Size(178, 38);
            this.textBoxA24.TabIndex = 21;
            // 
            // textBoxA23
            // 
            this.textBoxA23.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxA23.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxA23.Location = new System.Drawing.Point(169, 146);
            this.textBoxA23.Name = "textBoxA23";
            this.textBoxA23.Size = new System.Drawing.Size(178, 38);
            this.textBoxA23.TabIndex = 20;
            // 
            // textBoxA22
            // 
            this.textBoxA22.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxA22.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxA22.Location = new System.Drawing.Point(169, 102);
            this.textBoxA22.Name = "textBoxA22";
            this.textBoxA22.Size = new System.Drawing.Size(178, 38);
            this.textBoxA22.TabIndex = 19;
            // 
            // textBoxA21
            // 
            this.textBoxA21.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxA21.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxA21.Location = new System.Drawing.Point(169, 53);
            this.textBoxA21.Name = "textBoxA21";
            this.textBoxA21.Size = new System.Drawing.Size(178, 38);
            this.textBoxA21.TabIndex = 18;
            // 
            // textBoxQ2
            // 
            this.textBoxQ2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxQ2.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxQ2.Location = new System.Drawing.Point(169, 3);
            this.textBoxQ2.Name = "textBoxQ2";
            this.textBoxQ2.Size = new System.Drawing.Size(178, 38);
            this.textBoxQ2.TabIndex = 17;
            // 
            // labelA25
            // 
            this.labelA25.AutoSize = true;
            this.labelA25.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelA25.Location = new System.Drawing.Point(18, 241);
            this.labelA25.Name = "labelA25";
            this.labelA25.Size = new System.Drawing.Size(129, 32);
            this.labelA25.TabIndex = 16;
            this.labelA25.Text = "answer 5";
            // 
            // labelA24
            // 
            this.labelA24.AutoSize = true;
            this.labelA24.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelA24.Location = new System.Drawing.Point(18, 197);
            this.labelA24.Name = "labelA24";
            this.labelA24.Size = new System.Drawing.Size(129, 32);
            this.labelA24.TabIndex = 15;
            this.labelA24.Text = "answer 4";
            // 
            // labelA23
            // 
            this.labelA23.AutoSize = true;
            this.labelA23.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelA23.Location = new System.Drawing.Point(18, 152);
            this.labelA23.Name = "labelA23";
            this.labelA23.Size = new System.Drawing.Size(129, 32);
            this.labelA23.TabIndex = 14;
            this.labelA23.Text = "answer 3";
            // 
            // labelA22
            // 
            this.labelA22.AutoSize = true;
            this.labelA22.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelA22.Location = new System.Drawing.Point(18, 105);
            this.labelA22.Name = "labelA22";
            this.labelA22.Size = new System.Drawing.Size(129, 32);
            this.labelA22.TabIndex = 13;
            this.labelA22.Text = "answer 2";
            // 
            // labelA21
            // 
            this.labelA21.AutoSize = true;
            this.labelA21.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelA21.Location = new System.Drawing.Point(18, 56);
            this.labelA21.Name = "labelA21";
            this.labelA21.Size = new System.Drawing.Size(129, 32);
            this.labelA21.TabIndex = 12;
            this.labelA21.Text = "answer 1";
            // 
            // labelQ2
            // 
            this.labelQ2.AutoSize = true;
            this.labelQ2.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelQ2.Location = new System.Drawing.Point(2, 9);
            this.labelQ2.Name = "labelQ2";
            this.labelQ2.Size = new System.Drawing.Size(153, 32);
            this.labelQ2.TabIndex = 1;
            this.labelQ2.Text = "Question 2";
            this.labelQ2.Click += new System.EventHandler(this.labelQ2_Click);
            // 
            // tabQuestion3
            // 
            this.tabQuestion3.Controls.Add(this.textBoxA35);
            this.tabQuestion3.Controls.Add(this.textBoxA34);
            this.tabQuestion3.Controls.Add(this.textBoxA33);
            this.tabQuestion3.Controls.Add(this.textBoxA32);
            this.tabQuestion3.Controls.Add(this.textBoxA31);
            this.tabQuestion3.Controls.Add(this.textBoxQ3);
            this.tabQuestion3.Controls.Add(this.labelA35);
            this.tabQuestion3.Controls.Add(this.labelA34);
            this.tabQuestion3.Controls.Add(this.labelA33);
            this.tabQuestion3.Controls.Add(this.labelA32);
            this.tabQuestion3.Controls.Add(this.labelA31);
            this.tabQuestion3.Controls.Add(this.labelQ3);
            this.tabQuestion3.Location = new System.Drawing.Point(67, 4);
            this.tabQuestion3.Name = "tabQuestion3";
            this.tabQuestion3.Size = new System.Drawing.Size(329, 159);
            this.tabQuestion3.TabIndex = 3;
            this.tabQuestion3.Text = "Question 3";
            this.tabQuestion3.UseVisualStyleBackColor = true;
            // 
            // textBoxA35
            // 
            this.textBoxA35.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxA35.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxA35.Location = new System.Drawing.Point(171, 254);
            this.textBoxA35.Name = "textBoxA35";
            this.textBoxA35.Size = new System.Drawing.Size(134, 38);
            this.textBoxA35.TabIndex = 34;
            // 
            // textBoxA34
            // 
            this.textBoxA34.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxA34.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxA34.Location = new System.Drawing.Point(171, 200);
            this.textBoxA34.Name = "textBoxA34";
            this.textBoxA34.Size = new System.Drawing.Size(134, 38);
            this.textBoxA34.TabIndex = 33;
            // 
            // textBoxA33
            // 
            this.textBoxA33.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxA33.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxA33.Location = new System.Drawing.Point(171, 152);
            this.textBoxA33.Name = "textBoxA33";
            this.textBoxA33.Size = new System.Drawing.Size(134, 38);
            this.textBoxA33.TabIndex = 32;
            // 
            // textBoxA32
            // 
            this.textBoxA32.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxA32.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxA32.Location = new System.Drawing.Point(171, 108);
            this.textBoxA32.Name = "textBoxA32";
            this.textBoxA32.Size = new System.Drawing.Size(134, 38);
            this.textBoxA32.TabIndex = 31;
            // 
            // textBoxA31
            // 
            this.textBoxA31.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxA31.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxA31.Location = new System.Drawing.Point(171, 59);
            this.textBoxA31.Name = "textBoxA31";
            this.textBoxA31.Size = new System.Drawing.Size(134, 38);
            this.textBoxA31.TabIndex = 30;
            // 
            // textBoxQ3
            // 
            this.textBoxQ3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxQ3.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxQ3.Location = new System.Drawing.Point(171, 9);
            this.textBoxQ3.Name = "textBoxQ3";
            this.textBoxQ3.Size = new System.Drawing.Size(134, 38);
            this.textBoxQ3.TabIndex = 29;
            // 
            // labelA35
            // 
            this.labelA35.AutoSize = true;
            this.labelA35.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelA35.Location = new System.Drawing.Point(20, 254);
            this.labelA35.Name = "labelA35";
            this.labelA35.Size = new System.Drawing.Size(129, 32);
            this.labelA35.TabIndex = 28;
            this.labelA35.Text = "answer 5";
            // 
            // labelA34
            // 
            this.labelA34.AutoSize = true;
            this.labelA34.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelA34.Location = new System.Drawing.Point(20, 203);
            this.labelA34.Name = "labelA34";
            this.labelA34.Size = new System.Drawing.Size(129, 32);
            this.labelA34.TabIndex = 27;
            this.labelA34.Text = "answer 4";
            // 
            // labelA33
            // 
            this.labelA33.AutoSize = true;
            this.labelA33.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelA33.Location = new System.Drawing.Point(20, 155);
            this.labelA33.Name = "labelA33";
            this.labelA33.Size = new System.Drawing.Size(129, 32);
            this.labelA33.TabIndex = 26;
            this.labelA33.Text = "answer 3";
            // 
            // labelA32
            // 
            this.labelA32.AutoSize = true;
            this.labelA32.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelA32.Location = new System.Drawing.Point(20, 108);
            this.labelA32.Name = "labelA32";
            this.labelA32.Size = new System.Drawing.Size(129, 32);
            this.labelA32.TabIndex = 25;
            this.labelA32.Text = "answer 2";
            // 
            // labelA31
            // 
            this.labelA31.AutoSize = true;
            this.labelA31.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelA31.Location = new System.Drawing.Point(20, 59);
            this.labelA31.Name = "labelA31";
            this.labelA31.Size = new System.Drawing.Size(129, 32);
            this.labelA31.TabIndex = 24;
            this.labelA31.Text = "answer 1";
            // 
            // labelQ3
            // 
            this.labelQ3.AutoSize = true;
            this.labelQ3.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelQ3.Location = new System.Drawing.Point(4, 12);
            this.labelQ3.Name = "labelQ3";
            this.labelQ3.Size = new System.Drawing.Size(153, 32);
            this.labelQ3.TabIndex = 23;
            this.labelQ3.Text = "Question 3";
            // 
            // tabDownload
            // 
            this.tabDownload.Controls.Add(this.btnDownload);
            this.tabDownload.Controls.Add(this.dateTimePickerEndDate);
            this.tabDownload.Controls.Add(this.dateTimePickerStartDate);
            this.tabDownload.Controls.Add(this.label13);
            this.tabDownload.Controls.Add(this.label12);
            this.tabDownload.Location = new System.Drawing.Point(67, 4);
            this.tabDownload.Name = "tabDownload";
            this.tabDownload.Size = new System.Drawing.Size(329, 159);
            this.tabDownload.TabIndex = 4;
            this.tabDownload.Text = "Download";
            this.tabDownload.UseVisualStyleBackColor = true;
            // 
            // btnDownload
            // 
            this.btnDownload.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDownload.Location = new System.Drawing.Point(164, 106);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(169, 41);
            this.btnDownload.TabIndex = 6;
            this.btnDownload.Text = "Download";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // dateTimePickerEndDate
            // 
            this.dateTimePickerEndDate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePickerEndDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimePickerEndDate.Location = new System.Drawing.Point(164, 59);
            this.dateTimePickerEndDate.Name = "dateTimePickerEndDate";
            this.dateTimePickerEndDate.Size = new System.Drawing.Size(162, 38);
            this.dateTimePickerEndDate.TabIndex = 5;
            // 
            // dateTimePickerStartDate
            // 
            this.dateTimePickerStartDate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePickerStartDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimePickerStartDate.Location = new System.Drawing.Point(164, 12);
            this.dateTimePickerStartDate.Name = "dateTimePickerStartDate";
            this.dateTimePickerStartDate.Size = new System.Drawing.Size(162, 38);
            this.dateTimePickerStartDate.TabIndex = 4;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(6, 59);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(144, 32);
            this.label13.TabIndex = 1;
            this.label13.Text = "End date: ";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(6, 12);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(153, 32);
            this.label12.TabIndex = 0;
            this.label12.Text = "Start date: ";
            // 
            // tabAdvance
            // 
            this.tabAdvance.Controls.Add(this.pictureBox);
            this.tabAdvance.Controls.Add(this.btnUploadImage);
            this.tabAdvance.Controls.Add(this.textBoxEndMessage);
            this.tabAdvance.Controls.Add(this.comboBoxRandomQns);
            this.tabAdvance.Controls.Add(this.label17);
            this.tabAdvance.Controls.Add(this.textBoxTimeOut);
            this.tabAdvance.Controls.Add(this.label16);
            this.tabAdvance.Controls.Add(this.label15);
            this.tabAdvance.Controls.Add(this.label14);
            this.tabAdvance.Controls.Add(this.labelTimeOut);
            this.tabAdvance.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabAdvance.Location = new System.Drawing.Point(25, 4);
            this.tabAdvance.Name = "tabAdvance";
            this.tabAdvance.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.tabAdvance.Size = new System.Drawing.Size(371, 305);
            this.tabAdvance.TabIndex = 5;
            this.tabAdvance.Text = "Advance";
            this.tabAdvance.UseVisualStyleBackColor = true;
            // 
            // pictureBox
            // 
            this.pictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox.Location = new System.Drawing.Point(21, 292);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(172, 243);
            this.pictureBox.TabIndex = 9;
            this.pictureBox.TabStop = false;
            // 
            // btnUploadImage
            // 
            this.btnUploadImage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUploadImage.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUploadImage.Location = new System.Drawing.Point(277, 212);
            this.btnUploadImage.Name = "btnUploadImage";
            this.btnUploadImage.Size = new System.Drawing.Size(70, 54);
            this.btnUploadImage.TabIndex = 8;
            this.btnUploadImage.Text = "Upload Background Image";
            this.btnUploadImage.UseVisualStyleBackColor = true;
            this.btnUploadImage.Click += new System.EventHandler(this.btnUploadImage_Click);
            // 
            // textBoxEndMessage
            // 
            this.textBoxEndMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxEndMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxEndMessage.Location = new System.Drawing.Point(277, 150);
            this.textBoxEndMessage.Name = "textBoxEndMessage";
            this.textBoxEndMessage.Size = new System.Drawing.Size(70, 38);
            this.textBoxEndMessage.TabIndex = 7;
            // 
            // comboBoxRandomQns
            // 
            this.comboBoxRandomQns.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxRandomQns.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxRandomQns.FormattingEnabled = true;
            this.comboBoxRandomQns.Items.AddRange(new object[] {
            "Yes",
            "No"});
            this.comboBoxRandomQns.Location = new System.Drawing.Point(277, 87);
            this.comboBoxRandomQns.Name = "comboBoxRandomQns";
            this.comboBoxRandomQns.Size = new System.Drawing.Size(70, 39);
            this.comboBoxRandomQns.TabIndex = 6;
            // 
            // label17
            // 
            this.label17.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(554, 25);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(64, 32);
            this.label17.TabIndex = 5;
            this.label17.Text = "Sec";
            // 
            // textBoxTimeOut
            // 
            this.textBoxTimeOut.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTimeOut.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxTimeOut.Location = new System.Drawing.Point(277, 19);
            this.textBoxTimeOut.Name = "textBoxTimeOut";
            this.textBoxTimeOut.Size = new System.Drawing.Size(70, 38);
            this.textBoxTimeOut.TabIndex = 4;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(15, 224);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(260, 32);
            this.label16.TabIndex = 3;
            this.label16.Text = "Background image:";
            this.label16.Click += new System.EventHandler(this.label16_Click);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(15, 150);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(229, 32);
            this.label15.TabIndex = 2;
            this.label15.Text = "End Survey msg:";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(15, 89);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(189, 32);
            this.label14.TabIndex = 1;
            this.label14.Text = "Random Qns:";
            // 
            // labelTimeOut
            // 
            this.labelTimeOut.AutoSize = true;
            this.labelTimeOut.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTimeOut.Location = new System.Drawing.Point(15, 22);
            this.labelTimeOut.Name = "labelTimeOut";
            this.labelTimeOut.Size = new System.Drawing.Size(133, 32);
            this.labelTimeOut.TabIndex = 0;
            this.labelTimeOut.Text = "Time out:";
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.Location = new System.Drawing.Point(444, 303);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(117, 40);
            this.btnExit.TabIndex = 1;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSave.Location = new System.Drawing.Point(567, 303);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(117, 40);
            this.buttonSave.TabIndex = 3;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // btnEndApp
            // 
            this.btnEndApp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEndApp.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEndApp.Location = new System.Drawing.Point(204, 303);
            this.btnEndApp.Name = "btnEndApp";
            this.btnEndApp.Size = new System.Drawing.Size(236, 40);
            this.btnEndApp.TabIndex = 4;
            this.btnEndApp.Text = "End Application";
            this.btnEndApp.UseVisualStyleBackColor = true;
            this.btnEndApp.Click += new System.EventHandler(this.btnEndApp_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAdd.Location = new System.Drawing.Point(12, 303);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(93, 39);
            this.btnAdd.TabIndex = 5;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.Location = new System.Drawing.Point(105, 303);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(93, 39);
            this.btnDelete.TabIndex = 6;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // AdminForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(711, 360);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnEndApp);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.tabControl);
            this.Name = "AdminForm";
            this.Text = "AdminForm";
            this.Load += new System.EventHandler(this.AdminForm_Load);
            this.tabControl.ResumeLayout(false);
            this.tabTable.ResumeLayout(false);
            this.tabTable.PerformLayout();
            this.tabQuestion1.ResumeLayout(false);
            this.tabQuestion1.PerformLayout();
            this.tabQuestion2.ResumeLayout(false);
            this.tabQuestion2.PerformLayout();
            this.tabQuestion3.ResumeLayout(false);
            this.tabQuestion3.PerformLayout();
            this.tabDownload.ResumeLayout(false);
            this.tabDownload.PerformLayout();
            this.tabAdvance.ResumeLayout(false);
            this.tabAdvance.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabTable;
        private System.Windows.Forms.TabPage tabQuestion1;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.TabPage tabQuestion2;
        private System.Windows.Forms.TabPage tabQuestion3;
        private System.Windows.Forms.TabPage tabDownload;
        private System.Windows.Forms.TabPage tabAdvance;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label labelYAxis;
        private System.Windows.Forms.Label labelXAxis;
        private System.Windows.Forms.TextBox textBoxYAxis;
        private System.Windows.Forms.TextBox textBoxXAxis;
        private System.Windows.Forms.TextBox textBoxTitle;
        private System.Windows.Forms.Label labelA11;
        private System.Windows.Forms.Label labelQ1;
        private System.Windows.Forms.TextBox textBoxA15;
        private System.Windows.Forms.TextBox textBoxA14;
        private System.Windows.Forms.TextBox textBoxA13;
        private System.Windows.Forms.TextBox textBoxA12;
        private System.Windows.Forms.TextBox textBoxA11;
        private System.Windows.Forms.TextBox textBoxQ1;
        private System.Windows.Forms.Label labelA15;
        private System.Windows.Forms.Label labelA14;
        private System.Windows.Forms.Label labelA13;
        private System.Windows.Forms.Label labelA12;
        private System.Windows.Forms.TextBox textBoxA25;
        private System.Windows.Forms.TextBox textBoxA24;
        private System.Windows.Forms.TextBox textBoxA23;
        private System.Windows.Forms.TextBox textBoxA22;
        private System.Windows.Forms.TextBox textBoxA21;
        private System.Windows.Forms.TextBox textBoxQ2;
        private System.Windows.Forms.Label labelA25;
        private System.Windows.Forms.Label labelA24;
        private System.Windows.Forms.Label labelA23;
        private System.Windows.Forms.Label labelA22;
        private System.Windows.Forms.Label labelA21;
        private System.Windows.Forms.Label labelQ2;
        private System.Windows.Forms.TextBox textBoxA35;
        private System.Windows.Forms.TextBox textBoxA34;
        private System.Windows.Forms.TextBox textBoxA33;
        private System.Windows.Forms.TextBox textBoxA32;
        private System.Windows.Forms.TextBox textBoxA31;
        private System.Windows.Forms.TextBox textBoxQ3;
        private System.Windows.Forms.Label labelA35;
        private System.Windows.Forms.Label labelA34;
        private System.Windows.Forms.Label labelA33;
        private System.Windows.Forms.Label labelA32;
        private System.Windows.Forms.Label labelA31;
        private System.Windows.Forms.Label labelQ3;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label labelTimeOut;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.DateTimePicker dateTimePickerEndDate;
        private System.Windows.Forms.DateTimePicker dateTimePickerStartDate;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox textBoxTimeOut;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Button btnUploadImage;
        private System.Windows.Forms.TextBox textBoxEndMessage;
        private System.Windows.Forms.ComboBox comboBoxRandomQns;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.Button btnEndApp;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnDelete;
    }
}