using M5MouseController.Controller;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace M5MouseController
{
    public partial class Form1 : Form
    {
        NotifyIcon notifyIcon;
        M5StackBLE m5ble;
        MouseController mousec;
        private string ble_status;

        public Form1()
        {
            InitializeComponent();
            LoadSetting();
            SetupTaskTray();
            mousec = new MouseController();
            m5ble = new M5StackBLE();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                button1.PerformClick();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            statusChange("ble_connect");
            m5ble.device_name = textBox1.Text;
            m5ble.OnStatusChange += DgStatusChange;
            m5ble.OnChrChange += OnChrChange;
            m5ble.Start();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            notifyIcon.Visible = false;
        }

        private void NotifyIconMenuItem_Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void NotifyIcon_Click(object sender, EventArgs e)
        {
            // Formの表示/非表示を反転
            this.Visible = true;
        }

        private void textBox1_LostFocus(object sender, EventArgs e)
        {
            SaveSetting();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                Startup.setup();
            }
            else
            {
                Startup.remove();
            }
            SaveSetting();
        }

        private void statusChange(String code)
        {
            if (code == "ble_connect")
            {
                label4.Text = "Connecting...";
            }
            else if (code == "ble_success")
            {
                label4.Text = "Connection was Successful.";
                if (checkBox1.Checked == true)
                {
                    this.Visible = false;
                }
            }
            else if (code == "ble_conn_stop")
            {
                if (ble_status == "ble_connect")
                {
                    label4.Text = "Connection Timeout.";
                }
            }
            else if (code == "ble_disconnected")
            {
                label4.Text = "Disconnected.";
            }
                
            ble_status = code;
        }

        private void pointerChange(String txt)
        {
            label5.Text = txt;
        }

        private void OnChrChange(String txt)
        {
            int i_val = 0;
            int.TryParse(textBox4.Text, out i_val);
            mousec.scroll_adjust = i_val;
            mousec.Control(txt);
        }

        delegate void dg_form1(string txt);

        public void DgStatusChange(String txt)
        {
            Invoke(new dg_form1(statusChange), txt);
        }
        public void DgPointerChange(String txt)
        {
            Invoke(new dg_form1(pointerChange), txt);
        }

        private void SaveSetting()
        {
            
            try
            {
                dynamic setting = JSON.parse("{}");
                
                if (!Directory.Exists(Environment.GetEnvironmentVariable("userprofile") + "\\.m5mouse"))
                {
                    Directory.CreateDirectory(Environment.GetEnvironmentVariable("userprofile") + "\\.m5mouse");
                }

                setting.device_name = textBox1.Text;
                setting.scroll_adjust = textBox4.Text;
                setting.startup = checkBox1.Checked ? "true" : "false";

                File.WriteAllText(Environment.GetEnvironmentVariable("userprofile") + "\\.m5mouse\\setting.json", JSON.stringify(setting));

            }
            catch (Exception ex)
            {
                //ファイルや値取得の動作についてのエラーは無視
            }

        }
        private void LoadSetting()
        {
            try
            {
                dynamic setting = JSON.parse("{}");

                if (!Directory.Exists(Environment.GetEnvironmentVariable("userprofile") + "\\.m5mouse"))
                {
                    Directory.CreateDirectory(Environment.GetEnvironmentVariable("userprofile") + "\\.m5mouse");
                }

                if (File.Exists(Environment.GetEnvironmentVariable("userprofile") + "\\.m5mouse\\setting.json"))
                {
                    setting = JSON.parse(File.ReadAllText(Environment.GetEnvironmentVariable("userprofile") + "\\.m5mouse\\setting.json"));
                }

                if (setting.device_name == null)
                {
                    textBox1.Text = "m5mw_01";
                }
                else
                {
                    textBox1.Text = setting.device_name;
                }
                textBox1.Text = "m5mw_01";
                int i_val;
                string s_val = setting.scroll_adjust;
                if (!int.TryParse(s_val, out i_val))
                {
                    i_val = 60; // default
                }
                textBox4.Text = i_val.ToString();
                checkBox1.Checked = (setting.startup == "true");
            }
            catch (Exception ex)
            {
                //ファイルや値取得の動作についてのエラーは無視
                textBox1.Text = "m5mw_01";
                textBox4.Text = "60";
            }

        }

        private void SetupTaskTray()
        {
            notifyIcon = new NotifyIcon();
            notifyIcon.Icon = this.Icon;
            notifyIcon.Visible = true;
            notifyIcon.Text = "M5MouseController";

            ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
            ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem.Text = "&Exit";
            toolStripMenuItem.Click += NotifyIconMenuItem_Exit_Click;
            contextMenuStrip.Items.Add(toolStripMenuItem);
            notifyIcon.ContextMenuStrip = contextMenuStrip;
            notifyIcon.Click += NotifyIcon_Click;
        }

    }


}
