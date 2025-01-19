namespace HolidayAPI.Services
{
  public interface IService<TEntity, TKey>
  {
    Task<List<TEntity>> GetAllAsync();
    Task<TEntity> GetByIdAsync(TKey id);
    Task<TEntity> CreateAsync(TEntity entity);
    Task<TEntity> UpdateAsync(TKey id, TEntity entity);
    Task<bool> DeleteAsync(TKey id);
  }
}
