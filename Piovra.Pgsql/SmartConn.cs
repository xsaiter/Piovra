using Npgsql;

namespace Piovra.Pgsql;

public class SmartConn(Sql.Core.SmartDbConn<NpgsqlConnection>.Config cfg)
    : Sql.Core.SmartDbConn<NpgsqlConnection>(cfg);
