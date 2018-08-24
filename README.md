# YandexDiskNET
Simple client for use Yandex Disk in .NET

First clone this repo https://github.com/rsarov/YandexDiskNET.git

Build project

Add reference to YandexDiskNET.dll

usage example for Visual Studio 2015
```c#
using System;
using YandexDiskRestClient;

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
