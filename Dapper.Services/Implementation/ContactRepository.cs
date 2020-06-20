using Dapper.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Dapper;
using Dapper.Data;
using Microsoft.Extensions.Configuration;
using System.Reflection.Metadata.Ecma335;

namespace Dapper.Services.Implementation
{
    public class ContactRepository : BaseRepository, IContactRepository
    {
        public ContactRepository(IConfiguration config) : base(config)
        {
        }

        Contact IContactRepository.Find(int id)
        {
            return con.Query<Contact>("SELECT * FROM CONTACTS WHERE Id = @id",new { id}).FirstOrDefault();
        }

        IEnumerable<Contact> IContactRepository.GetAll() => con.Query<Contact>("SELECT * FROM CONTACTS").ToList();

        void IContactRepository.Remove(int id)
        {
            this.con.Execute("DELETE FROM [dbo].[Contacts] WHERE Id = @Id", new { id });
        }

        bool IContactRepository.Add(Contact contact)
        {
            bool inserted = false;
            var sql = "INSERT INTO[dbo].[Contacts] ([FirstName] ,[LastName] ,[Email] ,[Company] ,[Title]) VALUES " +
            " (@FirstName,@LastName,@Email,@Company,@Title) " +
            "SELECT CAST(SCOPE_IDENTITY() AS INT)";

            var id = this.con.Query(sql, contact).Single();
            if (id!=null)
            {
                inserted = true;
            }
            return inserted;
        }

        bool IContactRepository.Update(Contact contact)
        {
            bool inserted = false;
            var sql = "UPDATE [dbo].[Contacts] SET [FirstName] = @FirstName, [LastName] = @LastName, [Email] = @Email," +
            "[Company] = @Company, [Title] = @Title WHERE Id = @Id " +
            "SELECT CAST(SCOPE_IDENTITY() AS INT)";
            var id = this.con.Query(sql, contact).Single();
            if (id != null)
            {
                inserted = true;
            }
            return inserted;
        }
    }
}
