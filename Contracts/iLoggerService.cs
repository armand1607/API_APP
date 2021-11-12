using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_APP.Contracts
{
    public interface ILoggerService
    {
        void LogInfo(string message);

        void LogWarn(string message);

        void LogDebug(string message);

        void LogError(string message);

    }
}
