using System;
using System.Collections;
using System.Text;
using System.Xml;

namespace Built.Data.Box
{
    public class MemoryBox : Built.Data.Box.Box
    {
        // Fields
        protected Hashtable DataStore = new Hashtable();
        protected Hashtable DataStoreIDs = new Hashtable();

        // Methods
        public bool close()
        {
            this.DataStore = null;
            return (this.DataStore == null);
        }

        public bool createStream(Item streamPrototype)
        {
            return false;
        }

        public bool deleteStream(Item streamPrototype)
        {
            return false;
        }

        public XmlDocument Describe()
        {
            return new XmlDocument();
        }

        public void Finalize()
        {
            this.close();
        }

        protected Hashtable GetDataStore(string name)
        {
            if (!this.DataStore.ContainsKey(name))
            {
                this.DataStore[name] = new Hashtable();
            }
            return this.DataStore;
        }

        protected int GrantNewID(string name)
        {
            int num = 0;
            if (this.DataStoreIDs.ContainsKey(name))
            {
                num = (int)this.DataStoreIDs[name];
                num++;
            }
            this.DataStoreIDs[name] = num;
            return num;
        }

        public bool open()
        {
            if (this.DataStore == null)
            {
                this.DataStore = new Hashtable();
            }
            return (this.DataStore != null);
        }

        public bool open(string configFile)
        {
            return this.open();
        }

        public bool open(XmlDocument configFile)
        {
            return this.open();
        }

        public bool remove(Item item, int max)
        {
            return false;
        }

        public bool retrieve(Item item, ArrayList results, int max)
        {
            return false;
        }

        public bool store(Item item)
        {
            bool flag = false;
            Hashtable dataStore = this.GetDataStore(item.getStreamName());
            item.Id = this.GrantNewID(item.getStreamName()).ToString();
            try
            {
                dataStore[item.Id] = item.export();
                flag = true;
            }
            catch (Exception)
            {
            }
            return flag;
        }

        public bool streamExists(string name)
        {
            return false;
        }
    }

}
