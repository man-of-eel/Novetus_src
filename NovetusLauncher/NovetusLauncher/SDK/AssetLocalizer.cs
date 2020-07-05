﻿using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace NovetusLauncher
{
    public partial class AssetLocalizer : Form
    {
        private RobloxFileType currentType;
        private string path;
        private string name;
        private string meshname;

        public AssetLocalizer()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                Filter = (currentType == RobloxFileType.RBXL) ? "ROBLOX Level (*.rbxl)|*.rbxl" : "ROBLOX Model (*.rbxm)|*.rbxm",
                Title = "Open ROBLOX level or model"
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                path = openFileDialog1.FileName;

                backgroundWorker1.RunWorkerAsync();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 1:
                    currentType = RobloxFileType.RBXM;
                    break;
                case 2:
                    currentType = RobloxFileType.Hat;
                    break;
                case 3:
                    currentType = RobloxFileType.Head;
                    break;
                case 4:
                    currentType = RobloxFileType.Face;
                    break;
                case 5:
                    currentType = RobloxFileType.Shirt;
                    break;
                case 6:
                    currentType = RobloxFileType.TShirt;
                    break;
                case 7:
                    currentType = RobloxFileType.Pants;
                    break;
                default:
                    currentType = RobloxFileType.RBXL;
                    break;
            }
        }

        private void AssetLocalizer_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedItem = "RBXL";
            comboBox2.SelectedItem = "None";

            if (Directory.Exists(Directories.hatdirFonts))
            {
                DirectoryInfo dinfo = new DirectoryInfo(Directories.hatdirFonts);
                FileInfo[] Files = dinfo.GetFiles("*.mesh");
                foreach (FileInfo file in Files)
                {
                    if (file.Name.Equals(String.Empty))
                    {
                        continue;
                    }

                    comboBox2.Items.Add(file.Name);
                }
            }

            if (!Directory.Exists(Directories.AssetCacheDirFonts))
            {
                Directory.CreateDirectory(Directories.AssetCacheDirFonts);
            }

            if (!Directory.Exists(Directories.AssetCacheDirSky))
            {
                Directory.CreateDirectory(Directories.AssetCacheDirSky);
            }

            if (!Directory.Exists(Directories.AssetCacheDirSounds))
            {
                Directory.CreateDirectory(Directories.AssetCacheDirSounds);
            }

            if (!Directory.Exists(Directories.AssetCacheDirTexturesGUI))
            {
                Directory.CreateDirectory(Directories.AssetCacheDirTexturesGUI);
            }

            if (!Directory.Exists(Directories.AssetCacheDirScripts))
            {
                Directory.CreateDirectory(Directories.AssetCacheDirScripts);
            }
        }

        private string GetProgressString(int percent)
        {
            string progressString = "";

            switch (currentType)
            {
                case RobloxFileType.RBXL:
                    switch (percent)
                    {
                        case 0:
                            progressString = "Backing up RBXL...";
                            break;
                        case 5:
                            progressString = "Downloading RBXL Meshes and Textures...";
                            break;
                        case 10:
                            progressString = "Downloading RBXL Skybox Textures...";
                            break;
                        case 15:
                            progressString = "Downloading RBXL Decal Textures...";
                            break;
                        case 20:
                            progressString = "Downloading RBXL Textures...";
                            break;
                        case 25:
                            progressString = "Downloading RBXL Tool Textures...";
                            break;
                        case 30:
                            progressString = "Downloading RBXL HopperBin Textures...";
                            break;
                        case 40:
                            progressString = "Downloading RBXL Sounds...";
                            break;
                        case 50:
                            progressString = "Downloading RBXL GUI Textures...";
                            break;
                        case 60:
                            progressString = "Downloading RBXL Shirt Textures...";
                            break;
                        case 65:
                            progressString = "Downloading RBXL T-Shirt Textures...";
                            break;
                        case 70:
                            progressString = "Downloading RBXL Pants Textures...";
                            break;
                        case 80:
                            progressString = "Downloading RBXL Linked Scripts...";
                            break;
                        case 90:
                            progressString = "Downloading RBXL Linked LocalScripts...";
                            break;
                    }
                    break;
                case RobloxFileType.RBXM:
                    switch (percent)
                    {
                        case 0:
                            progressString = "Downloading RBXL Meshes and Textures...";
                            break;
                        case 10:
                            progressString = "Downloading RBXL Skybox Textures...";
                            break;
                        case 15:
                            progressString = "Downloading RBXL Decal Textures...";
                            break;
                        case 20:
                            progressString = "Downloading RBXL Textures...";
                            break;
                        case 25:
                            progressString = "Downloading RBXL Tool Textures...";
                            break;
                        case 30:
                            progressString = "Downloading RBXL HopperBin Textures...";
                            break;
                        case 40:
                            progressString = "Downloading RBXL Sounds...";
                            break;
                        case 50:
                            progressString = "Downloading RBXL GUI Textures...";
                            break;
                        case 60:
                            progressString = "Downloading RBXL Shirt Textures...";
                            break;
                        case 65:
                            progressString = "Downloading RBXL T-Shirt Textures...";
                            break;
                        case 70:
                            progressString = "Downloading RBXL Pants Textures...";
                            break;
                        case 80:
                            progressString = "Downloading RBXL Linked Scripts...";
                            break;
                        case 90:
                            progressString = "Downloading RBXL Linked LocalScripts...";
                            break;
                    }
                    break;
                case RobloxFileType.Hat:
                    switch (percent)
                    {
                        case 0:
                            progressString = "Downloading Hat Meshes and Textures...";
                            break;
                        case 25:
                            progressString = "Downloading Hat Sounds...";
                            break;
                        case 50:
                            progressString = "Downloading Hat Linked Scripts...";
                            break;
                        case 75:
                            progressString = "Downloading Hat Linked LocalScripts...";
                            break;
                    }
                    break;
                case RobloxFileType.Head:
                    //meshes
                    switch (percent)
                    {
                        case 0:
                            progressString = "Downloading Head Meshes and Textures...";
                            break;
                    }
                    break;
                case RobloxFileType.Face:
                    //decal
                    switch (percent)
                    {
                        case 0:
                            progressString = "Downloading Face Textures...";
                            break;
                    }
                    break;
                case RobloxFileType.TShirt:
                    //texture
                    switch (percent)
                    {
                        case 0:
                            progressString = "Downloading T-Shirt Textures...";
                            break;
                    }
                    break;
                case RobloxFileType.Shirt:
                    //texture
                    switch (percent)
                    {
                        case 0:
                            progressString = "Downloading Shirt Textures...";
                            break;
                    }
                    break;
                case RobloxFileType.Pants:
                    //texture
                    switch (percent)
                    {
                        case 0:
                            progressString = "Downloading Pants Textures...";
                            break;
                    }
                    break;
                default:
                    progressString = "Idle";
                    break;
            }

            return progressString + " " + percent.ToString() + "%";
        }

        // This event handler is where the time-consuming work is done.
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            try
            {
                switch (currentType)
                {
                    case RobloxFileType.RBXL:
                        //backup the original copy
                        try
                        {
                            worker.ReportProgress(0);
                            File.Copy(path, path.Replace(".rbxl", " BAK.rbxl"));
                        }
                        catch (Exception)
                        {
                            worker.ReportProgress(100);
                        }
                        //meshes
                        worker.ReportProgress(5);
                        RobloxXMLLocalizer.DownloadFromNodes(path, RobloxTypeDefs.Fonts);
                        RobloxXMLLocalizer.DownloadFromNodes(path, RobloxTypeDefs.Fonts, 1, 1, 1, 1);
                        //skybox
                        worker.ReportProgress(10);
                        RobloxXMLLocalizer.DownloadFromNodes(path, RobloxTypeDefs.Sky);
                        RobloxXMLLocalizer.DownloadFromNodes(path, RobloxTypeDefs.Sky, 1, 0, 0, 0);
                        RobloxXMLLocalizer.DownloadFromNodes(path, RobloxTypeDefs.Sky, 2, 0, 0, 0);
                        RobloxXMLLocalizer.DownloadFromNodes(path, RobloxTypeDefs.Sky, 3, 0, 0, 0);
                        RobloxXMLLocalizer.DownloadFromNodes(path, RobloxTypeDefs.Sky, 4, 0, 0, 0);
                        RobloxXMLLocalizer.DownloadFromNodes(path, RobloxTypeDefs.Sky, 5, 0, 0, 0);
                        //decal
                        worker.ReportProgress(15);
                        RobloxXMLLocalizer.DownloadFromNodes(path, RobloxTypeDefs.Decal);
                        //texture
                        worker.ReportProgress(20);
                        RobloxXMLLocalizer.DownloadFromNodes(path, RobloxTypeDefs.Texture);
                        //tools and hopperbin
                        worker.ReportProgress(25);
                        RobloxXMLLocalizer.DownloadFromNodes(path, RobloxTypeDefs.Tool);
                        worker.ReportProgress(30);
                        RobloxXMLLocalizer.DownloadFromNodes(path, RobloxTypeDefs.HopperBin);
                        //sound
                        worker.ReportProgress(40);
                        RobloxXMLLocalizer.DownloadFromNodes(path, RobloxTypeDefs.Sound);
                        worker.ReportProgress(50);
                        RobloxXMLLocalizer.DownloadFromNodes(path, RobloxTypeDefs.ImageLabel);
                        //clothing
                        worker.ReportProgress(60);
                        RobloxXMLLocalizer.DownloadFromNodes(path, RobloxTypeDefs.Shirt);
                        worker.ReportProgress(65);
                        RobloxXMLLocalizer.DownloadFromNodes(path, RobloxTypeDefs.ShirtGraphic);
                        worker.ReportProgress(70);
                        RobloxXMLLocalizer.DownloadFromNodes(path, RobloxTypeDefs.Pants);
                        //scripts
                        worker.ReportProgress(80);
                        RobloxXMLLocalizer.DownloadFromNodes(path, RobloxTypeDefs.Script);
                        worker.ReportProgress(90);
                        RobloxXMLLocalizer.DownloadFromNodes(path, RobloxTypeDefs.LocalScript);
                        worker.ReportProgress(100);
                        break;
                    case RobloxFileType.RBXM:
                        //meshes
                        worker.ReportProgress(0);
                        RobloxXMLLocalizer.DownloadFromNodes(path, RobloxTypeDefs.Fonts);
                        RobloxXMLLocalizer.DownloadFromNodes(path, RobloxTypeDefs.Fonts, 1, 1, 1, 1);
                        //skybox
                        worker.ReportProgress(10);
                        RobloxXMLLocalizer.DownloadFromNodes(path, RobloxTypeDefs.Sky);
                        RobloxXMLLocalizer.DownloadFromNodes(path, RobloxTypeDefs.Sky, 1, 0, 0, 0);
                        RobloxXMLLocalizer.DownloadFromNodes(path, RobloxTypeDefs.Sky, 2, 0, 0, 0);
                        RobloxXMLLocalizer.DownloadFromNodes(path, RobloxTypeDefs.Sky, 3, 0, 0, 0);
                        RobloxXMLLocalizer.DownloadFromNodes(path, RobloxTypeDefs.Sky, 4, 0, 0, 0);
                        RobloxXMLLocalizer.DownloadFromNodes(path, RobloxTypeDefs.Sky, 5, 0, 0, 0);
                        //decal
                        worker.ReportProgress(15);
                        RobloxXMLLocalizer.DownloadFromNodes(path, RobloxTypeDefs.Decal);
                        //texture
                        worker.ReportProgress(20);
                        RobloxXMLLocalizer.DownloadFromNodes(path, RobloxTypeDefs.Texture);
                        //tools and hopperbin
                        worker.ReportProgress(25);
                        RobloxXMLLocalizer.DownloadFromNodes(path, RobloxTypeDefs.Tool);
                        worker.ReportProgress(30);
                        RobloxXMLLocalizer.DownloadFromNodes(path, RobloxTypeDefs.HopperBin);
                        //sound
                        worker.ReportProgress(40);
                        RobloxXMLLocalizer.DownloadFromNodes(path, RobloxTypeDefs.Sound);
                        worker.ReportProgress(50);
                        RobloxXMLLocalizer.DownloadFromNodes(path, RobloxTypeDefs.ImageLabel);
                        //clothing
                        worker.ReportProgress(60);
                        RobloxXMLLocalizer.DownloadFromNodes(path, RobloxTypeDefs.Shirt);
                        worker.ReportProgress(65);
                        RobloxXMLLocalizer.DownloadFromNodes(path, RobloxTypeDefs.ShirtGraphic);
                        worker.ReportProgress(70);
                        RobloxXMLLocalizer.DownloadFromNodes(path, RobloxTypeDefs.Pants);
                        //scripts
                        worker.ReportProgress(80);
                        RobloxXMLLocalizer.DownloadFromNodes(path, RobloxTypeDefs.Script);
                        worker.ReportProgress(90);
                        RobloxXMLLocalizer.DownloadFromNodes(path, RobloxTypeDefs.LocalScript);
                        worker.ReportProgress(100);
                        break;
                    case RobloxFileType.Hat:
                        //meshes
                        worker.ReportProgress(0);
                        RobloxXMLLocalizer.DownloadFromNodes(path, RobloxTypeDefs.ItemHatFonts, name, meshname);
                        RobloxXMLLocalizer.DownloadFromNodes(path, RobloxTypeDefs.ItemHatFonts, 1, 1, 1, 1, name);
                        worker.ReportProgress(25);
                        RobloxXMLLocalizer.DownloadFromNodes(path, RobloxTypeDefs.ItemHatSound);
                        //scripts
                        worker.ReportProgress(50);
                        RobloxXMLLocalizer.DownloadFromNodes(path, RobloxTypeDefs.Script);
                        worker.ReportProgress(75);
                        RobloxXMLLocalizer.DownloadFromNodes(path, RobloxTypeDefs.LocalScript);
                        worker.ReportProgress(100);
                        break;
                    case RobloxFileType.Head:
                        //meshes
                        worker.ReportProgress(0);
                        RobloxXMLLocalizer.DownloadFromNodes(path, RobloxTypeDefs.ItemHeadFonts, name);
                        RobloxXMLLocalizer.DownloadFromNodes(path, RobloxTypeDefs.ItemHeadFonts, 1, 1, 1, 1, name);
                        worker.ReportProgress(100);
                        break;
                    case RobloxFileType.Face:
                        //decal
                        worker.ReportProgress(0);
                        RobloxXMLLocalizer.DownloadFromNodes(path, RobloxTypeDefs.ItemFaceTexture, name);
                        worker.ReportProgress(100);
                        break;
                    case RobloxFileType.TShirt:
                        //texture
                        worker.ReportProgress(0);
                        RobloxXMLLocalizer.DownloadFromNodes(path, RobloxTypeDefs.ItemTShirtTexture, name);
                        worker.ReportProgress(100);
                        break;
                    case RobloxFileType.Shirt:
                        //texture
                        worker.ReportProgress(0);
                        RobloxXMLLocalizer.DownloadFromNodes(path, RobloxTypeDefs.ItemShirtTexture, name);
                        worker.ReportProgress(100);
                        break;
                    case RobloxFileType.Pants:
                        //texture
                        worker.ReportProgress(0);
                        RobloxXMLLocalizer.DownloadFromNodes(path, RobloxTypeDefs.ItemPantsTexture, name);
                        worker.ReportProgress(100);
                        break;
                    default:
                        worker.ReportProgress(100);
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: Unable to localize the asset. " + ex.Message, "Novetus Asset Localizer", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // This event handler updates the progress.
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            label2.Text = GetProgressString(e.ProgressPercentage);
        }

        // This event handler deals with the results of the background operation.
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            switch (e)
            {
                case RunWorkerCompletedEventArgs can when can.Cancelled == true:
                    label2.Text = "Canceled!";
                    break;
                case RunWorkerCompletedEventArgs err when err.Error != null:
                    label2.Text = "Error: " + e.Error.Message;
                    break;
                default:
                    label2.Text = "Done!";
                    break;
            }
        }

        void AssetLocalizer_Close(object sender, CancelEventArgs e)
        {
            backgroundWorker1.CancelAsync();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            name = textBox1.Text;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem.ToString() == "None")
            {
                meshname = "";
            }
            else
            {
                meshname = comboBox2.SelectedItem.ToString();
            }
        }
    }
}
