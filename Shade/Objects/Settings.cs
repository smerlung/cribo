namespace Shade.Objects
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Shade.Extensions;

    public class Settings : SohSettingsBase<Settings>
    {
        [UserScopedSettingAttribute]
        [DebuggerNonUserCodeAttribute]
        [DefaultSettingValueAttribute(@"")]
        public string RecentFiles
        {
            get
            {
                return (string)this["RecentFiles"];
            }

            set
            {
                this["RecentFiles"] = value;
            }
        }

        public List<RecentFile> GetRecentFiles()
        {
            var result = this.RecentFiles.ToObjectFromJson<List<RecentFile>>();
            return result;
        }

        public void SetRecentFiles(IList<RecentFile> recentfiles)
        {
            this.RecentFiles = recentfiles.ToJson();
            this.Save();
        }

        public override void Initialize()
        {
            if (string.IsNullOrEmpty(this.RecentFiles))
            {
                this.SetRecentFiles(new List<RecentFile>());
            }
        }
    }
}
