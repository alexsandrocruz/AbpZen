using System;

namespace Volo.FileManagement.Files;

public class FileUploadPreInfoRequest
{
    public Guid? DirectoryId { get; set; }

    public string FileName { get; set; }

    public long Size { get; set; }
}
