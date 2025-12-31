using System;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.TextTemplateManagement.TextTemplates;

public class TextTemplateDefinitionContentRecord : Entity<Guid>
{
    public Guid DefinitionId { get; internal set; }

    public string FileName { get; set; }

    public string FileContent { get; set; }

    protected TextTemplateDefinitionContentRecord()
    {
    }

    public TextTemplateDefinitionContentRecord(
        Guid id,
        Guid definitionId,
        string fileName,
        string fileContent)
        : base(id)
    {
        DefinitionId = definitionId;
        FileName = Check.NotNullOrWhiteSpace(fileName, nameof(fileName), TemplateDefinitionContentRecordConsts.MaxFileNameLength);
        FileContent = fileContent;
    }

    public bool HasSameData(TextTemplateDefinitionContentRecord otherRecord)
    {
        if (DefinitionId != otherRecord.DefinitionId)
        {
            return false;
        }

        if (FileName != otherRecord.FileName)
        {
            return false;
        }

        if (FileContent != otherRecord.FileContent)
        {
            return false;
        }

        return true;
    }

    public void Patch(TextTemplateDefinitionContentRecord otherRecord)
    {
        if (DefinitionId != otherRecord.DefinitionId)
        {
            DefinitionId = otherRecord.DefinitionId;
        }

        if (FileName != otherRecord.FileName)
        {
            FileName = otherRecord.FileName;
        }

        if (FileContent != otherRecord.FileContent)
        {
            FileContent = otherRecord.FileContent;
        }
    }
}
