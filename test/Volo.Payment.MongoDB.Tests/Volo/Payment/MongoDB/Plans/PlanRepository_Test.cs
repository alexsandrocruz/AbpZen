using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Payment.Plans;
using Xunit;

namespace Volo.Payment.MongoDB.Plans;

[Collection(MongoTestCollection.Name)]
public class PlanRepository_Test : PlanRepository_Test<PaymentMongoDbTestModule>
{

}
