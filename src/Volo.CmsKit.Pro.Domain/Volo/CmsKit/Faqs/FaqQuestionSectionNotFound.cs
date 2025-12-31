using System;
using Volo.Abp;

namespace Volo.CmsKit.Faqs;

[Serializable]
public class FaqQuestionSectionNotFound() : BusinessException(CmsKitProErrorCodes.FaqQuestion.FaqQuestionSectionNotFound);