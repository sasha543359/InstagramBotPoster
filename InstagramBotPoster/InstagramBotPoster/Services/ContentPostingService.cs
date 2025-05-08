using AutoItX3Lib;
using InstagramBotPoster.Models;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace InstagramBotPoster.Services;

internal class ContentPostingService
{
    private readonly BrowserProfile _profile;

    public ContentPostingService(BrowserProfile profile)
    {
        _profile = profile;
    }

    public void StartPosting(string filePath)
    {
        var chromeOptions = new ChromeOptions();

        chromeOptions.AddArgument($"--user-data-dir={_profile.ProfilePath}");
        chromeOptions.AddArgument($"--disk-cache-dir={_profile.CachePath}");
        chromeOptions.AddArgument("--window-size=2000,1300");
        chromeOptions.BinaryLocation = @"C:\Program Files\Google\Chrome\Application\chrome.exe";

        using var driver = new ChromeDriver(chromeOptions);

        try
        {
            driver.Navigate().GoToUrl("https://www.instagram.com/");
            PostContent(driver, filePath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при постинге: {ex.Message}");
        }
    }

    private void PostContent(ChromeDriver driver, string filePath)
    {
        AutoItX3 autoIt = new AutoItX3();

        try
        {
            driver.FindElement(By.XPath("//span[text()='Создать']")).Click();
            Thread.Sleep(1000);

            driver.FindElement(By.XPath("//span[text()='Публикация']")).Click();
            Thread.Sleep(1000);

            driver.FindElement(By.XPath("//button[text()='Выбрать на компьютере']")).Click();
            Thread.Sleep(1000);

            autoIt.WinWait("Открытие", "", 3);
            autoIt.Send(filePath);
            Thread.Sleep(500);
            autoIt.Send("{ENTER}");

            Thread.Sleep(1000);

            driver.FindElement(By.XPath("//div[text()='Далее']")).Click();
            Thread.Sleep(1000);

            driver.FindElement(By.XPath("//div[text()='Далее']")).Click();
            Thread.Sleep(1000);

            // driver.FindElement(By.XPath("//div[text()='Поделиться']")).Click();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при выполнении действий: {ex.Message}");
        }
    }
}
