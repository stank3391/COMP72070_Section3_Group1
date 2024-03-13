namespace AstroServer
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_Post
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int PostId { get; set; }

        public int UserId { get; set; }

        [StringLength(255)]
        public string Type { get; set; }

        [StringLength(255)]
        public string Content { get; set; }

        public DateTime? Date { get; set; }
    }
}
