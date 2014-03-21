using System;
using System.Text;

namespace NppPluginNET
{
	class PluginBase
	{
		#region " Fields "
		internal static NppData nppData;
		internal static FuncItems _funcItems = new FuncItems();
		#endregion

		#region " Helper "
		internal static void SetCommand(int index, string commandName, NppFuncItemDelegate functionPointer)
		{
			SetCommand(index, commandName, functionPointer, new ShortcutKey(), false);
		}
		internal static void SetCommand(int index, string commandName, NppFuncItemDelegate functionPointer, ShortcutKey shortcut)
		{
			SetCommand(index, commandName, functionPointer, shortcut, false);
		}
		internal static void SetCommand(int index, string commandName, NppFuncItemDelegate functionPointer, bool checkOnInit)
		{
			SetCommand(index, commandName, functionPointer, new ShortcutKey(), checkOnInit);
		}
		internal static void SetCommand(int index, string commandName, NppFuncItemDelegate functionPointer, ShortcutKey shortcut, bool checkOnInit)
		{
			FuncItem funcItem = new FuncItem();
			funcItem._cmdID = index;
			funcItem._itemName = commandName;
			if (functionPointer != null)
				funcItem._pFunc = new NppFuncItemDelegate(functionPointer);
			if (shortcut._key != 0)
				funcItem._pShKey = shortcut;
			funcItem._init2Check = checkOnInit;
			_funcItems.Add(funcItem);
		}

		internal static IntPtr GetCurrentScintilla()
		{
			int curScintilla;
			Win32.SendMessage(nppData._nppHandle, NppMsg.NPPM_GETCURRENTSCINTILLA, 0, out curScintilla);
			return (curScintilla == 0) ? nppData._scintillaMainHandle : nppData._scintillaSecondHandle;
		}

		internal static string GetPluginsConfigDir()
		{
			StringBuilder sbIniFilePath = new StringBuilder(Win32.MAX_PATH);
			Win32.SendMessage(PluginBase.nppData._nppHandle, NppMsg.NPPM_GETPLUGINSCONFIGDIR, Win32.MAX_PATH, sbIniFilePath);
			return sbIniFilePath.ToString();
		}

		internal unsafe static void SetCurrentFileText(string text)
		{
			fixed (byte* p = Encoding.UTF8.GetBytes(text))
			{
				Win32.SendMessage(PluginBase.GetCurrentScintilla(), SciMsg.SCI_SETTEXT, 0, (IntPtr)p);
			}
		}

		internal unsafe static void AppendTextToCurrentFile(string text)
		{
			var bytes = Encoding.UTF8.GetBytes(text);
			fixed (byte* p = bytes)
			{
				Win32.SendMessage(PluginBase.GetCurrentScintilla(), SciMsg.SCI_APPENDTEXT, bytes.Length, (IntPtr)p);
			}
		}

		internal static string GetCurrentFileText(int length = -1)
		{
			if (length == -1)
				length = Win32.SendMessage(PluginBase.GetCurrentScintilla(), SciMsg.SCI_GETLENGTH, 0, 0).ToInt32();

			Sci_TextRange range = new Sci_TextRange(0, -1, length);
			Win32.SendMessage(PluginBase.GetCurrentScintilla(), SciMsg.SCI_GETTEXTRANGE, 0, range.NativePointer);
			return range.lpstrTextUtf8;
		}

		internal static string GetFullCurrentFileName()
		{
			StringBuilder builder = new StringBuilder(Win32.MAX_PATH);
			Win32.SendMessage(PluginBase.nppData._nppHandle, NppMsg.NPPM_GETFULLCURRENTPATH, 0, builder);
			return builder.ToString();
		}

		#endregion
	}
}
