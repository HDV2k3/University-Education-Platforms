using Microsoft.AspNetCore.Mvc;
using UNIVERSITY.EDUCATION.PLATFORMS.Application.Common.Domain;
using UNIVERSITY.EDUCATION.PLATFORMS.Application.Common.SmartTable;
using UNIVERSITY.EDUCATION.PLATFORMS.Application.Constants;
using UNIVERSITY.EDUCATION.PLATFORMS.Application.Extensions;
using UNIVERSITY.EDUCATION.PLATFORMS.Application.Models.BaseResponse;
using UNIVERSITY.EDUCATION.PLATFORMS.Application.Service;
using UNIVERSITY.EDUCATION.PLATFORMS.Common;
using UNIVERSITY.EDUCATION.PLATFORMS.Helpers;
using UNIVERSITY.EDUCATION.PLATFORMS.Service.Interface;

[ApiController]
public abstract class BaseController<TEntity, TPrimaryKey, TCreate, TUpdate, TViewOutput, TService> : ControllerBase
    where TPrimaryKey : struct
    where TEntity : AuditableBaseEntity
    where TCreate : class
    where TUpdate : DomainUpdate<TPrimaryKey>
    where TViewOutput : DomainResponse<TPrimaryKey>
    where TService : IAppBaseService<TEntity, TPrimaryKey, TCreate, TUpdate, TViewOutput>
{
    protected virtual string GetPolicyName { get; set; } = string.Empty;
    protected virtual string GetListPolicyName { get; set; } = string.Empty;
    protected virtual string CreatePolicyName { get; set; } = string.Empty;
    protected virtual string UpdatePolicyName { get; set; } = string.Empty;
    protected virtual string DeletePolicyName { get; set; } = string.Empty;
    protected virtual string CacheKey { get; set; } = string.Empty;

    private readonly TService _appBaseService;
    private readonly IAuthenticatedUserService _authenticatedUserService;

    protected BaseController(
      TService appBaseService,
      IAuthenticatedUserService authenticatedUserService)
    {
        _appBaseService = appBaseService ?? throw new ArgumentNullException(nameof(appBaseService));
        _authenticatedUserService = authenticatedUserService ?? throw new ArgumentNullException(nameof(authenticatedUserService));
    }

    // ========================== GET BY ID ==============================
    [Authorize]
    [HttpGet("{id}")]
    public virtual async Task<IActionResult> GetById(TPrimaryKey id)
    {
        if (!HasPermission(GetPolicyName))
            return ForwardForbid(GetPolicyName);

        var result = await _appBaseService.GetById(id);
        return Ok(new Response<TViewOutput>(result));
    }

    // ========================== PAGING ==============================
    [Authorize]
    [HttpPost("paging")]
    public async Task<IActionResult> Paging(SmartTableParam param)
    {
        if (!HasPermission(GetListPolicyName))
            return ForwardForbid(GetListPolicyName);

        var result = await _appBaseService.GetPaging(param);
        return Ok(result);
    }

    // ========================== CREATE ==============================
    [Authorize]
    [HttpPost]
    public virtual async Task<IActionResult> Create(TCreate request)
    {
        if (!HasPermission(CreatePolicyName))
            return ForwardForbid(CreatePolicyName);

        var result = await _appBaseService.Create(request);
        return Ok(new Response<TViewOutput>(result));
    }

    // ========================== UPDATE ==============================
    [Authorize]
    [HttpPut]
    public virtual async Task<IActionResult> Update(TUpdate request)
    {
        if (!HasPermission(UpdatePolicyName))
            return ForwardForbid(UpdatePolicyName);

        var result = await _appBaseService.Update(request);
        return Ok(new Response<TViewOutput>(result));
    }

    // ========================== CHANGE ACTIVE ==============================
    [Authorize]
    [HttpPut("{id}/change-active")]
    public virtual async Task<IActionResult> ChangeStatus(TPrimaryKey id, bool isActive)
    {
        if (!HasPermission(UpdatePolicyName))
            return ForwardForbid(UpdatePolicyName);

        var result = await _appBaseService.ChangeIsActive(id, isActive);
        return Ok(new Response<TViewOutput>(result));
    }

    // ========================== DELETE ==============================
    [Authorize]
    [HttpDelete("{id}")]
    public virtual async Task<IActionResult> Delete(TPrimaryKey id)
    {
        if (!HasPermission(DeletePolicyName))
            return ForwardForbid(DeletePolicyName);

        var result = await _appBaseService.Delete(id);
        return Ok(new Response<bool>(result));
    }

    // ========================== PERMISSION CHECK ==============================
    protected bool HasPermission(string func)
    {
        if (string.IsNullOrWhiteSpace(func))
            return true; // API không yêu cầu permission

        return _authenticatedUserService.HavePermission(func);
    }

    protected IActionResult ForwardForbid(string func)
    {
        return new ForbidActionResult(
            $"Bạn không có quyền thao tác {EnumHelper.GetDescriptionFromKey<CommandCode>(func)}"
        );
    }
}
