# YandexDiskNET
Simple client for use Yandex Disk in .NET

Supported Platforms:

	- .NET Framework 4.5+
	
	- .NET Standard 1.3 and .NET Standard 2.0; providing .NET Core support.	

Setup

Run "Install-Package YandexDiskNET" in Package Manager Console Visual Studio

or 

Clone this repo https://github.com/rsarov/YandexDiskNET.git

Build project

Add reference to YandexDiskNET.dll

usage example for Visual Studio 2015
```c#
using System;
using YandexDiskNET;

namespace ConsoleAppYandexDiskTest
{
    class Program
    {
        static void Main(string[] args)
        {
	    const string clientId = "";
            const string clientSecret = "";
            const string callbackUri = "Require://token";
            string pathCredentials = @"E:\Downloads\credit";
            YandexDiskOAuth.GetConfirmationCode(clientId, clientSecret, callbackUri, pathCredentials, 20).Wait();
            var token = YandexDiskOAuth.GetAccessToken(pathCredentials);
            YandexDiskRest disk = new YandexDiskRest(token);
            var info = disk.GetDiskInfo();           
            Console.Read();
        }
    }
}
```
More samples https://github.com/rsarov/YandexDiskNET/wiki
