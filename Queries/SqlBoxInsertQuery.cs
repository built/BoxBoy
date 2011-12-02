using System;
using System.Collections;
using System.Text;
using Built.Text;

namespace Built.Data.Box.Queries
{
    public class SqlBoxInsertQuery : SqlBoxQuery
    {
        // Methods
        public SqlBoxInsertQuery(Item item)
        {
            this.build(item);
        }

        public bool build(Item item)
        {
            StringTheory theory = new StringTheory();
            StringTheory theory2 = new StringTheory();
            Field field = null;
            Hashtable hashtable = item.getBoxView();
            ArrayList coll = new ArrayList();
            ArrayList list2 = new ArrayList();
            foreach (string str in hashtable.Keys)
            {
                field = (Field)hashtable[str];
                if ((field != null) && field.Touched())
                {
                    coll.Add(str);
                    list2.Add(field.toSql());
                }
            }
            theory.Join(coll, ", ");
            theory2.Join(list2, ", ");
            if (coll.Count > 0)
            {
                StringTheory val = new StringTheory("INSERT INTO %tableName% (%fields%) VALUES (%values%)");
                val.Replace("%tableName%", item.getStreamName());
                val.Replace("%fields%", theory);
                val.Replace("%values%", theory2);
                base.Append(val);
            }
            return false;
        }
    }


}
