namespace Quest2_VRC
{
    partial class OSCTest
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
            materialSlider1 = new MaterialSkin.Controls.MaterialSlider();
            materialSlider2 = new MaterialSkin.Controls.MaterialSlider();
            materialSlider3 = new MaterialSkin.Controls.MaterialSlider();
            materialCheckbox1 = new MaterialSkin.Controls.MaterialCheckbox();
            materialSlider4 = new MaterialSkin.Controls.MaterialSlider();
            materialTextBox1 = new MaterialSkin.Controls.MaterialTextBox();
            materialButton1 = new MaterialSkin.Controls.MaterialButton();
            materialLabel3 = new MaterialSkin.Controls.MaterialLabel();
            materialTextBox2 = new MaterialSkin.Controls.MaterialTextBox();
            materialTextBox3 = new MaterialSkin.Controls.MaterialTextBox();
            materialComboBox1 = new MaterialSkin.Controls.MaterialComboBox();
            materialButton2 = new MaterialSkin.Controls.MaterialButton();
            SuspendLayout();
            // 
            // materialSlider1
            // 
            materialSlider1.Depth = 0;
            materialSlider1.ForeColor = System.Drawing.Color.FromArgb(222, 0, 0, 0);
            materialSlider1.Location = new System.Drawing.Point(7, 99);
            materialSlider1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            materialSlider1.MouseState = MaterialSkin.MouseState.HOVER;
            materialSlider1.Name = "materialSlider1";
            materialSlider1.Size = new System.Drawing.Size(292, 40);
            materialSlider1.TabIndex = 0;
            materialSlider1.Text = "materialSlider1";
            materialSlider1.onValueChanged += materialSlider1_onValueChanged;
            // 
            // materialSlider2
            // 
            materialSlider2.Depth = 0;
            materialSlider2.ForeColor = System.Drawing.Color.FromArgb(222, 0, 0, 0);
            materialSlider2.Location = new System.Drawing.Point(7, 152);
            materialSlider2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            materialSlider2.MouseState = MaterialSkin.MouseState.HOVER;
            materialSlider2.Name = "materialSlider2";
            materialSlider2.Size = new System.Drawing.Size(292, 40);
            materialSlider2.TabIndex = 1;
            materialSlider2.Text = "materialSlider2";
            materialSlider2.onValueChanged += materialSlider2_onValueChanged;
            // 
            // materialSlider3
            // 
            materialSlider3.Depth = 0;
            materialSlider3.ForeColor = System.Drawing.Color.FromArgb(222, 0, 0, 0);
            materialSlider3.Location = new System.Drawing.Point(7, 205);
            materialSlider3.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            materialSlider3.MouseState = MaterialSkin.MouseState.HOVER;
            materialSlider3.Name = "materialSlider3";
            materialSlider3.Size = new System.Drawing.Size(292, 40);
            materialSlider3.TabIndex = 2;
            materialSlider3.Text = "materialSlider3";
            materialSlider3.onValueChanged += materialSlider3_onValueChanged;
            // 
            // materialCheckbox1
            // 
            materialCheckbox1.AutoSize = true;
            materialCheckbox1.Depth = 0;
            materialCheckbox1.Location = new System.Drawing.Point(4, 255);
            materialCheckbox1.Margin = new System.Windows.Forms.Padding(0);
            materialCheckbox1.MouseLocation = new System.Drawing.Point(-1, -1);
            materialCheckbox1.MouseState = MaterialSkin.MouseState.HOVER;
            materialCheckbox1.Name = "materialCheckbox1";
            materialCheckbox1.ReadOnly = false;
            materialCheckbox1.Ripple = true;
            materialCheckbox1.Size = new System.Drawing.Size(125, 37);
            materialCheckbox1.TabIndex = 3;
            materialCheckbox1.Text = "LowHMDBat";
            materialCheckbox1.UseVisualStyleBackColor = true;
            materialCheckbox1.CheckedChanged += materialCheckbox1_CheckedChanged;
            // 
            // materialSlider4
            // 
            materialSlider4.Depth = 0;
            materialSlider4.ForeColor = System.Drawing.Color.FromArgb(222, 0, 0, 0);
            materialSlider4.Location = new System.Drawing.Point(7, 301);
            materialSlider4.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            materialSlider4.MouseState = MaterialSkin.MouseState.HOVER;
            materialSlider4.Name = "materialSlider4";
            materialSlider4.Size = new System.Drawing.Size(292, 40);
            materialSlider4.TabIndex = 4;
            materialSlider4.Text = "WifiRSSI";
            materialSlider4.onValueChanged += materialSlider4_onValueChanged;
            // 
            // materialTextBox1
            // 
            materialTextBox1.AnimateReadOnly = false;
            materialTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            materialTextBox1.Depth = 0;
            materialTextBox1.Font = new System.Drawing.Font("Roboto", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            materialTextBox1.Hint = "Chatbox";
            materialTextBox1.LeadingIcon = null;
            materialTextBox1.Location = new System.Drawing.Point(4, 403);
            materialTextBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            materialTextBox1.MaxLength = 50;
            materialTextBox1.MouseState = MaterialSkin.MouseState.OUT;
            materialTextBox1.Multiline = false;
            materialTextBox1.Name = "materialTextBox1";
            materialTextBox1.Size = new System.Drawing.Size(640, 50);
            materialTextBox1.TabIndex = 5;
            materialTextBox1.Text = "";
            materialTextBox1.TrailingIcon = null;
            // 
            // materialButton1
            // 
            materialButton1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            materialButton1.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            materialButton1.Depth = 0;
            materialButton1.HighEmphasis = true;
            materialButton1.Icon = null;
            materialButton1.Location = new System.Drawing.Point(649, 417);
            materialButton1.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            materialButton1.MouseState = MaterialSkin.MouseState.HOVER;
            materialButton1.Name = "materialButton1";
            materialButton1.NoAccentTextColor = System.Drawing.Color.Empty;
            materialButton1.Size = new System.Drawing.Size(64, 36);
            materialButton1.TabIndex = 6;
            materialButton1.Text = "Send";
            materialButton1.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            materialButton1.UseAccentColor = false;
            materialButton1.UseVisualStyleBackColor = true;
            materialButton1.Click += materialButton1_Click;
            // 
            // materialLabel3
            // 
            materialLabel3.AutoSize = true;
            materialLabel3.Depth = 0;
            materialLabel3.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            materialLabel3.Location = new System.Drawing.Point(8, 366);
            materialLabel3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            materialLabel3.MouseState = MaterialSkin.MouseState.HOVER;
            materialLabel3.Name = "materialLabel3";
            materialLabel3.Size = new System.Drawing.Size(147, 19);
            materialLabel3.TabIndex = 12;
            materialLabel3.Text = "/avatar/parameters/";
            // 
            // materialTextBox2
            // 
            materialTextBox2.AnimateReadOnly = false;
            materialTextBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            materialTextBox2.Depth = 0;
            materialTextBox2.Font = new System.Drawing.Font("Roboto", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            materialTextBox2.Hint = "Any parameter";
            materialTextBox2.LeadingIcon = null;
            materialTextBox2.Location = new System.Drawing.Point(162, 347);
            materialTextBox2.MaxLength = 50;
            materialTextBox2.MouseState = MaterialSkin.MouseState.OUT;
            materialTextBox2.Multiline = false;
            materialTextBox2.Name = "materialTextBox2";
            materialTextBox2.Size = new System.Drawing.Size(137, 50);
            materialTextBox2.TabIndex = 13;
            materialTextBox2.Text = "";
            materialTextBox2.TrailingIcon = null;
            // 
            // materialTextBox3
            // 
            materialTextBox3.AnimateReadOnly = false;
            materialTextBox3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            materialTextBox3.Depth = 0;
            materialTextBox3.Font = new System.Drawing.Font("Roboto", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            materialTextBox3.Hint = "value";
            materialTextBox3.LeadingIcon = null;
            materialTextBox3.Location = new System.Drawing.Point(305, 347);
            materialTextBox3.MaxLength = 50;
            materialTextBox3.MouseState = MaterialSkin.MouseState.OUT;
            materialTextBox3.Multiline = false;
            materialTextBox3.Name = "materialTextBox3";
            materialTextBox3.Size = new System.Drawing.Size(100, 50);
            materialTextBox3.TabIndex = 14;
            materialTextBox3.Text = "";
            materialTextBox3.TrailingIcon = null;
            // 
            // materialComboBox1
            // 
            materialComboBox1.AutoResize = false;
            materialComboBox1.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
            materialComboBox1.Depth = 0;
            materialComboBox1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            materialComboBox1.DropDownHeight = 174;
            materialComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            materialComboBox1.DropDownWidth = 121;
            materialComboBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            materialComboBox1.ForeColor = System.Drawing.Color.FromArgb(222, 0, 0, 0);
            materialComboBox1.FormattingEnabled = true;
            materialComboBox1.Hint = "type";
            materialComboBox1.IntegralHeight = false;
            materialComboBox1.ItemHeight = 43;
            materialComboBox1.Location = new System.Drawing.Point(411, 347);
            materialComboBox1.MaxDropDownItems = 4;
            materialComboBox1.MouseState = MaterialSkin.MouseState.OUT;
            materialComboBox1.Name = "materialComboBox1";
            materialComboBox1.Size = new System.Drawing.Size(121, 49);
            materialComboBox1.StartIndex = 0;
            materialComboBox1.TabIndex = 15;
            // 
            // materialButton2
            // 
            materialButton2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            materialButton2.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            materialButton2.Depth = 0;
            materialButton2.HighEmphasis = true;
            materialButton2.Icon = null;
            materialButton2.Location = new System.Drawing.Point(540, 361);
            materialButton2.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            materialButton2.MouseState = MaterialSkin.MouseState.HOVER;
            materialButton2.Name = "materialButton2";
            materialButton2.NoAccentTextColor = System.Drawing.Color.Empty;
            materialButton2.Size = new System.Drawing.Size(113, 36);
            materialButton2.TabIndex = 16;
            materialButton2.Text = "Send param";
            materialButton2.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            materialButton2.UseAccentColor = false;
            materialButton2.UseVisualStyleBackColor = true;
            materialButton2.Click += materialButton2_Click;
            // 
            // Form2
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(726, 472);
            Controls.Add(materialButton2);
            Controls.Add(materialComboBox1);
            Controls.Add(materialTextBox3);
            Controls.Add(materialTextBox2);
            Controls.Add(materialLabel3);
            Controls.Add(materialButton1);
            Controls.Add(materialTextBox1);
            Controls.Add(materialSlider4);
            Controls.Add(materialCheckbox1);
            Controls.Add(materialSlider3);
            Controls.Add(materialSlider2);
            Controls.Add(materialSlider1);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Form2";
            Padding = new System.Windows.Forms.Padding(4, 74, 4, 3);
            ShowIcon = false;
            Sizable = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "OSC Tester";
            FormClosing += Form2_FormClosing;
            Load += Form2_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MaterialSkin.Controls.MaterialSlider materialSlider1;
        private MaterialSkin.Controls.MaterialSlider materialSlider2;
        private MaterialSkin.Controls.MaterialSlider materialSlider3;
        private MaterialSkin.Controls.MaterialCheckbox materialCheckbox1;
        private MaterialSkin.Controls.MaterialSlider materialSlider4;
        private MaterialSkin.Controls.MaterialTextBox materialTextBox1;
        private MaterialSkin.Controls.MaterialButton materialButton1;
        private MaterialSkin.Controls.MaterialLabel materialLabel3;
        private MaterialSkin.Controls.MaterialTextBox materialTextBox2;
        private MaterialSkin.Controls.MaterialTextBox materialTextBox3;
        private MaterialSkin.Controls.MaterialComboBox materialComboBox1;
        private MaterialSkin.Controls.MaterialButton materialButton2;
   
    }
}