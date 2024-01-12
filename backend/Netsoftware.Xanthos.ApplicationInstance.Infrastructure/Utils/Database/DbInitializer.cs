using System;
using System.Threading.Tasks;
using Netsoftware.Xanthos.ApplicationInstance.Database.Repositories;

namespace Netsoftware.Xanthos.ApplicationInstance.Infrastructure.Utils.Database;

public class DbInitializer : IDbInitializer
{
    private readonly IGenericRepository<ApplicationInstance.Database.Models.ApplicationInstance>
        _applicationInstanceRepository;

    public DbInitializer(IGenericRepository<ApplicationInstance.Database.Models.ApplicationInstance> aiRepository)
    {
        _applicationInstanceRepository = aiRepository;
    }

    public async Task Initialize()
    {
        var appInst1 = new ApplicationInstance.Database.Models.ApplicationInstance
        {
            Id = new Guid("18329ea1-5710-44da-bdbb-27d76e66b427"),
            OwnerId = new Guid("1ddeea51-2416-4547-b2f3-165dd9457848"),
            CompanyName = "Company app 1",
            Name = "First instance"
        };
        var existingAppInst1 = await _applicationInstanceRepository.GetAsync(ai => ai.Id.Equals(appInst1.Id));
        if (existingAppInst1 == null) await _applicationInstanceRepository.CreateAsync(appInst1);
        var appInst2 = new ApplicationInstance.Database.Models.ApplicationInstance
        {
            Id = new Guid("92554d79-6b2b-42cc-a114-523f91c2799f"),
            OwnerId = new Guid("1ddeea51-2416-4547-b2f3-165dd9457848"),
            CompanyName = "Company app 2",
            Name = "Second instance"
        };
        var existingAppInst2 = await _applicationInstanceRepository.GetAsync(ai => ai.Id.Equals(appInst2.Id));
        if (existingAppInst2 == null) await _applicationInstanceRepository.CreateAsync(appInst2);
    }
}