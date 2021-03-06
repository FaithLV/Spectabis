﻿using System;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace Spectabis_WPF.Domain
{
    class LaunchPCSX2
    {
        public static Process CreateGameProcess(string game, bool launchAndTerminate = false)
        {
            string BaseDirectory = App.BaseDirectory;
            string gamePath = $"{BaseDirectory}resources\\configs\\{game}";
            
            var _gameIni = new IniFile(gamePath + @"\spectabis.ini");
            var _isoDir = _gameIni.Read("isoDirectory", "Spectabis");

            //Launch arguments
            var _nogui = _gameIni.Read("nogui", "Spectabis");
            var _fullscreen = _gameIni.Read("fullscreen", "Spectabis");
            var _fullboot = _gameIni.Read("fullboot", "Spectabis");
            var _nohacks = _gameIni.Read("nohacks", "Spectabis");

            string _launchargs = "";

            if (_nogui == "1") { _launchargs = "--nogui "; }
            if (_fullscreen == "1") { _launchargs = _launchargs + "--fullscreen "; }
            if (_fullboot == "1") { _launchargs = _launchargs + "--fullboot "; }
            if (_nohacks == "1") { _launchargs = _launchargs + "--nohacks "; }

            Console.WriteLine($"{_launchargs} {_isoDir} --cfgpath={gamePath}");

            Process PCSX = new Process();

            //PCSX2 Process
            if(File.Exists(Properties.Settings.Default.emuDir))
            {
                var argument = $"{_launchargs} \"{_isoDir}\" --cfgpath=\"{gamePath}\"";

                if(_isoDir.EndsWith(".ELF") || _isoDir.EndsWith(".elf"))
                {
                    argument = $"--elf=\"{_isoDir}\" --cfgpath=\"{gamePath}\"";
                }

                PCSX.StartInfo.FileName = Properties.Settings.Default.emuDir;
                PCSX.StartInfo.Arguments = argument;

                if(launchAndTerminate)
                {
                    PCSX.Start();
                    Application.Current.Shutdown();
                }

            }
            else
            {
                Console.WriteLine(Properties.Settings.Default.emuDir + " does not exist!");
            }

            return PCSX;
        }

        public static Process CreateFirstTimeWizard()
        {
            var process = new Process();
            process.StartInfo.FileName = Properties.Settings.Default.emuDir;

            string cfgPath = $"{App.BaseDirectory}resources\\default_config";
            process.StartInfo.Arguments = $"--forcewiz --nogui --cfgpath=\"{cfgPath}\"";

            return process;
        }
    }
}
