using System;
using System.Collections;
using System.Text;
using Built.Text;

namespace Built.Data.Box.Queries
{
    public class SqlServerSelectQuery : SqlBoxSelectQuery
    {
        // Methods
        public SqlServerSelectQuery(Item entity)
            : base(entity)
        {
        }

        public SqlServerSelectQuery(Item entity, int max)
            : base(entity, max)
        {
        }

        protected override void LimitRecordsReturned(StringTheory query, int limit)
        {
            if (limit > -1)
            {
                query.Prepend(query.CutThru("SELECT") + " TOP " + limit);
            }
        }
    }


}
