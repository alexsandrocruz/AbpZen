using System;

namespace Volo.Payment.DemoApp;

public static class DemoAppData
{
    public static Guid PlanId { get; } = Guid.Parse("7e4d28af-0210-45ea-8e37-260497d69233");
    public static string PlanName { get; } = "Basic Plan";

    public static Guid Plan_2_Id { get; } = Guid.Parse("b5653bea-4c44-45bb-839e-726b980357a5");
    public static string Plan_2_Name { get; } = "Enterperise Plan";
}
