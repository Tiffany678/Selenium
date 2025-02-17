﻿
using SeleniumUtilities.Utils;
using OpenQA.Selenium;
using System.Drawing;
using OpenQA.Selenium.Support.UI;
using IdlingComplaints.Models.Register;


namespace IdlingComplaints.Tests.Register
{
    //[Parallelizable(ParallelScope.Children)]
    //[FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
    internal class Test10_RegistrationFunctionality : RegisterModel
    {
        private readonly int SLEEP_TIMER = 2000;
        private string Registered_EmailAddress = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName + "\\Files\\Text\\Registered_EmailAddress.txt";

        [SetUp]
        public void SetUp()
        {
            base.RegisterModelSetUp(false);
            Driver.Manage().Window.Size = new Size(1920, 1080);
        }

        [TearDown]
        public void TearDown()
        {
            if(SLEEP_TIMER>0) Thread.Sleep(SLEEP_TIMER);
            //base.RegisterModelTearDown();
        }

        [Test, Category("Scenario test#1: New user with all random text input")]
        public void RandomtextRegistrtration()
        {
            FirstNameControl.SendKeysWithDelay(RegistrationUtilities.GenerateRandomString(), SLEEP_TIMER);
            LastNameControl.SendKeysWithDelay(RegistrationUtilities.GenerateRandomString(), SLEEP_TIMER);

            string generatedEmail = RegistrationUtilities.GenerateRandomString();
            EmailControl.SendKeysWithDelay(generatedEmail, SLEEP_TIMER);
            
            string password = RegistrationUtilities.GenerateQualifiedPassword();
            PasswordControl.SendKeysWithDelay(password, SLEEP_TIMER);
            ConfirmPasswordControl.SendKeysWithDelay(password, SLEEP_TIMER);
            
            int securityRandomNumber = RegistrationUtilities.GenerateRandomNumberWithRange(1, 5);
            SelectSecurityQuestion(securityRandomNumber);
            
            string securityAnswer = RegistrationUtilities.GenerateRandomString();
            SecurityAnswerControl.SendKeysWithDelay(securityAnswer, SLEEP_TIMER);
            ScrollToButton();

            Address1Control.SendKeysWithDelay(RegistrationUtilities.GenerateRandomString(), SLEEP_TIMER);
            Address2Control.SendKeysWithDelay(RegistrationUtilities.GenerateRandomString(), SLEEP_TIMER);
            CityControl.SendKeysWithDelay(RegistrationUtilities.GenerateRandomString(), SLEEP_TIMER);
            
            int stateRandomNumber = RegistrationUtilities.GenerateRandomNumberWithRange(1, 49);
            Console.WriteLine("The state number is " + stateRandomNumber + " . And the State selected is " + StateControl);
            SelectState(stateRandomNumber);
            
           
            string zipCodeNumbers = RegistrationUtilities.GenerateRandomString();
            ZipCodeControl.SendKeysWithDelay(zipCodeNumbers, SLEEP_TIMER);
        
          string TelephoneNumbers = RegistrationUtilities.GenerateRandomString();
          TelephoneControl.SendKeysWithDelay(TelephoneNumbers, SLEEP_TIMER);
         
          ClickSubmitButton();
            
            var snackBarError =Driver.WaitUntilElementFound(By.TagName("simple-snack-bar"), 61).FindElement(By.TagName("span")); ;

           RegistrationUtilities.WriteIntoFile(Registered_EmailAddress, generatedEmail, password, securityAnswer);
          Console.WriteLine("The new user is "+ generatedEmail);
            Assert.That(snackBarError.Text.Trim(), Contains.Substring("Registration has been completed successfully"), "Flagged for inconsistency on purpose."); //Added period for consistency with other error messaging
        }

        [Test, Category("Scenario test#2: Registration with a exiting account")]
        public void FailedRegistrationDrtupEmail()
        {
            FirstNameControl.SendKeysWithDelay("Registered", SLEEP_TIMER);
            LastNameControl.SendKeysWithDelay("User", SLEEP_TIMER);
            EmailControl.SendKeysWithDelay("", SLEEP_TIMER);
            PasswordControl.SendKeysWithDelay("Testing@1234", SLEEP_TIMER);
            ConfirmPasswordControl.SendKeysWithDelay("Testing@1234", SLEEP_TIMER);
            SelectSecurityQuestion(1);
            SecurityAnswerControl.SendKeysWithDelay("Testing", SLEEP_TIMER);
            Address1Control.SendKeysWithDelay("XXXXX", SLEEP_TIMER);
            Address2Control.SendKeysWithDelay("XXXXX", SLEEP_TIMER);
            CityControl.SendKeysWithDelay("XXXXX", SLEEP_TIMER);
            SelectState(1);
            ZipCodeControl.SendKeysWithDelay("00000", SLEEP_TIMER);
            TelephoneControl.SendKeysWithDelay("000-000-0000", SLEEP_TIMER);
            ScrollToButton();
            ClickSubmitButton();

            //var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            //wait.Until(d =>
            //{
            //    var snackBarError = d.FindElement(By.TagName("simple-snack-bar")).FindElement(By.TagName("span"));
            //    Assert.IsNotNull(snackBarError);
            //    Assert.That(snackBarError.Text.Trim(), Is.EqualTo("Email " + EmailInput + " has already been registered. Please contact DEP hotline."));
            //    return snackBarError;
            //});

            var snackBarError = Driver.WaitUntilElementFound(By.TagName("simple-snack-bar"),10).FindElement(By.TagName("span"));
            Assert.IsNotNull(snackBarError);
            Assert.That(snackBarError.Text.Trim(), Is.EqualTo("Email " + EmailInput + " has already been registered. Please contact DEP hotline."));

        }

        [Test, Category("Scenario test#3: Cancel register")]
        public void CancelRegistrationRedirectsToLogin()
        {
            ScrollToButton();
            ClickCancelButton();
            //var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10)); //1 - too short
            //wait.Until(d => d.FindElement(By.TagName("h3")));

            Driver.WaitUntilElementFound(By.TagName("h3"), 10);
        }

        [Test, Category("Scenario test#4: Cancel register after fill in all the input fields")]
        public void CancelRegistrationFullFormRedirectsToLogin()
        {
            FirstNameControl.SendKeysWithDelay("Jane", SLEEP_TIMER);
            LastNameControl.SendKeysWithDelay("Doe", SLEEP_TIMER);
            EmailControl.SendKeysWithDelay("XXXXX@gmail.com", SLEEP_TIMER);
            PasswordControl.SendKeysWithDelay("T3sting@1234", SLEEP_TIMER);
            ConfirmPasswordControl.SendKeysWithDelay("T3sting@1234", SLEEP_TIMER);
            SelectSecurityQuestion(1);
            SecurityAnswerControl.SendKeysWithDelay("Testing", SLEEP_TIMER);
            Address1Control.SendKeysWithDelay("XXXXX", SLEEP_TIMER);
            Address2Control.SendKeysWithDelay("XXXXX", SLEEP_TIMER);
            CityControl.SendKeysWithDelay("XXXXX", SLEEP_TIMER);
            SelectState(1);
            ZipCodeControl.SendKeysWithDelay("00000", SLEEP_TIMER);
            TelephoneControl.SendKeysWithDelay("000-000-0000", SLEEP_TIMER);
            ScrollToButton();
            ClickCancelButton();

            //Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            //var loginButton = Driver.FindElement(By.XPath("/html/body/app-root/div/app-login/mat-card/mat-card-content/form/div[3]/button"));
            //Assert.IsNotNull(loginButton);

            var loginButton = Driver.WaitUntilElementFound(By.XPath("//app-login/mat-card/mat-card-content/form/div[3]/button"),10);
            Assert.IsNotNull(loginButton);

        }


    }
}
