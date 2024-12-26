using Dapper;
using RaythaZero.Application.Common.Interfaces;
using System.Data;

namespace RaythaZero.Infrastructure.Persistence;

public class RaythaRawDbInfo : IRaythaRawDbInfo
{
    private readonly IDbConnection _db;
    public RaythaRawDbInfo(IDbConnection db)
    {
        _db = db;
    }

    public DbSpaceUsed GetDatabaseSize()
    {
        string query = "SELECT pg_size_pretty(pg_database_size(current_database())) AS reserved FROM pg_class LIMIT 1;";
        DbSpaceUsed dbSizeInfo = _db.QueryFirst<DbSpaceUsed>(query);
        return dbSizeInfo;
    }
}
