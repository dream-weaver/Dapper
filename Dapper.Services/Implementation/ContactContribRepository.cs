using Dapper.Data;
using Dapper.Entity;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Dapper.Contrib.Extensions;
using System.Linq;
using Dapper.Entities;
using System;
using System.Transactions;

namespace Dapper.Services.Implementation
{
    public class ContactContribRepository : BaseRepository, IContactContribRepository
    {
        public ContactContribRepository(IConfiguration configuration) : base(configuration)
        {
        }
        #region Methods for MVC CRUD operation
        public IEnumerable<Contact> GetAll()
        {
            return con.GetAll<Contact>().ToList();
        }
        public bool Add(Contact contact)
        {
            var id = con.Insert(contact);
            return true;
        }
        public bool Update(Contact contact)
        {
            con.Update(contact);
            return true;
        }
        public Contact Find(int id)
        {
            return con.Get<Contact>(id);
        }
        public void Remove(int id)
        {
            con.Delete(new Contact { Id = id });
        }

        #endregion

        #region API call Methods for MVC CRUD operation using Angular
        public Contact AddContact(Contact contact)
        {
            var id = con.Insert(contact);
            contact.Id = Convert.ToInt32(id);
            return contact;
        }
        public Contact UpdateContact(Contact contact)
        {
            con.Update(contact);
            return contact;
        }
        public void Save(Contact contact)
        {
            using var txscope = new TransactionScope();           
            if (contact.IsNew)
            {
                this.AddContact(contact);
            }
            else
            {
                this.UpdateContact(contact);
            }
            foreach (var addr in contact.Addresses.Where(a => !a.IsDeleted))
            {
                addr.ContactId = contact.Id;
                if (addr.IsNew)
                {
                    this.AddAddress(addr);
                }
                else
                {
                    this.UpdateAddress(addr);
                }
            }
            foreach (var addr in contact.Addresses.Where(a => a.IsDeleted))
            {
                this.con.Execute("DELETE FROM Addresses WHERE Id = @Id", new { addr.Id });
            }
            txscope.Complete();
           
        }

        bool AddAddress(Address address)
        {
            bool inserted = false;
            var sql = "INSERT INTO[dbo].[Addresses] ([ContactId] ,[AddressType] ,[StreetAddress] ,[City] ,[StateId], [PostalCode]) VALUES " +
            " (@ContactId,@AddressType,@StreetAddress,@City,@StateId, @PostalCode) " +
            "SELECT CAST(SCOPE_IDENTITY() AS INT)";
            var id = this.con.Query(sql, address).Single();
            if (id != null)
            {
                inserted = true;
            }
            return inserted;
        }

        bool UpdateAddress(Address address)
        {
            bool inserted = false;
            var sql = "UPDATE [dbo].[Addresses] SET [ContactId] = @ContactId, [AddressType] = @AddressType, [StreetAddress] = @StreetAddress," +
            "[City] = @City, [StateId] = @StateId, [PostalCode] = @PostalCode  WHERE Id = @Id " +
            "SELECT CAST(SCOPE_IDENTITY() AS INT)";
            var id = this.con.Query(sql, address).Single();
            if (id != null)
            {
                inserted = true;
            }
            return inserted;
        }
              
        public IEnumerable<Contact> GetAllContacts()
        {
            var sql = "SELECT * FROM Contacts " +
                      "SELECT * FROM Addresses ";
            using (var multipleResults = this.con.QueryMultiple(sql))
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
        public Contact GetFullContact(int id)
        {
            var sql = "SELECT * FROM Contacts WHERE Id = @id " +
                      "SELECT * FROM Addresses WHERE ContactId = @id";
            using (var multipleResults = this.con.QueryMultiple(sql, new { Id = id }))
            {
                var contact = multipleResults.Read<Contact>().SingleOrDefault();
                var addresses = multipleResults.Read<Address>().ToList();
                if (contact!=null && addresses!=null)
                {
                    contact.Addresses.AddRange(addresses);
                }
                return contact;
            }
        }
        #endregion
    }
}
