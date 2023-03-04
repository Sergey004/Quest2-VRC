using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Quest2_VRC.PacketSender;

namespace Quest2_VRC
{
    public partial class Form2 : MaterialForm
    {
        public Form2()
        {
            InitializeComponent();
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);
        }




        private void Form2_Load(object sender, EventArgs e)
        {
            var dic = File.ReadAllLines("vars.txt")
                    .Select(l => l.Split(new[] { '=' }))
                    .ToDictionary(s => s[0].Trim(), s => s[1].Trim());

            string HMDBat = dic["HMDBat"];
            string ControllerBatL = dic["ControllerBatL"];
            string ControllerBatR = dic["ControllerBatR"];
            materialSlider1.Text = dic["HMDBat"];
            materialSlider2.Text = dic["ControllerBatL"];
            materialSlider3.Text = dic["ControllerBatR"];
        }
        private void materialSlider1_onValueChanged(object sender, int newValue)
        {
            var dic = File.ReadAllLines("vars.txt")
                    .Select(l => l.Split(new[] { '=' }))
                    .ToDictionary(s => s[0].Trim(), s => s[1].Trim());

            string HMDBat = dic["HMDBat"];
            VRChatMessage Msg_emu1 = new VRChatMessage(HMDBat, (float)newValue / 100);
            SendPacket(Msg_emu1);
        }

        private void materialSlider2_onValueChanged(object sender, int newValue)
        {
            var dic = File.ReadAllLines("vars.txt")
                    .Select(l => l.Split(new[] { '=' }))
                    .ToDictionary(s => s[0].Trim(), s => s[1].Trim());
            string ControllerBatL = dic["ControllerBatL"];
            VRChatMessage Msg_emu2 = new VRChatMessage(ControllerBatL, (float)newValue / 100);
            SendPacket(Msg_emu2);
        }

        private void materialSlider3_onValueChanged(object sender, int newValue)
        {
            var dic = File.ReadAllLines("vars.txt")
                    .Select(l => l.Split(new[] { '=' }))
                    .ToDictionary(s => s[0].Trim(), s => s[1].Trim());
            string ControllerBatR = dic["ControllerBatR"];
            VRChatMessage Msg_emu3 = new VRChatMessage(ControllerBatR, (float)newValue / 100);
            SendPacket(Msg_emu3);

        }

        private void materialCheckbox1_CheckedChanged(object sender, EventArgs e)
        {
            VRChatMessage Msg_emu5 = new VRChatMessage("LowHMDBat", materialCheckbox1.Checked);
            SendPacket(Msg_emu5);
        }

        private void materialSlider4_onValueChanged(object sender, int newValue)
        {
            
            VRChatMessage Msg_emu4 = new VRChatMessage("WifiRSSI", (float)newValue / 100);
            SendPacket(Msg_emu4);
        }
        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            var dic = File.ReadAllLines("vars.txt")
                    .Select(l => l.Split(new[] { '=' }))
                    .ToDictionary(s => s[0].Trim(), s => s[1].Trim());

            string HMDBat = dic["HMDBat"];
            string ControllerBatL = dic["ControllerBatL"];
            string ControllerBatR = dic["ControllerBatR"];
            VRChatMessage Msg_emu1 = new VRChatMessage(HMDBat, 0f);
            VRChatMessage Msg_emu2 = new VRChatMessage(ControllerBatL, 0f);
            VRChatMessage Msg_emu3 = new VRChatMessage(ControllerBatR, 0f);
            VRChatMessage Msg_emu4 = new VRChatMessage("WifiRSSI", 0f);
            VRChatMessage Msg_emu5 = new VRChatMessage("LowHMDBat", false);
            SendPacket(Msg_emu1, Msg_emu2, Msg_emu3, Msg_emu4, Msg_emu5);
        }

        private void materialButton1_Click(object sender, EventArgs e)
        {
            string inputbox = "input";
            VRChatMessage TestMsg = new VRChatMessage(inputbox, materialTextBox1.Text);
            SendPacket(TestMsg);
        }
    }
}
