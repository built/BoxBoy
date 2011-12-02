using System;
using System.Text;
using System.Xml;
using System.Collections;
using Built.Text;

namespace Built.Data.Box
{
    public class ConfigBox : Built.Data.Box.Box
    {
        // Fields
        protected DatabaseConfiguration Config = new DatabaseConfiguration();
        protected string Filename_ = "";
        protected bool ReadOnly_ = false;

        // Methods
        public override bool close()
        {
            return false;
        }

        protected bool CreateNew(Item item)
        {
            return false;
        }

        public bool createStream(Item item)
        {
            return this.streamExists(item);
        }

        public bool deleteStream(Item item)
        {
            return !this.streamExists(item);
        }

        public XmlDocument Describe()
        {
            return new XmlDocument();
        }

        public XmlDocument Describe(string streamName)
        {
            XmlDocument document = new XmlDocument();
            StringTheory theory = new StringTheory("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            return document;
        }

        public void Finalize()
        {
            this.close();
        }

        protected bool FindById(Item item)
        {
            if ((item != null) && !item.isNew())
            {
            }
            return false;
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
            return false;
        }

        public override bool open(string configFile)
        {
            return false;
        }

        public bool open(XmlNode boxInfo)
        {
            return false;
        }

        public override bool remove(Item item, int max)
        {
            return false;
        }

        public override bool retrieve(Item item, ArrayList results, int max)
        {
            return false;
        }

        public override bool store(Item item)
        {
            return false;
        }

        public override bool store(ICollection items)
        {
            return true;
        }

        public bool streamExists(Item item)
        {
            return this.streamExists(item.getStreamName());
        }

        public bool streamExists(string streamName)
        {
            return false;
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
            return false;
        }

        // Properties
        public string Filename
        {
            get
            {
                return this.Filename_;
            }
            set
            {
                this.Filename_ = value;
            }
        }

        public bool ReadOnly
        {
            get
            {
                return this.ReadOnly_;
            }
            set
            {
                this.ReadOnly_ = value;
            }
        }
    }

}
