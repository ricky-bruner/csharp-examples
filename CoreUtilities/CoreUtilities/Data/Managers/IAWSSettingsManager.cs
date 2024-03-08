using CoreUtilities.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreUtilities.Data.Managers
{
    public interface IAWSSettingsManager
    {
        Task<AWSSettings> GetAWSSettingsAsync();
    }
}
