using System;
using System.Collections;
using System.Text;
using Built.Text;

namespace Built.Data.Box.Queries
{
    public class SqlServerDeleteQuery : SqlBoxDeleteQuery
    {
        // Methods
        public SqlServerDeleteQuery(Item item)
            : base(item)
        {
        }

        public SqlServerDeleteQuery(Item item, int max)
            : base(item, max)
        {
        }

        protected override void build(Item item, int limit)
        {
            string val = item.getStreamName();
            string str2 = item.getIdField();
            StringTheory condition = new StringTheory();
            base.BuildCondition(condition, item);
            if (limit > -1)
            {
                StringTheory theory2 = new StringTheory("SELECT TOP %limit% %id_field% FROM %table%");
                theory2.Replace("%limit%", limit);
                theory2.Replace("%id_field%", item.getIdField());
                theory2.Replace("%table%", val);
                if ((condition != null) && (condition.Length > 0))
                {
                    theory2.Append(" WHERE ");
                    theory2.Append(condition);
                }
                base.Append("DELETE FROM ");
                base.Append(val);
                base.Append(" WHERE ");
                base.Append(string.Concat(new object[] { item.getIdField(), " IN (", theory2, ")" }));
            }
            else
            {
                base.Append("DELETE FROM ");
                base.Append(val);
                if ((condition != null) && (condition.Length > 0))
                {
                    base.Append(" WHERE ");
                    base.Append(condition);
                }
            }
        }
    }


}
