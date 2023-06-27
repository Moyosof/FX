using Microsoft.Extensions.Options;
using System;
using FX.Application.Contracts;
using FX.DTO;

namespace FX.Infrastructure.Contracts
{
    public class SqlDBObjects : ISqlDBObjects
    {
        private readonly string connection_String;

        public SqlDBObjects(IOptions<ConnectionStringDTO> options)
        {
            connection_String = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT").Equals("Production") ? Environment.GetEnvironmentVariable("WebApiDatabase_Production") : Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT").Equals("Staging") ? Environment.GetEnvironmentVariable("WebApiDatabase_Staging") : options.Value.WebApiDatabase_Development;

        }
        public IStoredProcedures StoredProcedures => new StoredProcedures(connection_String);

        public ITableFunctions TableFunctions => throw new NotImplementedException();
    }
}
