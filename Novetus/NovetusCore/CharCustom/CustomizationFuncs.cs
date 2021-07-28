﻿#region Usings
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
#endregion

#region Customization Functions
    class CustomizationFuncs
    {
        public static void ChangeItem(string item, string itemdir, string defaultitem, PictureBox outputImage, TextBox outputString, ListBox box, bool initial, bool hatsinextra = false)
        {
            ChangeItem(item, itemdir, defaultitem, outputImage, outputString, box, initial, null, hatsinextra);
        }

        public static void ChangeItem(string item, string itemdir, string defaultitem, PictureBox outputImage, TextBox outputString, ListBox box, bool initial, Provider provider, bool hatsinextra = false)
        {
            if (Directory.Exists(itemdir))
            {
                if (initial)
                {
                    if (!hatsinextra)
                    {
                        box.Items.Clear();
                    }
                    DirectoryInfo dinfo = new DirectoryInfo(itemdir);
                    FileInfo[] Files = dinfo.GetFiles("*.rbxm");
                    foreach (FileInfo file in Files)
                    {
                        if (file.Name.Equals(string.Empty))
                        {
                            continue;
                        }

                        if (hatsinextra)
                        {
                            if (file.Name.Equals("NoHat.rbxm"))
                            {
                                continue;
                            }
                        }

                        box.Items.Add(file.Name);
                    }
                    //selecting items triggers the event.
                    try
                    {
                        box.SelectedItem = item;
                    }
                    catch (Exception)
                    {
                        box.SelectedItem = defaultitem + ".rbxm";
                    }

                    box.Enabled = true;
                }
            }

            if (File.Exists(itemdir + @"\\" + item.Replace(".rbxm", "") + "_desc.txt"))
            {
                outputString.Text = File.ReadAllText(itemdir + @"\\" + item.Replace(".rbxm", "") + "_desc.txt");
            }
            else
            {
                outputString.Text = item;
            }

            if (provider != null && IsItemURL(item))
            {
                outputImage.Image = GetItemURLImageFromProvider(provider);
            }
            else
            {
                outputImage.Image = GlobalFuncs.LoadImage(itemdir + @"\\" + item.Replace(".rbxm", "") + ".png", itemdir + @"\\" + defaultitem + ".png");
            }
        }

        public static bool IsItemURL(string item)
        {
            if (item.Contains("http://"))
                return true;

            return false;
        }

        public static Image GetItemURLImageFromProvider(Provider provider)
        {
            if (provider != null)
                return GlobalFuncs.LoadImage(GlobalPaths.CustomPlayerDir + @"\\" + provider.Icon, GlobalPaths.extradir + @"\\NoExtra.png");

            return GlobalFuncs.LoadImage(GlobalPaths.extradir + @"\\NoExtra.png");
        }

        //we launch the 3dview seperately from normal clients.
        public static void Launch3DView()
        {
            GlobalFuncs.ReloadLoadoutValue();
            //HACK!
            try
            {
                GlobalFuncs.ChangeGameSettings("2011E");
            }
            catch(Exception)
            {
            }
            string luafile = "rbxasset://scripts\\\\CSView.lua";
            string mapfile = GlobalPaths.BasePathLauncher + "\\preview\\content\\fonts\\3DView.rbxl";
            string rbxexe = GlobalPaths.BasePathLauncher + "\\preview\\3DView.exe";
            string quote = "\"";
            string args = quote + mapfile + "\" -script \" dofile('" + luafile + "'); _G.CS3DView(0,'" + GlobalVars.UserConfiguration.PlayerName + "'," + GlobalVars.Loadout + ");" + quote;
            try
            {
                Process client = new Process();
                client.StartInfo.FileName = rbxexe;
                client.StartInfo.Arguments = args;
                client.Start();
                client.PriorityClass = ProcessPriorityClass.RealTime;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to launch Novetus. (Error: " + ex.Message + ")", "Novetus - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
    #endregion
