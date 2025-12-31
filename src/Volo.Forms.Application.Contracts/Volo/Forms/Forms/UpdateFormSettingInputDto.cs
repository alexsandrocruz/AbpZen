namespace Volo.Forms.Forms;

public class UpdateFormSettingInputDto
{
    public bool CanEditResponse { get; set; }
    public bool IsCollectingEmail { get; set; }
    public bool HasLimitOneResponsePerUser { get; set; }
    public bool IsAcceptingResponses { get; set; }
    public bool IsQuiz { get; set; }
    public bool RequiresLogin { get; set; }
}
