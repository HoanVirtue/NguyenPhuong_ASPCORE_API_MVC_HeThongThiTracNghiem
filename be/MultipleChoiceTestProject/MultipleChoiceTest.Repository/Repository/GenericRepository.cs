using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MultipleChoiceTest.Domain;
using MultipleChoiceTest.Domain.ModelViews;
using MultipleChoiceTest.Repository.Authorizations;
using System.Linq.Expressions;

namespace MultipleChoiceTest.Repository.Repository
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        Task<IEnumerable<TEntity>> GetAllAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");
        Task<Pagination<TEntity>> GetGridAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "",
            int? pageIndex = null,
            int? pageSize = null);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity?> GetByIdAsync(object id);
        Task<TEntity?> GetByIdAsync(object id, Expression<Func<TEntity, bool>> filter = null, string includeProperties = "");
        Task AddAsync(TEntity entity);
        Task AddRangeAsync(List<TEntity> entities);
        Task UpdateAsync(TEntity entity);
        Task UpdateRangeAsync(List<TEntity> entities);
        Task SoftRemoveAsync(TEntity entity);
        Task SoftRemoveRangeAsync(List<TEntity> entities);
        Task DeleteAsync(object id);
        Task Delete(TEntity entity);
        Task<int> CountAsync(Expression<Func<TEntity, bool>> filter = null);
    }

    public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        protected MultipleChoiceTestDbContext _dbContext;
        protected DbSet<TEntity> _dbSet;
        private MultipleChoiceTestDbContext dbContext;
        protected readonly IUserContextService _userContext;
        protected readonly IMapper _mapper;

        public GenericRepository(MultipleChoiceTestDbContext dbContext, IUserContextService userContextService, IMapper mapper)
        {
            _dbContext = dbContext;
            _userContext = userContextService;
            _dbSet = _dbContext.Set<TEntity>();
            _mapper = mapper;
        }

        public GenericRepository(MultipleChoiceTestDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // Get all with filter, order by, include properties or not
        public async Task<IEnumerable<TEntity>> GetAllAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return await query.ToListAsync();
        }

        public async Task<Pagination<TEntity>> GetGridAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "",
            int? pageIndex = null,
            int? pageSize = null)
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            var totalItemsCount = await query.CountAsync();

            if (pageIndex.HasValue && pageIndex.Value == -1)
            {
                pageSize = totalItemsCount; // Set pageSize to total count
                pageIndex = 0; // Reset pageIndex to 0
            }
            else if (pageIndex.HasValue && pageSize.HasValue)
            {
                int validPageIndex = pageIndex.Value > 0 ? pageIndex.Value : 0;
                int validPageSize = pageSize.Value > 0 ? pageSize.Value : 10; // Assuming a default pageSize of 10 if an invalid value is passed

                query = query.Skip(validPageIndex * validPageSize).Take(validPageSize);
            }

            var items = await query.ToListAsync();

            return new Pagination<TEntity>
            {
                TotalItemsCount = totalItemsCount,
                PageSize = pageSize ?? totalItemsCount,
                PageIndex = pageIndex ?? 0,
                Items = items
            };
        }

        public async Task<TEntity?> GetByIdAsync(object id)
        {
            return await _dbSet.SingleOrDefaultAsync(x => x.Id == int.Parse(id.ToString()) && x.IsDeleted != true);
        }

        public async Task<TEntity?> GetByIdAsync(object id, Expression<Func<TEntity, bool>> filter = null, string includeProperties = "")
        {
            IQueryable<TEntity> query = _dbSet;

            // Filter
            if (filter != null)
            {
                query = query.Where(filter);
            }

            // Include properties
            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            var keyName = _dbContext.Model.FindEntityType(typeof(TEntity)).FindPrimaryKey().Properties
                .Select(x => x.Name).Single();

            var parameter = Expression.Parameter(typeof(TEntity));
            var property = Expression.Property(parameter, keyName);
            var equal = Expression.Equal(property, Expression.Constant(id));
            var lambda = Expression.Lambda<Func<TEntity, bool>>(equal, parameter);

            return await query.FirstOrDefaultAsync(lambda);
        }

        // Add entity
        public async Task AddAsync(TEntity entity)
        {
            entity.CreatedDate = DateTime.Now;
            entity.CreatedBy = _userContext.GetCurrentUsername();
            entity.IsDeleted = false;
            await _dbSet.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        // Add range of entities
        public async Task AddRangeAsync(List<TEntity> entities)
        {
            await _dbSet.AddRangeAsync(entities);
            await _dbContext.SaveChangesAsync();
        }

        // Update entity
        public async Task UpdateAsync(TEntity entity)
        {
            entity.UpdatedDate = DateTime.Now;
            entity.UpdatedBy = _userContext.GetCurrentUsername();
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        // Update range of entities
        public async Task UpdateRangeAsync(List<TEntity> entities)
        {
            _dbSet.UpdateRange(entities);
            await _dbContext.SaveChangesAsync();
        }

        // Soft remove entity
        public async Task SoftRemoveAsync(TEntity entity)
        {
            entity.IsDeleted = true;
            _dbSet.Update(entity);
            await _dbContext.SaveChangesAsync();
        }

        // Soft remove range of entities
        public async Task SoftRemoveRangeAsync(List<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                entity.IsDeleted = true;
            }
            _dbSet.UpdateRange(entities);
            await _dbContext.SaveChangesAsync();
        }

        // Delete entity by Id
        public async Task DeleteAsync(object id)
        {
            TEntity entityToDelete = _dbSet.Find(id);
            await Delete(entityToDelete);
        }

        // Delete entity
        public async Task Delete(TEntity entityToDelete)
        {
            if (_dbContext.Entry(entityToDelete).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }
            _dbSet.Remove(entityToDelete);
            await _dbContext.SaveChangesAsync();
        }

        // Count entities with filter
        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.CountAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbSet.Where(x => x.IsDeleted != true).ToListAsync();
        }
    }
}
