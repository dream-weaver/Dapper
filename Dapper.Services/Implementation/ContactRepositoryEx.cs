using Dapper.Data;
using Dapper.Entity;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Dapper.Services.Implementation
{
    public class ContactRepositoryEx : BaseRepository
    {
        public ContactRepositoryEx(IConfiguration configuration) : base(configuration)
        {
        }
        //Using in operator support for Dapper
        public List<Contact> GetContactsById(params int[] ids)
        {
            return con.Query<Contact>("SELECT * FROM Contacts where Id IN @ids", new { Id = ids}).ToList();
        }
        //Dynamic Capabilities in Dapper
        public List<dynamic> GetDyncamicContactsById(params int[] ids)
        {
            return con.Query<dynamic>("SELECT * FROM Contacts where Id IN @ids", new { Id = ids }).ToList();
        }
        //Bulk Insert method in Dapper
        public int BulkInsertContacts(Contact contacts)
        {
            var sql = "INSERT INTO[dbo].[Contacts] ([FirstName] ,[LastName] ,[Email] ,[Company] ,[Title]) VALUES " +
                      " (@FirstName,@LastName,@Email,@Company,@Title) " +
                      "SELECT CAST(SCOPE_IDENTITY() AS INT)";
            return con.Execute(sql, contacts, commandType: CommandType.Text );
        }
        //Literal Replacements
        public List<Address> GetAddressesByState(int stateId)
        {
            return this.con.Query<Address>("SELECT * FROM Addresses WHERE StateId = {=stateId}", new { stateId }).ToList();
        }
        // == ** Multi Mapping ** ==
        public List<Contact> GetAllContactsWithAddresses()
        {
            var sql = "select * from contacts c " +
                     " inner join addresses a " +
                     " on a.ContactId = c.Id";
            var contactDict = new Dictionary<int, Contact>();
            var contacts =  con.Query<Contact, Address, Contact>(sql, (contact, address) =>
            {
                if (!contactDict.TryGetValue(contact.Id, out var currentContact))
                {
                    currentContact = contact;
                    contactDict.Add(currentContact.Id, currentContact);
                }
                currentContact.Addresses.Add(address);
                return currentContact;
            });
            return contacts.Distinct().ToList();
        }

        //Async Capabilities
        //public async Task<Contact> GetContactById(int id)
        //{
        //   var contact =  await con.QueryAsync<Contact>("SELECT * FROM Contacts where Id = @id", new {id }).FirstOrDefault();
        //   return contact;
        //}

    }
}
