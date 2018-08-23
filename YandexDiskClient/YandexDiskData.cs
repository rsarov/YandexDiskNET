using Newtonsoft.Json.Linq;
using System.Collections.Generic;

//This united representation answers Yandex Disk

namespace YandexDiskRestClient
{

    #region Structures for response


    /// <summary>
    /// Error response representation
    /// </summary>
    public struct ErrorResponse
    {
        /// <summary>
        /// Error message
        /// </summary>
        public string Message { get; set; }


        /// <summary>
        /// Description error
        /// </summary>
        public string Description { get; set; }


        /// <summary>
        /// Unique error code
        /// </summary>
        public string Error { get; set; }


        /// <summary>
        /// Deserialization json data to ErrorResponse
        /// </summary>
        /// <param name="content">String json</param>
        /// <returns>Structure ErrorResponse</returns>
        public ErrorResponse GetError(string content)
        {
            ErrorResponse response = new ErrorResponse();
            if (!string.IsNullOrEmpty(content))
            {
                JObject json = JObject.Parse(content);
                response.Message = (string)json.SelectToken("message");
                response.Description = (string)json.SelectToken("description");
                response.Error = (string)json.SelectToken("error");
            }
            return response;
        }
    }


    /// <summary>
    /// Meta information about disk 
    /// </summary>    
    public struct DiskInfo
    {
        /// <summary>
        /// Maximum file size.
        /// </summary>
        public long? Max_file_size { get; set; }


        /// <summary>
        /// Unlimited auto upload with mobile devices.
        /// </summary>
        public bool? Unlimited_autoupload_enabled { get; set; }


        /// <summary>
        /// Total size of disk (bytes)
        /// </summary>
        public long? Total_space { get; set; }


        /// <summary>
        /// Total size files in trash (bytes). Included in used space.
        /// </summary>
        public long? Trash_size { get; set; }


        /// <summary>
        /// Mark of  paid advanced place
        /// </summary>
        public bool? Is_paid { get; set; }


        /// <summary>
        /// Used space of disk (bytes)
        /// </summary>
        public long? Used_space { get; set; }


        /// <summary>
        /// List system folders
        /// </summary>
        public System_folders System_folders { get; set; }


        /// <summary>
        /// Disk  owner
        /// </summary>
        public User User { get; set; }


        /// <summary>
        /// Current disk revision
        /// </summary>
        public long? Revision { get; set; }


        /// <summary>
        /// Error response representation
        /// </summary>
        public ErrorResponse ErrorResponse { get; set; }


        /// <summary>
        /// Deserialization json data to DiskInfo
        /// </summary>
        /// <param name="content">String json for deserialization</param>
        /// <returns>Structure DiskInfo</returns>
        public static DiskInfo GetDiskInfo(string content)
        {
            DiskInfo diskInfo = new DiskInfo();
            System_folders system_folders = new System_folders();
            User user = new User();
            ErrorResponse errorResponse = new ErrorResponse();

            if (!string.IsNullOrEmpty(content))
            {
                JObject json = JObject.Parse(content);
                diskInfo.Max_file_size = (long?)json.SelectToken("max_file_size");
                diskInfo.Unlimited_autoupload_enabled = (bool?)json.SelectToken("unlimited_autoupload_enabled");
                diskInfo.Total_space = (long?)json.SelectToken("total_space");
                diskInfo.Trash_size = (long?)json.SelectToken("trash_size");
                diskInfo.Is_paid = (bool?)json.SelectToken("is_paid");
                diskInfo.Used_space = (long?)json.SelectToken("used_space");
                system_folders.Odnoklassniki = (string)json.SelectToken("system_folders.odnoklassniki");
                system_folders.Google = (string)json.SelectToken("system_folders.google");
                system_folders.Instagram = (string)json.SelectToken("system_folders.instagram");
                system_folders.Vkontakte = (string)json.SelectToken("system_folders.vkontakte");
                system_folders.Mailru = (string)json.SelectToken("system_folders.mailru");
                system_folders.Downloads = (string)json.SelectToken("system_folders.downloads");
                system_folders.Applications = (string)json.SelectToken("system_folders.applications");
                system_folders.Facebook = (string)json.SelectToken("system_folders.facebook");
                system_folders.Social = (string)json.SelectToken("system_folders.social");
                system_folders.Screenshots = (string)json.SelectToken("system_folders.screenshots");
                system_folders.Photostream = (string)json.SelectToken("system_folders.photostream");
                diskInfo.System_folders = system_folders;
                user.Login = (string)json.SelectToken("user.login");
                user.Uid = (string)json.SelectToken("user.uid");
                diskInfo.User = user;
                diskInfo.Revision = (long?)json.SelectToken("revision");
                diskInfo.ErrorResponse = errorResponse.GetError(content);
            }
            return diskInfo;
        }
    }


    /// <summary>
    /// System folders of disk
    /// </summary>
    public struct System_folders
    {
        /// <summary>
        /// Path to folder "Social net/Odnoklassniki".
        /// </summary>
        public string Odnoklassniki { get; set; }


        /// <summary>
        ///Path to folder "Social net/Google+".
        /// </summary>
        public string Google { get; set; }


        /// <summary>
        /// Path to folder "Social net/Instagram".
        /// </summary>
        public string Instagram { get; set; }


        /// <summary>
        /// Path to folder "Social net/Vkontakte".
        /// </summary>
        public string Vkontakte { get; set; }


        /// <summary>
        /// Path to folder "Social net/My world".
        /// </summary>
        public string Mailru { get; set; }


        /// <summary>
        /// Path to folder "Downloads".
        /// </summary>
        public string Downloads { get; set; }


        /// <summary>
        /// Path to folder "Applications".
        /// </summary>
        public string Applications { get; set; }


        /// <summary>
        /// Path to folder "Social net/Facebook".
        /// </summary>
        public string Facebook { get; set; }


        /// <summary>
        /// Path to folder "Social net".
        /// </summary>
        public string Social { get; set; }


        /// <summary>
        /// Path to folder "Screenshots".
        /// </summary>
        public string Screenshots { get; set; }


        /// <summary>
        /// Path to folder "Photo".
        /// </summary>
        public string Photostream { get; set; }
    }


    /// <summary>
    /// Disk owner data
    /// </summary>
    public struct User
    {
        /// <summary>
        /// Login
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// User id
        /// </summary>
        public string Uid { get; set; }
    }   


    /// <summary>
    /// Meta information about file or folder
    /// </summary>
    public struct ResInfo
    {
        /// <summary>
        /// Status antivirus check
        /// </summary>
        public string Antivirus_status { get; set; }


        /// <summary>
        /// Key public resource
        /// </summary>
        public string Public_key { get; set; }


        /// <summary>
        /// SHA256-hash
        /// </summary>
        public string Sha256 { get; set; }


        /// <summary>
        /// List nested resources
        /// </summary>
        public ResInfoList _Embedded { get; set; }


        /// <summary>
        /// Name resource
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// Meta information of media file
        /// </summary>
        public Exif Exif { get; set; }


        /// <summary>
        /// Resource id
        /// </summary>
        public string Resource_id { get; set; }


        /// <summary>
        /// User attributes of resource
        /// </summary>       
        public Custom_properties Custom_properties { get; set; }


        /// <summary>
        /// Public URL
        /// </summary>
        public string Public_url { get; set; }


        /// <summary>
        /// Information about shared folder
        /// </summary>
        public ShareInfo ShareInfo { get; set; }


        /// <summary>
        /// Date of change
        /// </summary>
        public string Modified { get; set; }


        /// <summary>
        /// Date of create
        /// </summary>
        public string Created { get; set; }


        /// <summary>
        /// Id comments
        /// </summary>
        public CommentIds CommentIds { get; set; }


        /// <summary>
        /// MD5 - hash
        /// </summary>
        public string Md5 { get; set; }


        /// <summary>
        /// File size
        /// </summary>
        public long? Size { get; set; }


        /// <summary>
        /// Disk-Specific File Type
        /// </summary>
        public string Media_type { get; set; }


        /// <summary>
        /// Path to resource
        /// </summary>
        public string Path { get; set; }


        /// <summary>
        /// URL preview file
        /// </summary>
        public string Preview { get; set; }


        /// <summary>
        /// Type resource
        /// </summary>
        public string Type { get; set; }


        /// <summary>
        /// MIME - type file
        /// </summary>
        public string Mime_type { get; set; }


        /// <summary>
        /// Disk audit in which this resource was last modified
        /// </summary>
        public long? Revision { get; set; }


        /// <summary>
        /// Error response representation
        /// </summary>
        public ErrorResponse ErrorResponse { get; set; }


        /// <summary>
        /// Create meta information about a file or directory
        /// </summary>
        /// <param name="content">Json string</param>
        /// <param name="onlyFiles">Request only for files</param>
        /// <returns></returns>
        public static ResInfo GetResInfo(string content, bool onlyFiles = false)
        {
            ResInfo resInfo = new ResInfo();
            ResInfoList resInfoList = new ResInfoList();
            List<ResInfo> items = new List<ResInfo>();
            ErrorResponse errorResponse = new ErrorResponse();
            JArray nodes = new JArray();

            JToken root;
            Exif exif;
            Custom_properties custom_properties;
            ShareInfo shareInfo;
            CommentIds commentIds;

            if (content != null)
            {
                JObject json = JObject.Parse(content);

                exif = new Exif();
                custom_properties = new Custom_properties();
                shareInfo = new ShareInfo();
                commentIds = new CommentIds();

                resInfo.Antivirus_status = (string)json.SelectToken("antivirus_status");
                resInfo.Public_key = (string)json.SelectToken("public_key");
                resInfo.Sha256 = (string)json.SelectToken("sha256");
                resInfo.Name = (string)json.SelectToken("name");
                exif.Date_time = (string)json.SelectToken("exif.date_time");
                resInfo.Exif = exif;
                resInfo.Resource_id = (string)json.SelectToken("resource_id");
                resInfo.Custom_properties = custom_properties;
                resInfo.Public_url = (string)json.SelectToken("public_url");
                shareInfo.Is_root = (bool?)json.SelectToken("share.is_root");
                shareInfo.Is_owned = (bool?)json.SelectToken("share.is_owned");
                shareInfo.Rights = (string)json.SelectToken("share.rights");
                resInfo.ShareInfo = shareInfo;
                resInfo.Modified = (string)json.SelectToken("modified");
                resInfo.Created = (string)json.SelectToken("created");
                commentIds.Private_resource = (string)json.SelectToken("comment_ids.private_resource");
                commentIds.Public_resource = (string)json.SelectToken("comment_ids.public_resource");
                resInfo.CommentIds = commentIds;
                resInfo.Md5 = (string)json.SelectToken("md5");
                resInfo.Size = (long?)json.SelectToken("size");
                resInfo.Media_type = (string)json.SelectToken("media_type");
                resInfo.Path = (string)json.SelectToken("path");
                resInfo.Preview = (string)json.SelectToken("preview");
                resInfo.Type = (string)json.SelectToken("type");
                resInfo.Mime_type = (string)json.SelectToken("mime_type");
                resInfo.Revision = (long?)json.SelectToken("revision");

                if (onlyFiles)
                    root = json.SelectToken("items");
                else
                    root = json.SelectToken("_embedded.items");

                if (root != null)
                    nodes = JArray.FromObject(root);
                if (nodes.Count != 0)
                {
                    foreach (JToken token in nodes)
                    {
                        ResInfo item = new ResInfo();

                        exif = new Exif();
                        custom_properties = new Custom_properties();
                        shareInfo = new ShareInfo();
                        commentIds = new CommentIds();

                        item.Antivirus_status = (string)token.SelectToken("antivirus_status");
                        item.Public_key = (string)token.SelectToken("public_key");
                        item.Sha256 = (string)token.SelectToken("sha256");
                        item.Name = (string)token.SelectToken("name");
                        exif.Date_time = (string)token.SelectToken("exif.date_time");
                        item.Exif = exif;
                        item.Resource_id = (string)token.SelectToken("resource_id");
                        item.Custom_properties = custom_properties;
                        item.Public_url = (string)token.SelectToken("public_url");
                        shareInfo.Is_root = (bool?)token.SelectToken("share.is_root");
                        shareInfo.Is_owned = (bool?)token.SelectToken("share.is_owned");
                        shareInfo.Rights = (string)token.SelectToken("share.rights");
                        item.ShareInfo = shareInfo;
                        item.Modified = (string)token.SelectToken("modified");
                        item.Created = (string)token.SelectToken("created");
                        commentIds.Private_resource = (string)token.SelectToken("comment_ids.private_resource");
                        commentIds.Public_resource = (string)token.SelectToken("comment_ids.public_resource");
                        item.CommentIds = commentIds;
                        item.Md5 = (string)token.SelectToken("md5");
                        item.Size = (long?)token.SelectToken("size");
                        item.Media_type = (string)token.SelectToken("media_type");
                        item.Path = (string)token.SelectToken("path");
                        item.Preview = (string)token.SelectToken("preview");
                        item.Type = (string)token.SelectToken("type");
                        item.Mime_type = (string)token.SelectToken("mime_type");
                        item.Revision = (long?)token.SelectToken("revision");

                        items.Add(item);
                    }

                    resInfoList.Sort = (string)json.SelectToken("_embedded.sort");
                    resInfoList.Items = items;
                    resInfoList.Limit = (int?)json.SelectToken("_embedded.limit");
                    resInfoList.Offset = (int?)json.SelectToken("_embedded.offset");
                    resInfoList.Path = (string)json.SelectToken("_embedded.path");
                    resInfoList.Total = (int?)json.SelectToken("_embedded.total");

                    resInfo._Embedded = resInfoList;
                }

                resInfo.ErrorResponse = errorResponse.GetError(content);
            }
            return resInfo;
        }
    }


    /// <summary>
    /// List of nested resources
    /// </summary>
    public struct ResInfoList
    {
        /// <summary>
        /// The field by which the list is sorted
        /// </summary>
        public string Sort { get; set; }


        /// <summary>
        /// List items
        /// </summary>
        public List<ResInfo> Items { get; set; }


        /// <summary>
        /// Number of items per page
        /// </summary>
        public int? Limit { get; set; }


        /// <summary>
        /// Offset from the top of the list
        /// </summary>
        public int? Offset { get; set; }


        /// <summary>
        /// The path to the resource for which the list is built
        /// </summary>
        public string Path { get; set; }


        /// <summary>
        /// Total number of items in the list
        /// </summary>
        public int? Total { get; set; }
    }


    /// <summary>
    /// Media file metadata
    /// </summary>
    public struct Exif
    {
        /// <summary>
        /// Date of filming.
        /// </summary>
        public string Date_time { get; set; }
    }


    /// <summary>
    /// Shared folder information
    /// </summary>
    public struct ShareInfo
    {
        /// <summary>
        /// A sign that the folder is the root of the group
        /// </summary>
        public bool? Is_root { get; set; }


        /// <summary>
        /// A sign that the current user is the owner of the public folder
        /// </summary>
        public bool? Is_owned { get; set; }


        /// <summary>
        /// Access rights
        /// </summary>
        public string Rights { get; set; }
    }


    /// <summary>
    /// Comment IDs
    /// </summary>
    public struct CommentIds
    {
        /// <summary>
        /// The comment ID for private resources.
        /// </summary>
        public string Private_resource { get; set; }


        /// <summary>
        /// The identifier of comments for public resources.
        /// </summary>
        public string Public_resource { get; set; }
    }


    /// <summary>
    /// Custom property attributes
    /// </summary>
    public struct Custom_properties
    {

    }


    /// <summary>
    /// Server response
    /// </summary>
    public struct Link
    {
        /// <summary>
        /// URL
        /// </summary>
        public string Href { get; set; }

        /// <summary>
        /// HTTP-metod
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// Symptom of a template URL
        /// </summary>
        public bool? Templated { get; set; }
    }


    /// <summary>
    /// Request parameters
    /// </summary>
    public struct Param
    {
        /// <summary>
        /// List returned fields.
        /// </summary>
        public string Fields { get; set; }


        /// <summary>
        /// Path to resource.
        /// </summary>
        public string Path { get; set; }


        /// <summary>
        /// The path of the resource for copy.
        /// </summary>
        public string From { get; set; }


        /// <summary>
        /// The number of displayed nested resources, by default 20
        /// </summary>
        public int? Limit { get; set; }


        /// <summary>
        /// Run asynchronously.
        /// </summary>
        public bool? Force_async { get; set; }


        /// <summary>
        /// Delete the resource without placing it in the Trash.
        /// </summary>
        public bool? Permanently { get; set; }


        /// <summary>
        /// Offset from the beginning of the list of nested resources.
        /// </summary>
        public int? Offset { get; set; }


        /// <summary>
        /// Allow trimming the preview.
        /// </summary>
        public bool? Preview_crop { get; set; }


        /// <summary>
        /// The size of the preview.
        /// </summary>
        public string Preview_size { get; set; }


        /// <summary>
        /// A field for sorting the nested resources.
        /// </summary>
        public string Sort { get; set; }


        /// <summary>
        /// Link
        /// </summary>
        public string Link { get; set; }


        /// <summary>
        /// Overwrite an existing resource.
        /// </summary>
        public bool? Overwrite { get; set; }


        /// <summary>
        /// Filter by media type.
        /// </summary>
        public string Media_type { get; set; }


        /// <summary>
        /// Resource type file or folder
        /// </summary>
        public string Type { get; set; }


        /// <summary>
        /// The name under which the resource will be restored.
        /// </summary>
        public string Restore_name { get; set; }
    }


    /// <summary>
    /// Build parameters for request
    /// </summary>
    public struct UrlBuilder
    {
        /// <summary>
        /// List of returned attributes.
        /// </summary>
        public string Fields { get; set; }


        /// <summary>
        /// The path to the resource.
        /// </summary>
        public string Path { get; set; }


        /// <summary>
        /// The path to the resource being copied.
        /// </summary>
        public string From { get; set; }


        /// <summary>
        /// The number of displayed nested resources, by default 20
        /// </summary>
        public string Limit { get; set; }


        /// <summary>
        /// Run asynchronously.
        /// </summary>
        public string Force_async { get; set; }


        /// <summary>
        /// Delete the resource without placing it in the Trash.
        /// </summary>
        public string Permanently { get; set; }


        /// <summary>
        /// Offset from the beginning of the list of nested resources.
        /// </summary>
        public string Offset { get; set; }


        /// <summary>
        /// Allow trimming the preview.
        /// </summary>
        public string Preview_crop { get; set; }


        /// <summary>
        /// The size of the preview.
        /// </summary>
        public string Preview_size { get; set; }


        /// <summary>
        /// A field for sorting the nested resources.
        /// </summary>
        public string Sort { get; set; }


        /// <summary>
        /// Link
        /// </summary>
        public string Link { get; set; }


        /// <summary>
        /// Overwrite an existing resource.
        /// </summary>
        public string Overwrite { get; set; }


        /// <summary>
        /// Filter by media type.
        /// </summary>
        public string Media_type { get; set; }


        /// <summary>
        /// Type resource file or folder
        /// </summary>
        public string Type { get; set; }


        /// <summary>
        /// The name under which the resource will be restored.
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// Build parameters of request in string url 
        /// </summary>
        /// <param name="param"></param>
        public UrlBuilder(Param param)
        {
            const string exept = "disk:/";

            Fields = (param.Fields == null) ? null : ("&fields=" + param.Fields.ToLower());
            Path = (param.Path == null) ? null : ("&path=/" + param.Path.Replace(exept, string.Empty));
            From = (param.From == null) ? null : ("&from=/" + param.From.Replace(exept, string.Empty));
            Limit = (param.Limit == null) ? null : ("&limit=" + param.Limit.ToString());
            Force_async = (param.Force_async == null) ? null : ("&force_async=" + param.Force_async.ToString().ToLower());
            Permanently = (param.Permanently == null) ? null : ("&permanently=" + param.Permanently.ToString().ToLower());
            Offset = (param.Offset == null) ? null : ("&offset=" + param.Offset.ToString());
            Preview_crop = (param.Preview_crop == null) ? null : ("&preview_crop=" + param.Preview_crop.ToString().ToLower());
            Preview_size = (param.Preview_size == null) ? null : ("&preview_size=" + param.Preview_size.ToLower());
            Sort = (param.Sort == null) ? null : ("&sort=" + param.Sort.ToLower());
            Link = (param.Link == null) ? null : (param.Link);
            Overwrite = (param.Overwrite == null) ? null : ("&overwrite=" + param.Overwrite.ToString().ToLower());
            Media_type = (param.Media_type == null) ? null : ("&media_type=" + param.Media_type.ToLower());
            Type = (param.Type == null) ? null : ("&type=" + param.Type.ToLower());
            Name = (param.Restore_name == null) ? null : ("&name=" + param.Restore_name.Replace(exept, string.Empty));
        }
    }


    #endregion


    #region Enumerations for request


    /// <summary>
    /// List fields of DiskInfo
    /// </summary>
    public enum DiskFields
    {
        /// <summary>
        /// Maximum file size.
        /// </summary>
        Max_file_size,


        /// <summary>
        /// Unlimited auto upload with mobile devices.
        /// </summary>
        Unlimited_autoupload_enabled,


        /// <summary>
        /// Total size of disk (bytes)
        /// </summary>
        Total_space,


        /// <summary>
        /// Total size files in trash (bytes). Included in used space.
        /// </summary>
        Trash_size,


        /// <summary>
        /// Mark of  paid advanced place
        /// </summary>
        Is_paid,


        /// <summary>
        /// Used space of disk (bytes)
        /// </summary>
        Used_space,


        /// <summary>
        /// List system folders
        /// </summary>
        System_folders,


        /// <summary>
        /// Disk  owner
        /// </summary>
        User
    }


    /// <summary>
    /// The type of files to include in the list. The disk determines the type of each file at boot time.
    /// </summary>
    public enum Media_type
    {
        /// <summary>
        /// Audio files.
        /// </summary>
        Audio,


        /// <summary>
        /// Backup and temporary backup files.
        /// </summary>
        Backup,


        /// <summary>
        /// E-books.
        /// </summary>
        Book,


        /// <summary>
        /// Compressed and archived files.
        /// </summary>
        Compressed,


        /// <summary>
        /// Files with databases.
        /// </summary>
        Data,


        /// <summary>
        /// Files with code (C ++, Java, XML, etc.), as well as service IDE files.
        /// </summary>
        Development,


        /// <summary>
        /// Images of media in various formats and related files (for example, CUE).
        /// </summary>
        Diskimage,


        /// <summary>
        /// Documents of office formats (Word, OpenOffice, etc.).
        /// </summary>
        Document,


        /// <summary>
        /// Encrypted files.
        /// </summary>
        Encoded,


        /// <summary>
        /// Executable files.
        /// </summary>
        Executable,


        /// <summary>
        /// Files with flash video or animation.
        /// </summary>
        Flash,


        /// <summary>
        /// Font files.
        /// </summary>
        Font,


        /// <summary>
        /// Images.
        /// </summary>
        Image,


        /// <summary>
        /// Settings files for various programs.
        /// </summary>
        Settings,


        /// <summary>
        /// Office files (Numbers, Lotus).
        /// </summary>
        Spreadsheet,


        /// <summary>
        /// Text files.
        /// </summary>
        Text,


        /// <summary>
        /// Unknown type.
        /// </summary>
        Unknown,


        /// <summary>
        /// Video files.
        /// </summary>
        Video,


        /// <summary>
        /// Various files used by browsers and sites (CSS, certificates, bookmark files).
        /// </summary>
        Web
    }


    /// <summary>
    /// The field by which the list of resources nested in the folder is sorted.
    /// </summary>
    public enum SortField
    {
        /// <summary>
        /// The name of the resource.
        /// </summary>
        Name,


        /// <summary>
        /// The path to the resource in Drive.
        /// </summary>
        Path,


        /// <summary>
        /// The date the resource was created.
        /// </summary>
        Created,


        /// <summary>
        /// The date the resource was modified.
        /// </summary>
        Modified,


        /// <summary>
        /// File size.
        /// </summary>
        Size
    }

    /// <summary>
    /// The field by which the list of resources in the trash is sorted.
    /// </summary>
    public enum SortDeleted
    {
        /// <summary>
        /// The date the resource was deleted.
        /// </summary>
        Deleted,


        /// <summary>
        /// The date the resource was created.
        /// </summary>
        Created,
    }


    /// <summary>
    /// List of fields in a file or folder
    /// </summary>
    public enum ResFields
    {
        /// <summary>
        /// Antivirus check status
        /// </summary>
        Antivirus_status,


        /// <summary>
        /// The key of the published resource
        /// </summary>
        Public_key,


        /// <summary>
        /// SHA256-hash
        /// </summary>
        Sha256,


        /// <summary>
        /// Name
        /// </summary>
        Name,


        /// <summary>
        /// Public URL
        /// </summary>
        Public_url,


        /// <summary>
        /// Date of change
        /// </summary>
        Modified,


        /// <summary>
        /// Date of creation
        /// </summary>
        Created,


        /// <summary>
        /// MD5-hash
        /// </summary>
        Md5,


        /// <summary>
        /// File size
        /// </summary>
        Size,


        /// <summary>
        /// Disk-Specific File Type
        /// </summary>
        Media_type,


        /// <summary>
        /// The path to the resource
        /// </summary>
        Path,


        /// <summary>
        /// File preview URL
        /// </summary>
        Preview,


        /// <summary>
        /// Type file or folder
        /// </summary>
        Type,


        /// <summary>
        /// MIME-file type
        /// </summary>
        Mime_type,


        /// <summary>
        /// List of nested ResInfoList resources
        /// </summary>
        _Embedded
    }


    /// <summary>
    /// Resource type
    /// </summary>
    public enum TypeRes
    {
        /// <summary>
        /// File
        /// </summary>
        File,


        /// <summary>
        /// Folder
        /// </summary>
        Dir
    }


    /// <summary>
    /// The status of an asynchronous operation
    /// </summary>  
    public enum StatusAsync
    {
        /// <summary>
        /// The completion status of the asynchronous operation 'completed successfully'
        /// </summary>
        Success,


        /// <summary>
        /// The execution status of the asynchronous operation 'in progress'
        /// </summary>
        InProgress,


        /// <summary>
        /// The completion status of the asynchronous operation 'completed with an error'
        /// </summary>
        Failed
    }


    #endregion
}
