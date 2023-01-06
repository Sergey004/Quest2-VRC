using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MaterialSkin;
using MaterialSkin.Controls;

namespace Quest2_VRC
{
    public partial class Form1 : MaterialForm
    {
        public Form1()
        {
            InitializeComponent();
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);
            this.materialMaskedTextBox1.Mask = "###\\.###\\.###\\.###";
            this.materialMaskedTextBox1.ValidatingType = typeof(System.Net.IPAddress);
        }

        private void maskedTextBox1_TypeValidationCompleted(object sender, TypeValidationEventArgs e)
        {
            this.materialMultiLineTextBox1.Text = String.Format(
                "Valid: {0}\nMessage: {1}\nReturned value: {2}",
                e.IsValidInput,
                e.Message,
                e.ReturnValue);
        }

        
    }
}
