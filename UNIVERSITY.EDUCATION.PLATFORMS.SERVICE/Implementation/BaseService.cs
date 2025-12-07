using AutoMapper;
using UNIVERSITY.EDUCATION.PLATFORMS.Application.Common.Infrastructure;
using UNIVERSITY.EDUCATION.PLATFORMS.Application.Service;
using UNIVERSITY.EDUCATION.PLATFORMS.Infrastructure.Persistence;

public abstract class BaseService : ApplicationDisposable
{
    protected readonly IMapper mapper;
    protected readonly IDatabaseService<UEPContext> _unitOfWork;
    protected readonly IAuthenticatedUserService? _authenticatedUserService;

    protected BaseService(
        IDatabaseService<UEPContext> unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    protected BaseService(
        IDatabaseService<UEPContext> unitOfWork,
        IMapper mapper,
        IAuthenticatedUserService authenticatedUserService)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _authenticatedUserService = authenticatedUserService ?? throw new ArgumentNullException(nameof(authenticatedUserService));
    }

    protected override void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        if (disposing)
        {
            _unitOfWork?.Dispose();
        }

        base.Dispose(disposing);
    }
}
