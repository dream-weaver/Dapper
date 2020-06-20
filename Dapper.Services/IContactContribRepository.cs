using Dapper.Entities;
using Dapper.Entity;
using System.Collections.Generic;

namespace Dapper.Services
{
    public interface IContactContribRepository
    {

        #region For API methods using Dapper Extension
        Contact GetFullContact(int id);
        IEnumerable<Contact> GetAllContacts();
        void Save(Contact contact);
        #endregion

        #region For MVC CRUD using Dapper Extension
        IEnumerable<Contact> GetAll();
        Contact Find(int id);
        bool Add(Contact contact);
        bool Update(Contact contact);
        void Remove(int id);
        #endregion
    }
}
