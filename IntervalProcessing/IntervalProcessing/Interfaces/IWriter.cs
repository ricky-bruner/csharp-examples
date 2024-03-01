using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntervalProcessing.Interfaces
{
    public interface IWriter<T>
    {
        void Write(T input);
        void WriteMultiple(IEnumerable<T> inputs);
        void Close();
    }
}
