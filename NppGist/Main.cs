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
    internal class Main : PluginMain
    {
        private const string pluginName = "NppGist";
        internal const string GistUrl = "https://gist.github.com";
        internal static string IniFileName;
        internal static string Login;
        internal static bool SaveLocally;
        internal static bool CloseSaveDialog = true;
        internal static bool CloseOpenDialog = true;

        static Bitmap tbLoad = Properties.Resources.download;
        static Bitmap tbSave = Properties.Resources.upload;
        static int TokenCommandId = 0;
        static int OpenCommandId = 1;
        static int SaveCommandId = 2;
        static int AboutCommandId = 3;

        public static GitHubService GitHubService { get; set; }

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
            string token;
            try
            {
                token = AccessToken.DecryptToken(str.ToString());
            }
            catch
            {
                token = "";
            }

            GitHubService = new GitHubService(token);
            SaveLocally = Convert.ToBoolean(Win32.GetPrivateProfileInt("Settings", "SaveToLocal", 1, IniFileName));
            CloseSaveDialog = Convert.ToBoolean(Win32.GetPrivateProfileInt("Settings", "CloseSaveDialog", 1, IniFileName));
            CloseOpenDialog = Convert.ToBoolean(Win32.GetPrivateProfileInt("Settings", "CloseOpenDialog", 1, IniFileName));
            GuiUtils.SecretGistColor = Color.FromArgb(Win32.GetPrivateProfileInt("Settings", "SecretGistBackgroundColor", GuiUtils.SecretGistColor.ToArgb(), IniFileName));
            GuiUtils.SecretGistForeColor = Color.FromArgb(Win32.GetPrivateProfileInt("Settings", "SecretGistForegroundColor", GuiUtils.SecretGistForeColor.ToArgb(), IniFileName));

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

        public override void PluginCleanUp()
        {
            GitHubService?.Dispose();
        }

        private static void EnterAccessTokenCommand()
        {
            var authForm = new dlgAuthorization();
            authForm.ShowDialog();
        }

        private static void OpenGistCommand()
        {
            if (!string.IsNullOrEmpty(GitHubService.Token) || (new dlgAuthorization()).ShowDialog() == DialogResult.OK)
            {
                var openGistForm = new dlgOpenGist();
                openGistForm.ShowDialog();
            }
        }

        private static void SaveGistCommand()
        {
            if (!string.IsNullOrEmpty(GitHubService.Token) || (new dlgAuthorization()).ShowDialog() == DialogResult.OK)
            {
                var saveGistForm = new dlgSaveGist();
                saveGistForm.ShowDialog();
            }
        }

        private static void AboutCommand()
        {
            var frmAbout = new frmAbout();
            frmAbout.ShowDialog();
        }

        private static Assembly ResolveEventHandler(object sender, ResolveEventArgs args)
        {
            string resource = $"{pluginName}.{args.Name.Remove(args.Name.IndexOf(','))}.dll";
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