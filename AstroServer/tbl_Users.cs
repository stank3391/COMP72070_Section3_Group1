namespace AstroServer
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_Users
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [StringLength(255)]
        public string Username { get; set; }

        [StringLength(255)]
        public string Email { get; set; }

        [StringLength(255)]
        public string ProfilePic { get; set; }

        public DateTime? JoinDate { get; set; }

        [StringLength(255)]
        public string Role { get; set; }
    }
}
