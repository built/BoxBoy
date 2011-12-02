using System;
using System.Collections;
using System.Text;
using Built.Text;

namespace Built.Data.Box.Queries
{
    public class MySqlDeleteQuery : SqlBoxDeleteQuery
    {
        // Methods
        public MySqlDeleteQuery(Item item)
            : base(item)
        {
        }

        public MySqlDeleteQuery(Item item, int max)
            : base(item, max)
        {
        }

        protected override void build(Item item, int limit)
        {
            string val = item.getStreamName();
            string str2 = item.getIdField();
            StringTheory condition = new StringTheory();
            base.BuildCondition(condition, item);
            base.Append("DELETE FROM ");
            base.Append(val);
            if ((condition != null) && (condition.Length > 0))
            {
                base.Append(" WHERE ");
                base.Append(condition);
            }
            if (limit > -1)
            {
                base.Append(" LIMIT " + limit);
            }
        }
    }


}
