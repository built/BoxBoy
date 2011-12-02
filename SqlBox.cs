using System;
using System.Collections;
using System.Text;
using System.Xml;
using Built.Data.Box.Queries;
using Built.Text;

namespace Built.Data.Box
{
    public class SqlBox : Built.Data.Box.Box
    {
        // Fields
        protected DatabaseConfiguration Config = new DatabaseConfiguration();
        protected SqlBoxDatabase SqlDatabase = new SqlBoxDatabase();

        // Methods
        public override bool close()
        {
            return this.SqlDatabase.disconnect();
        }

        protected bool CreateNew(Item item)
        {
            SqlBoxInsertQuery query = new SqlBoxInsertQuery(item);
            if (this.SqlDatabase.process(query) > 0)
            {
                if (item.UsesAutoId)
                {
                    return this.ReloadNewBoxItem(item);
                }
                return true;
            }
            base.error.message = this.SqlDatabase.error.message;
            base.error.info = this.SqlDatabase.error.info;
            return false;
        }

        public bool createStream(Item item)
        {
            SqlBoxQuery query = new SqlBoxQuery();
            if (item.UsesAutoId)
            {
                query.Append("CREATE TABLE %table%(%pk% int IDENTITY(1,1) PRIMARY KEY CLUSTERED%fields%)");
            }
            else
            {
                query.Append("CREATE TABLE %table%(%pk% int PRIMARY KEY CLUSTERED%fields%)");
            }
            string val = ", %name% varchar(255) DEFAULT NULL";
            string strA = item.getIdField();
            StringTheory theory = new StringTheory();
            foreach (string str3 in item.getBoxView().Keys)
            {
                if (string.Compare(strA, str3, true) != 0)
                {
                    theory.Append(val);
                    theory.Replace("%name%", str3);
                }
            }
            query.Replace("%table%", item.getStreamName());
            query.Replace("%pk%", strA);
            query.Replace("%fields%", theory);
            Console.WriteLine(query);
            this.SqlDatabase.process(query);
            return this.streamExists(item);
        }

        public bool deleteStream(Item item)
        {
            SqlBoxQuery query = new SqlBoxQuery();
            query.Append("drop table " + item.getStreamName());
            this.SqlDatabase.process(query);
            return !this.streamExists(item);
        }

        public XmlDocument Describe()
        {
            return this.SqlDatabase.Describe();
        }

        public XmlDocument Describe(string streamName)
        {
            XmlDocument document = new XmlDocument();
            StringTheory theory = new StringTheory("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            document.LoadXml(this.SqlDatabase.DescribeStream(streamName).ToString());
            return document;
        }

        public void Finalize()
        {
            this.close();
        }

        protected bool FindById(Item item)
        {
            SqlBoxQuery query;
            bool flag = false;
            if ((item == null) || item.isNew())
            {
                return flag;
            }
            if (string.Compare(this.Config.getDatabaseType(), "sqlserver", true) == 0)
            {
                query = new SqlServerFindQuery(item);
            }
            else
            {
                query = new SqlBoxFindQuery(item);
            }
            ArrayList results = new ArrayList();
            return (this.SqlDatabase.process(query, results) > 0);
        }

        protected string GetConfigurationItem(string itemName, XmlNode info)
        {
            string str = "";
            XmlNode node = info.SelectSingleNode(itemName);
            if ((node != null) && (node.InnerText != null))
            {
                str = node.InnerText;
            }
            return str;
        }

        public bool open()
        {
            this.SqlDatabase.disconnect();
            return this.SqlDatabase.connect(this.Config);
        }

        public override bool open(string configFile)
        {
            bool flag = false;
            StringTheory theory = new StringTheory();
            if (theory.LoadFile(configFile))
            {
                XmlDocument info = new XmlDocument();
                info.LoadXml(theory.ToString());
                if (info != null)
                {
                    StringTheory theory2 = new StringTheory("0" + this.GetConfigurationItem("max_connections", info));
                    this.Config.setDatabaseType(this.GetConfigurationItem("db_type", info.DocumentElement));
                    this.Config.setDriver(this.GetConfigurationItem("driver", info.DocumentElement));
                    this.Config.setMaxConnections(theory2.ToInt());
                    this.Config.setServer(this.GetConfigurationItem("server", info.DocumentElement));
                    this.Config.setDatabase(this.GetConfigurationItem("database", info.DocumentElement));
                    this.Config.setUser(this.GetConfigurationItem("user", info.DocumentElement));
                    this.Config.setPassword(this.GetConfigurationItem("password", info.DocumentElement));
                    flag = this.open();
                }
            }
            return flag;
        }

        public bool open(XmlNode boxInfo)
        {
            bool flag = false;
            if (boxInfo != null)
            {
                StringTheory theory = new StringTheory("0" + this.GetConfigurationItem("max_connections", boxInfo));
                this.Config.setDatabaseType(this.GetConfigurationItem("db_type", boxInfo));
                this.Config.setDriver(this.GetConfigurationItem("driver", boxInfo));
                this.Config.setMaxConnections(theory.ToInt());
                this.Config.setServer(this.GetConfigurationItem("server", boxInfo));
                this.Config.setDatabase(this.GetConfigurationItem("database", boxInfo));
                this.Config.setUser(this.GetConfigurationItem("user", boxInfo));
                this.Config.setPassword(this.GetConfigurationItem("password", boxInfo));
                flag = this.open();
            }
            return flag;
        }

        protected bool ReloadNewBoxItem(Item item)
        {
            item.reset();
            this.SqlDatabase.giveBoxItemLatestID(item);
            return base.retrieve(item);
        }

        public override bool remove(Item item, int max)
        {
            SqlBoxDeleteQuery query;
            bool flag = false;
            string strA = this.Config.getDatabaseType();
            if (string.Compare(strA, "oracle", true) == 0)
            {
                query = new OracleDeleteQuery(item, max);
            }
            else if (string.Compare(strA, "mysql", true) == 0)
            {
                query = new SqlServerDeleteQuery(item, max);
            }
            else
            {
                query = new SqlServerDeleteQuery(item, max);
            }
            flag = this.SqlDatabase.process(query) >= 0;
            if ((max == -1) && !flag)
            {
                flag = !base.find(item);
            }
            return flag;
        }

        public override bool retrieve(Item item, ArrayList results, int max)
        {
            SqlBoxSelectQuery query = null;
            string strA = this.Config.getDatabaseType();
            if (string.Compare(strA, "oracle", true) == 0)
            {
                query = new OracleSelectQuery(item, max);
            }
            else if (string.Compare(strA, "mysql", true) == 0)
            {
                query = new MySqlSelectQuery(item, max);
            }
            else
            {
                query = new SqlServerSelectQuery(item, max);
            }
            ArrayList list = new ArrayList();
            if (this.SqlDatabase.process(query, list) > 0)
            {
                ArrayList c = this.TransformHashesToBoxItems(list, item);
                results.AddRange(c);
                return (c.Count > 0);
            }
            base.error.copy(this.SqlDatabase.error);
            return false;
        }

        public override bool store(Item item)
        {
            if (item.Touched)
            {
                if (this.FindById(item))
                {
                    return this.UpdateExisting(item);
                }
                return this.CreateNew(item);
            }
            return true;
        }

        public override bool store(ICollection items)
        {
            bool flag = true;
            foreach (Item item in items)
            {
                flag = this.store(item);
            }
            return flag;
        }

        public bool streamExists(Item item)
        {
            return this.streamExists(item.getStreamName());
        }

        public bool streamExists(string streamName)
        {
            ArrayList results = new ArrayList();
            SqlBoxQuery query = new SqlBoxQuery();
            query.Append("SELECT * FROM information_schema.tables WHERE TABLE_NAME = '%StreamName%'");
            query.Replace("%StreamName%", streamName);
            this.SqlDatabase.process(query, results);
            return (results.Count > 0);
        }

        protected ArrayList TransformHashesToBoxItems(ArrayList list, Item likeThis)
        {
            for (int i = 0; i < list.Count; i++)
            {
                list[i] = this.TransformHashToBoxItem(likeThis, (Hashtable)list[i]);
            }
            return list;
        }

        protected Item TransformHashToBoxItem(Item prototype, Hashtable data)
        {
            Item item = null;
            try
            {
                item = (Item)Activator.CreateInstance(prototype.GetType());
                if (item != null)
                {
                    item.appropriate(data);
                }
            }
            catch (Exception exception)
            {
                base.error.message = "Unable to create an Item from a hash.";
                base.error.info = exception.ToString();
            }
            return item;
        }

        protected bool UpdateExisting(Item item)
        {
            SqlBoxUpdateQuery query = new SqlBoxUpdateQuery(item);
            return (this.SqlDatabase.process(query) >= 0);
        }

        // Properties
        public string Database
        {
            get
            {
                return this.Config.getDatabase();
            }
            set
            {
                this.Config.setDatabase(value);
            }
        }

        public string DbType
        {
            get
            {
                return this.Config.getDatabaseType();
            }
            set
            {
                this.Config.setDatabaseType(value);
            }
        }

        public string LastQuery
        {
            get
            {
                return this.SqlDatabase.LastQuery;
            }
        }

        public int MaxConnections
        {
            get
            {
                return this.Config.getMaxConnections();
            }
            set
            {
                this.Config.setMaxConnections(value);
            }
        }

        public string Password
        {
            get
            {
                return this.Config.getPassword();
            }
            set
            {
                this.Config.setPassword(value);
            }
        }

        public string Server
        {
            get
            {
                return this.Config.getServer();
            }
            set
            {
                this.Config.setServer(value);
            }
        }

        public string User
        {
            get
            {
                return this.Config.getUser();
            }
            set
            {
                this.Config.setUser(value);
            }
        }
    }


}
