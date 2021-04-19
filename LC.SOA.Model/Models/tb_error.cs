namespace LC.SOA.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tb_error
    {
        public long id { get; set; }

        public int mqpathid { get; set; }

        [Required]
        [StringLength(300)]
        public string mqpath { get; set; }

        [Required]
        [StringLength(500)]
        public string methodname { get; set; }

        [Required]
        public string info { get; set; }

        public DateTime createtime { get; set; }
    }
}
