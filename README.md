# YandexDiskClient
Simple client for use Yandex Disk

usage example
First clone this https://github.com/rsarov/YandexDiskClient.git
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
