namespace Volo.CmsKit.Faqs;

public static class FaqQuestionConst
{
    public static string DefaultSorting { get; set; } = "Order asc";
        
    public static int MaxTitleLength { get; set; } = 256;
    public static int MaxTextLength { get; set; } = 16_384;
}
