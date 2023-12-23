using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using System;
using System.Diagnostics;
using System.Threading;
using System.Collections.Generic;

namespace TestEduConnect
{
    public class TestBase
    {
        private const string WindowsApplicationDriverUrl = "http://127.0.0.1:4723";
        private const string ApplicationPath = @"D:\PhongselfProject\software testing\Do_an\EduConnectApp\EduConnectApp\bin\Debug\EduConnectApp.exe";
        private const string DeviceName = "WindowsPC";
        private const int WaitForAppLaunch = 5;
        private string WinAppDriverPath = @"C:\Program Files (x86)\Windows Application Driver\WinAppDriver.exe";
        private static Process? winAppDriverProcess;
        public WindowsDriver<WindowsElement> AppSession { get; private set; }
        public WindowsDriver<WindowsElement> DesktopSession { get; private set; }
        public List<LoginInfo> datas = new List<LoginInfo>();

        private void StartWinAppDriver()
        {
            ProcessStartInfo psi = new ProcessStartInfo(WinAppDriverPath);
            psi.UseShellExecute = true;
            psi.Verb = "runas"; // run as administrator
            winAppDriverProcess = Process.Start(psi);

            // Wait for WinAppDriver to start
            Thread.Sleep(5000);
        }

        public void Initialize()
        {
            // Start the WinAppDriver process
            StartWinAppDriver();

            // Set up Appium capabilities
            var appiumOptions = new AppiumOptions();
            appiumOptions.AddAdditionalCapability("app", ApplicationPath);
            appiumOptions.AddAdditionalCapability("deviceName", DeviceName);
            appiumOptions.AddAdditionalCapability("ms:waitForAppLaunch", WaitForAppLaunch);

            // Initialize the Appium session for the WPF app
            this.AppSession = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appiumOptions);

            // Assert that the Appium session and session ID are not null
            Assert.IsNotNull(AppSession);
            Assert.IsNotNull(AppSession.SessionId);

            // Set an implicit wait time for elements to be found
            AppSession.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }
        public void PrepareLoginData()
        {
            for (int i = 0; i < 20; i++)
            {
                var loginInfo = new LoginInfo()
                {
                    Username = "teacher" + i,
                    Password = "teacher" + i
                };
                if (i > 4)
                    loginInfo.Username = "wrong-teacher-username"+i;
                datas.Add(loginInfo);
            }
        }

        public void Cleanup()
        {
            // Close the Appium session and stop the WinAppDriver process
            if (AppSession != null)
            {
                AppSession.Quit();
            }

            if (winAppDriverProcess != null && !winAppDriverProcess.HasExited)
            {
                winAppDriverProcess.CloseMainWindow();
                winAppDriverProcess.WaitForExit(5000);
            }
        }
    }
}
