using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace YandexDiskNET
{
    /// <summary>
    /// Implementation  OAuth-authorization for Yandex services
    /// </summary>
    public class YandexDiskOAuth
    {
        /// <summary>
        /// Run OAuth-authorization for Yandex services
        /// </summary>
        /// <param name="clientId">Id application</param>
        /// <param name="clientSecret">Password application</param>
        /// <param name="callbackUri">Redirect uri scheme, example 'Require://token'</param>
        /// <param name="pathCredentials">Path for save access token</param>
        /// <param name="timeout">Time waiting authorization by default 10 sec</param>
        public static async Task GetConfirmationCode(string clientId, string clientSecret, string callbackUri, string pathCredentials, int timeout = 10)
        {
            await Task.Run(() => {
                if (SaveAppParameters(clientId, clientSecret, callbackUri, pathCredentials))
                {
                    const string access_token = "access_token";
                    int count = 0;
                    string app = "Получить код подтверждения.exe";
                    System.Diagnostics.Process.Start(app);
                    app = String.Format("https://oauth.yandex.ru/authorize?client_id={0}&response_type=code", clientId);
                    System.Diagnostics.Process.Start(app);                    
                    string path = Path.Combine(pathCredentials, access_token);
                    while (count < timeout)
                    {
                        if (!File.Exists(path))
                        {
                            Thread.Sleep(1000);
                            count++;
                        } 
                        else
                            break;
                    }
                }
            });           
        }

        /// <summary>
        /// Get access token from storage
        /// </summary>
        /// <param name="pathCredentials">Folder of storage</param>
        /// <returns></returns>
        public static string GetAccessToken(string pathCredentials)
        {
            const string access_token = "access_token";
            string path = Path.Combine(pathCredentials, access_token);
            try
            {
                JObject json = JObject.Parse(File.ReadAllText(path));
                return (string)json.SelectToken(access_token);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static bool SaveAppParameters(string clientId, string clientSecret, string callbackUri, string pathCredentials)
        {
            try
            {
                string path = Path.Combine(Path.GetTempPath(), "code");
                string content = clientId + "\u0004" + clientSecret + "\u0004" + callbackUri + "\u0004" + pathCredentials;
                File.WriteAllText(path, content);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }        
    }
}
