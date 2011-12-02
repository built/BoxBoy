using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Built.Text;

namespace Built.Data.Box
{
    public class Field : StringTheory, IDisposable
    {
        // Fields
        protected string[] Attributes;
        protected StringTheory Name;

        // Methods
        public Field()
        {
            this.Attributes = null;
            this.Name = new StringTheory();
        }

        public Field(string name)
        {
            this.Attributes = null;
            this.Name = new StringTheory();
            this.Name.Renew(name);
        }

        public Field(string name, string value)
        {
            this.Attributes = null;
            this.Name = new StringTheory();
            this.Name.Renew(name);
            base.Renew(value);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
            base.Erase();
        }

        ~Field()
        {
            this.Dispose(false);
        }

        public string[] getAttributes()
        {
            if (this.Attributes == null)
            {
                this.Attributes = new string[0];
            }
            return this.Attributes;
        }

        public string getName()
        {
            return this.ToString();
        }

        public string getValue()
        {
            return this.ToString();
        }

        public bool hasAttributes()
        {
            return (this.getAttributes().Length > 0);
        }

        public void Reset()
        {
            base.Renew();
        }

        public void setAttributes(string[] values)
        {
            this.Attributes = values;
        }

        public void setName(string text)
        {
            base.Erase();
            base.Append(text);
        }

        public void setValue(string text)
        {
            base.Erase();
            base.Append(text);
        }

        public string toSql()
        {
            StringTheory theory = new StringTheory(this);
            string phrase = "'";
            theory.Replace(phrase, phrase + phrase);
            theory.SingleQuote();
            return theory.ToString();
        }
    }

}
