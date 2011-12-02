using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Built.Data.Box.Queries
{
    public abstract class SqlBoxDeleteQuery : SqlBoxQuery
    {
        // Methods
        public SqlBoxDeleteQuery(Item entity)
        {
            this.build(entity, 1);
        }

        public SqlBoxDeleteQuery(Item entity, int max)
        {
            this.build(entity, max);
        }

        protected abstract void build(Item entity, int limit);
    }

 

}
