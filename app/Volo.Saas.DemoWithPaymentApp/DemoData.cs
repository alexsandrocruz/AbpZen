using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Volo.Saas.DemoWithPaymentApp;

public static class DemoData
{
    public static Guid Plan_1_Id { get; } = Guid.Parse("7e4d28af-0210-45ea-8e37-260497d69233");
    public static string Plan_1_Name { get; } = "Pro Plan";

    public static Guid Plan_2_Id { get; } = Guid.Parse("b5653bea-4c44-45bb-839e-726b980357a5");
    public static string Plan_2_Name { get; } = "Enterprise Plan";

    public static Guid Edition_1_Id { get; } = Guid.Parse("499bd002-bf96-11eb-8529-0242ac130003");
    public static string Edition_1_Name { get; } = "Pro Edition";

    public static Guid Edition_2_Id { get; } = Guid.Parse("499bd548-bf96-11eb-8529-0242ac130003");
    public static string Edition_2_Name { get; } = "Enterprise Edition";
}
