using System;
using System.Collections;
using System.Text;
using Built.Text;

namespace Built.Data.Box.Queries
{
    public class SqlBoxQuery : StringTheory
    {
        // Methods
        protected void BuildCondition(StringTheory condition, Item entity)
        {
            Hashtable hashtable = entity.getBoxView();
            Field field = null;
            foreach (string str in hashtable.Keys)
            {
                field = (Field)hashtable[str];
                if (field.Touched())
                {
                    if (condition.Length > 0)
                    {
                        condition.Append(" AND ");
                    }
                    if (field.Length < 1)
                    {
                        condition.Append(" ( len(" + str + ") < 1 or " + str + " IS NULL ) ");
                    }
                    else
                    {
                        condition.Append(str);
                        condition.Append("=");
                        condition.Append(field.toSql());
                    }
                }
                if (field.hasAttributes())
                {
                    string[] strArray = field.getAttributes();
                    for (int i = 0; i < strArray.Length; i++)
                    {
                        if (condition.Length > 0)
                        {
                            condition.Append(" AND ");
                        }
                        condition.Append(str);
                        condition.Append(strArray[i]);
                    }
                }
            }
        }
    }

}
