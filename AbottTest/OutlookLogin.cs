using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace AbottTest
{
    public class OutlookLogin : DriverHelper
    {
        public OutlookLogin(IWebDriver driver)
        {
            Driver = driver;
        }


        IWebElement userNameInput => Driver.FindElement(By.Id("i0116"));
        IWebElement passwordInput => Driver.FindElement(By.Id("i0118"));
        IWebElement signinbutton => Driver.FindElement(By.Id("idSIButton9"));

        IWebElement libreMessage => Driver.FindElement(By.XPath("//*[contains(text(), 'LibreView Verification Code')]"));

        IWebElement message => Driver.FindElement(By.XPath("//*[@id=\"x_backgroundTable\"]/tbody/tr/td/table/tbody/tr/td/table/tbody/tr/td/table[2]/tbody/tr/td/table[2]/tbody/tr[2]/td"));


        public void ClicksigninButton() => signinbutton.Click();

        public void ClicklibreMessage() => libreMessage.Click();



        internal void EnterUserName(string username)
        {
            userNameInput.SendKeys(username);

        }

        internal void EnterPassword(string passwordOutlook)
        {
            passwordInput.SendKeys(passwordOutlook);

        }

        internal string GetMessageText()
        {
            return message.Text;
        }

        internal string GetUserNameText()
        {
            return userNameInput.GetAttribute("value");
        }

        internal string GetPasswordText()
        {
            return passwordInput.GetAttribute("value");
        }
    }
}
