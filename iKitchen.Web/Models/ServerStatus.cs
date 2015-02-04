using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace iKitchen.Web.Models
{
    public class ServerStatus
    {
        private static ServerStatus instance = null;
        public static ServerStatus Instance
        {
            get
            {
                if (instance == null)
                {
                    // load server status
                    instance = new ServerStatus();
                    instance.MachineName = GetMachineName();
                    instance.OS = GetOS();
                    instance.IIS = GetIIS();
                    LoadDiskSpace(instance);
                    instance.Architecture = GetArchitecture();
                    // load 


                }
                return instance;
            }
        }

        private static string GetArchitecture()
        {
            return Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE");
        }

        private static void LoadDiskSpace(ServerStatus instance)
        {
            try
            {
                var directory = new DirectoryInfo(HttpContext.Current.Server.MapPath("~"));
                var drive = directory.Root.Name;
                var driveInfo = new DriveInfo(drive);
                instance.DiskSpace = driveInfo.TotalSize / (1024 * 1024 * 1024);
                instance.FreeDiskSpace = driveInfo.TotalFreeSpace / (1024 * 1024 * 1024);
            }
            catch (Exception)
            {
                // ignore
            }
        }

        private static string GetIIS()
        {
            return HttpContext.Current.Request.ServerVariables["SERVER_SOFTWARE"];
        }

        private static string GetOS()
        {
            try
            {
                return Environment.OSVersion.ToString();
            }
            catch
            {
                return "未知";
            }
        }

        private static string GetMachineName()
        {
            try
            {
                return HttpContext.Current.Server.MachineName;
            }
            catch
            {
                return "未知";
            }
        }

        private ServerStatus()
        {

        }


        public string MachineName { get; set; }

        public string OS { get; set; }

        public string IIS { get; set; }

        public decimal DiskSpace { get; set; }

        public decimal FreeDiskSpace { get; set; }

        public string Architecture { get; set; }
    }

}