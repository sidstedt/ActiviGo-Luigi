namespace ActiviGo.Application.Interfaces
{
    public interface IGenericService<TEntity, TDto, TCreateDto, TUpdateDto>
        where TEntity : class
    {
        Task<TDto> CreateAsync(TCreateDto dto);
        Task<IEnumerable<TDto>> GetAllAsync();
        Task<TDto?> GetByIdAsync(int id);
        Task<TDto?> UpdateAsync(int id, TUpdateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
