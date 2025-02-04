﻿#region Usings
using System.IO;
using System.Reflection;
#endregion

#region Global Paths

public class GlobalPaths
{
    #region Base Game Paths
    public static readonly string RootPathLauncher = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
    public static readonly string BasePathLauncher = RootPathLauncher.Replace(@"\", @"\\");
    public static readonly string RootPath = Directory.GetParent(RootPathLauncher).ToString();
    public static readonly string BasePath = RootPath.Replace(@"\", @"\\");
    public static readonly string DataPath = BasePath + @"\\shareddata";
    public static readonly string BinDir = BasePath + @"\\bin";
    public static readonly string ConfigDir = BasePath + @"\\config";
    public static readonly string ConfigDirClients = ConfigDir + @"\\clients";
    public static readonly string ConfigDirTemplates = ConfigDir + @"\\itemtemplates";
    public static readonly string DataDir = BinDir + @"\\data";
    public static readonly string ClientDir = BasePath + @"\\clients";
    public static readonly string MapsDir = BasePath + @"\\maps";
    public static readonly string MapsDirCustom = MapsDir + @"\\Custom";
    public static readonly string MapsDirBase = "maps";
    public static readonly string BaseGameDir = "rbxasset://../../../";
    public static readonly string AltBaseGameDir = "rbxasset://";
    public static readonly string SharedDataGameDir = BaseGameDir + "shareddata/";
    #endregion

    #region Customization Paths
    public static readonly string CustomPlayerDir = DataPath + "\\charcustom";
    public static readonly string hatdir = CustomPlayerDir + "\\hats";
    public static readonly string facedir = CustomPlayerDir + "\\faces";
    public static readonly string headdir = CustomPlayerDir + "\\heads";
    public static readonly string tshirtdir = CustomPlayerDir + "\\tshirts";
    public static readonly string shirtdir = CustomPlayerDir + "\\shirts";
    public static readonly string pantsdir = CustomPlayerDir + "\\pants";
    public static readonly string extradir = CustomPlayerDir + "\\custom";
    public static readonly string extradirIcons = extradir + "\\icons";

    public static readonly string CharCustomGameDir = SharedDataGameDir + "charcustom/";
    public static readonly string hatGameDir = CharCustomGameDir + "hats/";
    public static readonly string faceGameDir = CharCustomGameDir + "faces/";
    public static readonly string headGameDir = CharCustomGameDir + "heads/";
    public static readonly string tshirtGameDir = CharCustomGameDir + "tshirts/";
    public static readonly string shirtGameDir = CharCustomGameDir + "shirts/";
    public static readonly string pantsGameDir = CharCustomGameDir + "pants/";
    public static readonly string extraGameDir = CharCustomGameDir + "custom/";
    #endregion

    #region Asset Cache Paths

    #region Base Paths
    public static readonly string DirFonts = "\\fonts";
    public static readonly string DirSounds = "\\sounds";
    public static readonly string DirTextures = "\\textures";
    public static readonly string DirScripts = "\\scripts";
    public static readonly string FontsGameDir = "fonts/";
    public static readonly string SoundsGameDir = "sounds/";
    public static readonly string TexturesGameDir = "textures/";
    public static readonly string ScriptsGameDir = "scripts/";
    #endregion

    #region Asset Dirs
    public static string AssetCacheDir = DataPath + "\\assetcache";
    public static string AssetCacheDirSky = AssetCacheDir + "\\sky";
    public static string AssetCacheDirFonts = AssetCacheDir + DirFonts;
    public static string AssetCacheDirSounds = AssetCacheDir + DirSounds;
    public static string AssetCacheDirTextures = AssetCacheDir + DirTextures;
    public static string AssetCacheDirTexturesGUI = AssetCacheDirTextures + "\\gui";
    public static string AssetCacheDirScripts = AssetCacheDir + DirScripts;
    //public static string AssetCacheDirScriptAssets = AssetCacheDir + "\\scriptassets";

    public static string AssetCacheGameDir = SharedDataGameDir + "assetcache/";
    public static string AssetCacheFontsGameDir = AssetCacheGameDir + FontsGameDir;
    public static string AssetCacheSkyGameDir = AssetCacheGameDir + "sky/";
    public static string AssetCacheSoundsGameDir = AssetCacheGameDir + SoundsGameDir;
    public static string AssetCacheTexturesGameDir = AssetCacheGameDir + TexturesGameDir;
    public static string AssetCacheTexturesGUIGameDir = AssetCacheTexturesGameDir + "gui/";
    public static string AssetCacheScriptsGameDir = AssetCacheGameDir + ScriptsGameDir;
    //public static string AssetCacheScriptAssetsGameDir = AssetCacheGameDir + "scriptassets/";
    #endregion

    #region Item Dirs
    public static readonly string hatdirFonts = hatdir + DirFonts;
    public static readonly string hatdirTextures = hatdir + DirTextures;
    public static readonly string hatdirSounds = hatdir + DirSounds;
    public static readonly string hatdirScripts = hatdir + DirScripts;
    public static readonly string facedirTextures = facedir; //+ DirTextures;
    public static readonly string headdirFonts = headdir + DirFonts;
    public static readonly string headdirTextures = headdir + DirTextures;
    public static readonly string tshirtdirTextures = tshirtdir; //+ DirTextures;
    public static readonly string shirtdirTextures = shirtdir + DirTextures;
    public static readonly string pantsdirTextures = pantsdir + DirTextures;

    public static readonly string hatGameDirFonts = hatGameDir + FontsGameDir;
    public static readonly string hatGameDirTextures = hatGameDir + TexturesGameDir;
    public static readonly string hatGameDirSounds = hatGameDir + SoundsGameDir;
    public static readonly string hatGameDirScripts = hatGameDir + ScriptsGameDir;
    public static readonly string faceGameDirTextures = faceGameDir; //+ TexturesGameDir;
    public static readonly string headGameDirFonts = headGameDir + FontsGameDir;
    public static readonly string headGameDirTextures = headGameDir + TexturesGameDir;
    public static readonly string tshirtGameDirTextures = tshirtGameDir; //+ TexturesGameDir;
    public static readonly string shirtGameDirTextures = shirtGameDir + TexturesGameDir;
    public static readonly string pantsGameDirTextures = pantsGameDir + TexturesGameDir;
    #endregion

    #endregion

    #region File Names
    public static readonly string ConfigName = "config.ini";
    public static string ConfigNameCustomization = "config_customization.ini";
    public static readonly string InfoName = "info.ini";
    public static readonly string ScriptName = "CSMPFunctions";
    public static readonly string ScriptGenName = "CSMPBoot";
    public static readonly string ContentProviderXMLName = "ContentProviders.xml";
    public static readonly string PartColorXMLName = "PartColors.xml";
    public static readonly string FileDeleteFilterName = "FileDeleteFilter.txt";
    #endregion

    #region Empty Paths (automatically changed)
    public static string AddonScriptPath = "";
    #endregion
}
#endregion
