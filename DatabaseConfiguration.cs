using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Built.Data.Box
{
    public class DatabaseConfiguration
    {
        // Fields
        protected string Database = "";
        protected string DatabaseType = "";
        protected string Driver = "";
        protected int MaxConnections = 0;
        protected string Password = "";
        protected string Server = "";
        protected string User = "";

        // Methods
        public string getDatabase()
        {
            return this.Database;
        }

        public string getDatabaseType()
        {
            return this.DatabaseType;
        }

        public string getDriver()
        {
            return this.Driver;
        }

        public int getMaxConnections()
        {
            return this.MaxConnections;
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

        public void setDatabaseType(string value)
        {
            this.DatabaseType = value;
        }

        public void setDriver(string value)
        {
            this.Driver = value;
        }

        public void setMaxConnections(int value)
        {
            this.MaxConnections = value;
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
    }

}
