using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Dapper.Entity
{
    public class Contact
    {
        public int Id { get; set; }
        [Required, Display(Name ="First Name")]
        public string FirstName { get; set; }
        [Required, Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string Company { get; set; }
        public string Title { get; set; }

        [Computed]
        public bool IsNew => this.Id == default(int);

        [Write(false)]
        public List<Address> Addresses { get; set;  } = new List<Address>();

    }
}