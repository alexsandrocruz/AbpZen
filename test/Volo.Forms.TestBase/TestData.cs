using System;
using Volo.Abp.DependencyInjection;

namespace Volo.Forms;

public class TestData : ISingletonDependency
{
    public Guid TestUser1 { get; } = Guid.NewGuid();
    public Guid TestUser2 { get; } = Guid.NewGuid();
    public Guid FormResponse1 { get; } = Guid.NewGuid();
    public Guid FormResponse2 { get; } = Guid.NewGuid();
    public Guid FormResponse3 { get; } = Guid.NewGuid();
    public Guid TestFormId { get; } = Guid.NewGuid();
    public Guid TestShortTextId { get; } = Guid.NewGuid();
    public Guid TestCheckboxId { get; } = Guid.NewGuid();
    public Guid TestChoiceCSharp { get; } = Guid.NewGuid();
    public Guid TestChoiceJava { get; } = Guid.NewGuid();
    public Guid TestChoiceJavascript { get; } = Guid.NewGuid();
    public Guid TestMultiChoiceId { get; } = Guid.NewGuid();
    public Guid ShortTextAnswerId { get; } = Guid.NewGuid();
    public Guid MultiChoiceAnswerId { get; } = Guid.NewGuid();
}
