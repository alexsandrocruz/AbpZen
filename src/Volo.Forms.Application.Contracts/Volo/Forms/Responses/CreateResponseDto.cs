using System.Collections.Generic;
using Volo.Forms.Answers;

namespace Volo.Forms.Responses;

public class CreateResponseDto
{
    public string Email { get; set; }

    public List<CreateAnswerDto> Answers { get; set; }
}
