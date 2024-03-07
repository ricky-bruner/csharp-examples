using CoreUtilities.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreUtilities.Processors
{
    public interface IDataProcessor
    {
        Task Execute(RunCadence runCadence);
    }
}
