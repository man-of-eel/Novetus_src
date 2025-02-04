﻿#region Usings
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
#endregion

namespace NovetusLauncher
{
    #region LauncherForm - Stylish
    public partial class LauncherFormStylish : Form
    {
        #region Variables
        LauncherFormStylishInterface launcherFormStylishInterface1;
        #endregion

        #region Constructor
        public LauncherFormStylish()
        {
            InitializeComponent();
            launcherFormStylishInterface1 = new LauncherFormStylishInterface(this);
            elementHost1.Child = launcherFormStylishInterface1;
        }
        #endregion

        #region Form Events
        private void LauncherFormStylish_Load(object sender, EventArgs e)
        {
            try
            {
                //splash loader
                CryptoRandom rand = new CryptoRandom();
                int randColor = rand.Next(0, 3);

                if (randColor == 0)
                {
                    splashLabel.BackColor = Color.FromArgb(245, 135, 13);
                }
                else if (randColor == 1)
                {
                    splashLabel.BackColor = Color.FromArgb(255, 3, 2);
                }
                else if (randColor == 2)
                {
                    splashLabel.BackColor = Color.FromArgb(238, 154, 181);
                }

                launcherFormStylishInterface1.mapsBox.BackColor = Color.FromArgb(237, 237, 237);

                launcherFormStylishInterface1.launcherForm.SplashLabel = splashLabel;
                launcherFormStylishInterface1.launcherForm.InitForm();

                if (File.Exists(GlobalPaths.RootPath + "\\changelog.txt"))
                {
                    launcherFormStylishInterface1.changelogBox.Text = File.ReadAllText(GlobalPaths.RootPath + "\\changelog.txt");
                }

                if (File.Exists(GlobalPaths.RootPath + "\\README-AND-CREDITS.TXT"))
                {
                    launcherFormStylishInterface1.readmeBox.Text = File.ReadAllText(GlobalPaths.RootPath + "\\README-AND-CREDITS.TXT");
                }

                launcherFormStylishInterface1.versionLabel.Content = Application.ProductVersion;
                launcherFormStylishInterface1.versionNovetusLabel.Content = GlobalVars.ProgramInformation.Version;

                ReadConfigValues(true);

                if (launcherFormStylishInterface1.playTab != null && launcherFormStylishInterface1.playTab.IsSelected)
                {
                    if (launcherFormStylishInterface1.mapsBox.Nodes.Count == 0)
                    {
                        launcherFormStylishInterface1.launcherForm.RefreshMaps();
                        launcherFormStylishInterface1.LoadMapDesc();
                    }
                }

                launcherFormStylishInterface1.ToggleServerOptions();

                LocalVars.launcherInitState = false;
                CenterToScreen();
            }
            catch (Exception ex)
            {
                GlobalFuncs.LogExceptions(ex);
            }
        }

        void LauncherFormStylish_Close(object sender, CancelEventArgs e)
        {
            launcherFormStylishInterface1.launcherForm.CloseEvent(e);
        }
        #endregion

        #region Functions

        void splashLabel_Paint(object sender, PaintEventArgs e)
        {
            GlobalFuncs.DrawBorderSimple(e.Graphics, splashLabel.DisplayRectangle, Color.White, ButtonBorderStyle.Solid, 1);
        }

        public void ReadConfigValues(bool initial = false)
        {
            GlobalFuncs.Config(GlobalPaths.ConfigDir + "\\" + GlobalPaths.ConfigName, false);

            launcherFormStylishInterface1.minimizeOnLaunchBox.IsChecked = GlobalVars.UserConfiguration.CloseOnLaunch;
            launcherFormStylishInterface1.userIDBox.Text = GlobalVars.UserConfiguration.UserID.ToString();
            launcherFormStylishInterface1.tripcodeLabel.Content = GlobalVars.UserConfiguration.PlayerTripcode.ToString();
            launcherFormStylishInterface1.maxPlayersBox.Text = GlobalVars.UserConfiguration.PlayerLimit.ToString();
            launcherFormStylishInterface1.userNameBox.Text = GlobalVars.UserConfiguration.PlayerName;
            launcherFormStylishInterface1.mapsBox.SelectedNode = TreeNodeHelper.SearchTreeView(GlobalVars.UserConfiguration.Map, launcherFormStylishInterface1.mapsBox.Nodes);
            launcherFormStylishInterface1.mapsBox.Focus();
            //stupid fucking HACK because we aren't selecting it properly.
            if (launcherFormStylishInterface1.mapsBox.SelectedNode != null)
            {
                launcherFormStylishInterface1.mapsBox.SelectedNode.BackColor = SystemColors.Highlight;
                launcherFormStylishInterface1.mapsBox.SelectedNode.ForeColor = SystemColors.HighlightText;
            }
            launcherFormStylishInterface1.joinPortBox.Text = GlobalVars.JoinPort.ToString();
            launcherFormStylishInterface1.serverPortBox.Text = GlobalVars.UserConfiguration.RobloxPort.ToString();
            launcherFormStylishInterface1.discordRichPresenceBox.IsChecked = GlobalVars.UserConfiguration.DiscordPresence;
            launcherFormStylishInterface1.uPnPBox.IsChecked = GlobalVars.UserConfiguration.UPnP;
            launcherFormStylishInterface1.NotifBox.IsChecked = GlobalVars.UserConfiguration.ShowServerNotifications;
            launcherFormStylishInterface1.browserNameBox.Text = GlobalVars.UserConfiguration.ServerBrowserServerName;
            launcherFormStylishInterface1.browserAddressBox.Text = GlobalVars.UserConfiguration.ServerBrowserServerAddress;

            switch (GlobalVars.UserConfiguration.LauncherStyle)
            {
                case Settings.Style.Compact:
                    launcherFormStylishInterface1.styleBox.SelectedIndex = 1;
                    break;
                case Settings.Style.Extended:
                    launcherFormStylishInterface1.styleBox.SelectedIndex = 0;
                    break;
                case Settings.Style.Stylish:
                default:
                    launcherFormStylishInterface1.styleBox.SelectedIndex = 2;
                    break;
            }

            ReadClientValues(initial);
        }

        public void WriteConfigValues(bool ShowBox = false)
        {
            GlobalFuncs.Config(GlobalPaths.ConfigDir + "\\" + GlobalPaths.ConfigName, true);
            ReadClientValues();
            if (ShowBox)
            {
                MessageBox.Show("Config Saved!", "Novetus - Config Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void ResetConfigValues(bool ShowBox = false)
        {
            //https://stackoverflow.com/questions/9029351/close-all-open-forms-except-the-main-menu-in-c-sharp
            List<Form> openForms = new List<Form>();

            foreach (Form f in Application.OpenForms)
                openForms.Add(f);

            foreach (Form f in openForms)
            {
                if (f.Name != Name)
                    f.Close();
            }

            GlobalFuncs.ResetConfigValues(Settings.Style.Stylish);
            WriteConfigValues();
            ReadConfigValues();
            if (ShowBox)
            {
                MessageBox.Show("Config Reset!", "Novetus - Config Reset", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void ReadClientValues(bool initial = false)
        {
            //reset clients
            if (!launcherFormStylishInterface1.launcherForm.GenerateIfInvalid())
            {
                if (launcherFormStylishInterface1.clientTab != null && launcherFormStylishInterface1.clientTab.IsSelected)
                {
                    foreach (object o in launcherFormStylishInterface1.clientListBox.Items)
                    {
                        if ((o is ClientListItem) && (o as ClientListItem).ClientName.Contains(GlobalVars.UserConfiguration.SelectedClient))
                        {
                            launcherFormStylishInterface1.clientListBox.SelectedItem = o;
                            break;
                        }
                    }
                }
            }

            GlobalFuncs.ReadClientValues(null, initial);

            launcherFormStylishInterface1.userNameBox.IsEnabled = GlobalVars.SelectedClientInfo.UsesPlayerName;

            launcherFormStylishInterface1.userIDBox.IsEnabled = GlobalVars.SelectedClientInfo.UsesID;
            launcherFormStylishInterface1.regenerateIDButton.IsEnabled = GlobalVars.SelectedClientInfo.UsesID;

            if (!string.IsNullOrWhiteSpace(GlobalVars.SelectedClientInfo.Warning))
            {
                launcherFormStylishInterface1.clientWarningBox.Text = GlobalVars.SelectedClientInfo.Warning;
            }
            else
            {
                launcherFormStylishInterface1.clientWarningBox.Text = "";
            }

            launcherFormStylishInterface1.clientDescBox.Text = GlobalVars.UserConfiguration.SelectedClient + ": " + GlobalVars.SelectedClientInfo.Description;
        }
        #endregion
    }
    #endregion
}
