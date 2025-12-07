using UNIVERSITY.EDUCATION.PLATFORMS.Application.Common.Domain;
using UNIVERSITY.EDUCATION.PLATFORMS.Application.Common.SmartTable;
using UNIVERSITY.EDUCATION.PLATFORMS.Application.Models.BaseResponse;
using UNIVERSITY.EDUCATION.PLATFORMS.Application.Models.Paged;
using UNIVERSITY.EDUCATION.PLATFORMS.Domain.Common;

namespace UNIVERSITY.EDUCATION.PLATFORMS.SERVICE.Interface
{
    public interface IAppBaseService<TEntity, TPrimaryKey, TCreate, TUpdate, TViewOutput>
        where TCreate : class
        where TPrimaryKey : struct
        where TUpdate : DomainUpdate<TPrimaryKey>
        where TViewOutput : DomainResponse<TPrimaryKey>
    {
        Task<PagedResponse<IEnumerable<TViewOutput>>> GetPaging(SmartTableParam param);

        Task<TViewOutput> GetById(TPrimaryKey id);

        Task<TViewOutput> Create(TCreate input);

        Task<TViewOutput> Update(TUpdate input);

        Task<TViewOutput> ChangeIsActive(TPrimaryKey id, bool isActive);

        Task<bool> Delete(TPrimaryKey id);

    }
}
