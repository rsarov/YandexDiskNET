# YandexDiskClient
Simple client for use Yandex Disk

First clone this repo https://github.com/rsarov/YandexDiskClient.git

Build project

Add reference to YandexDiskClient.dll

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
