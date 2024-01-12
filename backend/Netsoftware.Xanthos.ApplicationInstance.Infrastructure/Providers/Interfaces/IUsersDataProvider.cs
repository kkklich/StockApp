using System;
using System.Threading.Tasks;

namespace Netsoftware.Xanthos.ApplicationInstance.Infrastructure.Providers.Interfaces;

public interface IUsersDataProvider
{
    Task<string> DownloadApplicationInstancesOwners();
    Task<string> DownloadApplicationInstanceUsers(string paramsJSON, Guid applicationInstanceId);
}