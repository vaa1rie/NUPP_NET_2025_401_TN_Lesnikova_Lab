using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Common
{
    public class CrudService<T> : ICrudService<T> where T : class
    {
        private readonly Dictionary<Guid, T> _storage = new Dictionary<Guid, T>();

        public void Create(T element)
        {
            var id = GetId(element);
            _storage[id] = element;
        }

        public T Read(Guid id)
        {
            return _storage.ContainsKey(id) ? _storage[id] : null;
        }

        public IEnumerable<T> ReadAll()
        {
            return _storage.Values;
        }

        public void Update(T element)
        {
            var id = GetId(element);
            if (_storage.ContainsKey(id))
            {
                _storage[id] = element;
            }
        }

        public void Remove(T element)
        {
            var id = GetId(element);
            _storage.Remove(id);
        }

   
        private Guid GetId(T element)
        {
            var idProperty = element.GetType().GetProperty("Id");
            if (idProperty == null)
                throw new Exception("The object does not have the property'Id'.");

            var idValue = idProperty.GetValue(element);
            if (idValue == null)
                throw new Exception("The 'Id' property has a null value.");

            return (Guid)idValue;
        }
    }
}
