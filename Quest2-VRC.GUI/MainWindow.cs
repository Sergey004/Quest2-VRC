using MaterialSkin;
using MaterialSkin.Controls;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quest2_VRC
{

    public partial class MainWindow : MaterialForm

    {
        public MainWindow()
        {

            ResourceManager resources = new ComponentResourceManager(typeof(MainWindow));
            InitializeComponent();
            materialTextBox1.Enabled = false;
            materialCheckbox1.Checked = false;
            materialLabel4.Text = $"{ThisAssembly.Git.Commit}";
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.Amber800, Primary.Amber900, Primary.Cyan500, Accent.Cyan700, TextShade.WHITE);
            materialLabel5.Text = resources.GetString("Ready");

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
            materialCheckbox3.Enabled = false;
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
            materialCheckbox3.Enabled = true;
        }

        private async void materialButton2_Click(object sender, EventArgs e)
        {
            ResourceManager resources = new ComponentResourceManager(typeof(MainWindow));
            disableButtons();
            materialLabel5.Text = resources.GetString("Exec");
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
                            materialTextBox1.Invoke(new Action(() => questip = materialTextBox1.Text));
                            if (ADB.StartADB(true, true, questip, true, materialCheckbox2.Checked, materialCheckbox3.Checked))
                            {

                                materialLabel5.Invoke(new Action(() => materialLabel5.Text = resources.GetString("ADBRunning")));

                            }

                        }
                        catch (Exception)
                        {
                            Console.WriteLine(resources.GetString("ADBFall"));
                            MessageBox.Show(resources.GetString("ADBFall"), resources.GetString("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                    }
                    else
                    {
                        try
                        {
                            var questip = "127.0.0.1:62001";
                            if (ADB.StartADB(true, true, questip, false, materialCheckbox2.Checked, materialCheckbox3.Checked))
                            {
                                materialLabel5.Invoke(new Action(() => materialLabel5.Text = resources.GetString("ADBRunning")));

                            }
                        }
                        catch (Exception)
                        {
                            Console.WriteLine(resources.GetString("ADBFall"));
                            MessageBox.Show(resources.GetString("ADBFall"), resources.GetString("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                    }

                });
            }
            else
            {
                materialLabel5.Invoke(new Action(() => materialLabel5.Text = resources.GetString("Ready")));
                enadleButtons();
                materialButton2.Enabled = false;
                MessageBox.Show(resources.GetString("LocalHostErr"), resources.GetString("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }
        private async void materialButton3_Click(object sender, EventArgs e)
        {
            ResourceManager resources = new ComponentResourceManager(typeof(MainWindow));
            materialLabel5.Text = resources.GetString("Exec");
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
                    materialLabel5.Invoke(new Action(() => materialLabel5.Text = resources.GetString("NoADB")));

                }
                catch (Exception)
                {
                    Console.WriteLine(resources.GetString("OSCFall"));
                    MessageBox.Show(resources.GetString("OSCFall"), resources.GetString("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            });
                enadleButtons();
                materialLabel5.Invoke(new Action(() => materialLabel5.Text = resources.GetString("Ready")));
            }
            else
            {
                materialLabel5.Invoke(new Action(() => materialLabel5.Text = resources.GetString("Ready")));
                enadleButtons();
                materialButton3.Enabled = false;
                MessageBox.Show(resources.GetString("LocalHostErr"), resources.GetString("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private async void materialButton1_Click(object sender, EventArgs e)
        {
            ResourceManager resources = new ComponentResourceManager(typeof(MainWindow));
            materialLabel5.Text = resources.GetString("Exec");
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

                            if (ADB.StartADB(false, true, questip, true, materialCheckbox2.Checked, materialCheckbox3.Checked))
                            {
                                materialLabel5.Invoke(new Action(() => materialLabel5.Text = resources.GetString("Receive")));
                                disableButtons();
                            }
                        }
                        catch (Exception)
                        {
                            Console.WriteLine(resources.GetString("ADBFall"));
                            MessageBox.Show(resources.GetString("ADBFall"), resources.GetString("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        try
                        {
                            var questip = "127.0.0.1:62001";
                            if (ADB.StartADB(false, true, questip, false, materialCheckbox2.Checked, materialCheckbox3.Checked))
                            {
                                materialLabel5.Invoke(new Action(() => materialLabel5.Text = resources.GetString("Receive")));
                                disableButtons();
                            }
                        }
                        catch (Exception)
                        {
                            Console.WriteLine(resources.GetString("ADBFall"));
                            MessageBox.Show(resources.GetString("ADBFall"), resources.GetString("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }


                });
                enadleButtons();
                materialLabel5.Invoke(new Action(() => materialLabel5.Text = resources.GetString("Ready")));
            }
            else
            {
                materialLabel5.Invoke(new Action(() => materialLabel5.Text = resources.GetString("Ready")));
                enadleButtons();
                materialButton1.Enabled = false;
                MessageBox.Show(resources.GetString("LocalHostErr"), resources.GetString("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void materialButton4_Click(object sender, EventArgs e)
        {
            ResourceManager resources = new ComponentResourceManager(typeof(MainWindow));
            materialLabel5.Text = resources.GetString("Exec");
            disableButtons();

            await Task.Run(() =>
            {
                if (materialSwitch1.Checked == true)
                {
                    try
                    {
                        var questip = materialTextBox1.Text;

                        if (ADB.StartADB(true, false, questip, true, materialCheckbox2.Checked, materialCheckbox3.Checked))
                        {
                            materialLabel5.Invoke(new Action(() => materialLabel5.Text = resources.GetString("Send")));

                        }
                    }
                    catch (Exception)
                    {
                        Console.WriteLine(resources.GetString("ADBFall"));
                        MessageBox.Show(resources.GetString("ADBFall"), resources.GetString("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    try
                    {
                        var questip = "127.0.0.1:62001";
                        if (ADB.StartADB(true, false, questip, false, materialCheckbox2.Checked, materialCheckbox3.Checked))
                        {
                            materialLabel5.Invoke(new Action(() => materialLabel5.Text = resources.GetString("Send")));

                        }
                    }
                    catch (Exception)
                    {
                        Console.WriteLine(resources.GetString("ADBFall"));
                        MessageBox.Show(resources.GetString("ADBFall"), resources.GetString("Error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }



            });
            enadleButtons();
            materialLabel5.Invoke(new Action(() => materialLabel5.Text = resources.GetString("Ready")));


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
            ResourceManager resources = new ComponentResourceManager(typeof(MainWindow));
            if (MessageBox.Show(resources.GetString("ExitNote"), resources.GetString("ExitAndStop"),
        MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
            {
                e.Cancel = true;
            }
            else
            {
                File.Delete("odtout.txt");
                ADB.StopADB();
            }

        }

        private async void materialSwitch1_CheckedChanged(object sender, EventArgs e)
        {
            ResourceManager resources = new ComponentResourceManager(typeof(MainWindow));
            materialTextBox1.Enabled = false;
            if (materialSwitch1.Checked)
            {
                materialLabel5.Text = resources.GetString("TCPIPmode");
                DialogResult dialogResult1 = MessageBox.Show(resources.GetString("NewOrOld"), resources.GetString("ADBInTCPIP"), MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult1 == DialogResult.Yes)
                {
                    MessageBox.Show(resources.GetString("WakeDevice"), resources.GetString("ADBInTCPIP"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    materialTextBox1.Text = await ADB.GetZeroConfIP();
                }
                else if (dialogResult1 == DialogResult.No)
                {
                    MessageBox.Show(resources.GetString("USBCon"), resources.GetString("ADBInTCPIP"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (Check_Device.CheckDevice())
                    {

                        ADB.StartTCPIP();
                        await Task.Delay(3000);
                        materialTextBox1.Text = ADB.GetIP();
                    }
                    else
                    {

                        MessageBox.Show(resources.GetString("NoHMD"), resources.GetString("ADBInTCPIP"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        materialSwitch1.Checked = false;
                        materialLabel5.Text = resources.GetString("NoHMD");
                    }

                }
            }
        }

        private void materialButton5_Click(object sender, EventArgs e)
        {
            ResourceManager resources = new ComponentResourceManager(typeof(MainWindow));
            MessageBox.Show(resources.GetString("ConnHelp"), "Help", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            OSCTest f2 = new OSCTest();
            f2.ShowDialog();
        }

        private void materialCheckbox3_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
