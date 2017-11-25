using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cribo.Objects
{
    /// <summary>
    /// Base class for settings where you don't want the user defined settings to be overridden when the application version is changed
    /// </summary>
    /// <typeparam name="T">The type of the settings child class</typeparam>
    public abstract class SohSettingsBase<T> : ApplicationSettingsBase where T : SohSettingsBase<T>, new()
    {
        private static T settings = null;

        /// <summary>
        /// Static accessor for the settings instance. This handles all the upgrade stuff
        /// </summary>
        public static T Default
        {
            get
            {
                if (settings == null)
                {
                    settings = new T();
                    settings.Initialize();
                }

                if (settings.NeedsUpgrade)
                {
                    settings.Upgrade();
                    settings.NeedsUpgrade = false;
                    settings.Save();
                }

                return settings;
            }
        }

        /// <summary>
        /// This has a default value of true. So when the settings is first created this is true.
        /// </summary>
        [UserScopedSettingAttribute()]
        [DebuggerNonUserCodeAttribute()]
        [DefaultSettingValueAttribute("True")]
        public bool NeedsUpgrade
        {
            get
            {
                return ((bool)(this["NeedsUpgrade"]));
            }
            set
            {
                this["NeedsUpgrade"] = value;
            }
        }

        /// <summary>
        /// This is called immediately after the new settings object has been created.
        /// </summary>
        public abstract void Initialize();
    }
}
