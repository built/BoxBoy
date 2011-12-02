using System;
using System.Collections;
using System.Text;
using Built.Text;

namespace Built.Data.Box.Queries
{
    public class MySqlSelectQuery : SqlBoxSelectQuery
    {
        // Methods
        public MySqlSelectQuery(Item entity)
            : base(entity)
        {
        }

        public MySqlSelectQuery(Item entity, int max)
            : base(entity, max)
        {
        }

        protected override void LimitRecordsReturned(StringTheory query, int limit)
        {
            if (limit > -1)
            {
                query.Append(" LIMIT ");
                query.Append(limit.ToString());
                query.Append(" ");
            }
        }
    }


}
