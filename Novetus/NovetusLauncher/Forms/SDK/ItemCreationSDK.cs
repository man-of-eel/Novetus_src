﻿#region Usings
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
#endregion

#region Item Creation SDK
public partial class ItemCreationSDK : Form
{
    #region Variables
    private static RobloxFileType type;
    private static string Template = "";
    private static string Option1Path = "";
    private static string Option2Path = "";
    private static bool Option1Required = false;
    private static bool Option2Required = false;
    private static bool RequiresIconForTexture = false;
    private static bool ItemEditing = false;
    private static bool IsReskin = false;
    private static bool IsResized = false;
    private OpenFileDialog openFileDialog1;
    private static string FileDialogFilter1 = "";
    private static string FileDialogName1 = "";
    private static string FileDialogFilter2 = "";
    private static string FileDialogName2 = "";
    public int partColorID = 194;
    #endregion

    #region Constructor
    public ItemCreationSDK()
    {
        InitializeComponent();
    }
    #endregion

    #region Form Events
    private void ItemCreationSDK_Load(object sender, EventArgs e)
    {
        Size = new Size(323, 450);
        ItemSettingsGroup.Height = 393;
        ItemTypeListBox.SelectedItem = "Hat";
        MeshTypeBox.SelectedItem = "BlockMesh";
        SpecialMeshTypeBox.SelectedItem = "Head";
        Reset(true);
        CenterToScreen();
    }

    private void ItemCreationSDK_Close(object sender, FormClosingEventArgs e)
    {
        DeleteStrayIcons();
    }

    private void BrowseImageButton_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(ItemNameBox.Text))
        {
            MessageBox.Show("You must assign an item name before you change the icon.", "Novetus Item Creation SDK - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        else
        {
            string previconpath = GetPathForType(type) + "\\" + ItemNameBox.Text.Replace(" ", "") + ".png";

            if (File.Exists(previconpath))
            {
               DialogResult result = MessageBox.Show("An icon with this item's name already exists. Would you like to replace it?", "Novetus Item Creation SDK - Icon already exists", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
               if (result == DialogResult.No)
               {
                    return;
               }
            }

            IconLoader icon = new IconLoader();
            icon.CopyToItemDir = true;
            icon.ItemDir = GetPathForType(type);
            icon.ItemName = ItemNameBox.Text;
            try
            {
                icon.LoadImage();
            }
            catch (Exception ex)
            {
                GlobalFuncs.LogExceptions(ex);
            }

            if (!string.IsNullOrWhiteSpace(icon.getInstallOutcome()))
            {
                MessageBoxIcon boxicon = MessageBoxIcon.Information;

                if (icon.getInstallOutcome().Contains("Error"))
                {
                    boxicon = MessageBoxIcon.Error;
                }

                MessageBox.Show(icon.getInstallOutcome(), "Novetus Item Creation SDK - Icon Copy Completed", MessageBoxButtons.OK, boxicon);
            }

            Image icon1 = GlobalFuncs.LoadImage(icon.ItemDir + "\\" + icon.ItemName.Replace(" ", "") + ".png", "");
            ItemIcon.Image = icon1;

            if (type == RobloxFileType.TShirt || type == RobloxFileType.Face)
            {
                Option1Path = icon.ItemPath;
                if (Option1TextBox.ReadOnly) Option1TextBox.ReadOnly = false;
                Option1TextBox.Text = Path.GetFileName(Option1Path);
                if (!Option1TextBox.ReadOnly) Option1TextBox.ReadOnly = true;
            }
        }
    }

    private void ItemNameBox_TextChanged(object sender, EventArgs e)
    {
        LoadItemIfExists();
        UpdateWarnings();
    }

    private void ItemTypeListBox_SelectedIndexChanged(object sender, EventArgs e)
    {
        DeleteStrayIcons();

        type = GetTypeForInt(ItemTypeListBox.SelectedIndex);

        switch (type)
        {
            case RobloxFileType.Hat:
                ToggleOptionSet(Option1Label, Option1TextBox, Option1BrowseButton, "Hat Mesh", true);
                Option1Path = "";
                Option1Required = true;
                ToggleHatMeshBox("Uses Existing Hat Mesh");
                ToggleHatTextureBox("Uses Existing Hat Texture");
                ToggleOptionSet(Option2Label, Option2TextBox, Option2BrowseButton, "Hat Texture", true);
                Option2Path = "";
                Option2Required = false;
                ToggleGroup(CoordGroup, "Hat Attachment Point");
                ToggleGroup(CoordGroup2, "Hat Mesh Scale");
                ToggleGroup(CoordGroup3, "Hat Mesh Vertex Color");
                ToggleGroup(MeshOptionsGroup, "", false);
                ToggleGroup(HatOptionsGroup, "Hat Options");
                Template = GlobalPaths.ConfigDirTemplates + "\\HatTemplate.rbxm";
                FileDialogFilter1 = "*.mesh";
                FileDialogName1 = "Hat Mesh";
                FileDialogFilter2 = "*.png";
                FileDialogName2 = "Hat Texture";
                RequiresIconForTexture = false;
                HatOptionsGroup.Location = new Point(610, 215);
                MeshOptionsGroup.Location = new Point(911, 20);
                break;
            case RobloxFileType.HeadNoCustomMesh:
                ToggleOptionSet(Option1Label, Option1TextBox, Option1BrowseButton, "", false, false);
                Option1Path = "";
                Option1Required = false;
                ToggleHatMeshBox("", false);
                ToggleHatTextureBox("", false);
                ToggleOptionSet(Option2Label, Option2TextBox, Option2BrowseButton, "", false, false);
                Option2Path = "";
                Option2Required = false;
                ToggleGroup(CoordGroup, "Head Mesh Scale");
                ToggleGroup(CoordGroup2, "Head Mesh Vertex Color");
                ToggleGroup(CoordGroup3, "", false);
                ToggleGroup(MeshOptionsGroup, "Head Mesh Options");
                ToggleGroup(HatOptionsGroup, "", false);
                Template = GlobalPaths.ConfigDirTemplates + "\\HeadNoCustomMeshTemplate.rbxm";
                RequiresIconForTexture = false;
                MeshOptionsGroup.Location = new Point(610, 215);
                HatOptionsGroup.Location = new Point(911, 20);
                break;
            case RobloxFileType.Head:
                ToggleOptionSet(Option1Label, Option1TextBox, Option1BrowseButton, "Head Mesh", true);
                Option1Path = "";
                Option1Required = true;
                ToggleHatMeshBox("", false);
                ToggleHatTextureBox("", false);
                ToggleOptionSet(Option2Label, Option2TextBox, Option2BrowseButton, "Head Texture", true);
                Option2Path = "";
                Option2Required = false;
                ToggleGroup(CoordGroup, "Head Mesh Scale");
                ToggleGroup(CoordGroup2, "Head Mesh Vertex Color");
                ToggleGroup(CoordGroup3, "", false);
                ToggleGroup(MeshOptionsGroup, "", false);
                ToggleGroup(HatOptionsGroup, "", false);
                Template = GlobalPaths.ConfigDirTemplates + "\\HeadTemplate.rbxm";
                FileDialogFilter1 = "*.mesh";
                FileDialogName1 = "Head Mesh";
                FileDialogFilter2 = "*.png";
                FileDialogName2 = "Head Texture";
                RequiresIconForTexture = false;
                MeshOptionsGroup.Location = new Point(610, 215);
                HatOptionsGroup.Location = new Point(911, 20);
                break;
            case RobloxFileType.Face:
                ToggleOptionSet(Option1Label, Option1TextBox, Option1BrowseButton, "Load the Item Icon to load a Face Texture.", false, false);
                Option1Path = "";
                Option1Required = false;
                ToggleHatMeshBox("", false);
                ToggleHatTextureBox("", false);
                ToggleOptionSet(Option2Label, Option2TextBox, Option2BrowseButton, "", false, false);
                Option2Path = "";
                Option2Required = false;
                ToggleGroup(CoordGroup, "", false);
                ToggleGroup(CoordGroup2, "", false);
                ToggleGroup(CoordGroup3, "", false);
                ToggleGroup(MeshOptionsGroup, "", false);
                ToggleGroup(HatOptionsGroup, "", false);
                Template = GlobalPaths.ConfigDirTemplates + "\\FaceTemplate.rbxm";
                RequiresIconForTexture = true;
                HatOptionsGroup.Location = new Point(610, 215);
                MeshOptionsGroup.Location = new Point(911, 20);
                break;
            case RobloxFileType.TShirt:
                ToggleOptionSet(Option1Label, Option1TextBox, Option1BrowseButton, "Load the Item Icon to load a T-Shirt Template.", false, false);
                Option1Path = "";
                Option1Required = false;
                ToggleHatMeshBox("", false);
                ToggleHatTextureBox("", false);
                ToggleOptionSet(Option2Label, Option2TextBox, Option2BrowseButton, "", false, false);
                Option2Path = "";
                Option2Required = false;
                ToggleGroup(CoordGroup, "", false);
                ToggleGroup(CoordGroup2, "", false);
                ToggleGroup(CoordGroup3, "", false);
                ToggleGroup(MeshOptionsGroup, "", false);
                ToggleGroup(HatOptionsGroup, "", false);
                Template = GlobalPaths.ConfigDirTemplates + "\\TShirtTemplate.rbxm";
                RequiresIconForTexture = true;
                HatOptionsGroup.Location = new Point(610, 215);
                MeshOptionsGroup.Location = new Point(911, 20);
                break;
            case RobloxFileType.Shirt:
                ToggleOptionSet(Option1Label, Option1TextBox, Option1BrowseButton, "Shirt Template", true);
                Option1Path = "";
                Option1Required = true;
                ToggleHatMeshBox("", false);
                ToggleHatTextureBox("", false);
                ToggleOptionSet(Option2Label, Option2TextBox, Option2BrowseButton, "", false, false);
                Option2Path = "";
                Option2Required = false;
                ToggleGroup(CoordGroup, "", false);
                ToggleGroup(CoordGroup2, "", false);
                ToggleGroup(CoordGroup3, "", false);
                ToggleGroup(MeshOptionsGroup, "", false);
                ToggleGroup(HatOptionsGroup, "", false);
                Template = GlobalPaths.ConfigDirTemplates + "\\ShirtTemplate.rbxm";
                FileDialogFilter1 = "*.png";
                FileDialogName1 = "Shirt Template";
                RequiresIconForTexture = false;
                HatOptionsGroup.Location = new Point(610, 215);
                MeshOptionsGroup.Location = new Point(911, 20);
                break;
            case RobloxFileType.Pants:
                ToggleOptionSet(Option1Label, Option1TextBox, Option1BrowseButton, "Pants Template", true);
                Option1Path = "";
                Option1Required = true;
                ToggleHatMeshBox("", false);
                ToggleHatTextureBox("", false);
                ToggleOptionSet(Option2Label, Option2TextBox, Option2BrowseButton, "", false, false);
                Option2Path = "";
                Option2Required = false;
                ToggleGroup(CoordGroup, "", false);
                ToggleGroup(CoordGroup2, "", false);
                ToggleGroup(CoordGroup3, "", false);
                ToggleGroup(MeshOptionsGroup, "", false);
                ToggleGroup(HatOptionsGroup, "", false);
                Template = GlobalPaths.ConfigDirTemplates + "\\PantsTemplate.rbxm";
                FileDialogFilter1 = "*.png";
                FileDialogName1 = "Pants Template";
                RequiresIconForTexture = false;
                HatOptionsGroup.Location = new Point(610, 215);
                MeshOptionsGroup.Location = new Point(911, 20);
                break;
            default:
                break;
        }

        LoadItemIfExists();
    }

    private void CreateItemButton_Click(object sender, EventArgs e)
    {
        if (!CheckItemRequirements())
            return;

        string ItemName = ItemNameBox.Text.Replace(" ", "");
        if (CreateItem(Template,
            type,
            ItemName,
            new string[] { Option1Path, Option2Path, Option1TextBox.Text, Option2TextBox.Text },
            new Vector3(Convert.ToDouble(XBox.Value), Convert.ToDouble(YBox.Value), Convert.ToDouble(ZBox.Value)),
            new Vector3(Convert.ToDouble(XBox360.Value), Convert.ToDouble(YBox2.Value), Convert.ToDouble(ZBox2.Value)),
            new Vector3(Convert.ToDouble(XBoxOne.Value), Convert.ToDouble(YBox3.Value), Convert.ToDouble(ZBox3.Value)),
            new Vector3[] { 
                new Vector3(Convert.ToDouble(rightXBox.Value), Convert.ToDouble(rightYBox.Value), Convert.ToDouble(rightZBox.Value)), 
                new Vector3(Convert.ToDouble(upXBox.Value), Convert.ToDouble(upYBox.Value), Convert.ToDouble(upZBox.Value)), 
                new Vector3(Convert.ToDouble(-forwardXBox.Value), Convert.ToDouble(-forwardYBox.Value), Convert.ToDouble(-forwardZBox.Value)) },
            Convert.ToDouble(transparencyBox.Value),
            Convert.ToDouble(reflectivenessBox.Value),
            new object[] { Convert.ToDouble(BevelBox.Value), 
                Convert.ToDouble(RoundnessBox.Value), 
                Convert.ToDouble(BulgeBox.Value), 
                SpecialMeshTypeBox.SelectedIndex, 
                MeshTypeBox.SelectedItem.ToString(),
                Convert.ToInt32(LODXBox.Value),
                Convert.ToInt32(LODYBox.Value)},
            DescBox.Text
            ))
        {
            DialogResult LaunchCharCustom = MessageBox.Show("The creation of your item, " + ItemNameBox.Text + ", is successful! Would you like to test your item out in Character Customization?", "Novetus Item Creation SDK - Item Creation Success", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (LaunchCharCustom == DialogResult.Yes)
            {
                GlobalFuncs.LaunchCharacterCustomization();
            }
        }
    }

    private void Option1BrowseButton_Click(object sender, EventArgs e)
    {
        Option1Path = LoadAsset(FileDialogName1, FileDialogFilter1);
        Option1TextBox.Text = Path.GetFileName(Option1Path);
    }

    private void Option2BrowseButton_Click(object sender, EventArgs e)
    {
        Option2Path = LoadAsset(FileDialogName2, FileDialogFilter2);
        Option2TextBox.Text = Path.GetFileName(Option2Path);
    }

    private void UsesHatMeshBox_SelectedIndexChanged(object sender, EventArgs e)
    {
        Option1Path = "";

        if (UsesHatMeshBox.SelectedItem.ToString() != "None")
        {
            Option1TextBox.Text = UsesHatMeshBox.Text;
            Option1TextBox.Enabled = false;
            Option1BrowseButton.Enabled = false;
        }
        else
        {
            Option1TextBox.Enabled = true;
            Option1BrowseButton.Enabled = true;
            Option1TextBox.Text = "";
        }
    }

    private void UsesHatTexBox_SelectedIndexChanged(object sender, EventArgs e)
    {
        Option2Path = "";

        if (UsesHatTexBox.SelectedItem.ToString() != "None")
        {
            Option2TextBox.Text = UsesHatTexBox.Text;
            Option2TextBox.Enabled = false;
            Option2BrowseButton.Enabled = false;
        }
        else
        {
            Option2TextBox.Enabled = true;
            Option2BrowseButton.Enabled = true;
            Option2TextBox.Text = "";
        }
    }

    private void EditItem_CheckedChanged(object sender, EventArgs e)
    {
        ItemEditing = EditItemBox.Checked;
        UpdateWarnings();
    }

    private void ReskinBox_CheckedChanged(object sender, EventArgs e)
    {
        IsReskin = ReskinBox.Checked;
    }

    private void ResetButton_Click(object sender, EventArgs e)
    {
        Reset(true);
        MessageBox.Show("All fields reset!", "Novetus Item Creation SDK - Reset", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void SettingsButton_Click(object sender, EventArgs e)
    {
        if (IsResized)
        {
            Size = new Size(323, 450);
            IsResized = false;
        }
        else
        {
            Size = new Size(914, 450);
            IsResized = true;
        }

        CenterToScreen();
    }

    private void HatColorButton_Click(object sender, EventArgs e)
    {
        ItemCreationSDKColorMenu menu = new ItemCreationSDKColorMenu(this);
        menu.Show();
    }
    #endregion

    #region Functions

    #region XML Editing/Fetching
    public static void SetItemFontVals(XDocument doc, AssetCacheDef assetdef, int idIndex, int outputPathIndex, int inGameDirIndex, string assetpath, string assetfilename)
    {
        SetItemFontVals(doc, assetdef.Class, assetdef.Id[idIndex], assetdef.Dir[outputPathIndex], assetdef.GameDir[inGameDirIndex], assetpath, assetfilename);
    }

    public static void SetItemFontVals(XDocument doc, string itemClassValue, string itemIdValue, string outputPath, string inGameDir, string assetpath, string assetfilename)
    {
        var v = from nodes in doc.Descendants("Item")
                where nodes.Attribute("class").Value == itemClassValue
                select nodes;

        foreach (var item in v)
        {
            var v2 = from nodes in item.Descendants("Content")
                     where nodes.Attribute("name").Value == itemIdValue
                     select nodes;

            foreach (var item2 in v2)
            {
                var v3 = from nodes in item2.Descendants("url")
                         select nodes;

                foreach (var item3 in v3)
                {
                    if (!string.IsNullOrWhiteSpace(assetpath))
                    {
                        GlobalFuncs.FixedFileCopy(assetpath, outputPath + "\\" + assetfilename, true);
                    }
                    item3.Value = inGameDir + assetfilename;
                }
            }
        }
    }

    public static void SetItemFontValEmpty(XDocument doc, AssetCacheDef assetdef, int idIndex)
    {
        var v = from nodes in doc.Descendants("Item")
                where nodes.Attribute("class").Value == assetdef.Class
                select nodes;

        foreach (var item in v)
        {
            var v2 = from nodes in item.Descendants("Content")
                     where nodes.Attribute("name").Value == assetdef.Id[idIndex]
                     select nodes;

            foreach (var item2 in v2)
            {
                var v3 = from nodes in item2.Descendants("url")
                         select nodes;

                foreach (var item3 in v3)
                {
                    item3.Value = "";
                }
            }
        }
    }

    public static string GetItemFontVals(XDocument doc, AssetCacheDef assetdef, int idIndex)
    {
        return GetItemFontVals(doc, assetdef.Class, assetdef.Id[idIndex]);
    }

    public static string GetItemFontVals(XDocument doc, string itemClassValue, string itemIdValue)
    {
        var v = from nodes in doc.Descendants("Item")
                where nodes.Attribute("class").Value == itemClassValue
                select nodes;

        foreach (var item in v)
        {
            var v2 = from nodes in item.Descendants("Content")
                     where nodes.Attribute("name").Value == itemIdValue
                     select nodes;

            foreach (var item2 in v2)
            {
                var v3 = from nodes in item2.Descendants("url")
                         select nodes;

                foreach (var item3 in v3)
                {
                    return item3.Value;
                }
            }
        } 
        
        return "";
    }

    public static void SetItemCoordVals(XDocument doc, AssetCacheDef assetdef, Vector3 coord, string CoordClass, string CoordName)
    {
        SetItemCoordVals(doc, assetdef.Class, coord, CoordClass, CoordName);
    }

    public static void SetItemCoordVals(XDocument doc, string itemClassValue, Vector3 coord, string CoordClass, string CoordName)
    {
        var v = from nodes in doc.Descendants("Item")
                where nodes.Attribute("class").Value == itemClassValue
                select nodes;

        SetItemCoordXML(v, coord, CoordClass, CoordName);
    }

    public static void SetItemCoordValsNoClassSearch(XDocument doc, Vector3 coord, string CoordClass, string CoordName)
    {
        var v = from nodes in doc.Descendants("Item")
                select nodes;

        SetItemCoordXML(v, coord, CoordClass, CoordName);
    }

    private static void SetItemCoordXML(IEnumerable<XElement> v, Vector3 coord, string CoordClass, string CoordName)
    {
        foreach (var item in v)
        {
            var v2 = from nodes in item.Descendants(CoordClass)
                     where nodes.Attribute("name").Value == CoordName
                     select nodes;

            foreach (var item2 in v2)
            {
                var v3 = from nodes in item2.Descendants("X")
                         select nodes;

                foreach (var item3 in v3)
                {
                    item3.Value = coord.X.ToString();
                }

                var v4 = from nodes in item2.Descendants("Y")
                         select nodes;

                foreach (var item4 in v4)
                {
                    item4.Value = coord.Y.ToString();
                }

                var v5 = from nodes in item2.Descendants("Z")
                         select nodes;

                foreach (var item5 in v5)
                {
                    item5.Value = coord.Z.ToString();
                }
            }
        }
    }

    public static string GetItemCoordVals(XDocument doc, AssetCacheDef assetdef, string CoordClass, string CoordName)
    {
        return GetItemCoordVals(doc, assetdef.Class, CoordClass, CoordName);
    }

    public static string GetItemCoordVals(XDocument doc, string itemClassValue, string CoordClass, string CoordName)
    {
        var v = from nodes in doc.Descendants("Item")
                where nodes.Attribute("class").Value == itemClassValue
                select nodes;

        return GetItemCoordXML(v, CoordClass, CoordName);
    }

    public static string GetItemCoordValsNoClassSearch(XDocument doc, string CoordClass, string CoordName)
    {
        var v = from nodes in doc.Descendants("Item")
                select nodes;

        return GetItemCoordXML(v, CoordClass, CoordName);
    }

    private static string GetItemCoordXML(IEnumerable<XElement> v, string CoordClass, string CoordName)
    {
        string coord = "";

        foreach (var item in v)
        {
            var v2 = from nodes in item.Descendants(CoordClass)
                     where nodes.Attribute("name").Value == CoordName
                     select nodes;

            foreach (var item2 in v2)
            {
                var v3 = from nodes in item2.Descendants("X")
                         select nodes;

                foreach (var item3 in v3)
                {
                    coord += item3.Value + ",";
                }

                var v4 = from nodes in item2.Descendants("Y")
                         select nodes;

                foreach (var item4 in v4)
                {
                    coord += item4.Value + ",";
                }

                var v5 = from nodes in item2.Descendants("Z")
                         select nodes;

                foreach (var item5 in v5)
                {
                    coord += item5.Value;
                }
            }
        }

        return coord;
    }

    public static void SetItemRotationVals(XDocument doc, AssetCacheDef assetdef, Vector3 right, Vector3 up, Vector3 forward, string CoordClass, string CoordName)
    {
        SetItemRotationVals(doc, assetdef.Class, right, up, forward, CoordClass, CoordName);
    }

    public static void SetItemRotationVals(XDocument doc, string itemClassValue, Vector3 right, Vector3 up, Vector3 forward, string CoordClass, string CoordName)
    {
        var v = from nodes in doc.Descendants("Item")
                where nodes.Attribute("class").Value == itemClassValue
                select nodes;

        SetItemRotationXML(v, right, up, forward, CoordClass, CoordName);
    }

    public static void SetItemRotationValsNoClassSearch(XDocument doc, Vector3 right, Vector3 up, Vector3 forward, string CoordClass, string CoordName)
    {
        var v = from nodes in doc.Descendants("Item")
                select nodes;

        SetItemRotationXML(v, right, up, forward, CoordClass, CoordName);
    }

    private static void SetItemRotationXML(IEnumerable<XElement> v, Vector3 right, Vector3 up, Vector3 forward, string CoordClass, string CoordName)
    {
        foreach (var item in v)
        {
            var v2 = from nodes in item.Descendants(CoordClass)
                     where nodes.Attribute("name").Value == CoordName
                     select nodes;

            foreach (var item2 in v2)
            {
                var v3 = from nodes in item2.Descendants("R00")
                         select nodes;

                foreach (var item3 in v3)
                {
                    item3.Value = right.X.ToString();
                }

                var v4 = from nodes in item2.Descendants("R01")
                         select nodes;

                foreach (var item4 in v4)
                {
                    item4.Value = right.Y.ToString();
                }

                var v5 = from nodes in item2.Descendants("R02")
                         select nodes;

                foreach (var item5 in v5)
                {
                    item5.Value = right.Z.ToString();
                }

                var v6 = from nodes in item2.Descendants("R10")
                         select nodes;

                foreach (var item6 in v6)
                {
                    item6.Value = up.X.ToString();
                }

                var v7 = from nodes in item2.Descendants("R11")
                         select nodes;

                foreach (var item7 in v7)
                {
                    item7.Value = up.Y.ToString();
                }

                var v8 = from nodes in item2.Descendants("R12")
                         select nodes;

                foreach (var item8 in v8)
                {
                    item8.Value = up.Z.ToString();
                }

                var v9 = from nodes in item2.Descendants("R20")
                         select nodes;

                foreach (var item9 in v9)
                {
                    item9.Value = forward.X.ToString();
                }

                var v10 = from nodes in item2.Descendants("R21")
                          select nodes;

                foreach (var item10 in v10)
                {
                   item10.Value = forward.Y.ToString();
                }

                var v11 = from nodes in item2.Descendants("R22")
                          select nodes;

                foreach (var item11 in v11)
                {
                    item11.Value = forward.Z.ToString();
                }
            }
        }
    }

    public static string GetItemRotationVals(XDocument doc, AssetCacheDef assetdef, string CoordClass, string CoordName)
    {
        return GetItemRotationVals(doc, assetdef.Class, CoordClass, CoordName);
    }

    public static string GetItemRotationVals(XDocument doc, string itemClassValue, string CoordClass, string CoordName)
    {
        var v = from nodes in doc.Descendants("Item")
                where nodes.Attribute("class").Value == itemClassValue
                select nodes;

        return GetItemRotationXML(v, CoordClass, CoordName);
    }

    public static string GetItemRotationValsNoClassSearch(XDocument doc, string CoordClass, string CoordName)
    {
        var v = from nodes in doc.Descendants("Item")
                select nodes;

        return GetItemRotationXML(v, CoordClass, CoordName);
    }

    private static string GetItemRotationXML(IEnumerable<XElement> v, string CoordClass, string CoordName)
    {
        string coord = "";

        foreach (var item in v)
        {
            var v2 = from nodes in item.Descendants(CoordClass)
                     where nodes.Attribute("name").Value == CoordName
                     select nodes;

            foreach (var item2 in v2)
            {
                var v3 = from nodes in item2.Descendants("R00")
                         select nodes;

                foreach (var item3 in v3)
                {
                    coord += item3.Value + ",";
                }

                var v4 = from nodes in item2.Descendants("R01")
                         select nodes;

                foreach (var item4 in v4)
                {
                    coord += item4.Value + ",";
                }

                var v5 = from nodes in item2.Descendants("R02")
                         select nodes;

                foreach (var item5 in v5)
                {
                    coord += item5.Value + ",";
                }

                var v6 = from nodes in item2.Descendants("R10")
                         select nodes;

                foreach (var item6 in v6)
                {
                    coord += item6.Value + ",";
                }

                var v7 = from nodes in item2.Descendants("R11")
                         select nodes;

                foreach (var item7 in v7)
                {
                    coord += item7.Value + ",";
                }

                var v8 = from nodes in item2.Descendants("R12")
                         select nodes;

                foreach (var item8 in v8)
                {
                    coord += item8.Value + ",";
                }

                var v9 = from nodes in item2.Descendants("R20")
                         select nodes;

                foreach (var item9 in v9)
                {
                    coord += item9.Value + ",";
                }

                var v10 = from nodes in item2.Descendants("R21")
                         select nodes;

                foreach (var item10 in v10)
                {
                    coord += item10.Value + ",";
                }

                var v11 = from nodes in item2.Descendants("R22")
                         select nodes;

                foreach (var item11 in v11)
                {
                    coord += item11.Value;
                }
            }
        }

        return coord;
    }

    public static void SetHatMeshVals(XDocument doc, Vector3 coord, string type, string val)
    {
        var v = from nodes in doc.Descendants("Item")
                where nodes.Attribute("class").Value == "Hat"
                select nodes;

        foreach (var item in v)
        {
            var v2 = from nodes in doc.Descendants("Item")
                     where nodes.Attribute("class").Value == "Part"
                     select nodes;

            foreach (var item2 in v2)
            {
                var v3 = from nodes in doc.Descendants("Item")
                         where nodes.Attribute("class").Value == "SpecialMesh"
                         select nodes;

                foreach (var item3 in v3)
                {
                    SetItemCoordXML(v3, coord, type, val);
                }
            }
        }
    }

    public static string GetHatMeshVals(XDocument doc, string type, string val)
    {
        var v = from nodes in doc.Descendants("Item")
                where nodes.Attribute("class").Value == "Hat"
                select nodes;

        foreach (var item in v)
        {
            var v2 = from nodes in doc.Descendants("Item")
                     where nodes.Attribute("class").Value == "Part"
                     select nodes;

            foreach (var item2 in v2)
            {
                var v3 = from nodes in doc.Descendants("Item")
                         where nodes.Attribute("class").Value == "SpecialMesh"
                         select nodes;

                foreach (var item3 in v3)
                {
                    return GetItemCoordXML(v3, type, val);
                }
            }
        }

        return "";
    }

    public static void SetHatPartVals(XDocument doc, int colorID, double transparency, double reflectiveness)
    {
        var v = from nodes in doc.Descendants("Item")
                where nodes.Attribute("class").Value == "Hat"
                select nodes;

        foreach (var item in v)
        {
            var v2 = from nodes in doc.Descendants("Item")
                     where nodes.Attribute("class").Value == "Part"
                     select nodes;

            foreach (var item2 in v2)
            {
                var v5 = from nodes in item.Descendants(XMLTypes.Int.ToString().ToLower())
                         where nodes.Attribute("name").Value == "BrickColor"
                         select nodes;

                foreach (var item5 in v5)
                {
                    item5.Value = colorID.ToString();
                }

                var v4 = from nodes in item.Descendants(XMLTypes.Float.ToString().ToLower())
                         where nodes.Attribute("name").Value == "Reflectance"
                         select nodes;

                foreach (var item4 in v4)
                {
                    item4.Value = reflectiveness.ToString();
                }

                var v3 = from nodes in item.Descendants(XMLTypes.Float.ToString().ToLower())
                         where nodes.Attribute("name").Value == "Transparency"
                         select nodes;

                foreach (var item3 in v3)
                {
                    item3.Value = transparency.ToString();
                }
            }
        }
    }

    public static bool DoesHatHavePartColor(XDocument doc)
    {
        var v = from nodes in doc.Descendants("Item")
                where nodes.Attribute("class").Value == "Hat"
                select nodes;

        foreach (var item in v)
        {
            var v2 = from nodes in doc.Descendants("Item")
                     where nodes.Attribute("class").Value == "Part"
                     select nodes;

            foreach (var item2 in v2)
            {
                var v5 = from nodes in item.Descendants(XMLTypes.Int.ToString().ToLower())
                         where nodes.Attribute("name").Value == "BrickColor"
                         select nodes;

                foreach (var item5 in v5)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public static string GetHatPartVals(XDocument doc)
    {
        string hatpartsettings = "";

        var v = from nodes in doc.Descendants("Item")
                where nodes.Attribute("class").Value == "Hat"
                select nodes;

        foreach (var item in v)
        {
            var v2 = from nodes in doc.Descendants("Item")
                     where nodes.Attribute("class").Value == "Part"
                     select nodes;

            foreach (var item2 in v2)
            {
                if (DoesHatHavePartColor(doc))
                {
                    var v5 = from nodes in item.Descendants(XMLTypes.Int.ToString().ToLower())
                             where nodes.Attribute("name").Value == "BrickColor"
                             select nodes;

                    foreach (var item5 in v5)
                    {
                        hatpartsettings += item5.Value + ",";
                    }
                }
                else
                {
                    hatpartsettings += "194,";
                }

                var v4 = from nodes in item.Descendants(XMLTypes.Float.ToString().ToLower())
                         where nodes.Attribute("name").Value == "Reflectance"
                         select nodes;

                foreach (var item4 in v4)
                {
                    hatpartsettings += item4.Value + ",";
                }

                var v3 = from nodes in item.Descendants(XMLTypes.Float.ToString().ToLower())
                         where nodes.Attribute("name").Value == "Transparency"
                         select nodes;

                foreach (var item3 in v3)
                {
                    hatpartsettings += item3.Value;
                }
            }
        }

        return hatpartsettings;
    }

    public static void SetHeadBevel(XDocument doc, double bevel, double bevelRoundness, double bulge, int meshtype, string meshclass, int LODX, int LODY)
    {
        var v = from nodes in doc.Descendants("Item")
                select nodes;

        foreach (var item in v)
        {
            item.SetAttributeValue("class", meshclass);

            var v2 = from nodes in item.Descendants(XMLTypes.Float.ToString().ToLower())
                     where nodes.Attribute("name").Value == "Bevel"
                     select nodes;

            foreach (var item2 in v2)
            {
                item2.Value = bevel.ToString();
            }

            var v3 = from nodes in item.Descendants(XMLTypes.Float.ToString().ToLower())
                     where nodes.Attribute("name").Value == "Bevel Roundness"
                     select nodes;

            foreach (var item3 in v3)
            {
                item3.Value = bevelRoundness.ToString();
            }

            var v4 = from nodes in item.Descendants(XMLTypes.Float.ToString().ToLower())
                     where nodes.Attribute("name").Value == "Bulge"
                     select nodes;

            foreach (var item4 in v4)
            {
                item4.Value = bulge.ToString();
            }

            var vX = from nodes in item.Descendants(XMLTypes.Token.ToString().ToLower())
                     where nodes.Attribute("name").Value == "LODX"
                     select nodes;

            foreach (var itemX in vX)
            {
                itemX.Value = LODX.ToString();
            }

            var vY = from nodes in item.Descendants(XMLTypes.Token.ToString().ToLower())
                     where nodes.Attribute("name").Value == "LODY"
                     select nodes;

            foreach (var itemY in vY)
            {
                itemY.Value = LODY.ToString();
            }

            var v5 = from nodes in item.Descendants(XMLTypes.Token.ToString().ToLower())
                     where nodes.Attribute("name").Value == "MeshType"
                     select nodes;

            foreach (var item5 in v5)
            {
                item5.Value = meshtype.ToString();
            }
        }
    }

    public static string GetHeadBevel(XDocument doc)
    {
        string bevelsettings = "";

        var v = from nodes in doc.Descendants("Item")
                select nodes;

        foreach (var item in v)
        {
            var v2 = from nodes in item.Descendants(XMLTypes.Float.ToString().ToLower())
                     where nodes.Attribute("name").Value == "Bevel"
                     select nodes;

            foreach (var item2 in v2)
            {
                bevelsettings += item2.Value + ",";
            }

            var v3 = from nodes in item.Descendants(XMLTypes.Float.ToString().ToLower())
                     where nodes.Attribute("name").Value == "Bevel Roundness"
                     select nodes;

            foreach (var item3 in v3)
            {
                bevelsettings += item3.Value + ",";
            }

            var v4 = from nodes in item.Descendants(XMLTypes.Float.ToString().ToLower())
                     where nodes.Attribute("name").Value == "Bulge"
                     select nodes;

            foreach (var item4 in v4)
            {
                bevelsettings += item4.Value + ",";
            }

            var vX = from nodes in item.Descendants(XMLTypes.Token.ToString().ToLower())
                     where nodes.Attribute("name").Value == "LODX"
                     select nodes;

            foreach (var itemX in vX)
            {
                bevelsettings += itemX.Value + ",";
            }

            var vY = from nodes in item.Descendants(XMLTypes.Token.ToString().ToLower())
                     where nodes.Attribute("name").Value == "LODY"
                     select nodes;

            foreach (var itemY in vY)
            {
                bevelsettings += itemY.Value + ",";
            }

            var v5 = from nodes in item.Descendants(XMLTypes.Token.ToString().ToLower())
                     where nodes.Attribute("name").Value == "MeshType"
                     select nodes;

            foreach (var item5 in v5)
            {
                bevelsettings += item5.Value;
            }
        }

        return bevelsettings;
    }

    public static bool IsHeadMesh(XDocument doc)
    {
        bool check = true;

        var v = from nodes in doc.Descendants("Item")
                where nodes.Attribute("class").Value == "SpecialMesh"
                select nodes;

        foreach (var item in v)
        {
            var v2 = from nodes in item.Descendants("Content")
                     where nodes.Attribute("name").Value == "MeshId"
                     select nodes;

            foreach (var item2 in v2)
            {
                var v3 = from nodes in item2.Descendants("url")
                         select nodes;

                foreach (var item3 in v3)
                {
                    if (!string.IsNullOrWhiteSpace(item3.Value))
                    {
                        check = false;
                    }
                }
            }
        }

        return check;
    }

    #endregion

    public static string GetPathForType(RobloxFileType type)
    {
        switch (type)
        {
            case RobloxFileType.Hat:
                return GlobalPaths.hatdir;
            case RobloxFileType.HeadNoCustomMesh:
            case RobloxFileType.Head:
                return GlobalPaths.headdir;
            case RobloxFileType.Face:
                return GlobalPaths.facedir;
            case RobloxFileType.TShirt:
                return GlobalPaths.tshirtdir;
            case RobloxFileType.Shirt:
                return GlobalPaths.shirtdir;
            case RobloxFileType.Pants:
                return GlobalPaths.pantsdir;
            default:
                return "";
        }
    }

    public static string[] GetOptionPathsForType(RobloxFileType type)
    {
        switch (type)
        {
            case RobloxFileType.Hat:
                return RobloxDefs.ItemHatFonts.Dir;
            case RobloxFileType.HeadNoCustomMesh:
            case RobloxFileType.Head:
                return RobloxDefs.ItemHeadFonts.Dir;
            case RobloxFileType.Face:
                return RobloxDefs.ItemFaceTexture.Dir;
            case RobloxFileType.TShirt:
                return RobloxDefs.ItemTShirtTexture.Dir;
            case RobloxFileType.Shirt:
                return RobloxDefs.ItemShirtTexture.Dir;
            case RobloxFileType.Pants:
                return RobloxDefs.ItemPantsTexture.Dir;
            default:
                return null;
        }
    }

    public static RobloxFileType GetTypeForInt(int type)
    {
        switch (type)
        {
            case 0:
                return RobloxFileType.Hat;
            case 1:
                return RobloxFileType.Head;
            case 2:
                return RobloxFileType.HeadNoCustomMesh;
            case 3:
                return RobloxFileType.Face;
            case 4:
                return RobloxFileType.TShirt;
            case 5:
                return RobloxFileType.Shirt;
            case 6:
                return RobloxFileType.Pants;
            default:
                return RobloxFileType.RBXM;
        }
    }

    public bool CreateItem(string filepath, RobloxFileType type, string itemname, string[] assetfilenames, Vector3 coordoptions, Vector3 coordoptions2, Vector3 coordoptions3, Vector3[] rotationoptions, double transparency, double reflectiveness, object[] headoptions, string desctext = "")
    {
        string oldfile = File.ReadAllText(filepath);
        string fixedfile = RobloxXML.RemoveInvalidXmlChars(RobloxXML.ReplaceHexadecimalSymbols(oldfile));
        XDocument doc = XDocument.Parse(fixedfile);
        string savDocPath = GetPathForType(type);
        bool success = true;

        try
        {
            switch (type)
            {
                case RobloxFileType.Hat:
                    SetItemFontVals(doc, RobloxDefs.ItemHatFonts, 0, 0, 0, assetfilenames[0], assetfilenames[2]);
                    if (!string.IsNullOrWhiteSpace(assetfilenames[3]))
                    {
                        SetItemFontVals(doc, RobloxDefs.ItemHatFonts, 1, 1, 1, assetfilenames[1], assetfilenames[3]);
                    }
                    else
                    {
                        SetItemFontValEmpty(doc, RobloxDefs.ItemHatFonts, 1);
                    }
                    SetItemCoordVals(doc, "Hat", coordoptions, "CoordinateFrame", "AttachmentPoint");
                    SetHatMeshVals(doc, coordoptions2, "Vector3", "Scale");
                    SetHatMeshVals(doc, coordoptions3, "Vector3", "VertexColor");
                    SetHatPartVals(doc, partColorID, transparency, reflectiveness);
                    SetItemRotationVals(doc, "Hat", rotationoptions[0], rotationoptions[1], rotationoptions[2], "CoordinateFrame", "AttachmentPoint");
                    break;
                case RobloxFileType.Head:
                    SetItemFontVals(doc, RobloxDefs.ItemHeadFonts, 0, 0, 0, assetfilenames[0], assetfilenames[2]);
                    if (!string.IsNullOrWhiteSpace(assetfilenames[3]))
                    {
                        SetItemFontVals(doc, RobloxDefs.ItemHeadFonts, 1, 1, 1, assetfilenames[1], assetfilenames[3]);
                    }
                    else
                    {
                        SetItemFontValEmpty(doc, RobloxDefs.ItemHeadFonts, 1);
                    }
                    SetItemCoordVals(doc, RobloxDefs.ItemHeadFonts, coordoptions, "Vector3", "Scale");
                    break;
                case RobloxFileType.Face:
                    SetItemFontVals(doc, RobloxDefs.ItemFaceTexture, 0, 0, 0, "", assetfilenames[2]);
                    break;
                case RobloxFileType.TShirt:
                    SetItemFontVals(doc, RobloxDefs.ItemTShirtTexture, 0, 0, 0, "", assetfilenames[2]);
                    break;
                case RobloxFileType.Shirt:
                    SetItemFontVals(doc, RobloxDefs.ItemShirtTexture, 0, 0, 0, assetfilenames[0], assetfilenames[2]);
                    savDocPath = GlobalPaths.shirtdir;
                    break;
                case RobloxFileType.Pants:
                    SetItemFontVals(doc, RobloxDefs.ItemPantsTexture, 0, 0, 0, assetfilenames[0], assetfilenames[2]);
                    break;
                case RobloxFileType.HeadNoCustomMesh:
                    SetHeadBevel(doc, Convert.ToDouble(headoptions[0]),
                        Convert.ToDouble(headoptions[1]),
                        Convert.ToDouble(headoptions[2]),
                        Convert.ToInt32(headoptions[3]),
                        headoptions[4].ToString(),
                        Convert.ToInt32(headoptions[5]),
                        Convert.ToInt32(headoptions[6]));
                    SetItemCoordValsNoClassSearch(doc, coordoptions, "Vector3", "Scale");
                    SetItemCoordValsNoClassSearch(doc, coordoptions2, "Vector3", "VertexColor");
                    break;
                default:
                    break;
            }
        }
        catch (Exception ex)
        {
            GlobalFuncs.LogExceptions(ex);
            MessageBox.Show("The Item Creation SDK has experienced an error: " + ex.Message, "Novetus Item Creation SDK - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            success = false;
        }
        finally
        {
            doc.Save(savDocPath + "\\" + itemname + ".rbxm");
            if (!string.IsNullOrWhiteSpace(desctext))
            {
                File.WriteAllText(savDocPath + "\\" + itemname + "_desc.txt", desctext);
            }
        }

        return success;
    }

    //https://stackoverflow.com/questions/6921105/given-a-filesystem-path-is-there-a-shorter-way-to-extract-the-filename-without
    static string GetFileBaseNameUsingSplit(string path)
    {
        string[] pathArr = path.Split('/');
        string[] fileArr = pathArr.Last().Split('.');
        string fileBaseName = fileArr.First().ToString();

        return fileBaseName;
    }

    public bool LoadItem(string filepath, RobloxFileType type)
    {
        Option1Path = "";
        Option2Path = "";

        string oldfile = File.ReadAllText(filepath);
        string fixedfile = RobloxXML.RemoveInvalidXmlChars(RobloxXML.ReplaceHexadecimalSymbols(oldfile));
        XDocument doc = XDocument.Parse(fixedfile);
        bool success = true;

        try
        {
            switch (type)
            {
                case RobloxFileType.Hat:
                    string MeshFilename = GetFileBaseNameUsingSplit(GetItemFontVals(doc, RobloxDefs.ItemHatFonts, 0)) + ".mesh";
                    string TextureFilename = GetFileBaseNameUsingSplit(GetItemFontVals(doc, RobloxDefs.ItemHatFonts, 1)) + ".png";

                    //https://stackoverflow.com/questions/10160708/how-do-i-find-an-item-by-value-in-an-combobox-in-c
                    int i = UsesHatMeshBox.FindStringExact(MeshFilename);
                    if (i >= 0)
                    {
                        UsesHatMeshBox.SelectedIndex = i;
                    }

                    int i2 = UsesHatTexBox.FindStringExact(TextureFilename);
                    if (i2 >= 0)
                    {
                        UsesHatTexBox.SelectedIndex = i2;
                    }

                    string HatCoords = GetItemCoordVals(doc, "Hat", "CoordinateFrame", "AttachmentPoint");

                    if (!string.IsNullOrWhiteSpace(HatCoords))
                    {
                        string[] HatCoordsSplit = HatCoords.Split(',');
                        XBox.Value = Convert.ToDecimal(HatCoordsSplit[0]);
                        YBox.Value = Convert.ToDecimal(HatCoordsSplit[1]);
                        ZBox.Value = Convert.ToDecimal(HatCoordsSplit[2]);
                    }

                    string HatScaleCoords = GetHatMeshVals(doc, "Vector3", "Scale");

                    if (!string.IsNullOrWhiteSpace(HatScaleCoords))
                    {
                        string[] HatScaleCoordsSplit = HatScaleCoords.Split(',');
                        XBox360.Value = Convert.ToDecimal(HatScaleCoordsSplit[0]);
                        YBox2.Value = Convert.ToDecimal(HatScaleCoordsSplit[1]);
                        ZBox2.Value = Convert.ToDecimal(HatScaleCoordsSplit[2]);
                    }

                    string HatColorCoords = GetHatMeshVals(doc, "Vector3", "VertexColor");

                    if (!string.IsNullOrWhiteSpace(HatColorCoords))
                    {
                        string[] HatColorCoordsSplit = HatColorCoords.Split(',');
                        XBoxOne.Value = Convert.ToDecimal(HatColorCoordsSplit[0]);
                        YBox3.Value = Convert.ToDecimal(HatColorCoordsSplit[1]);
                        ZBox3.Value = Convert.ToDecimal(HatColorCoordsSplit[2]);
                    }

                    string HatRotation = GetItemRotationVals(doc, "Hat", "CoordinateFrame", "AttachmentPoint");

                    if (!string.IsNullOrWhiteSpace(HatRotation))
                    {
                        string[] HatRotationSplit = HatRotation.Split(',');
                        rightXBox.Value = Convert.ToDecimal(HatRotationSplit[0]);
                        rightYBox.Value = Convert.ToDecimal(HatRotationSplit[1]);
                        rightZBox.Value = Convert.ToDecimal(HatRotationSplit[2]);
                        upXBox.Value = Convert.ToDecimal(HatRotationSplit[3]);
                        upYBox.Value = Convert.ToDecimal(HatRotationSplit[4]);
                        upZBox.Value = Convert.ToDecimal(HatRotationSplit[5]);
                        forwardXBox.Value = -Convert.ToDecimal(HatRotationSplit[6]);
                        forwardYBox.Value = -Convert.ToDecimal(HatRotationSplit[7]);
                        forwardZBox.Value = -Convert.ToDecimal(HatRotationSplit[8]);
                    }

                    string HatPartVals = GetHatPartVals(doc);

                    if (!string.IsNullOrWhiteSpace(HatPartVals))
                    {
                        string[] HatPartValsSplit = HatPartVals.Split(',');
                        partColorID = Convert.ToInt32(HatPartValsSplit[0]);
                        partColorLabel.Text = partColorID.ToString();
                        reflectivenessBox.Value = Convert.ToDecimal(HatPartValsSplit[1]);
                        transparencyBox.Value = Convert.ToDecimal(HatPartValsSplit[2]);
                    }

                    break;
                case RobloxFileType.Head:
                case RobloxFileType.HeadNoCustomMesh:
                    if (IsHeadMesh(doc))
                    {
                        string BevelCoords = GetHeadBevel(doc);
                        if (!string.IsNullOrWhiteSpace(BevelCoords))
                        {
                            string[] BevelCoordsSplit = BevelCoords.Split(',');

                            BevelBox.Value = Convert.ToDecimal(BevelCoordsSplit[0]);
                            RoundnessBox.Value = Convert.ToDecimal(BevelCoordsSplit[1]);
                            BulgeBox.Value = Convert.ToDecimal(BevelCoordsSplit[2]);
                            LODXBox.Value = Convert.ToDecimal(BevelCoordsSplit[3]);
                            LODYBox.Value = Convert.ToDecimal(BevelCoordsSplit[4]);

                            if (!string.IsNullOrWhiteSpace(BevelCoordsSplit[5]))
                            {
                                SpecialMeshTypeBox.SelectedIndex = Convert.ToInt32(BevelCoordsSplit[5]);
                            }
                        }

                        string HeadScaleCoords = GetItemCoordValsNoClassSearch(doc, "Vector3", "Scale");
                        if (!string.IsNullOrWhiteSpace(HeadScaleCoords))
                        {
                            string[] HeadScaleCoordsSplit = HeadScaleCoords.Split(',');
                            XBox.Value = Convert.ToDecimal(HeadScaleCoordsSplit[0]);
                            YBox.Value = Convert.ToDecimal(HeadScaleCoordsSplit[1]);
                            ZBox.Value = Convert.ToDecimal(HeadScaleCoordsSplit[2]);
                        }

                        string HeadColorCoords = GetItemCoordValsNoClassSearch(doc, "Vector3", "VertexColor");
                        if (!string.IsNullOrWhiteSpace(HeadColorCoords))
                        {
                            string[] HeadColorCoordsSplit = HeadColorCoords.Split(',');
                            XBox360.Value = Convert.ToDecimal(HeadColorCoordsSplit[0]);
                            YBox2.Value = Convert.ToDecimal(HeadColorCoordsSplit[1]);
                            ZBox2.Value = Convert.ToDecimal(HeadColorCoordsSplit[2]);
                        }

                        ItemTypeListBox.SelectedIndex = 2;
                    }
                    else
                    {
                        string HeadMeshFilename = GetFileBaseNameUsingSplit(GetItemFontVals(doc, RobloxDefs.ItemHeadFonts, 0)) + ".mesh";
                        string HeadTextureFilename = GetFileBaseNameUsingSplit(GetItemFontVals(doc, RobloxDefs.ItemHeadFonts, 1)) + ".png";

                        Option1TextBox.Text = HeadMeshFilename;
                        Option2TextBox.Text = HeadTextureFilename;

                        string HeadMeshScaleCoords = GetItemCoordVals(doc, RobloxDefs.ItemHeadFonts, "Vector3", "Scale");
                        if (!string.IsNullOrWhiteSpace(HeadMeshScaleCoords))
                        {
                            string[] HeadMeshScaleCoordsSplit = HeadMeshScaleCoords.Split(',');
                            XBox.Value = Convert.ToDecimal(HeadMeshScaleCoordsSplit[0]);
                            YBox.Value = Convert.ToDecimal(HeadMeshScaleCoordsSplit[1]);
                            ZBox.Value = Convert.ToDecimal(HeadMeshScaleCoordsSplit[2]);
                        }

                        string HeadMeshColorCoords = GetItemCoordVals(doc, RobloxDefs.ItemHeadFonts, "Vector3", "VertexColor");
                        if (!string.IsNullOrWhiteSpace(HeadMeshColorCoords))
                        {
                            string[] HeadMeshColorCoordsSplit = HeadMeshColorCoords.Split(',');
                            XBox360.Value = Convert.ToDecimal(HeadMeshColorCoordsSplit[0]);
                            YBox2.Value = Convert.ToDecimal(HeadMeshColorCoordsSplit[1]);
                            ZBox2.Value = Convert.ToDecimal(HeadMeshColorCoordsSplit[2]);
                        }

                        ItemTypeListBox.SelectedIndex = 1;
                    }
                    break;
                case RobloxFileType.Face:
                    string FaceTextureFilename = GetFileBaseNameUsingSplit(GetItemFontVals(doc, RobloxDefs.ItemFaceTexture, 0)) + ".png";
                    Option1TextBox.Text = FaceTextureFilename;
                    break;
                case RobloxFileType.TShirt:
                    string TShirtTextureFilename = GetFileBaseNameUsingSplit(GetItemFontVals(doc, RobloxDefs.ItemTShirtTexture, 0)) + ".png";
                    Option1TextBox.Text = TShirtTextureFilename;
                    break;
                case RobloxFileType.Shirt:
                    string ShirtTextureFilename = GetFileBaseNameUsingSplit(GetItemFontVals(doc, RobloxDefs.ItemShirtTexture, 0)) + ".png";
                    Option1TextBox.Text = ShirtTextureFilename;
                    break;
                case RobloxFileType.Pants:
                    string PantsTextureFilename = GetFileBaseNameUsingSplit(GetItemFontVals(doc, RobloxDefs.ItemPantsTexture, 0)) + ".png";
                    Option1TextBox.Text = PantsTextureFilename;
                    break;
                default:
                    break;
            }
        }
        catch (Exception ex)
        {
            GlobalFuncs.LogExceptions(ex);
            MessageBox.Show("The Item Creation SDK has experienced an error: " + ex.Message, "Novetus Item Creation SDK - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            success = false;
        }

        return success;
    }

    private void ToggleOptionSet(Label label, TextBox textbox, Button button, string labelText, bool browseButton, bool enable = true)
    {
        label.Text = enable ? labelText : (string.IsNullOrWhiteSpace(labelText) ? "This option is disabled." : labelText);
        textbox.ReadOnly = !enable;
        textbox.Text = "";
        button.Enabled = browseButton;
        ItemIcon.Image = GlobalFuncs.LoadImage("", "");
    }

    private void ToggleGroup(GroupBox groupbox, string labelText, bool enable = true)
    {
        groupbox.Text = enable ? labelText : (string.IsNullOrWhiteSpace(labelText) ? "This option is disabled." : labelText);
        groupbox.Enabled = enable;
        ItemIcon.Image = GlobalFuncs.LoadImage("", "");
    }

    private void ToggleHatMeshBox(string labelText, bool enable = true)
    {
        UsesHatMeshLabel.Text = enable ? labelText : (string.IsNullOrWhiteSpace(labelText) ? "This option is disabled." : labelText);
        UsesHatMeshBox.Enabled = enable;

        if (enable && Directory.Exists(GlobalPaths.hatdirFonts))
        {
            UsesHatMeshBox.Items.Add("None");
            DirectoryInfo dinfo = new DirectoryInfo(GlobalPaths.hatdirFonts);
            FileInfo[] Files = dinfo.GetFiles("*.mesh");
            foreach (FileInfo file in Files)
            {
                if (file.Name.Equals(string.Empty))
                {
                    continue;
                }

                UsesHatMeshBox.Items.Add(file.Name);
            }

            UsesHatMeshBox.SelectedItem = "None";
        }
        else
        {
            UsesHatMeshBox.Items.Clear();
        }
    }

    private void ToggleHatTextureBox(string labelText, bool enable = true)
    {
        UsesHatTexLabel.Text = enable ? labelText : (string.IsNullOrWhiteSpace(labelText) ? "This option is disabled." : labelText);
        UsesHatTexBox.Enabled = enable;

        if (enable && Directory.Exists(GlobalPaths.hatdirTextures))
        {
            UsesHatTexBox.Items.Add("None");
            DirectoryInfo dinfo = new DirectoryInfo(GlobalPaths.hatdirTextures);
            FileInfo[] Files = dinfo.GetFiles("*.png");
            foreach (FileInfo file in Files)
            {
                if (file.Name.Equals(string.Empty))
                {
                    continue;
                }

                UsesHatTexBox.Items.Add(file.Name);
            }

            UsesHatTexBox.SelectedItem = "None";
        }
        else
        {
            UsesHatTexBox.Items.Clear();
        }
    }

    private string LoadAsset(string assetName, string assetFilter)
    {
        openFileDialog1 = new OpenFileDialog()
        {
            FileName = "Select a " + assetName + " file",
            Filter = assetName + " (" + assetFilter + ")|" + assetFilter,
            Title = "Open " + assetName
        };

        if (openFileDialog1.ShowDialog() == DialogResult.OK)
        {
            return openFileDialog1.FileName;
        }
        else
        {
            return "";
        }
    }

    private bool CheckItemRequirements()
    {
        string msgboxtext = "Some issues have been found with your item:\n";
        bool passed = true;
        bool itemExists = false;

        if (string.IsNullOrWhiteSpace(ItemNameBox.Text))
        {
            msgboxtext += "\n - You must assign an item name.";
            passed = false;
        }
        else
        {
            if(File.Exists(GetPathForType(type) + "\\" + ItemNameBox.Text.Replace(" ", "") + ".rbxm") && !ItemEditing)
            {
                msgboxtext += "\n - The item already exists.";
                passed = false;
                itemExists = true;
            }
        }

        if (!itemExists)
        {
            if (ItemIcon.Image == null)
            {
                msgboxtext += "\n - You must assign an icon.";

                if (RequiresIconForTexture && ItemIcon.Image == null)
                {
                    msgboxtext += " This item type requires that you must select an Icon to use as a Template or Texture.";
                }

                passed = false;
            }

            if (Option1Required)
            {
                if (string.IsNullOrWhiteSpace(Option1TextBox.Text))
                {
                    msgboxtext += "\n - You must assign a " + Option1Label.Text + ".";
                    passed = false;
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(UsesHatTexBox.Text) || UsesHatMeshBox.Text == "None")
                    {
                        if (!File.Exists(Option1Path) && !File.Exists(GetOptionPathsForType(type)[0] + "\\" + Option1TextBox.Text) && !ItemEditing)
                        {
                            msgboxtext += "\n - The file assigned as a " + Option1Label.Text + " does not exist. Please rebrowse for the file again.";
                            passed = false;
                        }

                        if (File.Exists(GetOptionPathsForType(type)[0] + "\\" + Option1TextBox.Text) && !ItemEditing)
                        {
                            msgboxtext += "\n - The file assigned as a " + Option1Label.Text + " already exists. Please find an alternate file.";
                            passed = false;
                        }
                    }
                }
            }

            if (Option2Required)
            {
                if (string.IsNullOrWhiteSpace(Option2TextBox.Text))
                {
                    msgboxtext += "\n - You must assign a " + Option2Label.Text + ".";
                    passed = false;
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(UsesHatTexBox.Text) || UsesHatTexBox.Text == "None")
                    {
                        if (!File.Exists(Option2Path) && !File.Exists(GetOptionPathsForType(type)[1] + "\\" + Option2TextBox.Text) && !ItemEditing)
                        {
                            msgboxtext += "\n - The file assigned as a " + Option2Label.Text + " does not exist. Please rebrowse for the file again.";
                            passed = false;
                        }

                        if (File.Exists(GetOptionPathsForType(type)[1] + "\\" + Option2TextBox.Text) && !ItemEditing)
                        {
                            msgboxtext += "\n - The file assigned as a " + Option2Label.Text + " already exists. Please find an alternate file.";
                            passed = false;
                        }
                    }
                }
            }
        }

        if (!passed)
        {
            msgboxtext += "\n\nThese issues must be fixed before the item can be created.";
            MessageBox.Show(msgboxtext, "Novetus Item Creation SDK - Requirements", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        return passed;
    }

    private void LoadItemIfExists()
    {
        string iconpath = GetPathForType(type) + "\\" + ItemNameBox.Text.Replace(" ", "") + ".png";

        if (File.Exists(iconpath))
        {
            Image icon1 = GlobalFuncs.LoadImage(iconpath);
            ItemIcon.Image = icon1;
        }
        else
        {
            ItemIcon.Image = null;
        }

        string descpath = GetPathForType(type) + "\\" + ItemNameBox.Text.Replace(" ", "") + "_desc.txt";

        if (File.Exists(descpath))
        {
            DescBox.Text = File.ReadAllText(descpath);
        }
        else
        {
            DescBox.Text = "";
        }

        string rxbmpath = GetPathForType(type) + "\\" + ItemNameBox.Text.Replace(" ", "") + ".rbxm";

        if (File.Exists(rxbmpath))
        {
            LoadItem(rxbmpath, type);
        }
        else
        {
            if (!IsReskin)
            {
                Reset();
            }
        }
    }

    private void UpdateWarnings()
    {
        string warningtext = "";

        if (File.Exists(GetPathForType(type) + "\\" + ItemNameBox.Text.Replace(" ", "") + ".rbxm"))
        {
            warningtext += "Warning: This item already exists.";
            if (ItemEditing)
            {
                warningtext += " The item's settings will be overridden since Item Editing is enabled.";
            }
            else
            {
                warningtext += " Your item will not be created with this name unless Item Editing is enabled.";
            }
        }

        Warning.Text = warningtext;
    }

    private void DeleteStrayIcons()
    {
        string itempath = GetPathForType(type) + "\\" + ItemNameBox.Text.Replace(" ", "");
        string previconpath = itempath + ".png";
        string rbxmpath = itempath + ".rbxm";

        if (File.Exists(previconpath) && !File.Exists(rbxmpath))
        {
            GlobalFuncs.FixedFileDelete(previconpath);
        }
    }

    private void Reset(bool full = false)
    {
        if (full)
        {
            ItemNameBox.Text = "";
            DescBox.Text = "";
            ItemTypeListBox.SelectedItem = "Hat";
            ItemIcon.Image = null;
            Option1Path = "";
            Option2Path = "";
            EditItemBox.Checked = false;
            ReskinBox.Checked = false;
            UpdateWarnings();
        }

        UsesHatMeshBox.SelectedItem = "None";
        UsesHatTexBox.SelectedItem = "None";
        SpecialMeshTypeBox.SelectedItem = "Head";
        Option1TextBox.Text = "";
        Option2TextBox.Text = "";
        XBox.Value = 1;
        YBox.Value = 1;
        ZBox.Value = 1;
        XBox360.Value = 1;
        YBox2.Value = 1;
        ZBox2.Value = 1;
        XBoxOne.Value = 1;
        YBox3.Value = 1;
        ZBox3.Value = 1;
        BevelBox.Value = 0M;
        RoundnessBox.Value = 0M;
        BulgeBox.Value = 0M;
        LODXBox.Value = 1M;
        LODYBox.Value = 2M;
        rightXBox.Value = 1;
        rightYBox.Value = 0;
        rightZBox.Value = 0;
        upXBox.Value = 0;
        upYBox.Value = 1;
        upZBox.Value = 0;
        forwardXBox.Value = 0;
        forwardYBox.Value = 0;
        forwardZBox.Value = -1;
        transparencyBox.Value = 0;
        reflectivenessBox.Value = 0;
        partColorID = 194;
        partColorLabel.Text = partColorID.ToString();
    }
    #endregion
}
#endregion

