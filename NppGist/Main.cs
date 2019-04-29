using NppGist.Forms;
using NppNetInf;
using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace NppGist
{
    class Main : PluginMain
    {
        internal const string ApiUrl = "https://api.github.com";
        internal const string GistUrl = "https://gist.github.com";
        internal static string IniFileName = null;
        internal static string Token = null;
        internal static string Login = null;
        internal static bool SaveLocally = false;
        internal static bool CloseSaveDialog = true;
        internal static bool CloseOpenDialog = true;
        internal const string pluginName = "NppGist";

        static Bitmap tbLoad = Properties.Resources.download;
        static Bitmap tbSave = Properties.Resources.upload;
        static int TokenCommandId = 0;
        static int OpenCommandId = 1;
        static int SaveCommandId = 2;
        static int AboutCommandId = 3;

        public override string PluginName => pluginName;

        static Main()
        {
            AppDomain.CurrentDomain.AssemblyResolve += ResolveEventHandler;
        }

        public Main()
            : base()
        {
        }

        public override void CommandMenuInit()
        {
            string pluginsConfigDir = PluginBase.GetPluginsConfigDir();
            if (!Directory.Exists(pluginsConfigDir))
                Directory.CreateDirectory(pluginsConfigDir);
            IniFileName = Path.Combine(pluginsConfigDir, pluginName + ".ini");

            StringBuilder str = new StringBuilder(100);
            Win32.GetPrivateProfileString("Settings", "Login", string.Empty, str, (uint)str.Capacity, IniFileName);
            Login = str.ToString();

            str.Clear();
            Win32.GetPrivateProfileString("Settings", "AccessToken", string.Empty, str, (uint)str.Capacity, IniFileName);
            try
            {
                Token = AccessToken.DecryptToken(str.ToString());
            }
            catch
            {
                Token = "";
            }

            SaveLocally = Convert.ToBoolean(Win32.GetPrivateProfileInt("Settings", "SaveToLocal", 1, IniFileName));
            CloseSaveDialog = Convert.ToBoolean(Win32.GetPrivateProfileInt("Settings", "CloseSaveDialog", 1, IniFileName));
            CloseOpenDialog = Convert.ToBoolean(Win32.GetPrivateProfileInt("Settings", "CloseOpenDialog", 1, IniFileName));

            PluginBase.SetCommand(TokenCommandId, "Access Token", EnterAccessTokenCommand);
            PluginBase.SetCommand(OpenCommandId, "Open Gist", OpenGistCommand, new ShortcutKey(false, false, false, Keys.None));
            PluginBase.SetCommand(SaveCommandId, "Save Gist", SaveGistCommand, new ShortcutKey(false, false, false, Keys.None));
            PluginBase.SetCommand(AboutCommandId, "About", AboutCommand);
        }

        public override void SetToolBarIcon()
        {
            toolbarIcons tbIcons = new toolbarIcons();
            tbIcons.hToolbarBmp = tbLoad.GetHbitmap();
            IntPtr pTbIcons = Marshal.AllocHGlobal(Marshal.SizeOf(tbIcons));
            Marshal.StructureToPtr(tbIcons, pTbIcons, false);
            Win32.SendMessage(PluginBase.NppData._nppHandle, (uint)NppMsg.NPPM_ADDTOOLBARICON, PluginBase.FuncItems.Items[OpenCommandId]._cmdID, pTbIcons);
            Marshal.FreeHGlobal(pTbIcons);

            tbIcons = new toolbarIcons();
            tbIcons.hToolbarBmp = tbSave.GetHbitmap();
            pTbIcons = Marshal.AllocHGlobal(Marshal.SizeOf(tbIcons));
            Marshal.StructureToPtr(tbIcons, pTbIcons, false);
            Win32.SendMessage(PluginBase.NppData._nppHandle, (uint)NppMsg.NPPM_ADDTOOLBARICON, PluginBase.FuncItems.Items[SaveCommandId]._cmdID, pTbIcons);
            Marshal.FreeHGlobal(pTbIcons);
        }

        internal static void EnterAccessTokenCommand()
        {
            var authForm = new dlgAuthorization();
            authForm.ShowDialog();
        }

        internal static void OpenGistCommand()
        {
            if (!string.IsNullOrEmpty(Token) || (new dlgAuthorization()).ShowDialog() == DialogResult.OK)
            {
                var openGistForm = new dlgOpenGist();
                openGistForm.ShowDialog();
            }
        }

        internal static void SaveGistCommand()
        {
            if (!string.IsNullOrEmpty(Token) || (new dlgAuthorization()).ShowDialog() == DialogResult.OK)
            {
                var saveGistForm = new dlgSaveGist();
                saveGistForm.ShowDialog();
            }
        }

        internal static void AboutCommand()
        {
            var frmAbout = new frmAbout();
            frmAbout.ShowDialog();
        }

        private static Assembly ResolveEventHandler(object sender, ResolveEventArgs args)
        {
            string resource = string.Format("{0}.{1}.dll", pluginName, args.Name.Remove(args.Name.IndexOf(',')));
            Assembly currentAssembly = Assembly.GetExecutingAssembly();

            using (Stream stream = currentAssembly.GetManifestResourceStream(resource))
            {
                if (stream != null)
                {
                    var bytes = new byte[(int)stream.Length];
                    stream.Read(bytes, 0, (int)stream.Length);
                    return Assembly.Load(bytes);
                }
            }

            return null;
        }
    }
}