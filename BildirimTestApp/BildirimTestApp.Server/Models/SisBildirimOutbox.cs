using System;
using System.Collections.Generic;

namespace BildirimTestApp.Server.Models
{
    public partial class SisBildirimOutbox
    {
        public int BildirimOutboxId { get; set; }
        public int GonderimDenemeSayisi { get; set; }
        public int BildirimId { get; set; }

        public virtual SisBildirim Bildirim { get; set; } = null!;
    }
}
