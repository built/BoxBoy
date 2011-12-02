using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace Built.Data.Box.Utilities
{
    public class HashAdapter : Hashtable
    {
        // Fields
        protected Hashtable Target = null;

        // Methods
        protected object GetKey(object alias)
        {
            return base[alias];
        }

        public Hashtable getTarget()
        {
            return this.Target;
        }

        public void mapKey(string alias, string key)
        {
            base[alias] = key;
        }

        public void setTarget(Hashtable val)
        {
            this.Target = val;
        }

        // Properties
        public override object this[object alias]
        {
            get
            {
                object obj2 = null;
                if ((this.Target != null) && (alias != null))
                {
                    object key = this.GetKey(alias);
                    if (key != null)
                    {
                        obj2 = this.Target[key];
                    }
                }
                return obj2;
            }
            set
            {
                object obj2 = null;
                if ((this.Target != null) && (alias != null))
                {
                    obj2 = this.Target[this[alias]] = value;
                }
            }
        }
    }

}
