using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Logging;
using Netsoftware.Xanthos.Common.Resources.Utils;
using Netsoftware.Xanthos.EmailQueue.Database.Models;

namespace Netsoftware.Xanthos.Database;

public class ApplicationDbContext : DbContext
{
    private readonly Guid _applicationInstanceId;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger _logger;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ILogger<ApplicationDbContext> logger,
        IHttpContextAccessor httpContextAccessor) : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;

        if (_httpContextAccessor.HttpContext == null) return;
        var claim = _httpContextAccessor.HttpContext.User.FindFirst(CustomClaimTypes.ApplicationInstanceId);
        if (claim != null)
            _applicationInstanceId = new Guid(claim.Value);
        else
            _logger.LogError($"Claim '{CustomClaimTypes.ApplicationInstanceId}' not exist in user claims");
    }

    public DbSet<ApplicationInstance.Database.Models.ApplicationInstance> ApplicationInstances { get; set; }
    public DbSet<EmailQueueModel> EmailQueues { get; set; }  

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            entityType.SetTableName("Appstation_" + entityType.GetTableName());

        AddDatesUtcConversion(modelBuilder);
    }

    private static void AddDatesUtcConversion(ModelBuilder modelBuilder)
    {
        var dateTimeConverter = new ValueConverter<DateTime, DateTime>(
            v => v.ToUniversalTime(),
            v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

        var nullableDateTimeConverter = new ValueConverter<DateTime?, DateTime?>(
            v => v.HasValue ? v.Value.ToUniversalTime() : v,
            v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (entityType.IsKeyless) continue;

            foreach (var property in entityType.GetProperties())
                if (property.ClrType == typeof(DateTime))
                    property.SetValueConverter(dateTimeConverter);
                else if (property.ClrType == typeof(DateTime?)) property.SetValueConverter(nullableDateTimeConverter);
        }
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        if (_httpContextAccessor.HttpContext == null)
        {
            _logger.LogWarning("SaveChangesAsync operation was invoked without HttpContext");
            return base.SaveChangesAsync(cancellationToken);
        }

        var endpointHasAllowAnonymous = CheckIfEndpointHasAllowAnonymous();
        if (endpointHasAllowAnonymous) return base.SaveChangesAsync(cancellationToken);

        var roles = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role);
        if (roles == null)
        {
            _logger.LogError($"Claim '{ClaimTypes.Role}' not exist in user claims");
            throw new ArgumentNullException($"Claim '{ClaimTypes.Role}' not exist in user claims",
                new NullReferenceException("roles"));
        }

        if (roles.Value.Contains(ApplicationBaseRoles.SuperAdmin.ToString()))
            return base.SaveChangesAsync(cancellationToken);
        AssignApplicationId();
        return base.SaveChangesAsync(cancellationToken);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = default)
    {
        if (_httpContextAccessor.HttpContext == null)
        {
            _logger.LogWarning("SaveChangesAsync operation was invoked without HttpContext");
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        var endpointHasAllowAnonymous = CheckIfEndpointHasAllowAnonymous();
        if (endpointHasAllowAnonymous) return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

        var roles = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role);
        if (roles == null)
        {
            _logger.LogError($"Claim '{ClaimTypes.Role}' not exist in user claims");
            throw new ArgumentNullException($"Claim '{ClaimTypes.Role}' not exist in user claims",
                new NullReferenceException("roles"));
        }

        if (roles.Value.Contains(ApplicationBaseRoles.SuperAdmin.ToString()))
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        AssignApplicationId();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    private bool CheckIfEndpointHasAllowAnonymous()
    {
        var endpoint = _httpContextAccessor.HttpContext?.Features.Get<IEndpointFeature>()?.Endpoint;
        if (endpoint == null)
            throw new ArgumentNullException("Endpoint does not exists.", new NullReferenceException("endpoint"));

        var endpointHasAllowAnonymous = endpoint.Metadata
            .Any(em => em.GetType() == typeof(AllowAnonymousAttribute));
        return endpointHasAllowAnonymous;
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        if (_httpContextAccessor.HttpContext == null)
        {
            _logger.LogWarning("SaveChanges operation was invoked without HttpContext");
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        var endpointHasAllowAnonymous = CheckIfEndpointHasAllowAnonymous();
        if (endpointHasAllowAnonymous) return base.SaveChanges(acceptAllChangesOnSuccess);

        var roles = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role);
        if (roles == null)
        {
            _logger.LogError($"Claim '{ClaimTypes.Role}' not exist in user claims");
            throw new ArgumentNullException($"Claim '{ClaimTypes.Role}' not exist in user claims",
                new NullReferenceException("roles"));
        }

        if (roles.Value.Contains(ApplicationBaseRoles.SuperAdmin.ToString()))
            return base.SaveChanges(acceptAllChangesOnSuccess);
        AssignApplicationId();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override int SaveChanges()
    {
        if (_httpContextAccessor.HttpContext == null)
        {
            _logger.LogWarning("SaveChanges operation was invoked without HttpContext");
            return base.SaveChanges();
        }

        var endpointHasAllowAnonymous = CheckIfEndpointHasAllowAnonymous();
        if (endpointHasAllowAnonymous) return base.SaveChanges();

        var roles = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role);
        if (roles == null)
        {
            _logger.LogError($"Claim '{ClaimTypes.Role}' not exist in user claims");
            throw new ArgumentNullException($"Claim '{ClaimTypes.Role}' not exist in user claims",
                new NullReferenceException("roles"));
        }

        if (roles.Value.Contains(ApplicationBaseRoles.SuperAdmin.ToString())) return base.SaveChanges();
        AssignApplicationId();
        return base.SaveChanges();
    }

    private void AssignApplicationId()
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e =>
                e.State is EntityState.Modified or EntityState.Added or EntityState.Deleted);

        foreach (var entityEntry in entries)
        {
            var prop = entityEntry.CurrentValues.EntityType.FindProperty("AppId");
            if (prop == null) continue;
            if (entityEntry.State == EntityState.Added)
                entityEntry.Property("AppId").CurrentValue = _applicationInstanceId;
            if (entityEntry.State is not (EntityState.Modified or EntityState.Deleted) ||
                entityEntry.Property("AppId").CurrentValue.Equals(_applicationInstanceId)) continue;
            _logger.LogError("User try modified object which his doesn't owner");
            throw new UnauthorizedAccessException("User try modified object which his doesn't owner");
        }
    }
}