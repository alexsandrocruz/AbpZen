using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;

namespace Volo.Abp.Gdpr;

public class GdprTestDataBuilder : ITransientDependency
{
    private readonly IGdprRequestRepository _gdprRequestRepository;
    private readonly IGuidGenerator _guidGenerator;
    private readonly GdprTestData _gdprTestData;

    public GdprTestDataBuilder(IGdprRequestRepository gdprRequestRepository, IGuidGenerator guidGenerator, GdprTestData gdprTestData)
    {
        _gdprRequestRepository = gdprRequestRepository;
        _guidGenerator = guidGenerator;
        _gdprTestData = gdprTestData;
    }

    public async Task BuildAsync()
    {
        var gdprRequest1 = new GdprRequest(
            _gdprTestData.RequestId1,
            _gdprTestData.UserId1,
            DateTime.Now.AddDays(1)
        );
        gdprRequest1.AddData(_guidGenerator.Create(), "gdpr-data", "provider");

        await _gdprRequestRepository.InsertAsync(
            gdprRequest1
        );
        
        var gdprRequest2 = await _gdprRequestRepository.InsertAsync(
            new GdprRequest(
                _guidGenerator.Create(), 
                _gdprTestData.UserId1,
                DateTime.Now.AddHours(1)
            )
        );
    }
}