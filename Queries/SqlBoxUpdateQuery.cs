using System;
using System.Collections;
using System.Text;
using Built.Text;

namespace Built.Data.Box.Queries
{

    public class SqlBoxUpdateQuery : SqlBoxQuery
    {
        // Methods
        public SqlBoxUpdateQuery(Item item)
        {
            this.build(item);
        }

        public bool build(Item item)
        {
            Hashtable hashtable = item.getBoxView();
            Field field = (Field)hashtable[item.getIdField()];
            if (!field.IsEmpty())
            {
                Field field2 = null;
                ArrayList coll = new ArrayList();
                foreach (string str in hashtable.Keys)
                {
                    field2 = (Field)hashtable[str];
                    if (!str.Equals(item.getIdField()) && field2.Touched())
                    {
                        coll.Add(str + "=" + field2.toSql());
                    }
                }
                if (coll.Count > 0)
                {
                    StringTheory theory = new StringTheory();
                    theory.Join(coll, ", ");
                    StringTheory val = new StringTheory("UPDATE %tableName% SET %pairListing% WHERE %idField% = %id%");
                    val.Replace("%tableName%", item.getStreamName());
                    val.Replace("%pairListing%", theory);
                    val.Replace("%idField%", item.getIdField());
                    val.Replace("%id%", field.toSql());
                    base.Append(val);
                }
            }
            return false;
        }
    }

 



}
