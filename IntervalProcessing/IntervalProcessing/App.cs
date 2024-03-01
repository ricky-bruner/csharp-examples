using IntervalProcessing.Interfaces;

namespace IntervalProcessing
{
    public class App
    {
        private readonly IFileProcessor _inventoryFileProcessor;

        public App(IFileProcessor inventoryFileProcessor) 
        { 
            _inventoryFileProcessor = inventoryFileProcessor;
        }

        public void Run() 
        {
            _inventoryFileProcessor.Execute();
        }
    }
}
