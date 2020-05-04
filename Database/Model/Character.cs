using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace uRP.Database.Model
{
    public class Character
    {
        [Key]
        public long? id { get; private set; }

        public long gid { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
    }
}
