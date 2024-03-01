
namespace IntervalProcessing.Interfaces
{
    public interface IFileProcessorOptions
    {
        string QueryName { get; set; }
        string CollectionName { get; set; }
        string Projection { get; set; }
        string FileNameBase { get; set; }
        string WriterTypeKey { get; set; }
    }
}
