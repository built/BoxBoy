using System;
using System.Collections;
using System.Text;
using System.Xml;

namespace Built.Data.Box
{
    public class XmlBox : Built.Data.Box.Box
    {
        // Fields
        protected DatabaseConfiguration Config = new DatabaseConfiguration();
        private XmlDocument doc = new XmlDocument();
        protected string m_Filename = "";
        protected bool m_ReadOnly = false;

        // Methods
        public override bool close()
        {
            return true;
        }

        protected bool CreateNew(Item item)
        {
            return false;
        }

        public bool createStream(Item item)
        {
            if (!this.streamExists(item))
            {
            }
            return this.streamExists(item);
        }

        public bool deleteStream(Item item)
        {
            if (this.streamExists(item))
            {
                XmlNode node = this.fetchNode(item.getStreamName());
            }
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

        protected XmlNode fetchNode(string nodePath)
        {
            StringTheory theory = new StringTheory(nodePath);
            if (theory.First() != '/')
            {
                theory.Prepend("/");
            }
            return this.doc.get_DocumentElement().SelectSingleNode(theory.ToString());
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
            if ((node != null) && (node.get_InnerText() != null))
            {
                str = node.get_InnerText();
            }
            return str;
        }

        protected bool LoadXmlDocument()
        {
            bool flag = false;
            if (File.Exists(this.Filename))
            {
                StringTheory theory = new StringTheory();
                if (theory.LoadFile(this.Filename))
                {
                    this.doc.LoadXml(theory.ToString());
                    flag = true;
                }
            }
            return flag;
        }

        public bool open()
        {
            return this.LoadXmlDocument();
        }

        public override bool open(string configFile)
        {
            if (File.Exists(configFile))
            {
                StringTheory theory = new StringTheory();
                if (theory.LoadFile(configFile))
                {
                    XmlDocument boxInfo = new XmlDocument();
                    boxInfo.LoadXml(theory.ToString());
                    this.setFilenameUsingConfigXml(boxInfo);
                }
            }
            return this.LoadXmlDocument();
        }

        public bool open(XmlDocument boxInfo)
        {
            this.setFilenameUsingConfigXml(boxInfo);
            return this.LoadXmlDocument();
        }

        public override bool remove(Item item, int max)
        {
            return false;
        }

        public override bool retrieve(Item item, ArrayList results, int max)
        {
            return false;
        }

        protected void setFilenameUsingConfigXml(XmlDocument boxInfo)
        {
            this.Filename = boxInfo.get_DocumentElement().SelectSingleNode("/xmlbox/filename").get_InnerText();
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
            return (this.fetchNode(streamName) != null);
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
                return this.m_Filename;
            }
            set
            {
                this.m_Filename = value;
            }
        }

        public bool ReadOnly
        {
            get
            {
                return this.m_ReadOnly;
            }
            set
            {
                this.m_ReadOnly = value;
            }
        }
    }


}
