using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dapper.Data
{
    public class ApplicationDBContext : IdentityDbContext<AppIdentityStoreUser>
    {
        public ApplicationDBContext(DbContextOptions options) : base(options)
        {
        }
    }
}
