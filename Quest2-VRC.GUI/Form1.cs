using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace Quest2_VRC
{
    
    public partial class Form1 : MaterialForm
        
    {
        public Form1()
        {
            InitializeComponent();
            materialTextBox1.Enabled = materialSwitch1.Checked;
            materialSwitch1.Enabled = false;
            materialSwitch1.Text = "Broken, use this -->";
            Invoke((MethodInvoker)(() => { Invoke((MethodInvoker)(() => { Console.SetOut(new TBStreamWriter(materialMultiLineTextBox1)); })); }));
            Check_Vars.CheckVars();
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);
        }


        private void materialButton2_Click(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() => { Invoke((MethodInvoker)(() => { Console.SetOut(new TBStreamWriter(materialMultiLineTextBox1)); })); }));
            if (materialSwitch1.Checked == true)
            {
                var questip = materialTextBox1.Text;
                questip += ":5555";
                ADB.StartADB(true, true, questip);
                materialLabel2.Text = "Status: ADB is running";
                materialButton1.Enabled = false;
                materialButton2.Enabled = false;
                materialButton3.Enabled = false;
                materialButton4.Enabled = false;
                materialSwitch1.Enabled = false;

            }
            else {
                var questip = "127.0.0.1:62001";
                ADB.StartADB(true, true, questip);
                materialLabel2.Text = "Status: ADB is running";
                materialButton1.Enabled = false;
                materialButton2.Enabled = false;
                materialButton3.Enabled = false;
                materialButton4.Enabled = false;
                materialSwitch1.Enabled = false;
            }

        }
        private void materialButton3_Click(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() => { Invoke((MethodInvoker)(() => { Console.SetOut(new TBStreamWriter(materialMultiLineTextBox1)); })); }));
            OSC.StartOSC(false, true);
            materialLabel2.Text = "Status: No ADB mode";
            materialButton1.Enabled = false;
            materialButton2.Enabled = false;
            materialButton3.Enabled = false;
            materialButton4.Enabled = false;
            materialSwitch1.Enabled = false;

        }

        private void materialButton1_Click(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() => { Invoke((MethodInvoker)(() => { Console.SetOut(new TBStreamWriter(materialMultiLineTextBox1)); })); }));
            if (materialSwitch1.Checked == true)
            {
                var questip = materialTextBox1.Text;
                questip += ":5555"; ;
                ADB.StartADB(false, true, questip);
                materialLabel2.Text = "Status: Receive only";
                materialButton1.Enabled = false;
                materialButton2.Enabled = false;
                materialButton3.Enabled = false;
                materialButton4.Enabled = false;
                materialSwitch1.Enabled = false;
            }
            else
            {
                var questip = "127.0.0.1:62001";
                ADB.StartADB(false, true, questip);
                materialLabel2.Text = "Status: Receive only";
                materialButton1.Enabled = false;
                materialButton2.Enabled = false;
                materialButton3.Enabled = false;
                materialButton4.Enabled = false;
                materialSwitch1.Enabled = false;
            }
        }

        private void materialButton4_Click(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() => { Invoke((MethodInvoker)(() => { Console.SetOut(new TBStreamWriter(materialMultiLineTextBox1)); })); }));
            if (materialSwitch1.Checked == true)
            {
                var questip = materialTextBox1.Text;
                questip += ":5555";
                ADB.StartADB(true, false, questip);
                materialLabel2.Text = "Status: Send only";
                materialButton1.Enabled = false;
                materialButton2.Enabled = false;
                materialButton3.Enabled = false;
                materialButton4.Enabled = false;
                materialSwitch1.Enabled = false;
            }
            else
            {
                var questip = "127.0.0.1:62001";
                ADB.StartADB(true, false, questip);
                materialLabel2.Text = "Status: Send only";
                materialButton1.Enabled = false;
                materialButton2.Enabled = false;
                materialButton3.Enabled = false;
                materialButton4.Enabled = false;
                materialSwitch1.Enabled = false;
            }
        }



        public class TBStreamWriter : TextWriter
        {
            MaterialMultiLineTextBox _output = null;
            public TBStreamWriter(MaterialMultiLineTextBox output)
            {
                _output = output;
            }

            public override void Write(char value)
            {
                base.Write(value);
                _output.BeginInvoke((MethodInvoker)(() =>
                {
                    _output.AppendText(value.ToString());
                    _output.ScrollToCaret();
                }));
            }
            public override Encoding Encoding
            {
                get { return Encoding.UTF8; }
            }
        }


        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Do you want to exit?\nNote, this action will stop the ADB server", "Exit and stop ADB",
        MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
            {                 
                e.Cancel = true;
            }
            else
            {
                Console.WriteLine("Exiting program due to external CTRL-C, or process kill, or shutdown, or program exiting");
                Process process = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.FileName = "cmd.exe";
                startInfo.Arguments = "/C platform-tools\\adb.exe kill-server";
                process.StartInfo = startInfo;
                process.Start();
            }

        }

        private void materialSwitch1_CheckedChanged(object sender, EventArgs e)
        {
            materialTextBox1.Enabled = materialSwitch1.Checked;
        }

        private void materialButton5_Click(object sender, EventArgs e)
        {
            MessageBox.Show("1) Enter \"platform-tools\\adb tcpip 5555\" when your headset is connected to your computer via USB \n2) Use the \"Quest IP\" field for conndection\n \n \nIf this don't work\nIgnore the switch and  enter \n\"platform-tools\\adb tcpip 5555\" \n \"platform-tools\\adb connect  QUEST_IP:5555\"", "Help", MessageBoxButtons.OK,MessageBoxIcon.Information);
        }

        //private void materialButton5_Click(object sender, EventArgs e)
        //{
        //    PacketSender.GetOrSetIP IP = new PacketSender.GetOrSetIP();
        //    try
        //    {
        //        IPAddress ipaddress = IPAddress.Parse(materialTextBox1.Text + "." + materialTextBox2.Text + "." + materialTextBox3.Text + "." + materialTextBox4.Text);
        //        PacketSender.GetOrSetIP.IP = ipaddress;
        //        materialLabel2.Text = $"Status: IP set to {PacketSender.GetOrSetIP.IP}";
        //    }
        //    catch
        //    {
        //        PacketSender.GetOrSetIP.IP = IPAddress.Loopback;
        //        materialLabel2.Text = $"Status: IP set to {PacketSender.GetOrSetIP.IP}";
        //    }



        //}
    }
}
