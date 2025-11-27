using Transport.Infrastructure.Repositories;
using Transport.Infrastructure.Models;

namespace Transport.Infrastructure.Services
{
    public class BaseCrudService<T> : ICrudServiceAsync<T> where T : BaseEntity
    {
        private readonly IRepository<T> _repository;

        public BaseCrudService(IRepository<T> repository)
        {
            _repository = repository;
        }

        public async Task<bool> CreateAsync(T element)
        {
            try
            {
                await _repository.AddAsync(element);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<T> ReadAsync(Guid id)
        {
            return await _repository.GetByIdAsync((int)id.GetHashCode());
        }

        public async Task<IEnumerable<T>> ReadAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<IEnumerable<T>> ReadAllAsync(int page, int amount)
        {
            var allItems = await _repository.GetAllAsync();
            return allItems.Skip((page - 1) * amount).Take(amount);
        }

        public async Task<bool> UpdateAsync(T element)
        {
            try
            {
                await _repository.Update(element);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RemoveAsync(T element)
        {
            try
            {
                await _repository.Delete(element);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> SaveAsync()
        {
            try
            {
                // Оскільки репозиторій автоматично зберігає зміни після кожної операції,
                // цей метод залишається для сумісності з інтерфейсом
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}