using System;
using System.Collections;
using System.Text;
using Built.Text;

namespace Built.Data.Box.Queries
{
    public class SqlBoxFindQuery : SqlBoxQuery
    {
        // Methods
        public SqlBoxFindQuery(Item item)
        {
            this.build(item);
        }

        public void build(Item item)
        {
            StringTheory val = new StringTheory("SELECT %idField% FROM %tableName% WHERE %idField% = '%id%' LIMIT 1");
            val.Replace("%idField%", item.getIdField());
            val.Replace("%tableName%", item.getStreamName());
            val.Replace("%id%", item.Id);
            base.Append(val);
        }
    }

 

}
