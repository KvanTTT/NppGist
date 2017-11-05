namespace NppNetInf
{
    public abstract class PluginMain
    {
        public abstract string PluginName { get; }

        public virtual void CommandMenuInit()
        {
        }

        public virtual void OnNotification(ScNotification notification)
        {
        }

        public virtual void PluginCleanUp()
        {
        }

        public virtual void SetToolBarIcon()
        {
        }
    }
}
