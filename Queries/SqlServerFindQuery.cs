using System;
using System.Collections;
using System.Text;
using Built.Text;

namespace Built.Data.Box.Queries
{
    public class SqlServerFindQuery : SqlBoxQuery
    {
        // Methods
        public SqlServerFindQuery(Item item)
        {
            this.build(item);
        }

        public void build(Item item)
        {
            StringTheory val = new StringTheory("SELECT TOP 1 %idField% FROM %tableName% WHERE %idField% = '%id%'");
            val.Replace("%idField%", item.getIdField());
            val.Replace("%tableName%", item.getStreamName());
            val.Replace("%id%", item.Id);
            base.Append(val);
        }
    }


}
