using System;
using System.Collections;
using System.Text;
using Built.Text;

namespace Built.Data.Box.Queries
{
    public class OracleSelectQuery : SqlBoxSelectQuery
    {
        // Methods
        public OracleSelectQuery(Item entity)
            : base(entity)
        {
        }

        public OracleSelectQuery(Item entity, int max)
            : base(entity, max)
        {
        }

        protected override void LimitRecordsReturned(StringTheory query, int limit)
        {
            if (limit > -1)
            {
                query.Prepend("SELECT * FROM (");
                query.Append(") WHERE rownum < ");
                query.Append((limit + 1).ToString());
                query.Append(" ");
            }
        }
    }


}
