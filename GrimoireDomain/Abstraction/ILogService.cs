using System;
using Grimoire.Domain.Models;

namespace Grimoire.Domain.Abstraction.Services
{
    public interface ILogService
    {
        void Log(Exception e);

        void Log(string message, LogLevel level);
    }
}