using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Built.Utilities
{
    public class Error
    {
        // Fields
        protected StringBuilder Info_ = new StringBuilder();
        protected StringBuilder Message_ = new StringBuilder();

        // Methods
        public void copy(Error error)
        {
            if (error != null)
            {
                this.message = error.message;
                this.info = error.info;
            }
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(this.Message_);
            this.Message_.Length = 0;
            return builder.ToString();
        }

        // Properties
        public string info
        {
            get
            {
                return this.Info_.ToString();
            }
            set
            {
                this.Info_.Length = 0;
                this.Info_.Append(value);
            }
        }

        public string message
        {
            get
            {
                return this.ToString();
            }
            set
            {
                this.Message_.Length = 0;
                this.Message_.Append(value);
            }
        }

        public bool occurred
        {
            get
            {
                return (this.Message_.Length > 0);
            }
        }
    }

}
