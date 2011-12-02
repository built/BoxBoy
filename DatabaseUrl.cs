using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Built.Text;

namespace Built.Data.Box
{
    public class DatabaseUrl
    {
        // Fields
        protected string Database = "";
        protected string Password = "";
        protected string Server = "";
        protected StringTheory Url = new StringTheory();
        protected string User = "";

        // Methods
        protected virtual void Build()
        {
        }

        public string getDatabase()
        {
            return this.Database;
        }

        public string getPassword()
        {
            return this.Password;
        }

        public string getServer()
        {
            return this.Server;
        }

        public string getUser()
        {
            return this.User;
        }

        public void setDatabase(string value)
        {
            this.Database = value;
        }

        public void setPassword(string value)
        {
            this.Password = value;
        }

        public void setServer(string value)
        {
            this.Server = value;
        }

        public void setUser(string value)
        {
            this.User = value;
        }

        public override string ToString()
        {
            this.Build();
            return this.Url.ToString();
        }
    }

}
