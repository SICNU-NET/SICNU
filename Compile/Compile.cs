using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace Compile
{
    internal class Compile
    {
        private static void Main(string[] args)
        {
            string currentPath = Directory.GetCurrentDirectory();
            //Get the AppConfig settings
            string nuget = ConfigurationManager.AppSettings["nuget.exe"];
            string msbuildPath = ConfigurationManager.AppSettings["msbuildPath"];
            string compileTxt = ConfigurationManager.AppSettings["compileTxt"];
            string loggerClass = ConfigurationManager.AppSettings["loggerClass"];
            //string loggerPath = currentPath + "\\" + ConfigurationManager.AppSettings["loggerName"];
            string loggerPath = ConfigurationManager.AppSettings["loggerName"];
            string nugetPath = currentPath + "\\" + nuget;
            //Switch work path
            string diskDriveCMD = currentPath.Substring(0, 2) + " & cd " + currentPath;

            string compileTxtPath = currentPath + "\\" + compileTxt;
            //Setting up the log generation directory
            string dateTime = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
            string logDirctoryPath = currentPath + "\\Log\\" + dateTime;
            if (!File.Exists(nugetPath))
            {
                ShowMessage(nuget);
                return;
            }
            if (!File.Exists(msbuildPath))
            {
                ShowMessage(msbuildPath);
                return;
            }
            if (!File.Exists(compileTxtPath))
            {
                ShowMessage(compileTxt);
                return;
            }
            if (!File.Exists(loggerPath))
            {
                ShowMessage(loggerPath);
                return;
            }
            if (!Directory.Exists(logDirctoryPath))
            {
                Directory.CreateDirectory(logDirctoryPath);
            }
            //Call CMD
            Process compile = new Process();
            compile.StartInfo.FileName = "cmd.exe";
            compile.StartInfo.UseShellExecute = false;
            compile.StartInfo.RedirectStandardInput = true;
            compile.StartInfo.RedirectStandardOutput = true;
            compile.StartInfo.RedirectStandardError = true;
            compile.StartInfo.CreateNoWindow = true;
            compile.Start();
            compile.StandardInput.WriteLine(diskDriveCMD);
            string[] projectNames = File.ReadAllLines(compileTxtPath);
            //
            foreach (var item in projectNames)
            {
                var files = Directory.GetFiles(currentPath + "\\Project\\" + item, "*.sln");
                if (files.Length == 0)
                    continue;
                //Update command for nuget
                string updataCmd = nuget + " update " + files[0];
                //Nuget's recovery command
                string restoreCmd = nuget + " restore " + files[0];
                //Compile command of nuget
                string msbuildCmd = msbuildPath + " " + files[0] + " /t:Rebuild /p:Configuration=Debug /nologo /noconsolelogger /logger:" +
                    loggerClass + "," + loggerPath + ";" + logDirctoryPath + "\\" + item + ".xml";
                //Pass the command to the CMD
                compile.StandardInput.WriteLine(updataCmd);
                compile.StandardInput.WriteLine(restoreCmd);
                compile.StandardInput.WriteLine(msbuildCmd);
            }
            compile.StandardInput.AutoFlush = true;
            //The last command passes the exit command.Otherwise the last call to the ReadToEnd method will be suspended
            compile.StandardInput.WriteLine("exit");
            string str = compile.StandardOutput.ReadToEnd();//Why does not call this method at last will die
            compile.WaitForExit();
            compile.Close();
        }

        private static void ShowMessage(string messageStr)
        {
            MessageBox.Show(messageStr + " does not exist！" + "Please check the configuration file!");
        }
    }
}