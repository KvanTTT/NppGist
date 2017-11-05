// NPP plugin platform for .Net v0.93.96 by Kasper B. Graversen etc.
using System;
using System.Text;

namespace NppNetInf
{
    public class PluginBase
    {
        public static NppData NppData { get; internal set; }

        public static FuncItems FuncItems { get; private set; } = new FuncItems();

        public static void SetCommand(int index, string commandName, NppFuncItemDelegate functionPointer)
        {
            SetCommand(index, commandName, functionPointer, new ShortcutKey(), false);
        }

        public static void SetCommand(int index, string commandName, NppFuncItemDelegate functionPointer, ShortcutKey shortcut)
        {
            SetCommand(index, commandName, functionPointer, shortcut, false);
        }

        public static void SetCommand(int index, string commandName, NppFuncItemDelegate functionPointer, bool checkOnInit)
        {
            SetCommand(index, commandName, functionPointer, new ShortcutKey(), checkOnInit);
        }

        public static void SetCommand(int index, string commandName, NppFuncItemDelegate functionPointer, ShortcutKey shortcut, bool checkOnInit)
        {
            FuncItem funcItem = new FuncItem();
            funcItem._cmdID = index;
            funcItem._itemName = commandName;
            if (functionPointer != null)
                funcItem._pFunc = new NppFuncItemDelegate(functionPointer);
            if (shortcut._key != 0)
                funcItem._pShKey = shortcut;
            funcItem._init2Check = checkOnInit;
            FuncItems.Add(funcItem);
        }

        public static IntPtr GetCurrentScintilla()
        {
            int curScintilla;
            Win32.SendMessage(NppData._nppHandle, (uint) NppMsg.NPPM_GETCURRENTSCINTILLA, 0, out curScintilla);
            return (curScintilla == 0) ? NppData._scintillaMainHandle : NppData._scintillaSecondHandle;
        }

        public static string GetPluginsConfigDir()
        {
            StringBuilder sbIniFilePath = new StringBuilder(Win32.MAX_PATH);
            Win32.SendMessage(NppData._nppHandle, (uint) NppMsg.NPPM_GETPLUGINSCONFIGDIR, Win32.MAX_PATH, sbIniFilePath);
            return sbIniFilePath.ToString();
        }

        public unsafe static void SetCurrentFileText(string text)
        {
            fixed (byte* p = Encoding.UTF8.GetBytes(text))
            {
                Win32.SendMessage(GetCurrentScintilla(), SciMsg.SCI_SETTEXT, 0, (IntPtr)p);
            }
        }

        public unsafe static void AppendTextToCurrentFile(string text)
        {
            var bytes = Encoding.UTF8.GetBytes(text);
            fixed (byte* p = bytes)
            {
                Win32.SendMessage(GetCurrentScintilla(), SciMsg.SCI_APPENDTEXT, bytes.Length, (IntPtr)p);
            }
        }

        public static string GetCurrentFileText(int length = -1)
        {
            if (length == -1)
                length = Win32.SendMessage(GetCurrentScintilla(), SciMsg.SCI_GETLENGTH, 0, 0).ToInt32();

            Sci_TextRange range = new Sci_TextRange(0, -1, length);
            Win32.SendMessage(GetCurrentScintilla(), SciMsg.SCI_GETTEXTRANGE, 0, range.NativePointer);
            return range.lpstrTextUtf8;
        }

        public static string GetFullCurrentFileName()
        {
            StringBuilder builder = new StringBuilder(Win32.MAX_PATH);
            Win32.SendMessage(NppData._nppHandle, (uint) NppMsg.NPPM_GETFULLCURRENTPATH, 0, builder);
            return builder.ToString();
        }

        static readonly Func<IScintillaGateway> gatewayFactory = () => new ScintillaGateway(GetCurrentScintilla());

        public static Func<IScintillaGateway> GetGatewayFactory()
        {
            return gatewayFactory;
        }
    }
}
