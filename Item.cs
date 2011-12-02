using System;
using System.Collections;
using System.Text;
using Built.Data.Box.Utilities;
using Built.Text;

namespace Built.Data.Box
{
    public class Item : Hashtable
    {
        // Fields
        protected HashAdapter Adapter = null;
        protected string IdField = "ID";
        protected ArrayList SortFields = null;
        protected string StreamName = "";
        protected bool useAutoId = true;

        // Methods
        protected Field AddField(string fieldName)
        {
            Field field = new Field(fieldName);
            this[fieldName] = field;
            return field;
        }

        protected Field AddField(string fieldName, string aka)
        {
            Field field = new Field(fieldName);
            this[fieldName] = field;
            if (this.Adapter != null)
            {
                this.Adapter.mapKey(aka, fieldName);
            }
            return field;
        }

        protected Field AddIdField(string fieldName)
        {
            this.IdField = fieldName;
            this[this.IdField] = new Field(fieldName);
            return (Field)this[this.IdField];
        }

        protected Field AddIdField(string fieldName, string aka)
        {
            this.IdField = fieldName;
            Field field = new Field(fieldName);
            this[this.IdField] = field;
            if (this.IsUsingAdapter)
            {
                this.Adapter.mapKey(aka, fieldName);
                this.IdField = aka;
            }
            return field;
        }

        protected void AddSortField(string fieldName)
        {
            this.getSortList().Add(fieldName);
        }

        public int appropriate(Hashtable dataset)
        {
            int num = 0;
            if ((dataset != null) && (dataset.Count > 0))
            {
                if (this.Adapter != null)
                {
                    foreach (string str in this.Adapter.Keys)
                    {
                        if (dataset.ContainsKey(str))
                        {
                            ((Field)this.Adapter[str]).Renew(dataset[str].ToString());
                            num++;
                        }
                    }
                    return num;
                }
                this.assimilate(dataset);
            }
            return num;
        }

        public int assimilate(Hashtable dataset)
        {
            int num = 0;
            if ((dataset != null) && (dataset.Count > 0))
            {
                foreach (string str in this.Keys)
                {
                    if (dataset.ContainsKey(str))
                    {
                        ((Field)this[str]).Renew(dataset[str].ToString());
                        num++;
                    }
                }
            }
            return num;
        }

        public Hashtable export()
        {
            return new Hashtable(this);
        }

        public static string[] FieldList(string field1)
        {
            return new string[] { field1 };
        }

        public static string[] FieldList(string field1, string field2)
        {
            return new string[] { field1, field2 };
        }

        public static string[] FieldList(string field1, string field2, string field3)
        {
            return new string[] { field1, field2, field3 };
        }

        public static string[] FieldList(string field1, string field2, string field3, string field4)
        {
            return new string[] { field1, field2, field3, field4 };
        }

        public static string[] FieldList(string field1, string field2, string field3, string field4, string field5)
        {
            return new string[] { field1, field2, field3, field4, field5 };
        }

        public HashAdapter getAdapter()
        {
            return this.Adapter;
        }

        public Hashtable getBoxView()
        {
            Hashtable adapter = this;
            if (this.Adapter != null)
            {
                adapter = this.Adapter;
            }
            return adapter;
        }

        public ArrayList getFieldNames()
        {
            ArrayList list = new ArrayList();
            foreach (string str in this.getBoxView().Keys)
            {
                list.Add(str);
            }
            return list;
        }

        protected string GetFieldValue(string fieldName)
        {
            Field field = (Field)this[fieldName];
            return field.ToString();
        }

        public string getIdField()
        {
            return this.IdField;
        }

        public ArrayList getSortList()
        {
            if (this.SortFields == null)
            {
                this.SortFields = new ArrayList();
            }
            return this.SortFields;
        }

        public string getStreamName()
        {
            return this.StreamName;
        }

        public string getXml()
        {
            StringTheory theory = new StringTheory("<%islandName%>%body%</%islandName%>\n");
            StringTheory val = new StringTheory();
            StringTheory theory3 = new StringTheory();
            foreach (string str in this.getFieldNames())
            {
                val.Renew("    <%fieldName%>%fieldValue%</%fieldName%>\n");
                val.Replace("%fieldName%", str.ToLower());
                val.Replace("%fieldValue%", ((Field)this[str]).getValue());
                theory3.Append(val);
            }
            theory.Replace("%islandName%", this.StreamName.ToLower());
            theory.Replace("%body%", theory3);
            return theory.ToString();
        }

        public bool isNew()
        {
            Field field;
            if (this.IsUsingAdapter)
            {
                HashAdapter adapter = (HashAdapter)this.getBoxView();
                field = (Field)adapter[this.IdField];
            }
            else
            {
                field = (Field)this[this.IdField];
            }
            return field.IsEmpty();
        }

        private void ListAppend(ArrayList list, object[] array)
        {
            if (array != null)
            {
                for (int i = 0; i < array.Length; i++)
                {
                    list.Add(array[i]);
                }
            }
        }

        public void reset()
        {
            foreach (string str in this.Keys)
            {
                ((Field)this[str]).Reset();
            }
        }

        protected void SetFieldValue(string fieldName, string fieldValue)
        {
            ((Field)this[fieldName]).setValue(fieldValue);
        }

        protected void SetStreamName(string name)
        {
            this.StreamName = name;
        }

        public void showContents()
        {
            foreach (string str in this.getFieldNames())
            {
                Console.WriteLine(str + " -> " + this[str]);
            }
        }

        public void sortBy(string[] fields)
        {
            this.getSortList().Clear();
            this.ListAppend(this.getSortList(), fields);
        }

        protected void UseAdapter()
        {
            this.Adapter = new HashAdapter();
            this.Adapter.setTarget(this);
        }

        // Properties
        public string Id
        {
            get
            {
                string str = "";
                Hashtable hashtable = this.getBoxView();
                if (hashtable.ContainsKey(this.IdField))
                {
                    str = hashtable[this.IdField].ToString();
                }
                return str;
            }
            set
            {
                ((Field)this.getBoxView()[this.IdField]).setValue(value);
            }
        }

        public bool IsUsingAdapter
        {
            get
            {
                return (this.Adapter != null);
            }
        }

        public bool Touched
        {
            get
            {
                foreach (string str in this.getBoxView().Keys)
                {
                    Field field = (Field)this.getBoxView()[str];
                    if (field.Touched())
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public bool UsesAutoId
        {
            get
            {
                return this.useAutoId;
            }
            set
            {
                this.useAutoId = value;
            }
        }
    }

}
