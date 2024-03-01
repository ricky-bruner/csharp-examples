
namespace IntervalProcessing.Writers
{
    public interface IWriter<T>
    {
        void Write(T input);
        void WriteMultiple(IEnumerable<T> inputs);
        void Close();
    }
}
