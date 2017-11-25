namespace Cribo.Objects
{
    using Cribo.Enumerations;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class RecentFile : INotifyPropertyChanged
    {
        public RecentFileType Type { get; set; }

        public string FileName { get; set; }

        public string FileDirectory { get; set; }

        public string FtpHostName { get; set; }

        public string FtpPassword { get; set; }

        public string FtpUserName { get; set; }

        public int FtpPort { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void FirePropertyChanged(string property = "")
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        public string GetFilePath()
        {
            return Path.Combine(this.FileDirectory, this.FileName);
        }

        public string GetDownloadFilePath()
        {
            var downloaddirectorypath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DownloadedFiles");
            if (!Directory.Exists(downloaddirectorypath))
            {
                Directory.CreateDirectory(downloaddirectorypath);
            }

            var downloadfilepath = Path.Combine(downloaddirectorypath, this.FileName);
            return downloadfilepath;
        }

        public string GetFtpUrlFilePath()
        {
            return string.Format("ftp://{0}/{1}", this.FtpHostName, Path.Combine(this.FileDirectory, this.FileName));
        }
    }
}
