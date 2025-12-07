using AutoMapper;
using UNIVERSITY.EDUCATION.PLATFORMS.Infrastructure.Persistence;
using UNIVERSITY.EDUCATION.PLATFORMS.Application.Common.Domain;
using UNIVERSITY.EDUCATION.PLATFORMS.Application.Common.Infrastructure;
using UNIVERSITY.EDUCATION.PLATFORMS.Application.Service;
using UNIVERSITY.EDUCATION.PLATFORMS.Service.Interface;
using UNIVERSITY.EDUCATION.PLATFORMS.Application.Models.BaseResponse;
using UNIVERSITY.EDUCATION.PLATFORMS.Application.Models.Paged;
using UNIVERSITY.EDUCATION.PLATFORMS.Application.Common.SmartTable;
using UNIVERSITY.EDUCATION.PLATFORMS.Application.Helpers;

namespace UNIVERSITY.EDUCATION.PLATFORMS.Application.Services.GenericService
{
    public class AppBaseService<TEntity, TPrimaryKey, TCreate, TUpdate, TViewOutput>
        : BaseService,
          IAppBaseService<TEntity, TPrimaryKey, TCreate, TUpdate, TViewOutput>
        where TCreate : class
        where TPrimaryKey : struct
        where TEntity : DomainEntity<TPrimaryKey>
        where TUpdate : DomainUpdate<TPrimaryKey>
        where TViewOutput : DomainResponse<TPrimaryKey>
    {
        public AppBaseService(
            IMapper mapper,
            IDatabaseService<UEPContext> unitOfWork,
            IAuthenticatedUserService authenticatedUserService)
            : base(unitOfWork, mapper, authenticatedUserService)
        {
        }

        // ===============================================================
        // PAGING
        // ===============================================================
        public virtual async Task<PagedResponse<IEnumerable<TViewOutput>>> GetPaging(SmartTableParam param)
        {
            using var context = _unitOfWork.GetContext();

            var queryable = context.ToQueryable<TEntity>();

            var result = await _unitOfWork.GetPagedReponseAsync(queryable, param);

            var mapped = mapper.Map<IEnumerable<TViewOutput>>(result.Data);

            return new PagedResponse<IEnumerable<TViewOutput>>(mapped, result.PageNumber, result.PageSize, result.RowCount);
        }

        // ===============================================================
        // GET BY ID
        // ===============================================================
        public virtual async Task<TViewOutput> GetById(TPrimaryKey id)
        {
            using var context = _unitOfWork.GetContext();

            var entity = await context.FirstOrDefaultAsync<TEntity>(
                e => e.Id.Equals(id) && !e.IsDeleted);

            if (entity == null)
                throw new Exception("Id không hợp lệ.");

            return mapper.Map<TViewOutput>(entity);
        }

        // ===============================================================
        // CREATE
        // ===============================================================
        public virtual async Task<TViewOutput> Create(TCreate input)
        {
            using var context = _unitOfWork.GetContext();

            await CheckDataInsert(input, context);

            var entity = MapToEntity(input);

            await context.AddAsync(entity);

            return mapper.Map<TViewOutput>(entity);
        }

        // ===============================================================
        // UPDATE
        // ===============================================================
        public virtual async Task<TViewOutput> Update(TUpdate input)
        {
            using var context = _unitOfWork.GetContext();

            await CheckDataUpdate(input, context);

            var entity = await context.FirstOrDefaultAsync<TEntity>(e => e.Id.Equals(input.Id));

            if (entity == null)
                throw new Exception("Id không hợp lệ.");

            entity = AutoMapperHelper.Update(input, entity);

            await context.UpdateAsync(entity);

            return mapper.Map<TViewOutput>(entity);
        }

        // ===============================================================
        // CHANGE ACTIVE
        // ===============================================================
        public virtual async Task<TViewOutput> ChangeIsActive(TPrimaryKey id, bool isActive)
        {
            using var context = _unitOfWork.GetContext();

            var entity = await context.FirstOrDefaultAsync<TEntity>(e => e.Id.Equals(id));

            if (entity == null)
                throw new Exception("Id không hợp lệ.");

            if (entity.IsActive != isActive)
            {
                entity.IsActive = isActive;
                await context.UpdateAsync(entity);
            }

            return mapper.Map<TViewOutput>(entity);
        }

        // ===============================================================
        // DELETE (SOFT)
        // ===============================================================
        public virtual async Task<bool> Delete(TPrimaryKey id)
        {
            using var context = _unitOfWork.GetContext();

            await CheckDataDelete(id, context);

            var entity = await context.FirstOrDefaultAsync<TEntity>(e => e.Id.Equals(id));

            if (entity == null)
                throw new Exception("Id không hợp lệ.");

            entity.IsDeleted = true;

            await context.UpdateAsync(entity);

            return true;
        }

        // ===============================================================
        // VIRTUAL HOOKS (validate custom)
        // ===============================================================

        public virtual Task CheckDataDelete(TPrimaryKey id, UEPContext context)
        {
            return Task.CompletedTask;
        }

        public virtual TEntity MapToEntity(object input)
        {
            return mapper.Map<TEntity>(input);
        }

        public virtual Task CheckDataInsert(TCreate input, UEPContext context)
        {
            return Task.CompletedTask;
        }

        public virtual Task CheckDataUpdate(TUpdate input, UEPContext context)
        {
            return Task.CompletedTask;
        }
    }
}
