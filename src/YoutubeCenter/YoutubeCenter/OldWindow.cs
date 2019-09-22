using System;
using System.Windows.Forms;

namespace YoutubeCenter
{
    public class OldWindow : IWin32Window
    {
        private readonly IntPtr _handle;

        public OldWindow(IntPtr handle)
        {
            _handle = handle;
        }

        #region IWin32Window Members
        IntPtr IWin32Window.Handle => _handle;
        #endregion
    }
}
