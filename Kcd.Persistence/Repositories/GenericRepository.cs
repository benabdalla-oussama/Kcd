using Kcd.Domain.Common;
using Kcd.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace Kcd.Persistence.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    protected readonly UserApplicationDatabaseContext _context;

    public GenericRepository(UserApplicationDatabaseContext context)
    {
        this._context = context;
    }

    public async Task CreateAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _context.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        _context.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<T>> GetAsync(CancellationToken cancellationToken = default) =>
        await _context.Set<T>().ToListAsync(cancellationToken);

    public async Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await _context.Set<T>()
            .FirstOrDefaultAsync(q => q.Id == id, cancellationToken);

    public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync(cancellationToken);
    }
}
