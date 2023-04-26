namespace Quest2_VRC
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            materialCard1 = new MaterialSkin.Controls.MaterialCard();
            materialButton6 = new MaterialSkin.Controls.MaterialButton();
            materialButton2 = new MaterialSkin.Controls.MaterialButton();
            materialButton3 = new MaterialSkin.Controls.MaterialButton();
            materialButton1 = new MaterialSkin.Controls.MaterialButton();
            materialButton4 = new MaterialSkin.Controls.MaterialButton();
            materialMultiLineTextBox1 = new MaterialSkin.Controls.MaterialMultiLineTextBox();
            materialLabel1 = new MaterialSkin.Controls.MaterialLabel();
            materialCard4 = new MaterialSkin.Controls.MaterialCard();
            materialLabel2 = new MaterialSkin.Controls.MaterialLabel();
            materialCard2 = new MaterialSkin.Controls.MaterialCard();
            materialCheckbox2 = new MaterialSkin.Controls.MaterialCheckbox();
            materialButton5 = new MaterialSkin.Controls.MaterialButton();
            materialTextBox1 = new MaterialSkin.Controls.MaterialTextBox();
            materialSwitch1 = new MaterialSkin.Controls.MaterialSwitch();
            materialCheckbox1 = new MaterialSkin.Controls.MaterialCheckbox();
            materialLabel3 = new MaterialSkin.Controls.MaterialLabel();
            materialCard1.SuspendLayout();
            materialCard4.SuspendLayout();
            materialCard2.SuspendLayout();
            SuspendLayout();
            // 
            // materialCard1
            // 
            materialCard1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            materialCard1.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
            materialCard1.Controls.Add(materialButton6);
            materialCard1.Controls.Add(materialButton2);
            materialCard1.Depth = 0;
            materialCard1.ForeColor = System.Drawing.Color.FromArgb(222, 0, 0, 0);
            materialCard1.Location = new System.Drawing.Point(733, 90);
            materialCard1.Margin = new System.Windows.Forms.Padding(16);
            materialCard1.MouseState = MaterialSkin.MouseState.HOVER;
            materialCard1.Name = "materialCard1";
            materialCard1.Padding = new System.Windows.Forms.Padding(16);
            materialCard1.Size = new System.Drawing.Size(180, 137);
            materialCard1.TabIndex = 0;
            // 
            // materialButton6
            // 
            materialButton6.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            materialButton6.Cursor = System.Windows.Forms.Cursors.Hand;
            materialButton6.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            materialButton6.Depth = 0;
            materialButton6.HighEmphasis = true;
            materialButton6.Icon = null;
            materialButton6.Location = new System.Drawing.Point(22, 23);
            materialButton6.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            materialButton6.MouseState = MaterialSkin.MouseState.HOVER;
            materialButton6.Name = "materialButton6";
            materialButton6.NoAccentTextColor = System.Drawing.Color.Empty;
            materialButton6.Size = new System.Drawing.Size(89, 36);
            materialButton6.TabIndex = 1;
            materialButton6.Text = "OSC Test";
            materialButton6.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            materialButton6.UseAccentColor = false;
            materialButton6.UseVisualStyleBackColor = true;
            materialButton6.Click += materialButton6_Click;
            // 
            // materialButton2
            // 
            materialButton2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            materialButton2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            materialButton2.Cursor = System.Windows.Forms.Cursors.Hand;
            materialButton2.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            materialButton2.Depth = 0;
            materialButton2.HighEmphasis = true;
            materialButton2.Icon = null;
            materialButton2.Location = new System.Drawing.Point(22, 81);
            materialButton2.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            materialButton2.MouseState = MaterialSkin.MouseState.HOVER;
            materialButton2.Name = "materialButton2";
            materialButton2.NoAccentTextColor = System.Drawing.Color.Empty;
            materialButton2.Size = new System.Drawing.Size(117, 36);
            materialButton2.TabIndex = 0;
            materialButton2.Text = "Default run";
            materialButton2.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            materialButton2.UseAccentColor = false;
            materialButton2.UseVisualStyleBackColor = true;
            materialButton2.Click += materialButton2_Click;
            // 
            // materialButton3
            // 
            materialButton3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            materialButton3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            materialButton3.Cursor = System.Windows.Forms.Cursors.Hand;
            materialButton3.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            materialButton3.Depth = 0;
            materialButton3.HighEmphasis = true;
            materialButton3.Icon = null;
            materialButton3.Location = new System.Drawing.Point(21, 134);
            materialButton3.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            materialButton3.MouseState = MaterialSkin.MouseState.HOVER;
            materialButton3.Name = "materialButton3";
            materialButton3.NoAccentTextColor = System.Drawing.Color.Empty;
            materialButton3.Size = new System.Drawing.Size(123, 36);
            materialButton3.TabIndex = 1;
            materialButton3.Text = "No ADB/No VR";
            materialButton3.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            materialButton3.UseAccentColor = false;
            materialButton3.UseVisualStyleBackColor = true;
            materialButton3.Click += materialButton3_Click;
            // 
            // materialButton1
            // 
            materialButton1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            materialButton1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            materialButton1.Cursor = System.Windows.Forms.Cursors.Hand;
            materialButton1.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            materialButton1.Depth = 0;
            materialButton1.HighEmphasis = true;
            materialButton1.Icon = null;
            materialButton1.Location = new System.Drawing.Point(21, 78);
            materialButton1.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            materialButton1.MouseState = MaterialSkin.MouseState.HOVER;
            materialButton1.Name = "materialButton1";
            materialButton1.NoAccentTextColor = System.Drawing.Color.Empty;
            materialButton1.Size = new System.Drawing.Size(120, 36);
            materialButton1.TabIndex = 3;
            materialButton1.Text = "Receive only";
            materialButton1.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            materialButton1.UseAccentColor = false;
            materialButton1.UseVisualStyleBackColor = true;
            materialButton1.Click += materialButton1_Click;
            // 
            // materialButton4
            // 
            materialButton4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            materialButton4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            materialButton4.Cursor = System.Windows.Forms.Cursors.Hand;
            materialButton4.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            materialButton4.Depth = 0;
            materialButton4.HighEmphasis = true;
            materialButton4.Icon = null;
            materialButton4.Location = new System.Drawing.Point(21, 23);
            materialButton4.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            materialButton4.MouseState = MaterialSkin.MouseState.HOVER;
            materialButton4.Name = "materialButton4";
            materialButton4.NoAccentTextColor = System.Drawing.Color.Empty;
            materialButton4.Size = new System.Drawing.Size(100, 36);
            materialButton4.TabIndex = 2;
            materialButton4.Text = "Send only";
            materialButton4.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            materialButton4.UseAccentColor = false;
            materialButton4.UseVisualStyleBackColor = true;
            materialButton4.Click += materialButton4_Click;
            // 
            // materialMultiLineTextBox1
            // 
            materialMultiLineTextBox1.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
            materialMultiLineTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            materialMultiLineTextBox1.Cursor = System.Windows.Forms.Cursors.No;
            materialMultiLineTextBox1.Depth = 0;
            materialMultiLineTextBox1.Font = new System.Drawing.Font("Lucida Console", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            materialMultiLineTextBox1.ForeColor = System.Drawing.Color.FromArgb(222, 0, 0, 0);
            materialMultiLineTextBox1.Location = new System.Drawing.Point(20, 333);
            materialMultiLineTextBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            materialMultiLineTextBox1.MaxLength = 500;
            materialMultiLineTextBox1.MouseState = MaterialSkin.MouseState.HOVER;
            materialMultiLineTextBox1.Name = "materialMultiLineTextBox1";
            materialMultiLineTextBox1.ReadOnly = true;
            materialMultiLineTextBox1.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            materialMultiLineTextBox1.Size = new System.Drawing.Size(892, 222);
            materialMultiLineTextBox1.TabIndex = 0;
            materialMultiLineTextBox1.Text = "";
            materialMultiLineTextBox1.TextChanged += materialMultilineTextBox1_TextChanged;
            // 
            // materialLabel1
            // 
            materialLabel1.AutoSize = true;
            materialLabel1.Depth = 0;
            materialLabel1.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            materialLabel1.Location = new System.Drawing.Point(20, 297);
            materialLabel1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            materialLabel1.MouseState = MaterialSkin.MouseState.HOVER;
            materialLabel1.Name = "materialLabel1";
            materialLabel1.Size = new System.Drawing.Size(28, 19);
            materialLabel1.TabIndex = 3;
            materialLabel1.Text = "Log";
            // 
            // materialCard4
            // 
            materialCard4.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
            materialCard4.Controls.Add(materialButton3);
            materialCard4.Controls.Add(materialButton1);
            materialCard4.Controls.Add(materialButton4);
            materialCard4.Depth = 0;
            materialCard4.ForeColor = System.Drawing.Color.FromArgb(222, 0, 0, 0);
            materialCard4.Location = new System.Drawing.Point(20, 90);
            materialCard4.Margin = new System.Windows.Forms.Padding(16);
            materialCard4.MouseState = MaterialSkin.MouseState.HOVER;
            materialCard4.Name = "materialCard4";
            materialCard4.Padding = new System.Windows.Forms.Padding(16);
            materialCard4.Size = new System.Drawing.Size(194, 190);
            materialCard4.TabIndex = 8;
            // 
            // materialLabel2
            // 
            materialLabel2.AutoSize = true;
            materialLabel2.Depth = 0;
            materialLabel2.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            materialLabel2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            materialLabel2.Location = new System.Drawing.Point(20, 569);
            materialLabel2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            materialLabel2.MouseState = MaterialSkin.MouseState.HOVER;
            materialLabel2.Name = "materialLabel2";
            materialLabel2.Size = new System.Drawing.Size(51, 19);
            materialLabel2.TabIndex = 4;
            materialLabel2.Text = "Status:";
            // 
            // materialCard2
            // 
            materialCard2.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
            materialCard2.Controls.Add(materialCheckbox2);
            materialCard2.Controls.Add(materialButton5);
            materialCard2.Controls.Add(materialTextBox1);
            materialCard2.Controls.Add(materialSwitch1);
            materialCard2.Depth = 0;
            materialCard2.ForeColor = System.Drawing.Color.FromArgb(222, 0, 0, 0);
            materialCard2.Location = new System.Drawing.Point(246, 90);
            materialCard2.Margin = new System.Windows.Forms.Padding(16);
            materialCard2.MouseState = MaterialSkin.MouseState.HOVER;
            materialCard2.Name = "materialCard2";
            materialCard2.Padding = new System.Windows.Forms.Padding(16);
            materialCard2.Size = new System.Drawing.Size(454, 190);
            materialCard2.TabIndex = 9;
            // 
            // materialCheckbox2
            // 
            materialCheckbox2.AutoSize = true;
            materialCheckbox2.Cursor = System.Windows.Forms.Cursors.Hand;
            materialCheckbox2.Depth = 0;
            materialCheckbox2.Location = new System.Drawing.Point(16, 138);
            materialCheckbox2.Margin = new System.Windows.Forms.Padding(0);
            materialCheckbox2.MouseLocation = new System.Drawing.Point(-1, -1);
            materialCheckbox2.MouseState = MaterialSkin.MouseState.HOVER;
            materialCheckbox2.Name = "materialCheckbox2";
            materialCheckbox2.ReadOnly = false;
            materialCheckbox2.Ripple = true;
            materialCheckbox2.Size = new System.Drawing.Size(212, 37);
            materialCheckbox2.TabIndex = 3;
            materialCheckbox2.Text = "Audio notifications (WIP)";
            materialCheckbox2.UseVisualStyleBackColor = true;
            // 
            // materialButton5
            // 
            materialButton5.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            materialButton5.Cursor = System.Windows.Forms.Cursors.Hand;
            materialButton5.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            materialButton5.Depth = 0;
            materialButton5.HighEmphasis = true;
            materialButton5.Icon = null;
            materialButton5.Location = new System.Drawing.Point(358, 132);
            materialButton5.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            materialButton5.MouseState = MaterialSkin.MouseState.HOVER;
            materialButton5.Name = "materialButton5";
            materialButton5.NoAccentTextColor = System.Drawing.Color.Empty;
            materialButton5.Size = new System.Drawing.Size(64, 36);
            materialButton5.TabIndex = 2;
            materialButton5.Text = "help";
            materialButton5.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            materialButton5.UseAccentColor = false;
            materialButton5.UseVisualStyleBackColor = true;
            materialButton5.Click += materialButton5_Click;
            // 
            // materialTextBox1
            // 
            materialTextBox1.AnimateReadOnly = false;
            materialTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            materialTextBox1.Depth = 0;
            materialTextBox1.Font = new System.Drawing.Font("Roboto", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            materialTextBox1.Hint = "Quest IP (ADB only)";
            materialTextBox1.LeadingIcon = null;
            materialTextBox1.Location = new System.Drawing.Point(20, 35);
            materialTextBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            materialTextBox1.MaxLength = 50;
            materialTextBox1.MouseState = MaterialSkin.MouseState.OUT;
            materialTextBox1.Multiline = false;
            materialTextBox1.Name = "materialTextBox1";
            materialTextBox1.Size = new System.Drawing.Size(414, 50);
            materialTextBox1.TabIndex = 1;
            materialTextBox1.Text = "";
            materialTextBox1.TrailingIcon = null;
            // 
            // materialSwitch1
            // 
            materialSwitch1.AutoSize = true;
            materialSwitch1.Cursor = System.Windows.Forms.Cursors.Hand;
            materialSwitch1.Depth = 0;
            materialSwitch1.Location = new System.Drawing.Point(16, 96);
            materialSwitch1.Margin = new System.Windows.Forms.Padding(0);
            materialSwitch1.MouseLocation = new System.Drawing.Point(-1, -1);
            materialSwitch1.MouseState = MaterialSkin.MouseState.HOVER;
            materialSwitch1.Name = "materialSwitch1";
            materialSwitch1.Ripple = true;
            materialSwitch1.Size = new System.Drawing.Size(215, 37);
            materialSwitch1.TabIndex = 0;
            materialSwitch1.Text = "Link or AirLink (Or VD)";
            materialSwitch1.UseVisualStyleBackColor = true;
            materialSwitch1.CheckedChanged += materialSwitch1_CheckedChanged;
            // 
            // materialCheckbox1
            // 
            materialCheckbox1.AutoSize = true;
            materialCheckbox1.Checked = true;
            materialCheckbox1.CheckState = System.Windows.Forms.CheckState.Checked;
            materialCheckbox1.Cursor = System.Windows.Forms.Cursors.Hand;
            materialCheckbox1.Depth = 0;
            materialCheckbox1.Location = new System.Drawing.Point(803, 288);
            materialCheckbox1.Margin = new System.Windows.Forms.Padding(0);
            materialCheckbox1.MouseLocation = new System.Drawing.Point(-1, -1);
            materialCheckbox1.MouseState = MaterialSkin.MouseState.HOVER;
            materialCheckbox1.Name = "materialCheckbox1";
            materialCheckbox1.ReadOnly = false;
            materialCheckbox1.Ripple = true;
            materialCheckbox1.Size = new System.Drawing.Size(109, 37);
            materialCheckbox1.TabIndex = 10;
            materialCheckbox1.Text = "Show logs";
            materialCheckbox1.UseVisualStyleBackColor = true;
            materialCheckbox1.CheckedChanged += materialCheckbox1_CheckedChanged;
            // 
            // materialLabel3
            // 
            materialLabel3.AutoSize = true;
            materialLabel3.Depth = 0;
            materialLabel3.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            materialLabel3.Location = new System.Drawing.Point(755, 569);
            materialLabel3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            materialLabel3.MouseState = MaterialSkin.MouseState.HOVER;
            materialLabel3.Name = "materialLabel3";
            materialLabel3.Size = new System.Drawing.Size(57, 19);
            materialLabel3.TabIndex = 11;
            materialLabel3.Text = "Commit";
            // 
            // Form1
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.SystemColors.Control;
            ClientSize = new System.Drawing.Size(932, 604);
            Controls.Add(materialLabel3);
            Controls.Add(materialCheckbox1);
            Controls.Add(materialCard2);
            Controls.Add(materialLabel1);
            Controls.Add(materialCard4);
            Controls.Add(materialMultiLineTextBox1);
            Controls.Add(materialLabel2);
            Controls.Add(materialCard1);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MaximizeBox = false;
            Name = "Form1";
            Padding = new System.Windows.Forms.Padding(4, 74, 4, 3);
            Sizable = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Quest2-VRC GUI";
            FormClosing += Form1_FormClosing;
            Load += Form1_Load;
            materialCard1.ResumeLayout(false);
            materialCard1.PerformLayout();
            materialCard4.ResumeLayout(false);
            materialCard4.PerformLayout();
            materialCard2.ResumeLayout(false);
            materialCard2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MaterialSkin.Controls.MaterialCard materialCard1;
        private MaterialSkin.Controls.MaterialMultiLineTextBox materialMultiLineTextBox1;
        private MaterialSkin.Controls.MaterialLabel materialLabel1;
        private MaterialSkin.Controls.MaterialButton materialButton1;
        private MaterialSkin.Controls.MaterialButton materialButton4;
        private MaterialSkin.Controls.MaterialButton materialButton3;
        private MaterialSkin.Controls.MaterialButton materialButton2;
        private MaterialSkin.Controls.MaterialCard materialCard4;
        private MaterialSkin.Controls.MaterialLabel materialLabel2;
        private MaterialSkin.Controls.MaterialCard materialCard2;
        private MaterialSkin.Controls.MaterialSwitch materialSwitch1;
        private MaterialSkin.Controls.MaterialTextBox materialTextBox1;
        private MaterialSkin.Controls.MaterialButton materialButton5;
        private MaterialSkin.Controls.MaterialCheckbox materialCheckbox1;
        private MaterialSkin.Controls.MaterialButton materialButton6;
        private MaterialSkin.Controls.MaterialCheckbox materialCheckbox2;
        private MaterialSkin.Controls.MaterialLabel materialLabel3;
    }
}

