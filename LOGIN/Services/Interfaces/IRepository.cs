namespace LOGIN.Dtos
{
    public interface IRepository<T> where T : class
    {
        Task AddRangeAsync(IEnumerable<T> entities);
        Task<T> GetByIdAsync(Guid id);
    }
}
