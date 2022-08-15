using System;

namespace BHub.Infra.Environments
{
    public static class Env
    {
        public static readonly Func<string, string> Get = env => Environment.GetEnvironmentVariable(env);
    }
}
