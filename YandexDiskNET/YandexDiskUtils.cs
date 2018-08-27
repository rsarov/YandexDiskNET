using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Handlers;
using System.Threading.Tasks;

namespace YandexDiskNET
{
    /// <summary>
    /// Utils for YandexDiskNET
    /// </summary>
    internal class YandexDiskUtils
    {

        /// <summary>
        /// Download file from target url
        /// </summary>
        /// <param name="url">URL for download</param>
        /// <param name="destFileName">Destination file name</param>        
        /// <returns>True if download complete successful</returns>
        internal static bool DownloadFile(string url, string destFileName)
        {
            bool success = false;            
            HttpClient client = HttpClientFactory.Create();

            try
            {                             
                HttpResponseMessage response = client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result;
                if(response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    using (Stream stream = response.Content.ReadAsStreamAsync().Result)
                    {
                        using (FileStream file = new FileStream(destFileName, FileMode.Create, FileAccess.Write))
                        {
                            stream.CopyTo(file);
                            success = true;
                        }
                    }
                }                        
            }
            catch (Exception)
            {
                return success;
            }

            return success;
        }


        /// <summary>
        /// Download file from target url asynchronous
        /// </summary>
        /// <param name="url">URL for download</param>
        /// <param name="destFileName">Destination file name</param>
        /// <param name="progress">Function for display progress download</param>
        /// <returns>True if download complete successful</returns>
        internal static async Task<bool> DownloadFileAsync(string url, string destFileName, IProgress<double> progress = null)
        {
            bool success = false;
            ProgressMessageHandler progressHandler = new ProgressMessageHandler();
            HttpClient client = HttpClientFactory.Create(progressHandler);

            try
            {
                //Delete if exist
                using (FileStream file = new FileStream(destFileName, FileMode.Create, FileAccess.Write))
                {
                    file.Close();
                }

                progressHandler.HttpReceiveProgress += (send, value) => progress?.Report(value.ProgressPercentage);
                HttpResponseMessage response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);              

                using (Stream stream = await response.Content.ReadAsStreamAsync())
                {                   
                    byte[] buf = new byte[4096];
                    bool isMoreToRead = true;

                    do
                    {
                        int read = await stream.ReadAsync(buf, 0, buf.Length);
                        if (read != 0)
                        {
                            byte[] write = new byte[read];
                            buf.ToList().CopyTo(0, write, 0, read);
                            using (FileStream file = new FileStream(destFileName, FileMode.Append, FileAccess.Write))
                            {
                                file.Write(write, 0, read);
                            }                            
                        }
                        else
                        {
                            isMoreToRead = false;
                        }
                    } while (isMoreToRead);
                }
                success = true;
            }
            catch (Exception)
            {
                return success;
            }

            return success;
        }


        /// <summary>
        /// Upload file from target path
        /// </summary>
        /// <param name="url">URL for upload</param>
        /// <param name="sourceFileName">Target file name for upload</param>        
        /// <returns>True if upload complete successful</returns>
        internal static bool UploadFile(string url, string sourceFileName)
        {
            bool success = false;            
            HttpClient client = HttpClientFactory.Create();

            try
            {
                using (StreamContent streamContent = new StreamContent(new FileStream(sourceFileName, FileMode.Open, FileAccess.Read)))
                {                    
                    HttpResponseMessage response = client.PutAsync(url, streamContent).Result;
                    if (response.StatusCode == System.Net.HttpStatusCode.Created)
                        success = true;
                }
            }
            catch (Exception)
            {
                return success;
            }

            return success;
        }


        /// <summary>
        /// Upload file from target path asynchronous
        /// </summary>
        /// <param name="url">URL for upload</param>
        /// <param name="sourceFileName">Target file name for upload</param>
        /// <param name="progress">Function for display progress upload</param>
        /// <returns>True if upload complete successful</returns>
        internal static async Task<bool> UploadFileAsync(string url, string sourceFileName, IProgress<double> progress = null)
        {
            bool success = false;                 
            ProgressMessageHandler progressHandler = new ProgressMessageHandler();
            HttpClient client = HttpClientFactory.Create(progressHandler);

            try
            {
                using (StreamContent streamContent = new StreamContent(new FileStream(sourceFileName, FileMode.Open, FileAccess.Read)))
                {
                    progressHandler.HttpSendProgress += (send, value) => progress?.Report(value.ProgressPercentage);
                    HttpResponseMessage response = await client.PutAsync(url, streamContent);
                    if (response.StatusCode == System.Net.HttpStatusCode.Created)
                        success = true;
                }
            }
            catch (Exception)
            {
                return success;
            }          
          
            return success;
        }        
    }
}
