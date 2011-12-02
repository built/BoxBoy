using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Built.Data.Box
{
    public class MySqlUrl : DatabaseUrl
    {
        // Methods
        protected override void Build()
        {
            base.Url.Renew("jdbc:mysql://%server%/%database%?user=%user%&password=%password%");
            base.Url.Replace("%server%", base.Server);
            base.Url.Replace("%database%", base.Database);
            base.Url.Replace("%user%", base.User);
            base.Url.Replace("%password%", base.Password);
        }
    }


}
