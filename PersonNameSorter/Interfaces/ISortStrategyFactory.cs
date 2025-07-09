namespace PersonNameSorter.Interfaces
{
    public interface ISortStrategyFactory
    {
        ISortStrategy Create(string type);
    }
}