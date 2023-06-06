using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

namespace AbottTest
{
    public class RegionSelectionPage : DriverHelper
    {
        public RegionSelectionPage(IWebDriver driver)
        {
            Driver = driver;
        }

        IWebElement closeButton => Driver.FindElement(By.Id("truste-consent-close"));

        IWebElement submitbutton => Driver.FindElement(By.Id("submit-button"));

        IWebElement loginText => Driver.FindElement(By.Id("login-title-text"));

        public void ClickClose() => closeButton.Click();

        public void ClickSubmit() => submitbutton.Click();

        public string GetLoginText() => loginText.Text;
    }
}
