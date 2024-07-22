using HandlebarsDotNet;
using Microsoft.EntityFrameworkCore.Design;

namespace BildirimTestApp.Server.ScaffoldingDesignTimeServices
{
    //Scaffold-DbContext "Data Source=.;Initial Catalog=TestDb;Integrated Security=True;Trust Server Certificate=True" Microsoft.EntityFrameworkCore.SqlServer -o Models -force
    public class ScaffoldingDesignTimeServices : IDesignTimeServices
    {
        public void ConfigureDesignTimeServices(IServiceCollection services)
        {
            services.AddHandlebarsScaffolding(options =>
            {
                Handlebars.RegisterHelper("ifClass", (output, options, context, arguments) =>
                {
                    var parameters = arguments[0].ToString();
                    if (parameters == "SisBildirim" )
                    {
                        //handlebars yardımcı kodunun if blogunun calismasi icin
                        options.Template(output, context);
                    }
                    else
                    {
                        //handlebars yardımcı kodunun else blogunun calismasi icin
                        options.Inverse(output, context);
                    }
                });
            });
        }
    }
}
