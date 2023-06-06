using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

namespace AbottTest
{
    public class VerifyIdentityPage : DriverHelper
    {
        public VerifyIdentityPage(IWebDriver driver)
        {
            Driver = driver;
        }

        IWebElement wizheader => Driver.FindElement(By.Id("wizardCard-header-text"));

        IWebElement sendCodeButton => Driver.FindElement(By.Id("twoFactor-step1-next-button"));

        IWebElement verificationcode => Driver.FindElement(By.Id("twoFactor-step2-code-input"));
        IWebElement verifyButton => Driver.FindElement(By.Id("twoFactor-step2-next-button"));

        IWebElement processupload => Driver.FindElement(By.Id("uploadCard-upload-button"));

        public void ClicksendCodeButton() => sendCodeButton.Click();

        internal string GetWizardText()
        {
            return wizheader.Text;
        }

        public void ClickverifyButton() => verifyButton.Click();

        internal void SetVerificationCode(string code)
        {
            verificationcode.SendKeys(code);
        }

        internal string GetUploadButtonText()
        {
            return processupload.Text;
        }

        internal object GetCodeValueText()
        {
            return verificationcode.GetAttribute("value");
        }
    }
}
