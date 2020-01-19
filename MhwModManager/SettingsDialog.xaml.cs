﻿using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using Microsoft.Win32;
using System.Windows;
using System;

namespace MhwModManager
{
    public class Setting
    {
        public Settings settings = new Settings();

        public static string FindMHWInstallFolder()
        {
            var steamRoot = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Valve\Steam", "InstallPath", null) as string;
            var libFolders = new List<string>();
            libFolders.Add(Path.Combine(steamRoot));
            dynamic libraryfolders = Gameloop.Vdf.VdfConvert.Deserialize(File.ReadAllText(Path.Combine(steamRoot, "steamapps", "libraryfolders.vdf")));
            try
            {
                for (int i = 1; ; i++)
                    libFolders.Add(libraryfolders.Value[i.ToString()].ToString());
            }
            catch (Exception) { }
            const string appid = "582010";
            foreach (var folder in libFolders)
                foreach (var file in new DirectoryInfo(Path.Combine(folder, "steamapps")).GetFiles())
                    if (file.Extension == ".acf")
                        using (var text = file.OpenText())
                        {
                            dynamic content = Gameloop.Vdf.VdfConvert.Deserialize(text.ReadToEnd());
                            if (content.Value.appid.ToString() == appid)
                                return Path.Combine(file.DirectoryName, "common", content.Value.installdir.ToString());
                        }
            return null;
        }

        public void GenConfig()
        {
            if (!File.Exists(App.SettingsPath))
            {
                if (!Directory.Exists(App.AppData))
                    Directory.CreateDirectory(App.AppData);

                settings.dark_mode = false;
                settings.mhw_path = FindMHWInstallFolder();
                settings.mod_installed = new List<bool>();
                ParseSettingsJSON();
            }
            else
            {
                Setting sets;
                using (StreamReader file = new StreamReader(App.SettingsPath))
                {
                    sets = JsonConvert.DeserializeObject<Setting>(file.ReadToEnd());
                    file.Close();
                }

                settings.dark_mode = sets.settings.dark_mode;
                if (!Directory.Exists(sets.settings.mhw_path))
                    sets.settings.mhw_path = FindMHWInstallFolder();
                settings.mhw_path = sets.settings.mhw_path;
                settings.mod_installed = sets.settings.mod_installed;
            }
        }

        public void ParseSettingsJSON()
        {
            using (StreamWriter file = new StreamWriter(App.SettingsPath))
            {
                file.Write(JsonConvert.SerializeObject(this, Formatting.Indented));
                file.Close();
            }
        }

        public struct Settings
        {
            public bool dark_mode;
            public string mhw_path;
            public List<bool> mod_installed;
        }
    }

    /// <summary>
    /// Logique d'interaction pour SettingsDialog.xaml
    /// </summary>
    public partial class SettingsDialog : Window
    {
        public SettingsDialog()
        {
            InitializeComponent();
        }
    }
}