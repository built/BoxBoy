using System;
using System.Collections;
using System.Text;
using Built.Text;

namespace Built.Data.Box.Queries
{
    public abstract class SqlBoxSelectQuery : SqlBoxQuery
    {
        // Methods
        public SqlBoxSelectQuery(Item entity)
        {
            this.build(entity, -1);
        }

        public SqlBoxSelectQuery(Item entity, int max)
        {
            this.build(entity, max);
        }

        protected void AppendSelectQuery(string tableName, StringTheory conditions, StringTheory specifiers, int limit)
        {
            StringTheory val = new StringTheory("SELECT * FROM %tableName% WHERE %conditions% %specifiers%");
            if (conditions.IsEmpty())
            {
                val.Replace("WHERE ", "");
            }
            val.Replace("%tableName%", tableName);
            val.Replace("%conditions%", conditions);
            val.Replace("%specifiers%", specifiers);
            base.Append(val);
            this.LimitRecordsReturned(this, limit);
        }

        protected void AppendSortSpecification(StringTheory spec, Item entity)
        {
            ArrayList coll = entity.getSortList();
            if (coll.Count > 0)
            {
                spec.Append(" ORDER BY ");
                spec.Join(coll, ", ");
            }
        }

        public bool build(Item entity, int max)
        {
            base.Erase();
            if (entity != null)
            {
                StringTheory condition = new StringTheory();
                StringTheory spec = new StringTheory();
                base.BuildCondition(condition, entity);
                if (entity.getSortList().Count > 0)
                {
                    this.AppendSortSpecification(spec, entity);
                }
                this.AppendSelectQuery(entity.getStreamName(), condition, spec, max);
            }
            return (base.Length > 0);
        }

        protected abstract void LimitRecordsReturned(StringTheory query, int limit);
    }




}
