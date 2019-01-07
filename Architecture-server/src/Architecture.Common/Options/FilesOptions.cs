using System;
namespace Architecture.Common.Options
{
    public class FilesOptions
    {
        public string UploadFolder { get; set; }
        public InnerFolders InnerFolders { get; set; }
    }

    public class InnerFolders
    {
        public string PhotoFolder { get; set; }
    }
}
