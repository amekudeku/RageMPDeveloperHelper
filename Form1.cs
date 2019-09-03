using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;

namespace RageMP_Developer_Helper
{
    public partial class Form1 : Form
    {
        private ConfigModel configuration;
        public Form1()
        {
            InitializeComponent();
            if (File.Exists("config.json"))
            {
                string jsonString = File.ReadAllText("config.json");
                configuration = JsonConvert.DeserializeObject<ConfigModel>(jsonString);
            }
            if(configuration.ServerLocalPath.Length < 1)
            {
                serverFolderPath.Text = Environment.CurrentDirectory;
            }
            else
            {
                serverFolderPath.Text = configuration.ServerLocalPath;
            }
            clientFolderPath.Text = configuration.ClientLocalPath;
            if (!File.Exists("server.exe"))
            {
                MessageBox.Show("The restart button will not work until you place the program in the \"server - files\" folder.", "RageMP Developer Helper", MessageBoxButtons.OK, MessageBoxIcon.Information);
                restartButton.Enabled = false;
                restartButton.Cursor = Cursors.Arrow;
            }

            // AutoMinimize Checkbox Status
            if (configuration.AutoMinizime == true)
            {
                autoMinizimeCheckBox.Checked = true;
            }
        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void PictureBox1_MouseEnter(object sender, EventArgs e)
        {
            pictureBox1.BackColor = Color.FromArgb(240, 71, 71);
        }

        private void PictureBox1_MouseLeave(object sender, EventArgs e)
        {
            pictureBox1.BackColor = Color.FromArgb(32, 34, 37);
        }

        private void PictureBox2_MouseEnter(object sender, EventArgs e)
        {
            pictureBox2.BackColor = Color.FromArgb(43, 45, 48);
        }

        private void PictureBox2_MouseLeave(object sender, EventArgs e)
        {
            pictureBox2.BackColor = Color.FromArgb(32, 34, 37);
        }

        private void PictureBox2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void DefaultButton_Click(object sender, EventArgs e)
        {
            serverFolderPath.Text = @"D:\RAGEMP\server-files\";
        }

        private void ServerPathButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbdLocal = new FolderBrowserDialog();
            fbdLocal.Description = "Select the folder where the server is located";
            if (fbdLocal.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                serverFolderPath.Text = fbdLocal.SelectedPath;
            }
        }

        private void ClientPathButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbdLocal = new FolderBrowserDialog();
            fbdLocal.Description = "Select the folder where the client is located";
            if (fbdLocal.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
               clientFolderPath.Text = fbdLocal.SelectedPath;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Focus();
        }

        private void Panel2_Paint(object sender, PaintEventArgs e)
        {
            this.Focus();
        }

        private void Panel2_Click(object sender, EventArgs e)
        {
            this.Focus();
        }

        private void BunifuImageButton4_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject("amekudeku#9927");
            MessageBox.Show("Copied to clipboard.", "Information Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BunifuImageButton3_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://vk.com/datlesson");
        }

        private void BunifuImageButton2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/amekudeku");
        }

        private void BunifuImageButton1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://steamcommunity.com/id/amekudeku/");
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start($@"{serverFolderPath.Text}/server.exe");
                if(configuration.AutoMinizime == true)
                {
                    this.WindowState = FormWindowState.Minimized;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Check if you have entered the correct path!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (Process proc in Process.GetProcessesByName("server"))
                {
                    proc.Kill();
                }
                if (configuration.AutoMinizime == true)
                {
                    this.WindowState = FormWindowState.Minimized;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void RestartButton_Click(object sender, EventArgs e)
        {
            new Form1().Run().GetAwaiter().GetResult();
            if (configuration.AutoMinizime == true)
            {
                this.WindowState = FormWindowState.Minimized;
            }
        }

        private async Task Run()
        {
            try
            {
                await stopServerOnRestart();
                await runServerOnRestart();
            }
            catch
            {
                return;
            }
        }

        private Task stopServerOnRestart()
        {
            foreach (Process proc in Process.GetProcessesByName("server"))
            {
               proc.Kill();
            }
            return Task.FromResult(0);
        }

        private Task runServerOnRestart()
        {
            Process.Start(@"server.exe");
            return Task.FromResult(0);
        }

        private void BunifuFlatButton1_Click(object sender, EventArgs e)
        {
            configuration.ServerLocalPath = serverFolderPath.Text;
            configuration.ClientLocalPath = clientFolderPath.Text;
            SaveConfig();
        }

        private void BunifuFlatButton2_Click(object sender, EventArgs e)
        {
            if(configuration.AutoMinizime == false)
            {
                configuration.AutoMinizime = true;
                autoMinizimeCheckBox.Checked = true;
                SaveConfig();
                return;
            }
            configuration.AutoMinizime = false;
            autoMinizimeCheckBox.Checked = false;
            SaveConfig();
        }

        private void SaveConfig()
        {
            var configJson = JsonConvert.SerializeObject(configuration);
            File.WriteAllText("config.json", configJson);
            this.Refresh();
        } 
    }
}
