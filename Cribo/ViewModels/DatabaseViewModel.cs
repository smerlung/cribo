namespace Cribo.ViewModels
{
    using Cribo.Commands;
    using Cribo.Objects;
    using Cribo.Extensions;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using System.Windows;

    public class DatabaseViewModel : ViewModelBase
    {
        private const string UNTITLED = "Untitled";

        private Page selecteditem = null;

        private Dictionary<string, Page> store = null;
        private Repository<string, Page> repository = null;

        public DatabaseViewModel()
        {
            this.CommandAddNewPage = new CommandWithDelegates(this.AddNewPage, () => true);
            this.Items = new ObservableCollection<Page>();
        }

        public ObservableCollection<Page> Items { get; set; }

        public Page SelectedItem { get { return this.selecteditem; } set { this.selecteditem = value; this.FirePropertyChangedEvent("SelectedItem"); } }

        public ICommand CommandAddNewPage { get; set; }

        public bool SetJson(string json)
        {
            try
            {
                this.store = Serializer.ToStore(json);
            }
            catch
            {
                var result = MessageBox.Show("The descrypted database is incorrect, do you want to initialize a new one?", "Warning", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    this.store = new Dictionary<string, Page>();
                }
                else
                {
                    return false;
                }
            }

            this.repository = new Repository<string, Page>(this.store);
            this.repository.ReadAll().ToList().ForEach(n => this.Items.Add(n));
            return true;
        }

        public string GetJson()
        {
            return Serializer.ToJson(this.store);
        }

        public void Clear()
        {
            this.repository = null;
            this.store = null;
            this.Items.Clear();
            this.FirePropertyChangedEvent();
        }

        private void AddNewPage()
        {
            var page = this.GetNextDefaultPage();
            this.repository.Create(page);
            this.Items.Add(page);
            this.SelectedItem = page;
        }

        private int GetNextUntitledIndex()
        {
            var untitled = this.repository.ReadWhere(n => n.Key.StartsWith(UNTITLED));

            if (untitled.Count() == 0)
            {
                return 0;
            }

            var maxindex = untitled.Select(n =>
                {
                    try
                    {
                        return Convert.ToInt32(n.Key.Replace(UNTITLED, string.Empty));
                    }
                    catch
                    {
                        return 0;
                    }
                }).Max();

            return maxindex + 1;
        }

        private Page GetNextDefaultPage()
        {
            var items = new Dictionary<string, string>();
            items.Add("Username", "");
            items.Add("Password", "");
            var page = new Page() { Key = UNTITLED + this.GetNextUntitledIndex(), Items = items  };
            return page;
        }
    }
}
