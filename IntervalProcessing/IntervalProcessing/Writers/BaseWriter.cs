using IntervalProcessing.Interfaces;
using System;
using System.IO;

namespace IntervalProcessing.Writers
{
    public abstract class BaseWriter<T> : IWriter<T>
    {
        private StreamWriter _writer;
        public string FileName { get; private set; }
        public FileInfo FileInfo { get; private set; }

        public BaseWriter(FileInfo file)
        {
            FileInfo = file;
            FileName = file.Name;
            _writer = new StreamWriter(FileInfo.OpenWrite());
            WriteHeader();
        }

        protected abstract string Parse(T input);

        protected abstract string GetHeader();

        public void Write(T input)
        {
            string line = Parse(input);
            _writer.WriteLine(line);
        }

        public void WriteHeader()
        {
            string line = GetHeader();
            _writer.WriteLine(line);
        }

        public void WriteMultiple(IEnumerable<T> inputs)
        {
            foreach (T input in inputs)
            {
                Write(input);
            }
        }

        public void Close()
        {
            _writer?.Close();
            _writer?.Dispose();
        }
    }
}