using System;
using System.Threading;
using System.Net.Http;

using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace YandexDiskNET
{
    /// <summary>
    /// Implementation REST API Yandex Disk for .Net
    /// </summary>
    public class YandexDiskRest
    {
        /// <summary>
        /// Initialize API Disk
        /// </summary>
        /// <param name="oauth">OAuth token</param>
        public YandexDiskRest(string oauth)
        {
            Oauth = oauth;
        }


        /// <summary>
        ///  Authorization token
        /// </summary>
        private string Oauth { get; set; }


        #region Service for URL


        /// <summary>
        /// Yandex Disk query
        /// </summary>
        private enum YandexDiskAsk
        {
            /// <summary>
            /// Asynchronous operation
            /// </summary>
            Asynchronous_operation,


            /// <summary>
            /// Get meta information about disk 
            /// </summary>
            Get_meta_information_about_disk_user,


            /// <summary>
            /// Get meta information about file or folder
            /// </summary>
            Get_meta_information_about_file_or_folder,


            /// <summary>
            /// Get list all files order by name
            /// </summary>
            Get_list_files_order_by_name,


            /// <summary>
            /// Get list all files order by date download
            /// </summary>
            Get_list_files_order_by_download_date,


            /// <summary>
            /// Get list all public resources
            /// </summary>
            Get_list_public_resources,


            /// <summary>
            /// Create folder
            /// </summary>
            Create_folder,


            /// <summary>
            /// Delete file or folder 
            /// </summary>
            Remove_file_or_folder,


            /// <summary>
            /// Copy file or folder
            /// </summary>
            Copy_file_or_folder,


            /// <summary>
            /// Move file or folder
            /// </summary>
            Move_file_or_folder,


            /// <summary>
            /// On resource as public
            /// </summary>
            Public_resource_on,


            /// <summary>
            /// Off resource as public
            /// </summary>
            Public_resource_off,


            /// <summary>
            /// Get link for download file with disk
            /// </summary>
            Get_link_downloadable_file,


            /// <summary>
            /// Get link for upload file on disk
            /// </summary>
            Get_link_uploadable_file,


            /// <summary>
            /// Clear Trash
            /// </summary>
            Clear_trash,


            /// <summary>
            /// Get Trash resources
            /// </summary>
            Get_trash_content,


            /// <summary>
            /// Recover resource from Trash
            /// </summary>
            Recover_resource_from_trash
        }


        /// <summary>
        /// Get the status of an asynchronous operation
        /// </summary>
        /// <param name="link">Url asynchronous operation</param>
        /// <returns>The status of an asynchronous operation</returns>
        private StatusAsync GetAsyncStatus(string link)
        {
            const string success = "success";// The completion status of the asynchronous operation 'completed successfully'
            const string progress = "in-progress";// The execution status of the asynchronous operation 'in progress'

            string content;
            string status;
            StatusAsync statusAsync = StatusAsync.Failed;
            Param param = new Param();

            param.Link = link;

            content = CommandDisk(Oauth, YandexDiskAsk.Asynchronous_operation, param);

            if (content != null)
            {
                JObject json = JObject.Parse(content);
                status = (string)json.SelectToken("status");
                if (status == success)
                    statusAsync = StatusAsync.Success;
                else if (status == progress)
                    statusAsync = StatusAsync.InProgress;
                else
                    statusAsync = StatusAsync.Failed;
            }

            return statusAsync;
        }


        /// <summary>
        /// Await asynchronous operation complete 
        /// </summary>
        /// <param name="content">String json for deserialization</param>
        private void AwaitAsyncComplete(string content)
        {
            string link;
            StatusAsync status = StatusAsync.InProgress;

            JObject json = JObject.Parse(content);
            link = (string)json.SelectToken("href");

            if (link != null)
            {
                while (status == StatusAsync.InProgress)
                {
                    Thread.Sleep(1000);
                    status = GetAsyncStatus(link);
                    if (status != StatusAsync.InProgress)
                        break;
                }
            }
        }


        /// <summary>
        /// Send request to Yandex disk and return json string response or null if failure
        /// </summary>
        /// <param name="oauth">String  authorization token</param>
        /// <param name="diskAsk">Request to Yandex disk</param>        
        /// <param name="param">Ask parameters</param>
        /// <returns>Json string response or string empty if failure</returns>
        private string CommandDisk(string oauth, YandexDiskAsk diskAsk, Param param)
        {
            string content = null;
            string url;

            HttpMethod method = HttpMethod.Get;
            HttpClient client = new HttpClient();
            UrlBuilder urlBuilder = new UrlBuilder(param);

            switch (diskAsk)
            {
                case YandexDiskAsk.Asynchronous_operation:
                    url = urlBuilder.Link;
                    break;
                case YandexDiskAsk.Get_meta_information_about_disk_user:
                    url = "https://cloud-api.yandex.net/v1/disk?" + urlBuilder.Fields;
                    break;
                case YandexDiskAsk.Get_meta_information_about_file_or_folder:
                    url = "https://cloud-api.yandex.net/v1/disk/resources?" + urlBuilder.Path + urlBuilder.Fields + urlBuilder.Limit +
                        urlBuilder.Offset + urlBuilder.Preview_crop + urlBuilder.Preview_size + urlBuilder.Sort;
                    break;
                case YandexDiskAsk.Get_list_files_order_by_name:
                    url = "https://cloud-api.yandex.net/v1/disk/resources/files?" + urlBuilder.Fields + urlBuilder.Limit + urlBuilder.Media_type +
                        urlBuilder.Offset + urlBuilder.Preview_crop + urlBuilder.Preview_size + urlBuilder.Sort;
                    break;
                case YandexDiskAsk.Get_list_files_order_by_download_date:
                    url = "https://cloud-api.yandex.net/v1/disk/resources/last-uploaded?" + urlBuilder.Fields + urlBuilder.Limit + urlBuilder.Media_type +
                        urlBuilder.Preview_crop + urlBuilder.Preview_size;
                    break;
                case YandexDiskAsk.Get_list_public_resources:
                    url = "https://cloud-api.yandex.net/v1/disk/resources/public?" + urlBuilder.Fields + urlBuilder.Limit + urlBuilder.Offset +
                        urlBuilder.Preview_crop + urlBuilder.Preview_size + urlBuilder.Type;
                    break;
                case YandexDiskAsk.Create_folder:
                    url = "https://cloud-api.yandex.net/v1/disk/resources?" + urlBuilder.Path;
                    method = HttpMethod.Put;
                    break;
                case YandexDiskAsk.Remove_file_or_folder:
                    url = "https://cloud-api.yandex.net/v1/disk/resources?" + urlBuilder.Path + urlBuilder.Force_async + urlBuilder.Permanently;
                    method = HttpMethod.Delete;
                    break;
                case YandexDiskAsk.Copy_file_or_folder:
                    url = "https://cloud-api.yandex.net/v1/disk/resources/copy?" + urlBuilder.From + urlBuilder.Path +
                        urlBuilder.Force_async + urlBuilder.Overwrite;
                    method = HttpMethod.Post;
                    break;
                case YandexDiskAsk.Move_file_or_folder:
                    url = "https://cloud-api.yandex.net/v1/disk/resources/move?" + urlBuilder.From + urlBuilder.Path +
                        urlBuilder.Force_async + urlBuilder.Overwrite;
                    method = HttpMethod.Post;
                    break;
                case YandexDiskAsk.Public_resource_on:
                    url = "https://cloud-api.yandex.net/v1/disk/resources/publish?" + urlBuilder.Path;
                    method = HttpMethod.Put;
                    break;
                case YandexDiskAsk.Public_resource_off:
                    url = "https://cloud-api.yandex.net/v1/disk/resources/unpublish?" + urlBuilder.Path;
                    method = HttpMethod.Put;
                    break;
                case YandexDiskAsk.Get_link_downloadable_file:
                    url = "https://cloud-api.yandex.net/v1/disk/resources/download?" + urlBuilder.Path;
                    break;
                case YandexDiskAsk.Get_link_uploadable_file:
                    url = "https://cloud-api.yandex.net/v1/disk/resources/upload?" + urlBuilder.Path + urlBuilder.Overwrite;
                    break;
                case YandexDiskAsk.Clear_trash:
                    url = "https://cloud-api.yandex.net/v1/disk/trash/resources?" + urlBuilder.Force_async + urlBuilder.Path;
                    method = HttpMethod.Delete;
                    break;
                case YandexDiskAsk.Get_trash_content:
                    url = "https://cloud-api.yandex.net/v1/disk/trash/resources?" + urlBuilder.Path + urlBuilder.Fields + urlBuilder.Limit +
                        urlBuilder.Offset + urlBuilder.Preview_crop + urlBuilder.Preview_size + urlBuilder.Sort;
                    break;
                case YandexDiskAsk.Recover_resource_from_trash:
                    url = "https://cloud-api.yandex.net/v1/disk/trash/resources/restore?" + urlBuilder.Path + urlBuilder.Force_async + urlBuilder.Name +
                        urlBuilder.Overwrite;
                    method = HttpMethod.Put;
                    break;
                default:
                    url = "https://cloud-api.yandex.net/v1/disk";
                    break;
            }

            try
            {
                HttpRequestMessage request = new HttpRequestMessage(method, url);
                request.Headers.Add("Authorization", "OAuth " + oauth);
                request.Headers.Add("Accept", "application/json");
                HttpResponseMessage response = client.SendAsync(request).Result;
                content = response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception)
            {
                throw new HttpRequestException();
            }
            finally
            {
                client.Dispose();
            }

            return content;
        }


        #endregion


        /// <summary>
        /// Get meta information about disk, by default return all fields     
        /// </summary>
        /// <param name="fields">Array returned fields . Example use 'new DiskFields[] {DiskFields.Used_space, DiskFields.Unlimited_autoupload_enabled, DiskFields.System_folders}'</param>
        /// <returns>DiskInfo structure</returns>
        public DiskInfo GetDiskInfo(DiskFields[] fields = null)
        {
            string content;
            DiskInfo diskInfo = new DiskInfo();
            Param param = new Param();

            if (fields != null)
            {
                foreach (DiskFields i in fields)
                    param.Fields += i + ",";
                param.Fields = param.Fields.Remove(param.Fields.Length - 1);
            }

            content = CommandDisk(Oauth, YandexDiskAsk.Get_meta_information_about_disk_user, param);

            if (content != null)
                diskInfo = DiskInfo.GetDiskInfo(content);

            return diskInfo;
        }


        /// <summary>
        /// Get meta information about file or folder, dy default get meta information root disk
        /// </summary>    
        /// <param name="limit">Maximum number of output resources, default 20</param>
        /// <param name="path">Path to resource. Required value in Url - format. Example 'old/new/good.txt'</param>
        /// <param name="sort">A field for sorting the nested resources. The example 'SortField.Created' is sorted by the date of creation of the resource</param>
        /// <param name="fields">An array of return fields. Example 'new ResFields [] {ResFields.Name, ResFields.Type, ResFields.Size}'</param>
        /// <param name="offset">Offset from the beginning of the list of nested resources.</param>
        /// <param name="preview_crop">Allow trimming the preview.</param>
        /// <param name="preview_size">The size of the preview. Example "120x240"</param>
        /// <returns>The return value of the ResInfo structure</returns>
        public ResInfo GetResInfo(int? limit = null, string path = "", SortField? sort = null, ResFields[] fields = null,
            int? offset = null, bool? preview_crop = null, string preview_size = null)
        {
            string content;
            ResInfo resInfo = new ResInfo();
            Param param = new Param();

            param.Path = path;
            param.Limit = limit;

            if (sort != null)
                param.Sort = Enum.GetName(typeof(SortField), sort);

            if (fields != null)
            {
                foreach (ResFields i in fields)
                    if (i != ResFields._Embedded)
                        param.Fields += i + "," + "_embedded.items." + i + ",";
                param.Fields = param.Fields.Remove(param.Fields.Length - 1);
            }

            param.Offset = offset;
            param.Preview_crop = preview_crop;
            param.Preview_size = preview_size;

            content = CommandDisk(Oauth, YandexDiskAsk.Get_meta_information_about_file_or_folder, param);

            if (content != null)
                resInfo = ResInfo.GetResInfo(content);

            return resInfo;
        }


        /// <summary>
        /// Get a list of files ordered by name (structure ResInfo._Embedded), a flat list of all files in the Drive in alphabetical order.
        /// </summary> 
        /// <param name="limit">Maximum number of output resources, default 20</param>
        /// <param name="media_type">An array of file types to include in the list. Example 'new Media_type [] {Media_type.Unknown, Media_type.Text}'</param>
        /// <param name="sort">A field for sorting the nested resources. Example 'SortField.Size' sort by file size</param>
        /// <param name="fields">An array of return fields. Example 'new ResFields [] {ResFields.Name, ResFields.Type, ResFields.Size}'</param>
        /// <param name="offset">Offset from the beginning of the list of nested resources.</param>
        /// <param name="preview_crop">Allow trimming the preview.</param>
        /// <param name="preview_size">The size of the preview. Example "120x240"</param>
        /// <returns>The return value of the ResInfo._Embedded structure</returns>
        public ResInfo GetResourceByName(int? limit = null, Media_type[] media_type = null, SortField? sort = null, ResFields[] fields = null,
            int? offset = null, bool? preview_crop = null, string preview_size = null)
        {
            string content;
            ResInfo resInfo = new ResInfo();
            Param param = new Param();

            if (media_type != null)
            {
                foreach (Media_type i in media_type)
                    param.Media_type += i + ",";
                param.Media_type = param.Media_type.Remove(param.Media_type.Length - 1);
            }

            param.Limit = limit;

            if (sort != null)
                param.Sort = Enum.GetName(typeof(SortField), sort);

            if (fields != null)
            {
                foreach (ResFields i in fields)
                    if (i != ResFields._Embedded)
                        param.Fields += "items." + i + ",";
                param.Fields = param.Fields.Remove(param.Fields.Length - 1);
            }

            param.Offset = offset;
            param.Preview_crop = preview_crop;
            param.Preview_size = preview_size;

            content = CommandDisk(Oauth, YandexDiskAsk.Get_list_files_order_by_name, param);

            if (content != null)
                resInfo = ResInfo.GetResInfo(content, true);

            return resInfo;
        }


        /// <summary>
        /// Get a list of files ordered by the download date (structure ResInfo._Embedded), a flat list of all the files in the Drive.
        /// </summary>  
        /// <param name="limit">Maximum number of output resources, default 20</param>   
        /// <param name="media_type">An array of file types to include in the list. Example 'new Media_type [] {Media_type.Unknown, Media_type.Text}'</param>
        /// <param name="fields">An array of return fields. Example 'new ResFields [] {ResFields.Name, ResFields.Type, ResFields.Size}'</param>        
        /// <param name="preview_crop">Allow trimming the preview.</param>
        /// <param name="preview_size">The size of the preview. Example "120x240"</param>
        /// <returns>The return value of the ResInfo._Embedded structure</returns>
        public ResInfo GetResourceByDate(int? limit = null, Media_type[] media_type = null, ResFields[] fields = null, bool? preview_crop = null, string preview_size = null)
        {
            string content;
            ResInfo resInfo = new ResInfo();
            Param param = new Param();

            if (media_type != null)
            {
                foreach (Media_type i in media_type)
                    param.Media_type += i + ",";
                param.Media_type = param.Media_type.Remove(param.Media_type.Length - 1);
            }

            if (fields != null)
            {
                foreach (ResFields i in fields)
                    if (i != ResFields._Embedded)
                        param.Fields += "items." + i + ",";
                param.Fields = param.Fields.Remove(param.Fields.Length - 1);
            }

            param.Limit = limit;
            param.Preview_crop = preview_crop;
            param.Preview_size = preview_size;

            content = CommandDisk(Oauth, YandexDiskAsk.Get_list_files_order_by_download_date, param);

            if (content != null)
                resInfo = ResInfo.GetResInfo(content, true);

            return resInfo;
        }


        /// <summary>
        /// Get the list of published resources (structure ResInfo._Embedded), a flat list of all the list of published resources in the User's disk.
        /// </summary>   
        /// <param name="limit">Maximum number of output resources, default 20</param>        
        /// <param name="type">The type of resource to include in the list. Example 'TypeRes.Dir', folders only</param>
        /// <param name="fields">An array of return fields. Example 'new ResFields [] {ResFields.Name, ResFields.Type, ResFields.Size}'</param>
        /// <param name="offset">Offset from the beginning of the list of nested resources.</param>
        /// <param name="preview_crop">Allow trimming the preview.</param>
        /// <param name="preview_size">The size of the preview. Example "120x240"</param>
        /// <returns>The return value of the ResInfo._Embedded structure</returns>
        public ResInfo GetResourcePublic(int? limit = null, TypeRes? type = null, ResFields[] fields = null, int? offset = null, bool? preview_crop = null, string preview_size = null)
        {
            string content;
            ResInfo resInfo = new ResInfo();
            Param param = new Param();

            if (type != null)
                param.Type = Enum.GetName(typeof(TypeRes), type);

            param.Limit = limit;

            if (fields != null)
            {
                foreach (ResFields i in fields)
                    if (i != ResFields._Embedded)
                        param.Fields += "items." + i + ",";
                param.Fields = param.Fields.Remove(param.Fields.Length - 1);
            }

            param.Offset = offset;
            param.Preview_crop = preview_crop;
            param.Preview_size = preview_size;

            content = CommandDisk(Oauth, YandexDiskAsk.Get_list_public_resources, param);

            if (content != null)
                resInfo = ResInfo.GetResInfo(content, true);

            return resInfo;
        }


        /// <summary>
        /// Create folder
        /// </summary>
        /// <param name="path">The path to the created folder. The path in the parameter value should be encoded in the URL format. Example 'old/new'</param>       
        /// <returns>The return value of the ErrorResponse structure with null values if successful, else error message</returns>
        public ErrorResponse CreateFolder(string path)
        {
            string content;
            Param param = new Param();
            ErrorResponse errorResponse = new ErrorResponse();

            param.Path = path;
            content = CommandDisk(Oauth, YandexDiskAsk.Create_folder, param);

            if (content != null)
                errorResponse = errorResponse.GetError(content);

            return errorResponse;
        }


        /// <summary>
        /// Delete the file or folder. By default, it will delete the resource in the Recycle Bin. To remove the resource without placing it in the Recycle Bin, specify the recycle = false parameter.
        /// </summary>
        /// <param name="path">The path to the file or folder. The path in the parameter value should be encoded in the URL format. Example 'old/test.txt'</param>       
        /// <param name="recycle">Remove resource to Recycle bin. Default is true</param>
        /// <returns>The return value of the ErrorResponse structure with null values if successful, else error message</returns>
        public ErrorResponse DeleteResource(string path, bool recycle = true)
        {            
            string content;
            Param param = new Param();
            ErrorResponse errorResponse = new ErrorResponse();            

            param.Path = path;
            param.Force_async = true;
            param.Permanently = !recycle;

            content = CommandDisk(Oauth, YandexDiskAsk.Remove_file_or_folder, param);

            if (content != null)
            {
                AwaitAsyncComplete(content);
                errorResponse = errorResponse.GetError(content);
            }

            return errorResponse;
        }


        /// <summary>
        /// Create a copy of a file or folder
        /// </summary>
        /// <param name="from">The path to the resource being copied. The path in the parameter value should be encoded in the URL format. Example 'old/test.txt'</param>
        /// <param name="path">The path to the resource being created. The path in the parameter value should be encoded in the URL format. Example 'old/new'</param>
        /// <param name="overwrite">Overwrite an existing resource.</param>
        /// <returns>The return value of the ErrorResponse structure with null values if successful, else error message</returns>
        public ErrorResponse CopyResource(string from, string path, bool overwrite = false)
        {            
            string content;
            Param param = new Param();
            ErrorResponse errorResponse = new ErrorResponse();            

            param.From = from;
            param.Path = path;
            param.Overwrite = overwrite;
            param.Force_async = true;

            content = CommandDisk(Oauth, YandexDiskAsk.Copy_file_or_folder, param);

            if (content != null)
            {
                AwaitAsyncComplete(content);
                errorResponse = errorResponse.GetError(content);
            }

            return errorResponse;
        }


        /// <summary>
        /// Move file or folder
        /// </summary>
        /// <param name="from">The path to the roaming resource. The path in the parameter value should be encoded in the URL format. Example 'old/test.txt'</param>
        /// <param name="path">The path to the resource being created. The path in the parameter value should be encoded in the URL format. Example 'old/new'</param>
        /// <param name="overwrite">Overwrite an existing resource.</param>
        /// <returns>The return value of the ErrorResponse structure with null values if successful, else error message</returns>
        public ErrorResponse MoveResource(string from, string path, bool overwrite = false)
        {           
            string content;
            Param param = new Param();
            ErrorResponse errorResponse = new ErrorResponse();            

            param.From = from;
            param.Path = path;
            param.Overwrite = overwrite;
            param.Force_async = true;

            content = CommandDisk(Oauth, YandexDiskAsk.Move_file_or_folder, param);

            if (content != null)
            {
                AwaitAsyncComplete(content);
                errorResponse = errorResponse.GetError(content);
            }

            return errorResponse;
        }


        /// <summary>
        /// Publish this resource
        /// </summary>
        /// <param name="path">The path to the public resource. The path in the parameter value should be encoded in the URL format. Example 'old/test.txt'</param>        
        /// <returns>The return value of the ErrorResponse structure with null values if successful, else error message</returns>
        public ErrorResponse PublicResource(string path)
        {
            string content;
            Param param = new Param();
            ErrorResponse errorResponse = new ErrorResponse();

            param.Path = path;
            content = CommandDisk(Oauth, YandexDiskAsk.Public_resource_on, param);

            if (content != null)
                errorResponse = errorResponse.GetError(content);

            return errorResponse;
        }


        /// <summary>
        /// Unpublish the resource
        /// </summary>
        /// <param name="path">The path to the public resource. The path in the parameter value should be encoded in the URL format. Example 'old/test.txt'</param>        
        /// <returns>The return value of the ErrorResponse structure with null values if successful, else error message</returns>
        public ErrorResponse UnpublicResource(string path)
        {
            string content;
            Param param = new Param();
            ErrorResponse errorResponse = new ErrorResponse();

            param.Path = path;
            content = CommandDisk(Oauth, YandexDiskAsk.Public_resource_off, param);

            if (content != null)
                errorResponse = errorResponse.GetError(content);

            return errorResponse;
        }


        /// <summary>
        /// Clear the Recycle Bin. By default, clear all the entire Trash.
        /// </summary>
        /// <param name="path">The path to the resource in the Recycle Bin. The path in the parameter value should be encoded in the URL format. Example 'old/test.txt'</param>
        /// <returns>The return value of the ErrorResponse structure with null values if successful, else error message</returns>
        public ErrorResponse ClearRecycle(string path = null)
        {            
            string content;
            Param param = new Param();
            ErrorResponse errorResponse = new ErrorResponse();            

            param.Path = path;
            param.Force_async = true;

            content = CommandDisk(Oauth, YandexDiskAsk.Clear_trash, param);

            if (content != null)
            {
                AwaitAsyncComplete(content);
                errorResponse = errorResponse.GetError(content);
            }

            return errorResponse;
        }


        /// <summary>
        /// Get the contents of the Recycle Bin (ResInfo structure), by default display all the information.
        /// </summary>  
        /// <param name="limit">Maximum number of output resources, default 20</param>
        /// <param name="path">Path to the resource. Path in the parameter value should be encoded in the URL format. Example 'old/new/good.txt'</param>
        /// <param name="sort">A field for sorting the nested resources. Example 'SortDeleted.Deleted' sort by date of deletion of the resource</param>
        /// <param name="fields">An array of return fields. Example 'new ResFields [] {ResFields.Name, ResFields.Type, ResFields.Size}'</param>
        /// <param name="offset">Offset from the beginning of the list of nested resources.</param>
        /// <param name="preview_crop">Allow trimming the preview.</param>
        /// <param name="preview_size">The size of the preview. Example "120x240"</param>
        /// <returns>The return value of the ResInfo structure</returns>
        public ResInfo GetRecycleInfo(int? limit = null, string path = null, SortDeleted? sort = null, ResFields[] fields = null, int? offset = null, bool? preview_crop = null, string preview_size = null)
        {
            string content;
            Param param = new Param();
            ResInfo resInfo = new ResInfo();

            param.Path = path;
            param.Limit = limit;
            if (sort != null)
                param.Sort = Enum.GetName(typeof(SortDeleted), sort);

            if (fields != null)
            {
                foreach (ResFields i in fields)
                    if (i != ResFields._Embedded)
                        param.Fields += i + "," + "_embedded.items." + i + ",";
                param.Fields = param.Fields.Remove(param.Fields.Length - 1);
            }

            param.Offset = offset;
            param.Preview_crop = preview_crop;
            param.Preview_size = preview_size;

            content = CommandDisk(Oauth, YandexDiskAsk.Get_trash_content, param);

            if (content != null)
                resInfo = ResInfo.GetResInfo(content);

            return resInfo;
        }


        /// <summary>
        /// Restore resource from Trash
        /// </summary>        
        /// <param name="path">The path to the resource in the Recycle Bin. Path in the parameter value should be encoded in the URL format. Example 'old/new/good.txt'</param>
        /// <param name="name">The name under which the resource will be restored. Default does not change. Path in the parameter value should be encoded in the URL format. Example 'old/new'</param>
        /// <param name="overwrite">Overwrite an existing resource.</param>
        /// <returns>The return value of the ErrorResponse structure with null values if successful, else error message</returns>
        public ErrorResponse RestoreResource(string path, string name = null, bool overwrite = false)
        {            
            string content;
            Param param = new Param();
            ErrorResponse errorResponse = new ErrorResponse();            

            param.Path = path;
            param.Restore_name = name;
            param.Overwrite = overwrite;
            param.Force_async = true;

            content = CommandDisk(Oauth, YandexDiskAsk.Recover_resource_from_trash, param);

            if (content != null)
            {
                AwaitAsyncComplete(content);
                errorResponse = errorResponse.GetError(content);
            }

            return errorResponse;
        }


        /// <summary>
        /// Download file or folder from Disk, if is folder is downloaded to zip archive 
        /// </summary>        
        /// <param name="sourceFileName">The path to the resource in the Disk. Path in the parameter value should be encoded in the URL format. Example 'old/new/good.txt'</param>
        /// <param name="destFileName">Destination file name</param>       
        /// <returns>The return value of the ErrorResponse structure with null values if successful, else error message</returns>
        public ErrorResponse DownloadResource(string sourceFileName, string destFileName)
        {
            string url;
            string content;            
            Param param = new Param();
            ErrorResponse errorResponse = new ErrorResponse();
            param.Path = sourceFileName;
                   
            content = CommandDisk(Oauth, YandexDiskAsk.Get_link_downloadable_file, param);

            if (content != null)
            {
                JObject json = JObject.Parse(content);
                url = (string)json.SelectToken("href");
                destFileName = YandexDiskUtils.DownloadFileAsync(url, destFileName).Result;                   
                errorResponse = errorResponse.GetError(content);
            }

            if (destFileName == null)
            {
                errorResponse.Description = "Connection error.";
                errorResponse.Error = "Http request exception.";                
                errorResponse.Message = "Download failure.";
            }                

            return errorResponse;           
        }


        /// <summary>
        /// Download file or folder from Disk as asynchronous, if is folder is downloaded to zip archive 
        /// </summary>        
        /// <param name="sourceFileName">The path to the resource in the Disk. Path in the parameter value should be encoded in the URL format. Example 'old/new/good.txt'</param>
        /// <param name="destFileName">Destination file name</param> 
        /// <param name="progress">Function for display progress download</param>
        /// <returns>The return value of the ErrorResponse structure with null values if successful, else error message</returns>
        public async Task<ErrorResponse> DownloadResourceAcync(string sourceFileName, string destFileName, IProgress<double> progress = null)
        {
            string url;
            string content;
            Param param = new Param();
            ErrorResponse errorResponse = new ErrorResponse();
            param.Path = sourceFileName;

            content = CommandDisk(Oauth, YandexDiskAsk.Get_link_downloadable_file, param);

            if (content != null)
            {
                JObject json = JObject.Parse(content);
                url = (string)json.SelectToken("href");
                destFileName = await YandexDiskUtils.DownloadFileAsync(url, destFileName, progress);
                errorResponse = errorResponse.GetError(content);
            }

            if (destFileName == null)
            {
                errorResponse.Description = "Connection error.";
                errorResponse.Error = "Http request exception.";
                errorResponse.Message = "Download failure.";
            }

            return errorResponse;
        }


        /// <summary>
        /// Upload file to Disk
        /// </summary>
        /// <param name="destFileName">The path to the resource in the Disk. Path in the parameter value should be encoded in the URL format. Example 'old/new/good.txt'</param>
        /// <param name="sourceFileName">Path to file on local computer.</param>
        /// <param name="overwrite">Overwrite existing file.</param> 
        /// <returns>The return value of the ErrorResponse structure with null values if successful, else error message</returns>
        public ErrorResponse UploadResource(string destFileName, string sourceFileName, bool overwrite = false)
        {
            bool success = false;            
            string url;
            string content;
            Param param = new Param();
            ErrorResponse errorResponse = new ErrorResponse();           

            param.Path = destFileName;
            param.Overwrite = overwrite;

            content = CommandDisk(Oauth, YandexDiskAsk.Get_link_uploadable_file, param);

            if (content != null)
            {              
                JObject json = JObject.Parse(content);
                url = (string)json.SelectToken("href");
                success = YandexDiskUtils.UploadFileAsync(url, sourceFileName).Result;
                errorResponse = errorResponse.GetError(content);
            }

            if (!success && errorResponse.Error == null)
            {
                errorResponse.Description = "Connection error.";
                errorResponse.Error = "Http request exception.";
                errorResponse.Message = "Upload failure.";
            }

            return errorResponse;
        }


        /// <summary>
        /// Upload file to Disk as asynchronous
        /// </summary>
        /// <param name="destFileName">The path to the resource in the Disk. Path in the parameter value should be encoded in the URL format. Example 'old/new/good.txt'</param>
        /// <param name="sourceFileName">Path to file on local computer.</param>
        /// <param name="overwrite">Overwrite existing file.</param>  
        /// <param name="progress">Function for display progress download</param>
        /// <returns>The return value of the ErrorResponse structure with null values if successful, else error message</returns>
        public async Task<ErrorResponse> UploadResourceAsync(string destFileName, string sourceFileName, bool overwrite = false, IProgress<double> progress = null)
        {
            bool success = false;
            string url;
            string content;
            Param param = new Param();
            ErrorResponse errorResponse = new ErrorResponse();

            param.Path = destFileName;
            param.Overwrite = overwrite;

            content = CommandDisk(Oauth, YandexDiskAsk.Get_link_uploadable_file, param);

            if (content != null)
            {
                JObject json = JObject.Parse(content);
                url = (string)json.SelectToken("href");
                success = await YandexDiskUtils.UploadFileAsync(url, sourceFileName, progress);
                errorResponse = errorResponse.GetError(content);
            }

            if (!success && errorResponse.Error == null)
            {
                errorResponse.Description = "Connection error.";
                errorResponse.Error = "Http request exception.";
                errorResponse.Message = "Upload failure.";
            }

            return errorResponse;
        }
    }
}

