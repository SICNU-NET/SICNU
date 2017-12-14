using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace DownLoadProject
{
    internal class DownLoadProject
    {
        private static void Main(string[] args)
        {
            //Get the AppConfig settings
            string downSouceTxt = ConfigurationManager.AppSettings["downSouceTxt"];
            string currentPath = Directory.GetCurrentDirectory();
            //Switch work path command
            string diskDrive = currentPath.Substring(0, 2) + " & cd " + currentPath + "\\Project";

            string filePath = string.Format(currentPath + "\\" + downSouceTxt);
            string baseUrl = "https://github.com/ThinkGeo";
            //Because only a command is needed.So run a CMD command directly.  git clone url
            //test4444
            string cmd = "for /f %i in (" + filePath + ") do git clone " + baseUrl + "/" + "%i";
            if (!File.Exists(filePath))
            {
                MessageBox.Show(downSouceTxt + " does not exist！" + "Please check the configuration file!");
                return;
            }
            string projectPath = currentPath + "\\Project";
            if (!Directory.Exists(projectPath))
            {
                Directory.CreateDirectory(projectPath);
            }
            //call cmd
            Process downProjectCMD = new Process();
            downProjectCMD.StartInfo.FileName = "cmd.exe";
            downProjectCMD.StartInfo.UseShellExecute = false;
            downProjectCMD.StartInfo.RedirectStandardInput = true;
            downProjectCMD.StartInfo.RedirectStandardOutput = true;
            downProjectCMD.StartInfo.RedirectStandardError = true;
            downProjectCMD.StartInfo.CreateNoWindow = true;
            downProjectCMD.Start();
            //cd work path
            downProjectCMD.StandardInput.WriteLine(diskDrive);
            //call git cmd
            downProjectCMD.StandardInput.WriteLine(cmd);
            //exit
            downProjectCMD.StandardInput.WriteLine("&exit");
            downProjectCMD.StandardInput.AutoFlush = true;

            downProjectCMD.WaitForExit();
            downProjectCMD.Close();
        }
    }
}