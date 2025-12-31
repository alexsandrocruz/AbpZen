using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Guids;
using Volo.Abp.Json;
using Volo.Abp.Uow;

namespace Volo.Abp.Gdpr;

public class GdprUserDataEventHandler : IDistributedEventHandler<GdprUserDataPreparedEto>, ITransientDependency
{
    protected IGdprRequestRepository GdprRequestRepository { get; }
    protected IGuidGenerator GuidGenerator { get; }
    protected IUnitOfWorkManager UnitOfWorkManager { get; }
    
    protected IJsonSerializer JsonSerializer { get; }

    public GdprUserDataEventHandler(
        IGdprRequestRepository gdprRequestRepository,
        IGuidGenerator guidGenerator,
        IUnitOfWorkManager unitOfWorkManager, 
        IJsonSerializer jsonSerializer)
    {
        GdprRequestRepository = gdprRequestRepository;
        GuidGenerator = guidGenerator;
        UnitOfWorkManager = unitOfWorkManager;
        JsonSerializer = jsonSerializer;
    }

    [UnitOfWork(isTransactional: true)]
    public virtual async Task HandleEventAsync(GdprUserDataPreparedEto eventData)
    {
        if (!eventData.Data.Any())
        {
            return;
        }

        var gdprRequest = await GdprRequestRepository.FindAsync(eventData.RequestId);
        if (gdprRequest == null)
        {
            return;
        }

        var data = JsonSerializer.Serialize(eventData.Data);
        gdprRequest.AddData(GuidGenerator.Create(), data, eventData.Provider);
        
        await GdprRequestRepository.UpdateAsync(gdprRequest);
    }
}