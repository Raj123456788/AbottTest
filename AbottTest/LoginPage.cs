using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace AbottTest
{
    public class LoginPage : DriverHelper
    {

        public LoginPage(IWebDriver driver)
        {
            Driver = driver;
        }

        IWebElement usernameInput => Driver.FindElement(By.Id("loginForm-email-input"));
        IWebElement passwordInput => Driver.FindElement(By.Id("loginForm-password-input"));
        IWebElement loginButton => Driver.FindElement(By.Id("loginForm-submit-button"));


        public void ClickloginButton() => loginButton.Click();

        internal void EnterUsernamePassword(string username, string passwordLibView)
        {
            usernameInput.SendKeys(username);
            Thread.Sleep(1000);
            passwordInput.SendKeys(passwordLibView);
        }

        internal object GetUsernameText()
        {
            return usernameInput.GetAttribute("value");
        }

        internal object GetPasswordText()
        {
            return passwordInput.GetAttribute("value");
        }
    }
}
