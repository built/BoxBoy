using System;
using System.Collections;
using System.Text;
using Built.Text;

namespace Built.Data.Box.Queries
{
    public class OracleDeleteQuery : SqlBoxDeleteQuery
    {
        // Methods
        public OracleDeleteQuery(Item entity)
            : base(entity)
        {
        }

        public OracleDeleteQuery(Item entity, int max)
            : base(entity, max)
        {
        }

        protected override void build(Item entity, int limit)
        {
            string str = entity.getStreamName();
            string str2 = entity.getIdField();
            StringTheory condition = new StringTheory();
            base.BuildCondition(condition, entity);
            OracleSelectQuery query = new OracleSelectQuery(entity, -1);
            base.Append("DELETE FROM " + str);
            if (condition.Length > 0)
            {
                base.Append(" WHERE " + condition);
                if (limit > -1)
                {
                    base.Append(" AND rownum < ");
                    base.Append(limit + 1);
                }
            }
            else if (limit > -1)
            {
                base.Append(" WHERE rownum < ");
                base.Append(limit + 1);
            }
        }
    }


}
