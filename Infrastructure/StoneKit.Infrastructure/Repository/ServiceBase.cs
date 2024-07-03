using StoneKit.Infrastructure.Services;

namespace StoneKit.Infrastructure.Repository;

/// <summary>
/// Provides a base implementation for service classes, encapsulating the Unit of Work pattern.
/// </summary>
public abstract class ServiceBase : IService
{
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceBase"/> class.
    /// </summary>
    public ServiceBase()
    {
        _unitOfWork = null!;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceBase"/> class with the specified unit of work.
    /// </summary>
    /// <param name="unitOfWork">The unit of work to be used by the service.</param>
    public ServiceBase(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Gets the unit of work associated with the service.
    /// </summary>
    protected IUnitOfWork UnitOfWork => _unitOfWork;
}
