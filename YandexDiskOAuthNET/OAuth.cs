using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace YandexDiskOAuthNET
{
    /// <summary>
    /// Implementation  OAuth-authorization for Yandex services
    /// </summary>
    public class OAuth
    {
        /// <summary>
        /// Run OAuth-authorization for Yandex services
        /// </summary>
        /// <param name="clientId">Id application</param>
        /// <param name="clientSecret">Password application</param>
        /// <param name="callbackUri">Redirect uri scheme, example 'Confirm://'</param>
        /// <param name="pathCredentials">Path for save access token</param>
        public static void GetConfirmationCode(string clientId, string clientSecret, string callbackUri, string pathCredentials)
        {
            if (SaveAppParameters(clientId, clientSecret, callbackUri, pathCredentials))
            {
                CreateUriScheme(callbackUri);
                string authorizationEdnpoint = String.Format("https://oauth.yandex.ru/authorize?response_type=code&client_id={0}", clientId);
                System.Diagnostics.Process.Start(authorizationEdnpoint);
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

        private enum CodeConfirmParam
        {
            /// <summary>
            /// Требуемый ответ. При запросе кода подтверждения следует указать значение «code». Обязательный.
            /// </summary>
            Response_type,

            /// <summary>
            /// Идентификатор приложения. Доступен в свойствах приложения (нажмите название приложения, чтобы открыть его свойства).Обязательный.
            /// </summary>
            Client_id,

            /// <summary>
            /// Уникальный идентификатор устройства, для которого запрашивается токен. Чтобы обеспечить уникальность, достаточно один раз сгенерировать UUID и использовать его при каждом запросе нового токена с данного устройства.
            /// </summary>
            Device_id,

            /// <summary>
            /// Имя устройства, которое следует показывать пользователям. Не длиннее 100 символов.
            /// </summary>
            Device_name,

            /// <summary>
            /// URL, на который нужно перенаправить пользователя после того, как он разрешил или отказал приложению в доступе. По умолчанию используется первый Callback URI, указанный в настройках приложения (Платформы → Веб-сервисы → Callback URI).
            /// </summary>
            Redirect_uri,

            /// <summary>
            /// Явное указание аккаунта, для которого запрашивается токен. В значении параметра можно передавать логин аккаунта на Яндексе, а также адрес Яндекс.Почты или Яндекс.Почты для домена.
            /// </summary>
            Login_hint,

            /// <summary>
            /// Список необходимых приложению в данный момент прав доступа, разделенных пробелом. Права должны запрашиваться из перечня, определенного при регистрации приложения.
            /// </summary>
            Scope,

            /// <summary>
            /// Список разделенных пробелом опциональных прав доступа, без которых приложение может обойтись.
            /// </summary>
            Optional_scope,

            /// <summary>
            /// Признак того, что у пользователя обязательно нужно запросить разрешение на доступ к аккаунту (даже если пользователь уже разрешил доступ данному приложению). 
            /// </summary>
            Force_confirm,

            /// <summary>
            /// Строка состояния, которую Яндекс.OAuth возвращает без изменения.
            /// </summary>
            State
        }

        private string ParseCodeConfirmParam(CodeConfirmParam param)
        {
            string result = null;

            switch (param)
            {
                case CodeConfirmParam.Response_type:
                    result = Enum.GetName(typeof(CodeConfirmParam), param);
                    break;
                case CodeConfirmParam.Client_id:
                    result = Enum.GetName(typeof(CodeConfirmParam), param);
                    break;
                case CodeConfirmParam.Device_id:
                    result = Enum.GetName(typeof(CodeConfirmParam), param);
                    break;
                case CodeConfirmParam.Device_name:
                    result = Enum.GetName(typeof(CodeConfirmParam), param);
                    break;
                case CodeConfirmParam.Redirect_uri:
                    result = Enum.GetName(typeof(CodeConfirmParam), param);
                    break;
                case CodeConfirmParam.Login_hint:
                    result = Enum.GetName(typeof(CodeConfirmParam), param);
                    break;
                case CodeConfirmParam.Scope:
                    result = Enum.GetName(typeof(CodeConfirmParam), param);
                    break;
                case CodeConfirmParam.Optional_scope:
                    result = Enum.GetName(typeof(CodeConfirmParam), param);
                    break;
                case CodeConfirmParam.Force_confirm:
                    result = Enum.GetName(typeof(CodeConfirmParam), param);
                    break;
                case CodeConfirmParam.State:
                    result = Enum.GetName(typeof(CodeConfirmParam), param);
                    break;
            }
            return result;
        }

        private string BuildCodeConfirmUri(string baseUri, CodeConfirmParam[] param, string[] values)
        {
            string uri = baseUri;

            try
            {
                for (int i = 0; i < param.Length; i++)
                    uri += param[i] + "=" + values[i] + "&";
                uri = uri.Remove(uri.Length - 1);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return uri;
        }


    }
}
