using System;
using System.Collections.Generic;

namespace BildirimTestApp.Server.Models
{
    public partial class SisBildirimIcerik
    {
        public SisBildirimIcerik()
        {
            SisBildirims = new HashSet<SisBildirim>();
        }

        public int BildirimIcerikId { get; set; }
        public string Json { get; set; } = null!;
        public DateTime OlusturulanTarih { get; set; }
        public string? Aciklama { get; set; }

        public virtual ICollection<SisBildirim> SisBildirims { get; set; }
    }
}
