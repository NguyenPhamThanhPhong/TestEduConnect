using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Support.UI;

namespace TestEduConnect
{
    [TestFixture]
    public class Tests : TestBase
    {
        [SetUp]
        public void Setup()
        {
            Initialize();
            PrepareLoginData();
        }
        private WindowsElement? GetElement(string automationId, string propertyName)
        {
            WindowsElement element = null;
            var wait = new DefaultWait<WindowsDriver<WindowsElement>>(AppSession)
            {
                Timeout = TimeSpan.FromSeconds(10),
                Message = $"Element with automationId \"{automationId}\" not found."
            };

            wait.IgnoreExceptionTypes(typeof(WebDriverException));

            try
            {
                wait.Until(driver =>
                {
                    try
                    {
                        element = driver.FindElementByAccessibilityId(automationId);
                        return element != null;
                    }
                    catch (NoSuchElementException)
                    {
                        return false;
                    }
                });
            }
            catch (WebDriverTimeoutException ex)
            {
                Console.WriteLine($"{ex}, {automationId}, {propertyName}");
                Assert.Fail(ex.Message);
            }

            return element;
        }

        private WindowsElement? GetElementbyName(string name, string propertyName)
        {
            WindowsElement element = null;
            var wait = new DefaultWait<WindowsDriver<WindowsElement>>(AppSession)
            {
                Timeout = TimeSpan.FromSeconds(10),
                Message = $"Element with automationId \"{name}\" not found."
            };

            wait.IgnoreExceptionTypes(typeof(WebDriverException));

            try
            {
                wait.Until(driver =>
                {
                    try
                    {
                        element = driver.FindElementByName(name);
                        return element != null;
                    }
                    catch (NoSuchElementException)
                    {
                        return false;
                    }
                });
            }
            catch (WebDriverTimeoutException ex)
            {
                Console.WriteLine($"{ex}, {name}, {propertyName}");
                Assert.Fail(ex.Message);
            }

            return element;
        }
        [Test]
        public void TestLoginLogout()
        {
            // Assuming your textboxes and buttons have the specified AutomationId


            
            // Fill in username and password
            foreach(var item in datas)
            {
                WindowsElement? usernameTextBox = GetElement("txtbox-username", "username text box");
                WindowsElement? passwordTextBox = GetElement("txtbox-password", "password text box");
                WindowsElement? loginButton = GetElement("btn-login", "login button");
                var username = item.Username;
                var password = item.Password;
                if (usernameTextBox == null)
                {
                    Console.WriteLine("not found username textbox");
                    return;
                }
                if (passwordTextBox == null)
                {
                    Console.WriteLine("not found password");
                    return;
                }
                if (loginButton == null)
                    return;
                usernameTextBox.Clear();
                usernameTextBox.SendKeys(username);
                passwordTextBox.Clear();
                passwordTextBox.SendKeys(password);
                


                if (usernameTextBox.Text!=username)
                {
                    Console.WriteLine("filling username field failed");
                }
                if(passwordTextBox.Text != password)
                {
                    Console.WriteLine("filling password field failed");
                }
                loginButton.Click();
                if (username == "teacher1" || username == "teacher2" || username == "teacher3"|| username == "teacher4")
                {
                    SwitchToMainWindow();
                    ClickLogout();
                    var yesButton = GetElementbyName("Yes", "button yes");
                    yesButton?.Click();
                    SwitchToLoginWindow();

                    continue;
                }
                else
                {
                    // help me navigate to the opended window here (go back to login - it's just a call name not it's whole name)
                    ClickOk();
                    continue;
                }
            }
        }
        private void SwitchToMainWindow()
        {
            // Get the handles of all open windows
            var windowHandles = AppSession.WindowHandles;
            // Assuming the main window has AutomationId "main-window"
            var mainWindowHandle = windowHandles.FirstOrDefault(handle =>
            {
                AppSession.SwitchTo().Window(handle);
                return AppSession.FindElementByAccessibilityId("main-window") != null;
            });

            if (mainWindowHandle != null)
            {
                // Switch to the main window
                AppSession.SwitchTo().Window(mainWindowHandle);
            }
        }
        private void SwitchToLoginWindow()
        {
            // Get the handles of all open windows
            var windowHandles = AppSession.WindowHandles;

            // Assuming the login window has AutomationId "login-window"
            var loginWindowHandle = windowHandles.FirstOrDefault(handle =>
            {
                AppSession.SwitchTo().Window(handle);
                return AppSession.FindElementByAccessibilityId("login-window") != null;
            });

            if (loginWindowHandle != null)
            {
                // Switch to the login window
                AppSession.SwitchTo().Window(loginWindowHandle);
            }
        }

        private void ClickOk()
        {
            try
            {
                WindowsElement? okButton = GetElementbyName("OK", "button ok");
                okButton?.Click();
                return;
            }
            catch(Exception ex)
            {
                Console.WriteLine("failed click ok");
                Console.WriteLine(ex.Message.ToString());
            }
        }
        private void ClickLogout()
        {
            try
            {
                var logoutButton = GetElement("btn-logout", "logout button");
                logoutButton?.Click();
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine("failed click ok");
                Console.WriteLine(ex.Message.ToString());
            }
        }
        [TearDown]
        public void End()
        {

        }
    }
}

