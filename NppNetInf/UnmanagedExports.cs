// NPP plugin platform for .Net v0.93.96 by Kasper B. Graversen etc.
using NppPlugin.DllExport;
using System;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace NppNetInf
{
    class UnmanagedExports
    {
        private static IntPtr _ptrPluginName = IntPtr.Zero;

        private static PluginMain _main;

        static UnmanagedExports()
        {
            Type pluginMain = Assembly.GetExecutingAssembly().GetTypes()
                .FirstOrDefault(type => type.IsSubclassOf(typeof(PluginMain)));
            _main = (PluginMain)Activator.CreateInstance(pluginMain);
        }

        [DllExport(CallingConvention = CallingConvention.Cdecl)]
        private static bool isUnicode()
        {
            return true;
        }

        [DllExport(CallingConvention = CallingConvention.Cdecl)]
        private static void setInfo(NppData notepadPlusData)
        {
            PluginBase.NppData = notepadPlusData;
            _main.CommandMenuInit();
        }

        [DllExport(CallingConvention = CallingConvention.Cdecl)]
        private static IntPtr getFuncsArray(ref int nbF)
        {
            nbF = PluginBase.FuncItems.Items.Count;
            return PluginBase.FuncItems.NativePointer;
        }

        [DllExport(CallingConvention = CallingConvention.Cdecl)]
        private static uint messageProc(uint Message, IntPtr wParam, IntPtr lParam)
        {
            return 1;
        }

        [DllExport(CallingConvention = CallingConvention.Cdecl)]
        private static IntPtr getName()
        {
            if (_ptrPluginName == IntPtr.Zero)
                _ptrPluginName = Marshal.StringToHGlobalUni(_main.PluginName ?? "unknown");
            return _ptrPluginName;
        }

        [DllExport(CallingConvention = CallingConvention.Cdecl)]
        private static void beNotified(IntPtr notifyCode)
        {
            ScNotification notification = (ScNotification)Marshal.PtrToStructure(notifyCode, typeof(ScNotification));
            if (notification.Header.Code == (uint)NppMsg.NPPN_TBMODIFICATION)
            {
                PluginBase.FuncItems.RefreshItems();
                _main.SetToolBarIcon();
            }
            else if (notification.Header.Code == (uint)NppMsg.NPPN_SHUTDOWN)
            {
                _main.PluginCleanUp();
                Marshal.FreeHGlobal(_ptrPluginName);
            }
            else
            {
                _main.OnNotification(notification);
            }
        }
    }
}
