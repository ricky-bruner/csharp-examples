using System;
using System.IO;

namespace IntervalProcessing.Utilities
{
    public abstract class BaseWriter<T>
    {
        private StreamWriter _writer;
        public string FileName { get; private set; }
        public FileInfo FileInfo { get; private set; }

        public BaseWriter(string fileName)
        {
            FileName = fileName;
            FileInfo = new FileInfo(fileName);
            _writer = new StreamWriter(FileInfo.OpenWrite());
        }

        protected abstract string Parse(T input);

        public void Write(T input)
        {
            string line = Parse(input);
            _writer.WriteLine(line);
        }

        public void WriteMultiple(IEnumerable<T> inputs)
        {
            foreach (var input in inputs)
            {
                Write(input);
            }
        }

        public void Close()
        {
            _writer?.Close();
            Dispose();
        }

        public void Dispose()
        {
            _writer?.Dispose();
        }
    }
}