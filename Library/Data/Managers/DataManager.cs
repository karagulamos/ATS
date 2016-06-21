namespace Library.Data.Managers
{
    internal class DataManager<T> : AbstractDataManager<T, AtsDbContext>, IDataManager<T> where T : class
    {
    }
}
