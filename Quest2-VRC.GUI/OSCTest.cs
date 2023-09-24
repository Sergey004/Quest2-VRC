using MaterialSkin;
using MaterialSkin.Controls;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Windows.Forms;
using static Quest2_VRC.PacketSender;

namespace Quest2_VRC
{
    public partial class OSCTest : MaterialForm
    {
        public OSCTest()
        {
            InitializeComponent();
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.Amber800, Primary.Amber900, Primary.Cyan500, Accent.Cyan700, TextShade.WHITE);
            materialComboBox1.Items.AddRange(new string[] { "bool", "int", "float", "string" });
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            string json = File.ReadAllText("vars.json");
            JObject vars = JObject.Parse(json);
            materialSlider1.Text = (string)vars["HMDBat"];
            materialSlider2.Text = (string)vars["ControllerBatL"];
            materialSlider3.Text = (string)vars["ControllerBatR"];
        }
        private void materialSlider1_onValueChanged(object sender, int newValue)
        {
            string json = File.ReadAllText("vars.json");
            JObject vars = JObject.Parse(json);

            string HMDBat = (string)vars["HMDBat"];
            VRChatMessage Msg_emu1 = new VRChatMessage(HMDBat, (float)newValue / 100);
            SendPacket(Msg_emu1);
        }

        private void materialSlider2_onValueChanged(object sender, int newValue)
        {
            string json = File.ReadAllText("vars.json");
            JObject vars = JObject.Parse(json);

            string ControllerBatL = (string)vars["ControllerBatL"];
            VRChatMessage Msg_emu2 = new VRChatMessage(ControllerBatL, (float)newValue / 100);
            SendPacket(Msg_emu2);
        }

        private void materialSlider3_onValueChanged(object sender, int newValue)
        {
            string json = File.ReadAllText("vars.json");
            JObject vars = JObject.Parse(json);

            string ControllerBatR = (string)vars["ControllerBatR"];
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
            string json = File.ReadAllText("vars.json");
            JObject vars = JObject.Parse(json);

            string HMDBat = (string)vars["HMDBat"];
            string ControllerBatL = (string)vars["ControllerBatL"];
            string ControllerBatR = (string)vars["ControllerBatR"];
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

        private void materialButton2_Click(object sender, EventArgs e)
        {
            if (materialComboBox1.SelectedIndex.ToString() == "1")
            {
                VRChatMessage int1 = new VRChatMessage(materialTextBox2.Text, int.Parse(materialTextBox3.Text));
                SendPacket(int1);
            }
            if (materialComboBox1.SelectedIndex.ToString() == "2")
            {
                VRChatMessage float1 = new VRChatMessage(materialTextBox2.Text, float.Parse(materialTextBox3.Text));
                SendPacket(float1);
            }
            if (materialComboBox1.SelectedIndex.ToString() == "0")
            {
                VRChatMessage bool1 = new VRChatMessage(materialTextBox2.Text, bool.Parse(materialTextBox3.Text));
                SendPacket(bool1);
            }
            if (materialComboBox1.SelectedIndex.ToString() == "3")
            {
                VRChatMessage string1 = new VRChatMessage(materialTextBox2.Text, materialTextBox3.Text);
                SendPacket(string1);
            }

        }

       
    }
}
