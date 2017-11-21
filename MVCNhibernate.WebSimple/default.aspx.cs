using MVCNhibernate.Domain.Entities;
using NHibernate;
using NHibernate.Cfg;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MVCNhibernate.WebSimple
{
    public partial class _default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //ISession session = NHibernateHelper.GetCurrentSession();
            //MVCNhibernate.Data.NHibernateHelper<Student> nhelper = new Data.NHibernateHelper<Student>();

            //Student s = nhelper.Get("793ea618-cc6d-4f5e-b6f2-9d8ed4877a1a");
            //Response.Write(s.StudentId);
            //Response.Write(s.Name);
            //Response.Write(s.Age);
            //IList list = nhelper.ExecuteSql("select * from student");
            //if(list.Count>0)
            //{
            //    Response.Write(((object[])list[0])[1].ToString());
            //}

            //Student s = new Student{StudentId=System.Guid.NewGuid().ToString(), Name="s1",Age=12};

            //session.Save(s);
            //session.Flush();
            //session.Close();
            //Response.Write("陈宫");


            ISessionFactory sessionFactory = new Configuration().Configure().BuildSessionFactory();
            ISession session = sessionFactory.OpenSession(new SQLWatcher());
            int value = 1;
            try
            {
                IDbCommand cmd = session.Connection.CreateCommand();
                cmd.CommandText = "InputPramWithNameAndReturnScalar";
                cmd.CommandType = CommandType.StoredProcedure;
                IDataParameter dp = cmd.CreateParameter();
                dp.DbType = DbType.Int32;
                dp.ParameterName = "number1";
                dp.Direction = ParameterDirection.Input;
                dp.Value = 10;
                cmd.Parameters.Add(dp);

                IDataParameter dp2 = cmd.CreateParameter();
                dp2.DbType = DbType.Int32;
                dp2.ParameterName = "square";
                dp2.Direction = ParameterDirection.Output;
                dp2.Value = null;
                cmd.Parameters.Add(dp2);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                session.Close();

            }
        }
    }
}