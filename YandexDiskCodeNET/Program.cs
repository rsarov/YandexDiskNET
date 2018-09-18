using System;
using System.IO;
using System.Text;
using System.Net.Http;
using Microsoft.Win32;
using System.Threading;
using System.Threading.Tasks;
using System.Security.AccessControl;
using System.Reflection;

namespace YandexDiskCodeNET
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] split;
            string path = Path.Combine(Path.GetTempPath(), "code");
            string clientId = null;
            string clientSecret = null;
            string callbackUri = null;
            string pathCredentials = null;

            if (File.Exists(path))
            {
                split = File.ReadAllText(path).Split('\u0004');
                clientId = split[0];
                clientSecret = split[1];
                callbackUri = split[2];
                pathCredentials = split[3];
            }
            else
            {
                Console.WriteLine("Не удалось получить токен доступа.");
                return;
            }                          

            if (args.Length == 1)
            {
                bool success = GetAccessToken(args[0], clientId, clientSecret, pathCredentials).Result;
                DeleteUriScheme(callbackUri);
                File.Delete(path);
                if (success)
                    Console.WriteLine("Tокен доступа сохранен, вернитесь в приложение.");
                else
                    Console.WriteLine("Не удалось получить токен доступа.");
                Thread.Sleep(3000);
            }
            else
                CreateUriScheme(callbackUri);            
        }

        private static async Task<bool> GetAccessToken(string codeConfirm, string clientId, string clientSecret, string pathCredentials)
        {
            codeConfirm = ParseCodeConfirm(codeConfirm);
            string requestUri = "https://oauth.yandex.ru/token";
            string requestBody = String.Format("grant_type=authorization_code&code={0}&client_id={1}&client_secret={2}",
                codeConfirm,
                clientId,
                clientSecret);
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri);
            byte[] data = Encoding.ASCII.GetBytes(requestBody);
            request.Content = new ByteArrayContent(data);
            request.Content.Headers.Add("Content-type", "application/x-www-form-urlencoded");
            request.Content.Headers.Add("Content-Length", data.Length.ToString());

            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.SendAsync(request);
                string body = await response.Content.ReadAsStringAsync();
                string token = "access_token";                
                Directory.CreateDirectory(pathCredentials);
                DirectoryInfo di = new DirectoryInfo(pathCredentials);
                di.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
                di.Create();
                pathCredentials += "\\" + token;
                File.WriteAllText(pathCredentials, body);

                return body.Contains(token) ? true : false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static string ParseCodeConfirm(string codeConfirm)
        {
            const string code = "code=";
            if (codeConfirm.Contains(code))
                return codeConfirm.Substring(codeConfirm.IndexOf("code") + code.Length);
            else
                return null;
        }

        private static void CreateUriScheme(string uriSchemeName)
        {
            const string trimm = "://";
            if (uriSchemeName.Contains(trimm))
                uriSchemeName = uriSchemeName.Remove(uriSchemeName.IndexOf(trimm));

            string app = "\"" + Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Получить код подтверждения.exe") + "\" \"%1\"";
            string user = Environment.UserDomainName + "\\" + Environment.UserName;
            RegistryAccessRule rule = new RegistryAccessRule(user, RegistryRights.FullControl, AccessControlType.Allow);
            RegistrySecurity rs = new RegistrySecurity();
            rs.AddAccessRule(rule);
            RegistryKey key = Registry.ClassesRoot.CreateSubKey(uriSchemeName, RegistryKeyPermissionCheck.ReadWriteSubTree, rs);
            key.SetValue("URL Protocol", "", RegistryValueKind.String);
            key = key.CreateSubKey(@"shell\\open\\command", RegistryKeyPermissionCheck.ReadWriteSubTree, rs);
            key.SetValue("", app);
        }

        private static void DeleteUriScheme(string uriSchemeName)
        {
            const string trimm = "://";
            if (uriSchemeName.Contains(trimm))
                uriSchemeName = uriSchemeName.Remove(uriSchemeName.IndexOf(trimm));
            try
            {
                Registry.ClassesRoot.DeleteSubKeyTree(uriSchemeName);
            }
            catch (Exception)
            {
                
            }
        }
    }
}
