using System.Security.Claims;
using BildirimTestApp.Server.Servisler.Kullanici;
using Microsoft.AspNetCore.Authentication;

namespace BildirimTestApp.Server.Servisler;

public class ClaimsTransformerGelistirici : IClaimsTransformation
{
    private readonly ILogger logger;
    private readonly IKullaniciBilgiServisi kullaniciBilgiServisi;

    public ClaimsTransformerGelistirici(
        ILogger<ClaimsTransformerGelistirici> logger,
        IKullaniciBilgiServisi kullaniciBilgiServisi
    )
    {
        this.logger = logger;
        this.kullaniciBilgiServisi = kullaniciBilgiServisi;
    }

    public class TokenKullaniciBilgisiGelistirici
    {
        public string KullaniciAdi { get; set; } = null!;

        public TokenKullaniciBilgisiGelistirici(ClaimsIdentity claimsIdentity)
        {
            if (claimsIdentity.HasClaim(x => x.Type == "kullaniciAdi"))
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                claimsIdentity.AddClaim(
                    new Claim(
                        ClaimTypes.Name,
                        KullaniciAdi = claimsIdentity.FindFirst(x => x.Type == "kullaniciAdi").Value
                    )
                );
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            else
                throw new Exception("Kullanıcı adı hatası");
        }
    }

    public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        try
        {
            ClaimsIdentity claimsIdentity = (ClaimsIdentity)principal.Identity;
            if (claimsIdentity != null && claimsIdentity.IsAuthenticated)
            {
                TokenKullaniciBilgisiGelistirici tokenKullaniciBilgisi;
                try
                {
                    tokenKullaniciBilgisi = new TokenKullaniciBilgisiGelistirici(claimsIdentity);
                    //var kullaniciBilgi = kullaniciBilgiServisi.GetKullaniciBilgi(tokenKullaniciBilgisi.KullaniciAdi);
                    var kullaniciBilgi = kullaniciBilgiServisi.GetKullaniciBilgi(
                        tokenKullaniciBilgisi.KullaniciAdi
                    );
                    if (kullaniciBilgi == null)
                        throw new Exception("Kullanıcı hatası");
                    //foreach (var m in kullaniciBilgi.Roller)
                    //   claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, m));
                }
                catch (Exception e)
                {
                    logger.LogError(
                        "Token kullanıcı bilgisi alınamadı " + claimsIdentity.Claims.ToString()
                    );
                    throw e;
                }
            }
        }
        catch (Exception)
        {
            throw;
            //logger.LogError(nameof(ClaimsTransformer));
        }

        return Task.FromResult(principal);
    }
}
