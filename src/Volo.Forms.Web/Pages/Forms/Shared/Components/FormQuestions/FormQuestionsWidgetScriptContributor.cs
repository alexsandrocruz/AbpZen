using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Volo.Forms.Web.Pages.Forms.Shared.Components.FormQuestions;

public class FormQuestionsWidgetScriptContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        context.Files.Add("/client-proxies/form-proxy.js");
        context.Files.Add("/Pages/Forms/Shared/Components/FormQuestions/Vue-question-choice.js");
        context.Files.Add("/Pages/Forms/Shared/Components/FormQuestions/Vue-question-types.js");
        context.Files.Add("/Pages/Forms/Shared/Components/FormQuestions/Vue-question-item.js");
        context.Files.Add("/Pages/Forms/Shared/Components/FormQuestions/Default.js");

    }
}
