using Dapper.Data;
using Dapper.Entity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using System.Linq;
using System.Data;
using System.Transactions;

namespace Dapper.Services.Implementation
{
    public class ContactRepositorySP : BaseRepository, IContactRepository
    {
        public ContactRepositorySP(IConfiguration configuration) : base(configuration)
        {
        }

        public bool Add(Contact contact)
        {
            using var txscope = new TransactionScope();
            var parameters = new DynamicParameters();
            parameters.Add("@Id", value: contact.Id, dbType: DbType.Int32, direction: ParameterDirection.InputOutput);
            parameters.Add("@FirstName", contact.FirstName);
            parameters.Add("@LastName", contact.LastName);
            parameters.Add("@Company", contact.Company);
            parameters.Add("@Title", contact.Title);
            parameters.Add("@Email", contact.Email);
            this.con.Execute("SaveContact", parameters, commandType: CommandType.StoredProcedure);
            contact.Id = parameters.Get<int>("@Id");

            foreach (var addr in contact.Addresses.Where(a=>!a.IsDeleted))
            {
                addr.ContactId = contact.Id;
                var addrParameters = new DynamicParameters( new 
                {
                    ContactId = addr.ContactId,
                    AddressType = addr.AddressType,
                    StreetAddress = addr.StreetAddress,
                    City = addr.City,
                    StateId = addr.StateId,
                    PostalCode = addr.PostalCode
                });
                addrParameters.Add("@Id", value: addr.Id, dbType: DbType.Int32, direction: ParameterDirection.InputOutput);
                this.con.Execute("SaveAddress", addrParameters, commandType: CommandType.StoredProcedure);
                addr.Id = parameters.Get<int>("Id");
            }
            foreach (var addr in contact.Addresses.Where(a => a.IsDeleted))
            {
                this.con.Execute("DeleteAddress", new { Id=addr.Id}, commandType: CommandType.StoredProcedure );
            }
            
            txscope.Complete();
            return true;
        }

        public Contact Find(int id)
        {
            using (var multipleResults = this.con.QueryMultiple("GetContact", new { Id = id }, commandType: CommandType.StoredProcedure))
            {
                var contact = multipleResults.Read<Contact>().SingleOrDefault();
                var addresses = multipleResults.Read<Address>().ToList();
                if (contact != null && addresses != null)
                {
                    contact.Addresses.AddRange(addresses);
                }
                return contact;
            }
        }

        public IEnumerable<Contact> GetAll()
        {
            using (var multipleResults = this.con.QueryMultiple("GetFullContact", commandType: CommandType.StoredProcedure))
            {
                var contacts = multipleResults.Read<Contact>().ToList();
                var addresses = multipleResults.Read<Address>().ToList();

                foreach (var contact in contacts)
                {
                    contact.Addresses.AddRange(addresses.Where(a => a.ContactId == contact.Id));
                }

                return contacts;
            }
        }

        public void Remove(int id)
        {
            this.con.Execute("DeleteContact", commandType: CommandType.StoredProcedure);
        }

        public bool Update(Contact contact)
        {
            throw new NotImplementedException();
        }
    }
}
