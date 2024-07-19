using System;
using System.Collections.Generic;

namespace BildirimTestApp.Server.Models
{
    public partial class Gorev
    {
        public Gorev()
        {
            SisKullanicisKullanicis = new HashSet<SisKullanici>();
        }

        public int GorevId { get; set; }
        public string? Aciklama { get; set; }
        public DateTime GorevSonTarih { get; set; }

        public virtual ICollection<SisKullanici> SisKullanicisKullanicis { get; set; }
    }
}
