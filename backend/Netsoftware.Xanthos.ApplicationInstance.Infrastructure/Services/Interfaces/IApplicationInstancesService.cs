using System;
using System.Threading.Tasks;
using Netsoftware.Xanthos.ApplicationInstance.Infrastructure.Resources;
using Netsoftware.Xanthos.Common.Resources.Account;
using Netsoftware.Xanthos.Common.Resources.GridResources;

namespace Netsoftware.Xanthos.ApplicationInstance.Infrastructure.Services.Interfaces;

public interface IApplicationInstancesService
{
    Task Register(RegisterAccountResource resource);

    Task<GridResponseResource<ApplicationInstanceResource>> GetApplicationInstancesTableData(
        GridParamsResource gridParams);

    Task<ApplicationInstanceResource> GetApplicationInstance(Guid id);
}