using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quest2_VRC
{
    
    public partial class Form1 : MaterialForm
        
    {
        public Form1()
        {
            InitializeComponent();
            materialTextBox1.Enabled = false;
            materialMultiLineTextBox1.Visible = true;
            //materialSwitch1.Enabled = false;
            //materialSwitch1.Text = "Broken, use this -->";
            TBStreamWriter streamWriter = new TBStreamWriter(materialMultiLineTextBox1);
            Invoke((MethodInvoker)(() => { Invoke((MethodInvoker)(() => { Console.SetOut(streamWriter); })); }));
            Check_Vars.CheckVars();
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.Cyan700, Primary.Cyan800, Primary.Cyan500, Accent.Cyan700, TextShade.WHITE);
            materialLabel2.Text = "Status: Ready";
        }

        private void disableButtons()
        {
            materialButton1.Enabled = false;
            materialButton2.Enabled = false;
            materialButton3.Enabled = false;
            materialButton4.Enabled = false;
            materialButton5.Enabled = false;
            materialButton6.Enabled = false;
            materialSwitch1.Enabled = false;
            materialTextBox1.Enabled = false;
        }


        private void materialButton2_Click(object sender, EventArgs e)
        {
            //Invoke((MethodInvoker)(() => { Invoke((MethodInvoker)(() => { Console.SetOut(new TBStreamWriter(materialMultiLineTextBox1)); })); }));
            if (materialSwitch1.Checked == true)
            {
                try
                {
                    var questip = materialTextBox1.Text;
                    //questip += ":5555";
                    if(ADB.StartADB(true, true, questip, true))
                    {
                        materialLabel2.Text = "Status: ADB is running";
                        disableButtons();
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Failed to start ADB");
                }

            }
            else {
                try
                {
                    var questip = "127.0.0.1:62001";
                    if (ADB.StartADB(true, true, questip, false))
                    {
                        materialLabel2.Text = "Status: ADB is running";
                        disableButtons();
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Failed to start ADB");
                }
            }

        }
        private void materialButton3_Click(object sender, EventArgs e)
        {
            try
            {
                //Invoke((MethodInvoker)(() => { Invoke((MethodInvoker)(() => { Console.SetOut(new TBStreamWriter(materialMultiLineTextBox1)); })); }));
                OSC.StartOSC(false, true);
                materialLabel2.Text = "Status: No ADB mode";
                disableButtons();
            }
            catch (Exception)
            {
                Console.WriteLine("Failed to start OSC");
            }

        }

        private void materialButton1_Click(object sender, EventArgs e)
        {
            //Invoke((MethodInvoker)(() => { Invoke((MethodInvoker)(() => { Console.SetOut(new TBStreamWriter(materialMultiLineTextBox1)); })); }));
            if (materialSwitch1.Checked == true)
            {
                try
                {
                    var questip = materialTextBox1.Text;
                    //questip += ":5555"; ;
                    if (ADB.StartADB(false, true, questip, true))
                    {
                        materialLabel2.Text = "Status: Receive only";
                        disableButtons();
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Failed to start ADB");
                }
            }
            else
            {
                try
                {
                    var questip = "127.0.0.1:62001";
                    if (ADB.StartADB(false, true, questip, false))
                    {
                        materialLabel2.Text = "Status: Receive only";
                        disableButtons();
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Failed to start ADB");
                }
            }
        }

        private void materialButton4_Click(object sender, EventArgs e)
        {
            //Invoke((MethodInvoker)(() => { Invoke((MethodInvoker)(() => { Console.SetOut(new TBStreamWriter(materialMultiLineTextBox1)); })); }));
            if (materialSwitch1.Checked == true)
            {
                try
                {
                    var questip = materialTextBox1.Text;
                    //questip += ":5555";
                    if (ADB.StartADB(true, false, questip, true))
                    {
                        materialLabel2.Text = "Status: Send only";
                        disableButtons();
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Failed to start ADB");
                }
            }
            else
            {
                try
                {
                    var questip = "127.0.0.1:62001";
                    if (ADB.StartADB(true, false, questip, false))
                    {
                        materialLabel2.Text = "Status: Send only";
                        disableButtons();
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Failed to start ADB");
                }
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

                Process process = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.FileName = "cmd.exe";
                startInfo.Arguments = "/C platform-tools\\adb.exe kill-server";
                process.StartInfo = startInfo;
                process.Start();
            }

        }

        private async void materialSwitch1_CheckedChanged(object sender, EventArgs e)
        {
            materialTextBox1.Enabled = false;
            if (materialSwitch1.Checked)
            {
                MessageBox.Show("Connect your Quest to your computer and click \"OK\" to continue", "Restarting ADB in TCPIP mode",MessageBoxButtons.OK,MessageBoxIcon.Information);
                if (Check_Device.CheckDevice())
                {
                    ADB.StartTCPIP();
                    await Task.Delay(3000);
                    materialLabel2.Text = "Status: In TCPIP mode";
                    materialTextBox1.Text = ADB.GetIP();
                    MessageBox.Show("ADB restarted in TCPIP mode, ready to wireless conection", "Restarting ADB in TCPIP mode", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Headset not connected", "Restarting ADB in TCPIP mode", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    materialSwitch1.Checked = false;
                    materialLabel2.Text = "Status: Headset not connected";
                }
                
            }

        }

        private void materialButton5_Click(object sender, EventArgs e)
        {
            MessageBox.Show("1) Connect the headset to your computer\n2) Turn the switch to the \"AirLink (or VD)\" position.\n3) Wait for ADB to switch to TCPIP mode\n4) Press \"Default run\" to connect \n \nIf this don't work\nManual connection\nEnter \n\"platform-tools\\adb tcpip 5555\" \n\"platform-tools\\adb connect  QUEST_IP:5555", "Help", MessageBoxButtons.OK,MessageBoxIcon.Information);
        }

        private void materialCheckbox1_CheckedChanged(object sender, EventArgs e)
        {
            materialMultiLineTextBox1.Visible = materialCheckbox1.Checked;
        }

        private void materialMultilineTextBox1_TextChanged(object sender, EventArgs e)
        {
            string[] lines = materialMultiLineTextBox1.Lines;
            int linesCount = lines.Length;
            if (linesCount > 100)
            {
                materialMultiLineTextBox1.Lines = materialMultiLineTextBox1.Lines.Skip(linesCount - 100).ToArray();
                materialMultiLineTextBox1.ScrollToCaret();
                materialMultiLineTextBox1.Select(materialMultiLineTextBox1.Text.Length, 0);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void materialButton6_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            f2.ShowDialog();
        }
    }
}
