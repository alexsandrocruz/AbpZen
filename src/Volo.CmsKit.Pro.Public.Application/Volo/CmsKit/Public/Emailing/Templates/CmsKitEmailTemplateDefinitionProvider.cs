using Volo.Abp.Emailing.Templates;
using Volo.Abp.TextTemplating;
using Volo.CmsKit.Localization;

namespace Volo.CmsKit.Public.Emailing.Templates;

public class CmsKitEmailTemplateDefinitionProvider : TemplateDefinitionProvider
{
    public override void Define(ITemplateDefinitionContext context)
    {

        context.Add(
            new TemplateDefinition(
                CmsKitEmailTemplates.NewsletterEmailTemplate,
                localizationResource: typeof(CmsKitResource),
                layout: StandardEmailTemplates.Layout
            ).WithVirtualFilePath("/Volo/CmsKit/Public/Emailing/Templates/NewsletterEmailLayout.tpl", true)
        );
    }
}
