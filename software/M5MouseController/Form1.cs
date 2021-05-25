using M5MouseController.Controller;
using Newtonsoft.Json.Linq;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace M5MouseController
{
    public partial class Form1 : Form
    {
        NotifyIcon notifyIcon;

        public Form1()
        {
            InitializeComponent();
            LoadSetting();
            SetupTaskTray();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            statusChange("Connecting...");
            M5StackBLE m5ble = new M5StackBLE(this);
            m5ble.device_name = textBox1.Text;
            m5ble.service_uuid = textBox2.Text;
            m5ble.chara_uuid = textBox3.Text;
            m5ble.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaveSetting();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Visible = false;
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

        private void statusChange(String txt)
        {
            label4.Text = txt;
        }

        private void pointerChange(String txt)
        {
            label5.Text = txt;
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
                dynamic setting = JObject.Parse("{}");

                if (!Directory.Exists(Environment.GetEnvironmentVariable("userprofile") + "\\.m5mouse"))
                {
                    Directory.CreateDirectory(Environment.GetEnvironmentVariable("userprofile") + "\\.m5mouse");
                }

                setting.device_name = textBox1.Text;
                setting.service_uuid = textBox2.Text;
                setting.chara_uuid = textBox3.Text;
                File.WriteAllText(Environment.GetEnvironmentVariable("userprofile") + "\\.m5mouse\\setting.json", Newtonsoft.Json.JsonConvert.SerializeObject(setting));

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
                dynamic setting = JObject.Parse("{}");

                if (!Directory.Exists(Environment.GetEnvironmentVariable("userprofile") + "\\.m5mouse"))
                {
                    Directory.CreateDirectory(Environment.GetEnvironmentVariable("userprofile") + "\\.m5mouse");
                }

                if (File.Exists(Environment.GetEnvironmentVariable("userprofile") + "\\.m5mouse\\setting.json"))
                {
                    setting = JObject.Parse(File.ReadAllText(Environment.GetEnvironmentVariable("userprofile") + "\\.m5mouse\\setting.json"));
                }

                textBox1.Text = setting.device_name;
                textBox2.Text = setting.service_uuid;
                textBox3.Text = setting.chara_uuid;
            }
            catch (Exception ex)
            {
                //ファイルや値取得の動作についてのエラーは無視
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
