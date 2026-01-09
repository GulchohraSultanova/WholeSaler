using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WholeSalers.Application.Abstracts.Repositories;
using WholeSalers.Persistance.Context;

namespace WholeSalers.Persistance.Concretes.Repositories
{
    public class WriteRepository<T> : IWriteRepository<T> where T : class, new()
    {
        private readonly WholeSalersDbContext _WholeSalersDbContext;

        public WriteRepository(WholeSalersDbContext WholeSalersDbContext)
        {
            _WholeSalersDbContext = WholeSalersDbContext;
        }

        private DbSet<T> Table { get => _WholeSalersDbContext.Set<T>(); }
        public async Task AddAsync(T entity)
        {
            await Table.AddAsync(entity);
        }

        public async Task HardDeleteAsync(T entity)
        {
            await Task.Run(() => Table.Remove(entity));

        }
        public DbContext GetDbContext()
        {
            return _WholeSalersDbContext;
        }
        public async Task SoftDeleteAsync(T entity)
        {
            // Stub nesneyi attach et
            Table.Attach(entity);

            // Sadece IsDeleted alanını güncelle
            var entry = Table.Entry(entity);
            entry.Property("IsDeleted").CurrentValue = true;
            entry.Property("IsDeleted").IsModified = true;

            await Task.CompletedTask;
        }

        public async Task<T> UpdateAsync(T entity)
        {
            await Task.Run(() => Table.Update(entity));
            return entity;
        }

        public async Task<int> CommitAsync()
        {
            return await _WholeSalersDbContext.SaveChangesAsync();
        }
    }
}

