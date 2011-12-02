using System;
using System.Collections;
using System.Data.OleDb;
using System.Text;
using System.Xml;
using System.Data;
using Built.Utilities;
using Built.Data.Box.Queries;
using Built.Text;

namespace Built.Data.Box
{
    public class SqlBoxDatabase
    {
        // Fields
        protected DatabaseConfiguration Config = new DatabaseConfiguration();
        protected DatabaseUrl ConnectionUrl = null;
        protected OleDbConnection DbConnection = null;
        public Error error = new Error();
        protected string m_lastQuery = "";

        // Methods
        protected bool CollectResultsInList(ArrayList results, OleDbDataReader resultSet)
        {
            int count = results.Count;
            try
            {
                Hashtable hashtable = new Hashtable();
                foreach (DataRow row in resultSet.GetSchemaTable().Rows)
                {
                    hashtable.Add(row["ColumnName"].ToString(), null);
                }
                Hashtable hashtable2 = null;
                while (resultSet.Read())
                {
                    hashtable2 = (Hashtable)hashtable.Clone();
                    foreach (string str in hashtable.Keys)
                    {
                        hashtable2[str] = resultSet[str];
                        if (hashtable2[str] == null)
                        {
                            hashtable2[str] = "";
                        }
                    }
                    results.Add(hashtable2);
                }
            }
            catch (Exception exception)
            {
                this.error.message = "An error occurred while collecting the results of your query.";
                this.error.info = exception.ToString();
            }
            return (results.Count > count);
        }

        public bool connect(DatabaseConfiguration config)
        {
            bool flag = false;
            if (config != null)
            {
                this.Config = config;
            }
            if (this.CreateConnectionUrl())
            {
                try
                {
                    this.DbConnection = ConnectionManager.checkOutConnection(this.ConnectionUrl.ToString(), this.Config.getMaxConnections());
                    flag = (this.DbConnection != null) && (this.DbConnection.State == System.Data.ConnectionState.Open);
                }
                catch (Exception exception)
                {
                    this.error.message = "Couldn't connect to the database.";
                    this.error.info = exception.ToString();
                }
            }
            return flag;
        }

        protected bool CreateConnectionUrl()
        {
            bool flag = false;
            string strA = this.Config.getDatabaseType();
            if (string.Compare(strA, "mysql", true) == 0)
            {
                this.ConnectionUrl = new MySqlUrl();
                flag = true;
            }
            else if (string.Compare(strA, "oracle", true) == 0)
            {
                this.ConnectionUrl = new OracleUrl();
                flag = true;
            }
            else if (string.Compare(strA, "sqlserver", true) == 0)
            {
                this.ConnectionUrl = new SqlServerUrl();
                flag = true;
            }
            if (this.ConnectionUrl != null)
            {
                this.ConnectionUrl.setServer(this.Config.getServer());
                this.ConnectionUrl.setDatabase(this.Config.getDatabase());
                this.ConnectionUrl.setUser(this.Config.getUser());
                this.ConnectionUrl.setPassword(this.Config.getPassword());
            }
            return flag;
        }

        public XmlDocument Describe()
        {
            XmlDocument document = new XmlDocument();
            StringTheory theory = new StringTheory("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            ArrayList list = new ArrayList();
            theory.Append("<streams>\n");
            this.ListStreams(list);
            foreach (Hashtable hashtable in list)
            {
                theory.Append(this.DescribeStream(hashtable["TABLE_NAME"].ToString()));
            }
            theory.Append("</streams>\n");
            document.LoadXml(theory.ToString());
            return document;
        }

        public string DescribeStream(string stream)
        {
            StringTheory theory = new StringTheory("<stream name=\"%stream%\">\n%fields%</stream>\n");
            StringTheory theory2 = new StringTheory();
            ArrayList list = new ArrayList();
            this.ListColumns(stream, list);
            theory.Replace("%stream%", stream);
            foreach (Hashtable hashtable in list)
            {
                theory2.Append("<field name=\"%COLUMN_NAME%\" type=\"%DATA_TYPE%\" identity=\"%IDENTITY%\"/>\n");
                theory2.Populate(hashtable, "%*%");
                theory2.Replace("%IDENTITY%", this.IsIdentity(hashtable));
            }
            theory.Replace("%fields%", theory2);
            return theory.ToString();
        }

        public bool disconnect()
        {
            try
            {
                ConnectionManager.checkInConnection(this.DbConnection);
                this.DbConnection = null;
            }
            catch (DataException exception)
            {
                this.error.message = "Couldn't disconnect from the database.";
                this.error.info = exception.ToString();
            }
            return (this.DbConnection == null);
        }

        public bool giveBoxItemLatestID(Item entity)
        {
            bool flag = false;
            if (this.DbConnection != null)
            {
                try
                {
                    string str = null;
                    string strA = this.Config.getDatabaseType().ToLower();
                    if (string.Compare(strA, "oracle", true) == 0)
                    {
                        str = "SELECT \"SYSTEM\".BOXDBAUTOINCREMENT.CURRVAL AS " + entity.getIdField() + " from DUAL";
                    }
                    else if (string.Compare(strA, "mysql", true) == 0)
                    {
                        str = "SELECT last_insert_id() AS " + entity.getIdField();
                    }
                    else
                    {
                        str = "SELECT @@IDENTITY AS " + entity.getIdField();
                    }
                    OleDbCommand command = this.DbConnection.CreateCommand();
                    command.CommandText = str.ToString();
                    OleDbDataReader resultSet = command.ExecuteReader();
                    ArrayList results = new ArrayList();
                    this.CollectResultsInList(results, resultSet);
                    if (results.Count > 0)
                    {
                        entity.Id = ((Hashtable)results[0])[entity.getIdField()].ToString();
                        flag = entity.Id.Length > 0;
                    }
                    resultSet.Close();
                }
                catch (DataException exception)
                {
                    this.error.message = "An error occurred while retrieving the ID of a new Item.";
                    this.error.info = exception.ToString();
                }
            }
            return flag;
        }

        protected bool IsIdentity(Hashtable fieldInfo)
        {
            bool flag = false;
            StringTheory theory = new StringTheory("SELECT COLUMNPROPERTY( OBJECT_ID('%TABLE_NAME%'),'%COLUMN_NAME%','IsIdentity') AS IS_IDENTITY");
            theory.Populate(fieldInfo, "%*%");
            ArrayList results = new ArrayList();
            this.process(theory.ToString(), results);
            if (results.Count > 0)
            {
                flag = ((Hashtable)results[0])["IS_IDENTITY"].ToString() == "1";
            }
            return flag;
        }

        protected int ListColumns(string stream, ArrayList list)
        {
            if (this.DbConnection != null)
            {
                try
                {
                    string str = "";
                    string strA = this.Config.getDatabaseType().ToLower();
                    if (string.Compare(strA, "oracle", true) == 0)
                    {
                        str = "";
                    }
                    else if (string.Compare(strA, "mysql", true) == 0)
                    {
                        str = "";
                    }
                    else
                    {
                        str = "SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='" + stream + "'";
                    }
                    OleDbCommand command = this.DbConnection.CreateCommand();
                    command.CommandText = str.ToString();
                    OleDbDataReader resultSet = command.ExecuteReader();
                    this.CollectResultsInList(list, resultSet);
                    resultSet.Close();
                }
                catch (DataException exception)
                {
                    this.error.message = "An error occurred while retrieving the ID of a new Item.";
                    this.error.info = exception.ToString();
                }
            }
            return list.Count;
        }

        protected int ListStreams(ArrayList list)
        {
            if (this.DbConnection != null)
            {
                try
                {
                    string str = "";
                    string strA = this.Config.getDatabaseType().ToLower();
                    if (string.Compare(strA, "oracle", true) == 0)
                    {
                        str = "";
                    }
                    else if (string.Compare(strA, "mysql", true) == 0)
                    {
                        str = "";
                    }
                    else
                    {
                        str = "SELECT * FROM INFORMATION_SCHEMA.TABLES";
                    }
                    OleDbCommand command = this.DbConnection.CreateCommand();
                    command.CommandText = str.ToString();
                    OleDbDataReader resultSet = command.ExecuteReader();
                    this.CollectResultsInList(list, resultSet);
                    resultSet.Close();
                }
                catch (DataException exception)
                {
                    this.error.message = "An error occurred while retrieving the ID of a new Item.";
                    this.error.info = exception.ToString();
                }
            }
            return list.Count;
        }

        public int process(SqlBoxQuery query)
        {
            int num = -1;
            if (!query.IsEmpty())
            {
                this.m_lastQuery = query.ToString();
                if ((this.DbConnection == null) || (this.DbConnection.State != System.Data.ConnectionState.Open))
                {
                    return num;
                }
                try
                {
                    OleDbCommand command = this.DbConnection.CreateCommand();
                    command.CommandText = query.ToString();
                    num = command.ExecuteNonQuery();
                }
                catch (Exception exception)
                {
                    this.error.message = "An error occurred while processing a query.";
                    this.error.info = exception.ToString();
                }
            }
            return num;
        }

        public int process(SqlBoxQuery query, ArrayList results)
        {
            int count = -1;
            if (!query.IsEmpty())
            {
                this.m_lastQuery = query.ToString();
                if (this.DbConnection == null)
                {
                    return count;
                }
                try
                {
                    OleDbCommand command = this.DbConnection.CreateCommand();
                    command.CommandText = query.ToString();
                    OleDbDataReader resultSet = command.ExecuteReader();
                    this.CollectResultsInList(results, resultSet);
                    count = results.Count;
                    resultSet.Close();
                }
                catch (Exception exception)
                {
                    this.error.message = "An error occurred while processing a query.";
                    this.error.info = exception.ToString();
                }
            }
            return count;
        }

        public int process(string query, ArrayList results)
        {
            int count = -1;
            if ((query != null) && (query.Length > 0))
            {
                this.m_lastQuery = query.ToString();
                if (this.DbConnection == null)
                {
                    return count;
                }
                try
                {
                    OleDbCommand command = this.DbConnection.CreateCommand();
                    command.CommandText = query;
                    OleDbDataReader resultSet = command.ExecuteReader();
                    this.CollectResultsInList(results, resultSet);
                    count = results.Count;
                    resultSet.Close();
                }
                catch (Exception exception)
                {
                    this.error.message = "An error occurred while processing a query.";
                    this.error.info = exception.ToString();
                }
            }
            return count;
        }

        // Properties
        public string LastQuery
        {
            get
            {
                return this.m_lastQuery;
            }
        }
    }

}
