using Volo.Forms.Forms;

namespace Volo.Forms.Responses;

public class FormWithResponseDto
{
    public FormDto Form { get; set; }
    public FormResponseDto Response { get; set; }
}
