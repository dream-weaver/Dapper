using Microsoft.Extensions.Configuration;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace Dapper.Data
{
    public class BaseRepository : IDisposable
    {
        private readonly IConfiguration config;
        protected IDbConnection con;
        public BaseRepository(IConfiguration config)
        {
            this.config = config;
            con = new SqlConnection(config.GetConnectionString("DapperDBConn"));
   
        }
        public void Dispose()
        {
            //throw new NotImplementedException();  
        }
    }
}
