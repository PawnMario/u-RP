using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace uRP.Database.Model
{
    public class Account
    {
        [Key]
        public long? member_id { get; private set; }

        public string name { get; set; }

        public string member_pass_hash { get; private set; }
    }
}
