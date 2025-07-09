/// <summary>
/// Interface or utility involved in name sorting system.
/// </summary>
/// <remarks>
/// Defines contracts supporting DIP and Strategy pattern thereby allowing testability and flexibility.
/// </remarks>
namespace PersonNameSorter.Interfaces
{
    public interface IPersonNameSortProcessor
    {
        void Process(string inputPath);
    }
}