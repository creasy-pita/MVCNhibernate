using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCNhibernate.Web.Tests
{
    public class CommonDataParameter : IDataParameter
    {

        public ParameterDirection Direction { get; set; }

        public DbType DbType { get; set; }

        public string ParameterName { get; set; }

        public int Value { get; set; }


        public bool IsNullable
        {
            get { throw new NotImplementedException(); }
        }

        public string SourceColumn
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public DataRowVersion SourceVersion
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        object IDataParameter.Value
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
