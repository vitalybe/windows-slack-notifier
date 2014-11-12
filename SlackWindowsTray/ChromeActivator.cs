using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace SlackWindowsTray
{
    class ChromeActivator
    {
        private delegate bool EnumWindowsDelegate(IntPtr hWnd, IntPtr lparam);

        [SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage"), SuppressUnmanagedCodeSecurity]
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage"), SuppressUnmanagedCodeSecurity]
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private extern static bool EnumWindows(EnumWindowsDelegate lpEnumFunc, IntPtr lparam);

        [SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage"), SuppressUnmanagedCodeSecurity]
        [DllImport("User32", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowText(IntPtr windowHandle, StringBuilder stringBuilder, int nMaxCount);

        [DllImport("user32.dll", EntryPoint = "GetWindowTextLength", SetLastError = true)]
        private static extern int GetWindowTextLength(IntPtr hwnd);

        [DllImport("user32.dll")]
        private static extern long SetForegroundWindow(IntPtr hWnd);

        private const int SW_SHOWNORMAL = 1;
        private const int SW_SHOWMAXIMIZED = 3;
        private const int SW_RESTORE = 9;

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        /// <summary>Find the windows matching the specified class name.</summary>
        private static IEnumerable<IntPtr> WindowsMatching(string className)
        {
            return new ChromeActivator(className)._result;
        }

        private ChromeActivator(string className)
        {
            _className = className;
            EnumWindows(callback, IntPtr.Zero);
        }

        private bool callback(IntPtr hWnd, IntPtr lparam)
        {
            if (GetClassName(hWnd, _apiResult, _apiResult.Capacity) != 0)
            {
                if (string.CompareOrdinal(_apiResult.ToString(), _className) == 0)
                {
                    _result.Add(hWnd);
                }
            }

            return true; // Keep enumerating.
        }

        private static IEnumerable<HwndWindow> WindowsForClass(string className)
        {
            foreach (var windowHandle in WindowsMatchingClassName(className))
            {
                int length = GetWindowTextLength(windowHandle);
                StringBuilder sb = new StringBuilder(length + 1);
                GetWindowText(windowHandle, sb, sb.Capacity);
                yield return new HwndWindow() { Hwnd = windowHandle, Title = sb.ToString() };
            }
        }

        private static IEnumerable<IntPtr> WindowsMatchingClassName(string className)
        {
            if (string.IsNullOrWhiteSpace(className))
                throw new ArgumentOutOfRangeException("className", className, "className can't be null or blank.");

            return WindowsMatching(className);
        }

        private static IEnumerable<HwndWindow> ChromeWindowTitles()
        {
            foreach (var window in WindowsForClass("Chrome_WidgetWin_0"))
                if (!string.IsNullOrWhiteSpace(window.Title))
                    yield return window;

            foreach (var window in WindowsForClass("Chrome_WidgetWin_1"))
                if (!string.IsNullOrWhiteSpace(window.Title))
                    yield return window;
        }

        public static bool ActivateChromeWindowByTitle(Func<HwndWindow, bool> windowFinderPredicate)
        {
            var slackChromeWindow = ChromeWindowTitles().Where(windowFinderPredicate).FirstOrDefault();
            if (slackChromeWindow != null)
            {
                ShowWindow(slackChromeWindow.Hwnd, SW_RESTORE); 
                SetForegroundWindow(slackChromeWindow.Hwnd);
                return true;
            }
            else
            {
                return false;
            }
        }

        private readonly string _className;
        private readonly List<IntPtr> _result = new List<IntPtr>();
        private readonly StringBuilder _apiResult = new StringBuilder(1024);
    }
}