## 1. Introduction
<p>This project demonstrates how to build a Selenium automation testing using C#.NET while following the Page Object Pattern.</p>
<p><strong>Note:</strong> The test cases in this project are not executable due to the removal of sensitive information, such as navigation links and login credentials. The purpose of this repository is to introduce the project structure and the implementation of the Page Object Pattern.</p>


## 2. Tools for Automated UI Testing

<p>Let's add a some dependencies to our project to execute the unit test. They can be installed with NuGet Package Manager.</p>

* <strong>Selenium WebDriver</strong>
    * Selenium WebDriver interacts with web elements.

* <strong>Browser Drivers</strong>
    * Download the appropriate browser drivers for the browsers you intend to test, such as ChromeDriver for Google Chrome,GeckoDriver for Mozilla Firefox, etc

* <strong>NUnit</strong>
    * NUnit provides built-in support for generating HTML reports.
    * The below image shows the NUnite generated test report for the web application.
      <img height="180" src="https://github.com/Tiffany678/Selenium/blob/main/IdlingComplaintTest3/Files/Images/ReadmeImage/Report.png" alt="Get request" width="500"/>


## 2.1. Additional Methods

<p>To initialize the web driver, we can create a method for the driver mode, like stander mode or headless mode.</p>

  ``` 
    protected IWebDriver CreateHeadlessDriver(string browserName)
       {
            string headless = "--headless=new";
            switch (browserName.ToLowerInvariant())
            {
                case "chrome":
                       var chromeOptions = new ChromeOptions();
                       chromeOptions.AddArguments(headless);
                       return new ChromeDriver(chromeOptions);
                case "firefox":
                       var firefoxOptions = new FirefoxOptions();
                       firefoxOptions.AddArguments(headless);
                       return new FirefoxDriver(firefoxOptions);
                case "edge":
                       var edgeOptions = new EdgeOptions();
                       edgeOptions.AddArguments(headless);
                       return new EdgeDriver(edgeOptions);
                 default:
                      throw new Exception("Provided browser is not supported.");
              }
    }
  ```


## 3. Page Object Pattern

<p>Before writing test cases, it’s essential to understand the Page Object Pattern, as it helps organize code within the project efficiently.</p>
<p>The Page Object Pattern is a design approach that enhances the structure of a test automation framework by separating test logic from UI interactions. This improves maintainability, reduces redundancy, and makes tests more readable and scalable.</p>

* Our Project Folder Structure Map <br/>
  <img height="550" src="https://github.com/Tiffany678/Selenium/blob/main/IdlingComplaintTest3/Files/Images/ReadmeImage/ProjectStructure.png" alt="Get request" width="650"/>

* Let’s go ahead and <b>create our page object</b> – in this case, our login page:
  ``` 
    namespace IdlingComplaints.Models.Login
    {
       internal class LoginModel : BaseModel
       {
          public IWebElement EmailControl => Driver.FindElement(By.Name("email"));
           // ...
          public string EmailInput
          {
              get
              {
                  return EmailControl.GetAttribute("value");
              }
              set
              {
                  EmailControl.SendKeys(value);
              }
           }
           // ...
       }
    }
  ```

* Let’s write a quick test, where we simply test the login functionality.
    ```
    private readonly int SLEEP_TIMER = 2000;
    private readonly string registered_EmailAddress = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName + "\\Files\\Text\\Registered_EmailAddress.txt";

    [Test, Category("Successful Login - Error Label Hidden")]
    public void LoginValidEmailAndPassword()
    {
        string[] lines = File.ReadAllLines(registered_EmailAddress);
        int userRowIndex = random.Next(0, lines.Length - 1);

        string email = RegistrationUtilities.RetrieveRecordValue(registered_EmailAddress, userRowIndex, 0);
        string password = RegistrationUtilities.RetrieveRecordValue(registered_EmailAddress, userRowIndex, 1);

        EmailControl.SendKeysWithDelay(email, SLEEP_TIMER);
        PasswordControl.SendKeysWithDelay(password, SLEEP_TIMER);
        ClickLoginButton();

        Driver.WaitUntilElementFound(By.CssSelector("button[routerlink='idlingcomplaint/new']"), 20);
      
        Driver.WaitUntilElementIsNoLongerFound(By.CssSelector("div[dir = 'ltr']"), 20);
    }
    ```

## 4. Consclusion

In this quick tutorial, we focused on improving our usage of Selenium/WebDriver with the help of the Page-Object Pattern. We went through different examples and implementations to see the practical ways of utilizing the pattern to interact with the testing website.

