using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Built.Data.Box
{
    public class OracleUrl : DatabaseUrl
    {
        // Methods
        protected override void Build()
        {
            base.Url.Renew("jdbc:oracle:thin:%User%/%Password%@%Server%:%Database%");
            base.Url.Replace("%User%", base.User);
            base.Url.Replace("%Password%", base.Password);
            base.Url.Replace("%Server%", base.Server);
            base.Url.Replace("%Database%", base.Database);
        }
    }

 

}
