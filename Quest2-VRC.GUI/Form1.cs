using MaterialSkin;
using MaterialSkin.Controls;
using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.IO;
using System.Linq;
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
            materialCheckbox1.Checked = false;
            //materialSwitch1.Enabled = false;
            //materialSwitch1.Text = "Broken, use this -->";
            materialLabel3.Text = $"Last commit: {ThisAssembly.Git.Commit}";
            Check_Vars.CheckVars();
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.Amber800, Primary.Amber900, Primary.Cyan500, Accent.Cyan700, TextShade.WHITE);
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
            materialCheckbox2.Enabled = false;
            materialTextBox1.Enabled = false;
        }
        private void enadleButtons()
        {
            materialButton1.Enabled = true;
            materialButton2.Enabled = true;
            materialButton3.Enabled = true;
            materialButton4.Enabled = true;
            materialButton5.Enabled = true;
            materialButton6.Enabled = true;
            materialSwitch1.Enabled = true;
            materialCheckbox2.Enabled = true;
            materialTextBox1.Enabled = true;
        }

        private async void materialButton2_Click(object sender, EventArgs e)
        {
            disableButtons();
            materialLabel2.Text = "Status: Executing...";
            string json = File.ReadAllText("vars.json");
            JObject vars = JObject.Parse(json);

            if ((string)vars["HostIP"] == "127.0.0.1")
            {
                await Task.Run(() =>
                {

                    if (materialSwitch1.Checked == true)
                    {
                        try
                        {
                            var questip = "null";
                            materialTextBox1.Invoke(new Action(() => questip =     materialTextBox1.Text));
                            if (ADB.StartADB(true, true, questip, true, materialCheckbox2.Checked))
                            {

                                materialLabel2.Invoke(new Action(() => materialLabel2.Text = "Status: ADB is running"));

                            }

                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Failed to start ADB");
                            MessageBox.Show("Failed to start ADB", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                    }
                    else
                    {
                        try
                        {
                            var questip = "127.0.0.1:62001";
                            if (ADB.StartADB(true, true, questip, false, materialCheckbox2.Checked))
                            {
                                materialLabel2.Invoke(new Action(() => materialLabel2.Text = "Status: ADB is running"));

                            }
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Failed to start ADB");
                            MessageBox.Show("Failed to start ADB", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                    }

                });
            }
            else
            {
                materialLabel2.Invoke(new Action(() => materialLabel2.Text = "Status: Ready"));
                enadleButtons();
                materialButton2.Enabled = false;
                MessageBox.Show("This function cannot be activated if HostIP is not 127.0.0.1", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }
        private async void materialButton3_Click(object sender, EventArgs e)
        {
            materialLabel2.Text = "Status: Executing...";
            disableButtons();
            string json = File.ReadAllText("vars.json");
            JObject vars = JObject.Parse(json);

            if ((string)vars["HostIP"] == "127.0.0.1")
            {
                await Task.Run(() =>
            {
                try
                {
                    OSC.StartOSC(false, true);
                    materialLabel2.Invoke(new Action(() => materialLabel2.Text = "Status: No ADB mode")); ;

                }
                catch (Exception)
                {
                    Console.WriteLine("Failed to start OSC");
                    MessageBox.Show("Failed to start OSC", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            });
                enadleButtons();
                materialLabel2.Invoke(new Action(() => materialLabel2.Text = "Status: Ready"));
            }
            else
            {
                materialLabel2.Invoke(new Action(() => materialLabel2.Text = "Status: Ready"));
                enadleButtons();
                materialButton3.Enabled = false;
                MessageBox.Show("This function cannot be activated if HostIP is not 127.0.0.1", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private async void materialButton1_Click(object sender, EventArgs e)
        {
            materialLabel2.Text = "Status: Executing...";
            string json = File.ReadAllText("vars.json");
            JObject vars = JObject.Parse(json);

            if ((string)vars["HostIP"] == "127.0.0.1")
            {
                disableButtons();
                await Task.Run(() =>
                {

                    if (materialSwitch1.Checked == true)
                    {
                        try
                        {
                            var questip = materialTextBox1.Text;
                           
                            if (ADB.StartADB(false, true, questip, true, materialCheckbox2.Checked))
                            {
                                materialLabel2.Invoke(new Action(() => materialLabel2.Text = "Status: Receive only"));
                                disableButtons();
                            }
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Failed to start ADB");
                            MessageBox.Show("Failed to start ADB", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        try
                        {
                            var questip = "127.0.0.1:62001";
                            if (ADB.StartADB(false, true, questip, false, materialCheckbox2.Checked))
                            {
                                materialLabel2.Invoke(new Action(() => materialLabel2.Text = "Status: Receive only"));
                                disableButtons();
                            }
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Failed to start ADB");
                            MessageBox.Show("Failed to start ADB", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }


                });
                enadleButtons();
                materialLabel2.Invoke(new Action(() => materialLabel2.Text = "Status: Ready"));
            }
            else
            {
                materialLabel2.Invoke(new Action(() => materialLabel2.Text = "Status: Ready"));
                enadleButtons();
                materialButton1.Enabled = false;
                MessageBox.Show("This function cannot be activated if HostIP is not 127.0.0.1", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void materialButton4_Click(object sender, EventArgs e)
        {
            materialLabel2.Text = "Status: Executing...";
            disableButtons();

            await Task.Run(() =>
            {
                if (materialSwitch1.Checked == true)
                {
                    try
                    {
                        var questip = materialTextBox1.Text;
                        
                        if (ADB.StartADB(true, false, questip, true, materialCheckbox2.Checked))
                        {
                            materialLabel2.Invoke(new Action(() => materialLabel2.Text = "Status: Send only"));

                        }
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Failed to start ADB");
                        MessageBox.Show("Failed to start ADB", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    try
                    {
                        var questip = "127.0.0.1:62001";
                        if (ADB.StartADB(true, false, questip, false, materialCheckbox2.Checked))
                        {
                            materialLabel2.Invoke(new Action(() => materialLabel2.Text = "Status: Send only"));

                        }
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Failed to start ADB");
                        MessageBox.Show("Failed to start ADB", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }



            });
            enadleButtons();
            materialLabel2.Invoke(new Action(() => materialLabel2.Text = "Status: Ready"));


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
                ADB.StopADB();
            }

        }

        private async void materialSwitch1_CheckedChanged(object sender, EventArgs e)
        {
            materialTextBox1.Enabled = false;
            if (materialSwitch1.Checked)
            {
                materialLabel2.Text = "Status: In TCPIP mode";
                DialogResult dialogResult1 = MessageBox.Show("Do you want the program to find the IP address by old method or by using ZeroConf\nSearching through ZeroConf requires installing an additional program \"https://github.com/Sergey004/Quest2-VRC/releases/tag/Addon\"\nYes for ZeroConf, No for old method", "Restarting ADB in TCPIP mode", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult1 == DialogResult.Yes)
                {
                    MessageBox.Show("Don't forget to wake up your device", "Restarting ADB in TCPIP mode", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    materialTextBox1.Text = await ADB.GetZeroConfIP();
                }
                else if (dialogResult1 == DialogResult.No)
                {
                    MessageBox.Show("Do not forget to connect the device via USB", "Restarting ADB in TCPIP mode", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (Check_Device.CheckDevice())
                    {
                        
                        ADB.StartTCPIP();
                        await Task.Delay(3000);
                        materialTextBox1.Text = ADB.GetIP();
                    }
                    else
                    {

                        MessageBox.Show("Headset not connected", "Restarting ADB in TCPIP mode", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        materialSwitch1.Checked = false;
                        materialLabel2.Text = "Status: Headset not connected";
                    }

                }
            }
        }

        private void materialButton5_Click(object sender, EventArgs e)
        {
            MessageBox.Show("1) Connect the headset to your computer\n2) Turn the switch to the \"AirLink (or VD)\" position.\n3) Wait for ADB to switch to TCPIP mode\n4) Press \"Default run\" to connect \n \nIf this don't work\nManual connection\nEnter \n\"platform-tools\\adb tcpip 5555\" \n\"platform-tools\\adb connect  QUEST_IP:5555", "Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void materialCheckbox1_CheckedChanged(object sender, EventArgs e)
        {
            materialMultiLineTextBox1.Visible = materialCheckbox1.Checked;
            if (materialCheckbox1.Checked)
            {
                TBStreamWriter streamWriter = new TBStreamWriter(materialMultiLineTextBox1);
                Invoke((MethodInvoker)(() => { Invoke((MethodInvoker)(() => { Console.SetOut(streamWriter); })); }));
            }
            else
            {
                Invoke((MethodInvoker)(() => { Invoke((MethodInvoker)(() => { Console.SetOut(new StreamWriter(Stream.Null)); })); }));
                materialMultiLineTextBox1.Text = null;
            }


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
