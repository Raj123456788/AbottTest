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
    public class LibreViewLoginTest : DriverHelper
    {


        // Please set the Driver path
        private string DriverPath { get; set; } = Constants.Constants.driverPath;

        private string BaseUrl { get; set; } = "https://www.libreview.com/";

        private VerifyIdentityPage verifyPage;

        private string OutlookUrl { get; set; } = "https://login.live.com/login.srf?wa=wsignin1.0&rpsnv=13&ct=1685989985&rver=7.0.6737.0&wp=MBI_SSL&wreply=https%3a%2f%2foutlook.live.com%2fowa%2f%3fnlp%3d1%26RpsCsrfState%3dc076e70a-0cdd-1b71-0165-b990248f0ed9&id=292841&aadredir=1&CBCXT=out&lw=1&fl=dob%2cflname%2cwld&cobrandid=90015";

        [TearDown]
        public void TearDown()
        {
            Driver.Quit();
        }


        [Test]
        public void LibViewLoginTest()
        {
            SelectRegionTest();
            ValidateLoginTest();
            RequestForAuthTest();
            GetAuthCode();

        }

        [SetUp]
        public void Setup()
        {
            Driver = GetChromeDriver();
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(120);
            verifyPage = new VerifyIdentityPage(Driver);
        }

        private WebDriver GetChromeDriver()
        {
            var options = new ChromeOptions();

            return new ChromeDriver(DriverPath, options, TimeSpan.FromSeconds(300));
        }
        #region Tests
        /// <summary>
        /// Test The region Page.
        /// </summary>

        private void SelectRegionTest()
        {
            // Navigate to login page
            Driver.Navigate().GoToUrl(BaseUrl);
            RegionSelectionPage regionPage = new RegionSelectionPage(Driver);

            regionPage.ClickClose();

            //Select the country dropdown 
            SelectElement dropDownCountry = new SelectElement(Driver.FindElement(By.Id("country-select")));
            //Select the US as option 
            dropDownCountry.SelectByValue("US");
            Assert.AreEqual("United States", dropDownCountry.SelectedOption.Text);
            //Select the country dropdown 
            SelectElement dropDownlanguage = new SelectElement(Driver.FindElement(By.Id("language-select")));
            dropDownlanguage.SelectByValue("en-US");
            Assert.AreEqual("English", dropDownlanguage.SelectedOption.Text);

            regionPage.ClickSubmit();
            // Validate we navigated login page.
            var startTimestamp = DateTime.Now.Millisecond;
            var endTimstamp = startTimestamp + 10000;

            while (true)
            {
                try
                {
                    // Get the login text element                 
                    Assert.AreEqual("Member Login", regionPage.GetLoginText());
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
        private void ValidateLoginTest()
        {
            LoginPage loginPage = new LoginPage(Driver);

            loginPage.EnterUsernamePassword(Constants.Constants.username, Constants.Constants.passwordLibView);

            Assert.AreEqual(Constants.Constants.username, loginPage.GetUsernameText());
            Assert.AreEqual(Constants.Constants.passwordLibView, loginPage.GetPasswordText());
            loginPage.ClickloginButton();

        }

        /// <summary>
        /// Request the Auth Code.
        /// </summary>
        private void RequestForAuthTest()
        {

            var startTimestamp = DateTime.Now.Millisecond;
            var endTimstamp = startTimestamp + 10000;

            while (true)
            {
                try
                {
                    string pageLabel = verifyPage.GetWizardText();
                    Assert.AreEqual("2-Factor Authentication", pageLabel);
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
            verifyPage.ClicksendCodeButton();

        }

        /// <summary>
        /// Gets The Auth Code for logging into the account.
        /// </summary>
        /// <returns></returns>
        private void GetAuthCode()
        {
            //open new window for outlook login
            Driver.SwitchTo().NewWindow(WindowType.Tab);
            Driver.Navigate().GoToUrl(OutlookUrl);

            OutlookLogin outlookLogin = new OutlookLogin(Driver);
            // Enter Email & password. 
            outlookLogin.EnterUserName(Constants.Constants.username);
            Assert.AreEqual(Constants.Constants.username, outlookLogin.GetUserNameText());

            outlookLogin.ClicksigninButton();
            Thread.Sleep(2000);
            outlookLogin.EnterPassword(Constants.Constants.passwordOutlook);
            Assert.AreEqual(Constants.Constants.passwordOutlook, outlookLogin.GetPasswordText());
            Thread.Sleep(2000);

            outlookLogin.ClicksigninButton();
            outlookLogin.ClicksigninButton();


            outlookLogin.ClicklibreMessage();
            Thread.Sleep(2000);
            var message = outlookLogin.GetMessageText();
            var code = message.Split(" ")[4];
            VerifyButtonExists(code);

        }

        /// <summary>
        /// Verify if the Upload button exists after successfull login.
        /// </summary>
        /// <param name="code"></param>

        private void VerifyButtonExists(string code)
        {
            // Open a new window
            Driver.SwitchTo().Window(Driver.WindowHandles[0]);
            verifyPage.SetVerificationCode(code);

            Assert.AreEqual(code, verifyPage.GetCodeValueText());
            verifyPage.ClickverifyButton();
            var processupload = verifyPage.GetUploadButtonText();
            Assert.AreEqual("Upload a Device", processupload);

        }

    }
    #endregion
}