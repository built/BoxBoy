using System;
using System.Collections;
using System.Text;
using Built.Utilities;
using Built.Data.Box;

namespace Built.Data.Box
{
    public abstract class Box
    {
        // Fields
        protected const int ALL = -1;
        public Error error = new Error();

        // Methods
        public virtual bool close()
        {
            return false;
        }

        public bool find(Item item)
        {
            ArrayList entitiesFound = new ArrayList();
            this.retrieve(item, entitiesFound, 1);
            return (entitiesFound.Count > 0);
        }

        public virtual bool open(string configFile)
        {
            return false;
        }

        public bool remove(Item item)
        {
            return this.remove(item, 1);
        }

        public virtual bool remove(Item item, int maximum)
        {
            return false;
        }

        public bool removeAll(Item item)
        {
            return this.remove(item, -1);
        }

        public bool retrieve(Item item)
        {
            bool flag = false;
            ArrayList entitiesFound = new ArrayList();
            if (this.retrieve(item, entitiesFound, 1) && (entitiesFound.Count > 0))
            {
                item.assimilate((Item)entitiesFound[0]);
                flag = true;
            }
            return flag;
        }

        public virtual bool retrieve(Item item, ArrayList entitiesFound, int max)
        {
            return false;
        }

        public bool retrieveAll(Item item, ArrayList entitiesFound)
        {
            return this.retrieve(item, entitiesFound, -1);
        }

        public virtual bool store(Item item)
        {
            return false;
        }

        public virtual bool store(ICollection items)
        {
            return false;
        }
    }


}
