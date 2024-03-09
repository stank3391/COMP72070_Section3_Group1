namespace AstroServer
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_Token
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TokenId { get; set; }

        public int UserId { get; set; }

        [StringLength(255)]
        public string Provider { get; set; }

        [StringLength(255)]
        public string AccessToken { get; set; }

        [StringLength(255)]
        public string RefreshToken { get; set; }

        public DateTime? CreationDate { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public virtual tbl_Users tbl_Users { get; set; }
    }
}
