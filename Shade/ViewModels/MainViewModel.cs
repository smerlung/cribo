﻿using Microsoft.Win32;
using Shade.Commands;
using Shade.Enumerations;
using Shade.Objects;
using Shade.Views;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Shade.ViewModels
{
    public class MainViewModel
    {
        private byte[] buffer;
        private byte[] salt = UnicodeEncoding.UTF8.GetBytes("a");

        public MainViewModel()
        {
            this.State = State.Idle;
            this.IsUnsaved = false;

            this.CommandOpen = new CommandWithDelegates(() => 
            {
                OpenFileDialog dlg = new OpenFileDialog();
                bool? result = dlg.ShowDialog();
                if (result.HasValue && result.Value)
                {
                    this.OpenImage(dlg.FileName);
                }
            }, () => true);

            this.CommandSave = new CommandWithDelegates(() =>
            {
                OpenFileDialog dlg = new OpenFileDialog();
                bool? result = dlg.ShowDialog();
                if (result.HasValue && result.Value)
                {
                    this.SaveImage(dlg.FileName);
                }
            }, () => this.State == State.Encrypted && this.IsUnsaved);

            this.CommandDecrypt = new CommandWithDelegates(this.Decrypt, () => this.State == State.Encrypted);
            this.CommandEncrypt = new CommandWithDelegates(this.Encrypt, () => this.State == State.Decrypted);
            this.CommandExit = new CommandWithDelegates(this.Exit, () => true);
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

        public bool IsUnsaved { get; set; }

        public string Data { get; set; }

        public string SelectedImagePath { get; set; }

        public State State { get; set; }

        public ICommand CommandOpen { get; set; }

        public ICommand CommandSave { get; set; }

        public ICommand CommandEncrypt { get; set; }

        public ICommand CommandDecrypt { get; set; }

        public ICommand CommandExit { get; set; }

        public BitmapImage SelectedImage { get; set; }

        private void OpenImage(string imagepath)
        {
            this.SelectedImagePath = imagepath;
            using (FileStream fs = new FileStream(imagepath, FileMode.Open))
            {
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = fs;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.EndInit();
                this.SelectedImage = image;
            }

            byte[] data = ImageStream.GetImageBytes(imagepath);
            this.buffer = ShadeStream.GetShadeBytes(data);
            this.State = State.Encrypted;
            this.UpdateText();
        }

        private void SaveImage(string imagepath)
        {
            if (this.State != State.Encrypted)
            {
                MessageBox.Show("You cannot save the data if it is in plaintext mode");
                return;
            }

            byte[] data = ImageStream.GetImageBytes(imagepath);
            ShadeStream.SetShadeBytes(data, this.buffer);
            ImageStream.SetImageBytes(imagepath, data);
            this.IsUnsaved = false;
            this.FirePropertyChangedEvent(null);
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
            if(password == null)
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
