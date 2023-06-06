using NUnit.Framework;
using NUnit.Framework.Internal;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Runtime.Intrinsics.X86;
using System.Text.RegularExpressions;
using System.Threading;

namespace AbottTest
{
    /// <summary>
    /// Option 2 - Complex UI Automation:
    /// 1.	Successfully complete UI automation from step 1 to step 8 with below requirements
    //•	Use any test framework or language
    //•	Step 6: Fetch verification code, can be done through UI or POP3/IMAP if you prefer.
    //Notes:
    //Please set the driver location before running the test.

    /// </summary>
    public class LibreViewLoginTest
    {
        private WebDriver WebDriver { get; set; }

        // Please set the 
        private string DriverPath { get; set; } = @"C:\Users\GIRIRAJ\Downloads\Webdriver\chromedriver_win32";

        private string BaseUrl { get; set; } = "https://www.libreview.com/";

        private string OutlookUrl { get; set; } = "https://login.live.com/login.srf?wa=wsignin1.0&rpsnv=13&ct=1685989985&rver=7.0.6737.0&wp=MBI_SSL&wreply=https%3a%2f%2foutlook.live.com%2fowa%2f%3fnlp%3d1%26RpsCsrfState%3dc076e70a-0cdd-1b71-0165-b990248f0ed9&id=292841&aadredir=1&CBCXT=out&lw=1&fl=dob%2cflname%2cwld&cobrandid=90015";

        [TearDown]
        public void TearDown()
        {
            WebDriver.Quit();
        }

        public void SelectRegionTest()
        {
            // Navigate to login page
            WebDriver.Navigate().GoToUrl(BaseUrl);
            var closebutton = WebDriver.FindElement(By.Id("truste-consent-close"));
            closebutton.Click();

            //Select the country dropdown 
            SelectElement dropDownCountry = new SelectElement(WebDriver.FindElement(By.Id("country-select")));
            //Select the US as option 
            dropDownCountry.SelectByValue("US");
            Assert.AreEqual("United States", dropDownCountry.SelectedOption.Text);
            //Select the country dropdown 
            SelectElement dropDownlanguage = new SelectElement(WebDriver.FindElement(By.Id("language-select")));
            dropDownlanguage.SelectByValue("en-US");
            Assert.AreEqual("English", dropDownlanguage.SelectedOption.Text);

            var button = WebDriver.FindElement(By.Id("submit-button"));
            button.Click();
            // Validate we navigated login page.
            var startTimestamp = DateTime.Now.Millisecond;
            var endTimstamp = startTimestamp + 10000;

            while (true)
            {
                try
                {
                    // Get the login text element
                    var logintext = WebDriver.FindElement(By.Id("login-title-text"));
                    Assert.AreEqual("Member Login", logintext.Text);
                    break;
                }
                catch
                {
                    var currentTimestamp = DateTime.Now.Millisecond;
                    if (currentTimestamp > endTimstamp)
                    {
                        throw;
                    }
                    Thread.Sleep(2000);
                }
            }

        }

        /// <summary>
        /// Open Login Page & validate authentication.
        /// </summary>
        public void ValidateOutlookLoginTest()
        {
            // get the emailinput element.
            var emailInput = WebDriver.FindElement(By.Id("loginForm-email-input"));
            emailInput.Clear();
            // set the text of emailInput
            emailInput.SendKeys(Constants.Constants.username);
            Thread.Sleep(1000);
            Assert.AreEqual(Constants.Constants.username, emailInput.GetAttribute("value"));

            var passwordInput = WebDriver.FindElement(By.Id("loginForm-password-input"));
            passwordInput.Clear();
            // set the text of passwordInput
            passwordInput.SendKeys(Constants.Constants.passwordLibView);
            Assert.AreEqual(Constants.Constants.passwordLibView, passwordInput.GetAttribute("value"));

            // get the Login submit button element.
            var loginButton = WebDriver.FindElement(By.Id("loginForm-submit-button"));
            loginButton.Click();
            //Validate login message
            var startTimestamp = DateTime.Now.Millisecond;
            var endTimstamp = startTimestamp + 10000;

            while (true)
            {
                try
                {
                    var wizheader = WebDriver.FindElement(By.Id("wizardCard-header-text"));
                    Assert.AreEqual("2-Factor Authentication", wizheader.Text);
                    break;
                }
                catch
                {
                    var currentTimestamp = DateTime.Now.Millisecond;
                    if (currentTimestamp > endTimstamp)
                    {
                        throw;
                    }
                    Thread.Sleep(2000);
                }
            }

        }

        /// <summary>
        /// Request the Auth Code.
        /// </summary>
        public void RequestForAuthTest()
        {
            var sendCodeButton = WebDriver.FindElement(By.Id("twoFactor-step1-next-button"));
            sendCodeButton.Click();

        }

        /// <summary>
        /// Gets The Auth Code for logging into the account.
        /// </summary>
        /// <returns></returns>
        public string GetAuthCode()
        {
            //open new window for outlook login
            WebDriver.SwitchTo().NewWindow(WindowType.Tab);
            WebDriver.Navigate().GoToUrl(OutlookUrl);

            // Enter Email & password. 
            var emailText = WebDriver.FindElement(By.Id("i0116"));
            emailText.Clear();
            emailText.SendKeys(Constants.Constants.username);
            Assert.AreEqual(Constants.Constants.username, emailText.GetAttribute("value"));
            var signinnextbutton = WebDriver.FindElement(By.Id("idSIButton9"));
            signinnextbutton.Click();
            Thread.Sleep(2000);
            var passwordText = WebDriver.FindElement(By.Id("i0118"));
            passwordText.Clear();
            passwordText.SendKeys(Constants.Constants.passwordOutlook);
            Assert.AreEqual(Constants.Constants.passwordOutlook, passwordText.GetAttribute("value"));
            Thread.Sleep(2000);


            var signinbutton = WebDriver.FindElement(By.Id("idSIButton9"));

            signinbutton.Click();

            var staysignedIn = WebDriver.FindElement(By.Id("displayName"));
            if (staysignedIn != null)
            {
                signinbutton = WebDriver.FindElement(By.Id("idSIButton9"));
                signinbutton.Click();
            }
            else
            {
                // This button appears sometimes
                var microsoftButton = WebDriver.FindElement(By.Id("id__0"));
                microsoftButton.Click();

            }

            var libreMessage = WebDriver.FindElement(By.XPath("//*[contains(text(), 'LibreView Verification Code')]"));
            libreMessage.Click();
            Thread.Sleep(2000);
            // Does not an id hence the xpath.
            var message = WebDriver.FindElement(By.XPath("//*[@id=\"x_backgroundTable\"]/tbody/tr/td/table/tbody/tr/td/table/tbody/tr/td/table[2]/tbody/tr/td/table[2]/tbody/tr[2]/td"));
            var code = message.Text.Split(" ");
            return code[4];
        }

        /// <summary>
        /// Verify if the Upload button exists after successfull login.
        /// </summary>
        /// <param name="code"></param>

        public void VerifyButtonExists(string code)
        {
            // Open a new window
            WebDriver.SwitchTo().Window(WebDriver.WindowHandles[0]);
            // get the input element to enter the code
            var verificationcode = WebDriver.FindElement(By.Id("twoFactor-step2-code-input"));
            verificationcode.Clear();
            // enter the code
            verificationcode.SendKeys(code);
            Assert.AreEqual(code, verificationcode.GetAttribute("value"));
            // get Verify input Button
            var verifyButton = WebDriver.FindElement(By.Id("twoFactor-step2-next-button"));
            verifyButton.Click();
            // get the process upload Button
            var processupload = WebDriver.FindElement(By.Id("uploadCard-upload-button"));
            Assert.AreEqual("Upload a Device", processupload.Text);
        }

        [Test]
        public void LibViewLoginTest()
        {
            SelectRegionTest();
            ValidateOutlookLoginTest();
            RequestForAuthTest();
            string code = GetAuthCode();
            VerifyButtonExists(code);

        }

        [SetUp]
        public void Setup()
        {
            WebDriver = GetChromeDriver();
            WebDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(120);
        }

        private WebDriver GetChromeDriver()
        {
            var options = new ChromeOptions();

            return new ChromeDriver(DriverPath, options, TimeSpan.FromSeconds(300));
        }
    }
}