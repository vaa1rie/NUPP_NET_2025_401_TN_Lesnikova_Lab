using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;

public class InMemoryCrudServiceAsync<T> : ICrudServiceAsync<T> where T : class
{
    private readonly ConcurrentDictionary<Guid, T> _storage = new ConcurrentDictionary<Guid, T>();
    private readonly string _filePath;
    private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

    public InMemoryCrudServiceAsync(string filePath)
    {
        _filePath = filePath;
        LoadFromFile();
    }

    public async Task<bool> CreateAsync(T element)
    {
        var id = (Guid)element.GetType().GetProperty("Id").GetValue(element);
        var result = _storage.TryAdd(id, element);
        await Task.CompletedTask;
        return result;
    }

    public async Task<T> ReadAsync(Guid id)
    {
        _storage.TryGetValue(id, out var value);
        await Task.CompletedTask;
        return value;
    }

    public async Task<IEnumerable<T>> ReadAllAsync()
    {
        await Task.CompletedTask;
        return _storage.Values.ToList();
    }

    public async Task<IEnumerable<T>> ReadAllAsync(int page, int amount)
    {
        await Task.CompletedTask;
        return _storage.Values.Skip((page - 1) * amount).Take(amount).ToList();
    }

    public async Task<bool> UpdateAsync(T element)
    {
        var id = (Guid)element.GetType().GetProperty("Id").GetValue(element);
        if (_storage.ContainsKey(id))
        {
            _storage[id] = element;
            await Task.CompletedTask;
            return true;
        }
        return false;
    }

    public async Task<bool> RemoveAsync(T element)
    {
        var id = (Guid)element.GetType().GetProperty("Id").GetValue(element);
        var result = _storage.TryRemove(id, out _);
        await Task.CompletedTask;
        return result;
    }

    public async Task<bool> SaveAsync()
    {
        return await Task.Run(() =>
        {
            _lock.EnterWriteLock();
            try
            {
                using (var fs = new FileStream(_filePath, FileMode.Create))
                {
                    var formatter = new BinaryFormatter();
                    formatter.Serialize(fs, _storage.Values.ToList());
                }
                return true;
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        });
    }

    private void LoadFromFile()
    {
        if (!File.Exists(_filePath)) return;
        _lock.EnterWriteLock();
        try
        {
            using (var fs = new FileStream(_filePath, FileMode.Open))
            {
                var formatter = new BinaryFormatter();
                var list = (List<T>)formatter.Deserialize(fs);
                foreach (var item in list)
                {
                    var id = (Guid)item.GetType().GetProperty("Id").GetValue(item);
                    _storage.TryAdd(id, item);
                }
            }
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    public IEnumerator<T> GetEnumerator() => _storage.Values.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
