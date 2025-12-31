using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using Shouldly;
using Volo.Abp.Json.SystemTextJson.Modifiers;
using Xunit;

namespace Volo.Abp.TextTemplateManagement.TextTemplates;

public class CalculateHash_Tests : TextTemplateManagementDomainTestBase
{
    [Fact]
    public void Test()
    {
        var jsonSerializerOptions = new JsonSerializerOptions
        {
            TypeInfoResolver = new DefaultJsonTypeInfoResolver
            {
                Modifiers =
                {
                    new AbpIgnorePropertiesModifiers<TextTemplateDefinitionRecord, Guid>().CreateModifyAction(x => x.Id),
                    new AbpIgnorePropertiesModifiers<TextTemplateDefinitionContentRecord, Guid>().CreateModifyAction(x => x.Id)
                }
            }
        };

        var id = Guid.NewGuid();
        var json = JsonSerializer.Serialize(new List<TextTemplateDefinitionRecord>()
            {
                new TextTemplateDefinitionRecord(id, "Test", "Test", false, "Test", "Test", false, "Test", "Test")
            },
            jsonSerializerOptions);

        json.ShouldNotContain("\"Id\"");
        json.ShouldNotContain(id.ToString("D"));

        json = JsonSerializer.Serialize(new List<TextTemplateDefinitionContentRecord>()
            {
                new TextTemplateDefinitionContentRecord(id, Guid.NewGuid(), "Test", "Test")
            },
            jsonSerializerOptions);

        json.ShouldNotContain("\"Id\"");
        json.ShouldNotContain(id.ToString("D"));
    }
}
