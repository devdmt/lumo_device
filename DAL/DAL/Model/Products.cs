using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Model
{
    public class PartnersProducts
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(100)]
        public string? Description { get; set; }
        public virtual Partners Partner { get; set; }
        public int? PartnerId { get; set; }
        public string? Image { get; set; }
        public bool Active { get; set; } =false;
        public DateTime? CreatedOn { get; set; }
        public string? CreatedBy { get; set; }
    }
}
