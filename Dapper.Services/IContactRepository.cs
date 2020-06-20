using Dapper.Entity;
using System.Collections.Generic;

namespace Dapper.Services
{
    public interface IContactRepository
    {
        #region using Dapper (only)
        Contact Find(int id);
        IEnumerable<Contact> GetAll();
        bool Add(Contact contact);
        bool Update(Contact contact);
        void Remove(int id);
        #endregion 
    }
}
