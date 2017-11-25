namespace Cribo.ViewModels
{
    using Microsoft.Win32;
    using Cribo.Commands;
    using Cribo.Enumerations;
    using Cribo.Objects;
    using Cribo.Views;
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.IO;
    using System.Reflection;
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Linq;

    public class MainViewModel : INotifyPropertyChanged
    {
        private byte[] buffer;
        private byte[] salt = UnicodeEncoding.UTF8.GetBytes("a");

        public MainViewModel()
        {
            this.State = State.Idle;
            this.IsUnsaved = false;

            this.CommandOpenRecent = new CommandWithDelegates((n) =>
            {
                var recentfiles = Settings.Default.GetRecentFiles();
                var file = recentfiles[Convert.ToInt32(n) - 1];
                this.OpenImage(file);
            }, () => true);

            this.InitializeRecentFiles();
            this.CommandOpen = new CommandWithDelegates(() =>
            {
                RecentFile file = new RecentFile();
                FileSelectorView view = new FileSelectorView();
                view.DataContext = file;
                var result = view.ShowDialog();

                if (result.HasValue && result.Value)
                {
                    var recentfiles = Settings.Default.GetRecentFiles();
                    recentfiles.Insert(0, file);
                    Settings.Default.SetRecentFiles(recentfiles);
                    Settings.Default.Save();
                    this.InitializeRecentFiles();

                    this.OpenImage(file);
                }
            }, () => true);

            this.CommandSave = new CommandWithDelegates(() => this.SaveImage(this.CurrentFile), () => this.State == State.Encrypted && this.IsUnsaved);

            this.CommandDecrypt = new CommandWithDelegates(this.Decrypt, () => this.State == State.Encrypted);
            this.CommandEncrypt = new CommandWithDelegates(this.Encrypt, () => this.State == State.Decrypted);
            this.CommandExit = new CommandWithDelegates(this.Exit, () => true);
        }

        private void InitializeRecentFiles()
        {
            int number = 1;
            var menuitems = Settings.Default.GetRecentFiles()
                .Select(n =>
                    {
                        try
                        {
                            return new MenuItem()
                                {
                                    Icon = new TextBlock() { Text = number.ToString() },
                                    Header = string.Format("({0}) {1}", n.Type, Path.Combine(n.FileDirectory, n.FileName)),
                                    Command = this.CommandOpenRecent,
                                    CommandParameter = number++,

                                };
                        }
                        catch { return null; }
                    })
                .Where(n => n!= null)
                .ToList();

            this.RecentFiles = new ObservableCollection<MenuItem>(menuitems);
            this.FirePropertyChangedEvent("RecentFiles");
        }

        public bool IsUnsaved { get; set; }

        public ObservableCollection<MenuItem> RecentFiles { get; set; }

        public string Data { get; set; }

        public string SelectedImagePath { get; set; }

        public RecentFile CurrentFile { get; set; }

        public State State { get; set; }

        public ICommand CommandOpen { get; set; }

        public ICommand CommandOpenRecent { get; set; }

        public ICommand CommandSave { get; set; }

        public ICommand CommandEncrypt { get; set; }

        public ICommand CommandDecrypt { get; set; }

        public ICommand CommandExit { get; set; }

        public ImageSource SelectedImage { get; set; }

        public string TitleText
        {
            get
            {
                var version = Assembly.GetExecutingAssembly().GetName().Version;
                return string.Format(
                    "Cribo vs. {0}.{1}{2}",
                    version.Major,
                    version.Minor,
                    string.IsNullOrEmpty(this.SelectedImagePath) ? string.Empty : " -> " + this.SelectedImagePath);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Fires the PropertyChanged event if anyone is listening.
        /// It can be used to update bindings on a view. 
        /// </summary>
        /// <param name="propertyname">if null then all bindings will be updated</param>
        public void FirePropertyChangedEvent(string propertyname)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyname));
            }
        }

        private void OpenImage(RecentFile recentfile)
        {
            if (recentfile.Type == RecentFileType.Local)
            {
                this.SelectedImagePath = recentfile.GetFilePath();
                this.OpenImageLocal(this.SelectedImagePath);
            }

            if (recentfile.Type == RecentFileType.Ftp)
            {
                this.SelectedImagePath = recentfile.GetFtpUrlFilePath();
                this.OpenImageFtp(recentfile);
            }

            this.CurrentFile = recentfile;
        }

        private void OpenImageLocal(string imagepath)
        {
            byte[] data = ImageStream.GetImageBytes(imagepath);
            this.buffer = CriboStream.GetCriboBytes(data);

            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.UriSource = new Uri(imagepath);
            image.EndInit();
            this.SelectedImage = image;

            this.State = State.Encrypted;
            this.UpdateText();

        }

        private void OpenImageFtp(RecentFile imagepath)
        {
            var downloadfilepath = imagepath.GetDownloadFilePath();
            var test = Path.Combine(imagepath.FtpHostName, imagepath.FileDirectory, imagepath.FileName);
            var url = imagepath.GetFtpUrlFilePath();

            Ftp.DownloadFile(
                imagepath.FtpUserName,
                imagepath.FtpPassword,
                url,
                downloadfilepath);

            this.OpenImageLocal(downloadfilepath);
        }

        private void SaveImage(RecentFile recentfile)
        {
            if (recentfile.Type == RecentFileType.Local)
            {
                this.SaveImageLocal(recentfile.GetFilePath());
            }

            if (recentfile.Type == RecentFileType.Ftp)
            {
                this.SaveImageFtp(recentfile);
            }

            this.IsUnsaved = false;
            this.FirePropertyChangedEvent(null);
        }

        private void SaveImageFtp(RecentFile recentfile)
        {
            this.SaveImageLocal(recentfile.GetDownloadFilePath());
            Ftp.UploadFile(recentfile.FtpUserName,
                recentfile.FtpPassword,
                recentfile.GetFtpUrlFilePath(),
                recentfile.GetDownloadFilePath());
        }

        private void SaveImageLocal(string imagefilepath)
        {
            if (this.State != State.Encrypted)
            {
                MessageBox.Show("You cannot save the data if it is in plaintext mode");
                return;
            }

            byte[] data = ImageStream.GetImageBytes(imagefilepath);
            CriboStream.SetCriboBytes(data, this.buffer);
            ImageStream.SetImageBytes(imagefilepath, data);
        }

        private void Encrypt()
        {
            if (this.State == State.Encrypted)
            {
                MessageBox.Show("Data is already encrypted");
                return;
            }

            string password = PasswordView.GetPassword();
            if (password == null)
            {
                return;
            }

            this.buffer = UnicodeEncoding.UTF8.GetBytes(this.Data);
            this.buffer = Crypto.Encrypt(this.buffer, this.GetHash(password));
            this.State = State.Encrypted;
            this.UpdateText();
        }

        private void Decrypt()
        {
            if (this.State == State.Decrypted)
            {
                MessageBox.Show("Data is not encrypted");
                return;
            }

            string password = PasswordView.GetPassword();
            if (password == null)
            {
                return;
            }

            this.buffer = Crypto.Decrypt(this.buffer, this.GetHash(password));
            this.State = State.Decrypted;
            this.UpdateText();
        }

        private void Exit()
        {
            App.Current.Shutdown();
        }

        private byte[] GetHash(string password)
        {
            byte[] hash = SimpleHash.Create(password, this.salt);
            return hash;
        }

        private void UpdateText()
        {
            this.Data = UnicodeEncoding.UTF8.GetString(this.buffer);
            this.IsUnsaved = true;
            this.FirePropertyChangedEvent(null);
        }
    }
}
