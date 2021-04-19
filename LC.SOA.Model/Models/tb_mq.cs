namespace LC.SOA.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tb_mq
    {
        public int id { get; set; }

        [Required]
        [StringLength(50)]
        public string mqname { get; set; }

        public DateTime createtime { get; set; }
    }
}
