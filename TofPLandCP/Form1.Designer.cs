namespace TofPLandCP
{
    partial class Form1
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
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.codeLabel = new System.Windows.Forms.Label();
            this.textCode = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.saveResult1 = new System.Windows.Forms.Button();
            this.selectDirectory = new System.Windows.Forms.Button();
            this.saveFileCode = new System.Windows.Forms.Button();
            this.openFileCode = new System.Windows.Forms.Button();
            this.windowControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.textResult = new System.Windows.Forms.TextBox();
            this.lab1Button = new System.Windows.Forms.Button();
            this.lab2Button = new System.Windows.Forms.Button();
            this.lab3Button = new System.Windows.Forms.Button();
            this.labButtonBox = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.windowControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.labButtonBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog";
            // 
            // codeLabel
            // 
            this.codeLabel.AutoSize = true;
            this.codeLabel.Location = new System.Drawing.Point(6, 3);
            this.codeLabel.Name = "codeLabel";
            this.codeLabel.Size = new System.Drawing.Size(149, 13);
            this.codeLabel.TabIndex = 0;
            this.codeLabel.Text = "Текст исходной программы";
            // 
            // textCode
            // 
            this.textCode.BackColor = System.Drawing.SystemColors.Control;
            this.textCode.Font = new System.Drawing.Font("Times New Roman", 8.25F);
            this.textCode.Location = new System.Drawing.Point(9, 19);
            this.textCode.Multiline = true;
            this.textCode.Name = "textCode";
            this.textCode.Size = new System.Drawing.Size(822, 564);
            this.textCode.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.saveResult1);
            this.groupBox1.Controls.Add(this.selectDirectory);
            this.groupBox1.Controls.Add(this.saveFileCode);
            this.groupBox1.Controls.Add(this.openFileCode);
            this.groupBox1.Location = new System.Drawing.Point(12, 641);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1023, 54);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Работа с файлами";
            // 
            // saveResult1
            // 
            this.saveResult1.Enabled = false;
            this.saveResult1.Location = new System.Drawing.Point(540, 19);
            this.saveResult1.Name = "saveResult1";
            this.saveResult1.Size = new System.Drawing.Size(172, 23);
            this.saveResult1.TabIndex = 3;
            this.saveResult1.Text = "Сохранить результат работы";
            this.saveResult1.UseVisualStyleBackColor = true;
            this.saveResult1.Click += new System.EventHandler(this.saveResult1_Click);
            // 
            // selectDirectory
            // 
            this.selectDirectory.Location = new System.Drawing.Point(6, 19);
            this.selectDirectory.Name = "selectDirectory";
            this.selectDirectory.Size = new System.Drawing.Size(172, 23);
            this.selectDirectory.TabIndex = 2;
            this.selectDirectory.Text = "Выбор рабочей папки";
            this.selectDirectory.UseVisualStyleBackColor = true;
            this.selectDirectory.Click += new System.EventHandler(this.selectDirectory_Click);
            // 
            // saveFileCode
            // 
            this.saveFileCode.Enabled = false;
            this.saveFileCode.Location = new System.Drawing.Point(362, 19);
            this.saveFileCode.Name = "saveFileCode";
            this.saveFileCode.Size = new System.Drawing.Size(172, 23);
            this.saveFileCode.TabIndex = 1;
            this.saveFileCode.Text = "Сохранить файл с программой";
            this.saveFileCode.UseVisualStyleBackColor = true;
            this.saveFileCode.Click += new System.EventHandler(this.saveFileCode_Click);
            // 
            // openFileCode
            // 
            this.openFileCode.Enabled = false;
            this.openFileCode.Location = new System.Drawing.Point(184, 19);
            this.openFileCode.Name = "openFileCode";
            this.openFileCode.Size = new System.Drawing.Size(172, 23);
            this.openFileCode.TabIndex = 0;
            this.openFileCode.Text = "Открыть файл с программой";
            this.openFileCode.UseVisualStyleBackColor = true;
            this.openFileCode.Click += new System.EventHandler(this.openFileCode_Click);
            // 
            // windowControl
            // 
            this.windowControl.Controls.Add(this.tabPage1);
            this.windowControl.Controls.Add(this.tabPage2);
            this.windowControl.Location = new System.Drawing.Point(12, 12);
            this.windowControl.Name = "windowControl";
            this.windowControl.SelectedIndex = 0;
            this.windowControl.Size = new System.Drawing.Size(845, 627);
            this.windowControl.TabIndex = 4;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.LightGray;
            this.tabPage1.Controls.Add(this.codeLabel);
            this.tabPage1.Controls.Add(this.textCode);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(837, 601);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Исходный код";
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.LightGray;
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Controls.Add(this.textResult);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(837, 601);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Результат работы";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Результат работы";
            // 
            // textResult
            // 
            this.textResult.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textResult.Location = new System.Drawing.Point(9, 19);
            this.textResult.Multiline = true;
            this.textResult.Name = "textResult";
            this.textResult.ReadOnly = true;
            this.textResult.Size = new System.Drawing.Size(822, 564);
            this.textResult.TabIndex = 2;
            // 
            // lab1Button
            // 
            this.lab1Button.Enabled = false;
            this.lab1Button.Location = new System.Drawing.Point(6, 19);
            this.lab1Button.Name = "lab1Button";
            this.lab1Button.Size = new System.Drawing.Size(157, 42);
            this.lab1Button.TabIndex = 5;
            this.lab1Button.Text = "Лексический анализатор";
            this.lab1Button.UseVisualStyleBackColor = true;
            this.lab1Button.Click += new System.EventHandler(this.lab1Button_Click);
            // 
            // lab2Button
            // 
            this.lab2Button.Enabled = false;
            this.lab2Button.Location = new System.Drawing.Point(6, 67);
            this.lab2Button.Name = "lab2Button";
            this.lab2Button.Size = new System.Drawing.Size(157, 42);
            this.lab2Button.TabIndex = 6;
            this.lab2Button.Text = "Перевод кода в ОПЗ";
            this.lab2Button.UseVisualStyleBackColor = true;
            this.lab2Button.Click += new System.EventHandler(this.lab2Button_Click);
            // 
            // lab3Button
            // 
            this.lab3Button.Enabled = false;
            this.lab3Button.Location = new System.Drawing.Point(6, 115);
            this.lab3Button.Name = "lab3Button";
            this.lab3Button.Size = new System.Drawing.Size(157, 42);
            this.lab3Button.TabIndex = 7;
            this.lab3Button.Text = "Перевод ОПЗ в код Basic";
            this.lab3Button.UseVisualStyleBackColor = true;
            this.lab3Button.Click += new System.EventHandler(this.lab3Button_Click);
            // 
            // labButtonBox
            // 
            this.labButtonBox.Controls.Add(this.lab1Button);
            this.labButtonBox.Controls.Add(this.lab3Button);
            this.labButtonBox.Controls.Add(this.lab2Button);
            this.labButtonBox.Location = new System.Drawing.Point(863, 34);
            this.labButtonBox.Name = "labButtonBox";
            this.labButtonBox.Size = new System.Drawing.Size(172, 601);
            this.labButtonBox.TabIndex = 8;
            this.labButtonBox.TabStop = false;
            this.labButtonBox.Text = "Этапы выполнения";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1047, 707);
            this.Controls.Add(this.labButtonBox);
            this.Controls.Add(this.windowControl);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Fortran to Basic";
            this.groupBox1.ResumeLayout(false);
            this.windowControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.labButtonBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.Label codeLabel;
        private System.Windows.Forms.TextBox textCode;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button saveResult1;
        private System.Windows.Forms.Button selectDirectory;
        private System.Windows.Forms.Button saveFileCode;
        private System.Windows.Forms.Button openFileCode;
        private System.Windows.Forms.TabControl windowControl;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textResult;
        private System.Windows.Forms.Button lab1Button;
        private System.Windows.Forms.Button lab2Button;
        private System.Windows.Forms.Button lab3Button;
        private System.Windows.Forms.GroupBox labButtonBox;
    }
}

