using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Netsoftware.Xanthos.ApplicationInstance.Database.Repositories;
using Netsoftware.Xanthos.ApplicationInstance.Infrastructure.Resources;
using Netsoftware.Xanthos.ApplicationInstance.Infrastructure.Services.Interfaces;
using Netsoftware.Xanthos.Common.Resources.Account;
using Netsoftware.Xanthos.Common.Resources.GridResources;

namespace Netsoftware.Xanthos.ApplicationInstance.Infrastructure.Services;

public class ApplicationInstancesService : IApplicationInstancesService
{
    private readonly IGenericRepository<Database.Models.ApplicationInstance> _applicationInstancesRepository;
    private readonly ILogger _logger;
    private readonly IMapper _mapper;

    public ApplicationInstancesService(ILogger<ApplicationInstancesService> logger, IMapper mapper,
        IGenericRepository<Database.Models.ApplicationInstance> applicationInstancesRepository)
    {
        _logger = logger;
        _mapper = mapper;
        _applicationInstancesRepository = applicationInstancesRepository;
    }

    public async Task<GridResponseResource<ApplicationInstanceResource>> GetApplicationInstancesTableData(
        GridParamsResource gridParams)
    {
        var result = await _applicationInstancesRepository
            .GetGridTableRows(gridParams, ai => !ai.IsDelete)
            .Select(x => _mapper.Map<Database.Models.ApplicationInstance, ApplicationInstanceResource>(x))
            .ToListAsync();

        var elementsCount = await _applicationInstancesRepository.GetGridTableRowsCount(gridParams, ai => !ai.IsDelete);

        return new GridResponseResource<ApplicationInstanceResource>
        {
            Rows = result,
            ElementsCount = elementsCount
        };
    }

    public async Task Register(RegisterAccountResource resource)
    {
        try
        {
            var appInstance = new Database.Models.ApplicationInstance
            {
                Id = resource.AppId,
                CompanyName = resource.Company,
                CreatedBy = new Guid(resource.Id),
                OwnerId = new Guid(resource.Id)
            };
            await _applicationInstancesRepository.CreateAsync(appInstance);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error during execute method Register().", resource);
            throw;
        }
    }

    public async Task<ApplicationInstanceResource> GetApplicationInstance(Guid id)
    {
        try
        {
            var appInstance = (await _applicationInstancesRepository.GetAsync(ai => ai.Id == id)).FirstOrDefault();
            if (appInstance == null)
            {
                _logger.LogInformation($"Application instance with id = {id} not found.");
                return null;
            }

            return _mapper.Map<ApplicationInstanceResource>(appInstance);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error during execute method GetApplicationInstance().", id);
            throw;
        }
    }
}