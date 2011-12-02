using System;
using System.Collections;
using System.Text;
using System.Data.OleDb;

namespace Built.Data.Box
{
    public class ConnectionManager
    {
        // Fields
        protected static ArrayList Connections = new ArrayList();

        // Methods
        public static void checkInConnection(OleDbConnection conn)
        {
            for (int i = 0; i < Connections.Count; i++)
            {
                if (Connections[i] == conn)
                {
                    ((OleDbConnection)Connections[i]).Close();
                    Connections.Remove(i);
                }
            }
        }

        public static OleDbConnection checkOutConnection(string connUrl, int maxConns)
        {
            OleDbConnection connection = null;
            if ((Connections.Count < maxConns) || (maxConns < 1))
            {
                try
                {
                    connection = new OleDbConnection(connUrl);
                    Connections.Add(connection);
                }
                catch (Exception)
                {
                }
            }
            if ((connection != null) && (connection.State != System.Data.ConnectionState.Open))
            {
                connection.Open();
            }
            return connection;
        }
    }

}
