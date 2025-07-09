namespace PersonNameSorter.Interfaces
{
    public interface IWriteStrategyFactory
    {
        IWriteStrategy Create(string type, string? outputPath = null);
    }
}