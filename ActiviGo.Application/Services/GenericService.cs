using ActiviGo.Application.Interfaces;
using ActiviGo.Domain.Interfaces;
using AutoMapper;

namespace ActiviGo.Application.Services
{
    public class GenericService<TEntity, TDto, TCreateDto, TUpdateDto>
            : IGenericService<TEntity, TDto, TCreateDto, TUpdateDto>
            where TEntity : class
    {
        protected readonly IGenericRepository<TEntity> _repository;
        protected readonly IMapper _mapper;

        public virtual async Task<TDto> CreateAsync(TCreateDto dto)
        {
            var entity = _mapper.Map<TEntity>(dto);
            await _repository.AddAsync(entity);
            return _mapper.Map<TDto>(entity);
        }

        public GenericService(IGenericRepository<TEntity> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public virtual async Task<IEnumerable<TDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<TDto>>(entities);
        }

        public virtual async Task<TDto?> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return entity is null ? default : _mapper.Map<TDto>(entity);
        }


        public virtual async Task<TDto?> UpdateAsync(int id, TUpdateDto dto)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing is null) return default;

            _mapper.Map(dto, existing);
            await _repository.UpdateAsync(existing);
            return _mapper.Map<TDto>(existing);
        }

        public virtual async Task<bool> DeleteAsync(int id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing is null) return false;
            await _repository.DeleteAsync(id);
            return true;
        }
    }
}
