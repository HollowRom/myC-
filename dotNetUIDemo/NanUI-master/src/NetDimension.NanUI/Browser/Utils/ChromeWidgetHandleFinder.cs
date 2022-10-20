using Vanara.PInvoke;

namespace NetDimension.NanUI.Browser;

using static Vanara.PInvoke.User32;

internal static class BrowserWidgetHandleFinder
{
    //private delegate bool EnumWindowProc(IntPtr hwnd, IntPtr lParam);

    //[DllImport("user32")]
    //[return: MarshalAs(UnmanagedType.Bool)]
    //private static extern bool EnumChildWindows(IntPtr window, EnumWindowProc callback, IntPtr lParam);

    //[DllImport("user32.dll", CharSet = CharSet.Unicode)]
    //private static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);


    private class ClassDetails
    {
        public HWND DescendantFound { get; set; }
    }

    private static bool EnumWindow(HWND hWnd, IntPtr lParam)
    {
        const string CHROMIUM_WIDGET_HOST_CLASS_NAME = "Chrome_RenderWidgetHostHWND";

        var buffer = new StringBuilder(256);


        GetClassName(hWnd, buffer, buffer.Capacity);

        if (buffer.ToString() == CHROMIUM_WIDGET_HOST_CLASS_NAME)
        {
            var gcHandle = GCHandle.FromIntPtr(lParam);

            var classDetails = (ClassDetails)gcHandle.Target;

            classDetails.DescendantFound = hWnd;
            return false;
        }

        return true;
    }

    internal static bool TryFindHandle(IntPtr browserHandle, out HWND chromeWidgetHostHandle)
    {
        var classDetails = new ClassDetails();
        var gcHandle = GCHandle.Alloc(classDetails);

        var childProc = new EnumWindowsProc(EnumWindow);

        EnumChildWindows(browserHandle, childProc, GCHandle.ToIntPtr(gcHandle));

        chromeWidgetHostHandle = classDetails.DescendantFound;

        gcHandle.Free();

        return classDetails.DescendantFound != HWND.NULL;
    }
}
