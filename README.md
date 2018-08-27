# YandexDiskNET
Simple client for use Yandex Disk in .NET

Supported Platforms:

	- .NET Framework 4.5+
	
	- .NET Standard 1.3 and .NET Standard 2.0; providing .NET Core support.	

Setup

- Run "Install-Package YandexDiskNET" in Package Manager Console Visual Studio

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
            string oauth = "your token";
            YandexDiskRest disk = new YandexDiskRest(oauth);
            var inf = disk.GetDiskInfo();
            if(inf.User.Login == null)
                Console.WriteLine("No connect to disk.");
            else
                Console.WriteLine("Hello {0}", inf.User.Login);
            Console.Read();
        }
    }
}
```
More samples https://github.com/rsarov/YandexDiskNET/wiki
