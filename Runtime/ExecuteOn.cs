using System;

namespace Mirzipan.Heist
{
    [Flags]
    public enum ExecuteOn
    {
        None = 0,
        OneClient = 1 << 0,
        AllClients = 1 << 1,
        Server = 1 << 2,
        OneClientAndServer = OneClient | Server,
        AllClientsAndServer = AllClients | Server,
    }
}