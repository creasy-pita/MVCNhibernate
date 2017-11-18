using NHibernate;
using NHibernate.Cfg;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCNhibernate.Data
{
    public class NHibernateHelper<T>
    {
        private ISessionFactory sessionFactory;

        private ISession session;

        private bool IsOpen;

        private bool IsKeepConnect;

        private ITransaction transaction;

        private bool IsTran;

        public NHibernateHelper()
        {
            this.sessionFactory = new Configuration().Configure().BuildSessionFactory();
            session = sessionFactory.OpenSession();
        }

        public void OpenSession()
        {
            if(!IsOpen)
            {
                session = sessionFactory.OpenSession();
            }
        }

        public void CloseSession()
        {
            IsOpen = false;
            session.Close();
        }

        public void BeginTransaction()
        {
            if(transaction==null)
            {
                IsTran = true;
                transaction = session.BeginTransaction();
            }
        }

        public void RollBack()
        {
            if (transaction != null)
            {
                IsTran = false;
                transaction.Rollback();
            }
        }

        public void Commit()
        {
            if (transaction != null)
            {
                IsTran = false;
                transaction.Commit();
            }
        }

        public T Get(string keyId)
        {
            try
            {
                OpenSession();
                return session.Get<T>(keyId);

            }
            catch (Exception e)
            {
                return default(T);
            }
            finally
            {
                if (!IsTran || !IsKeepConnect)
                {
                    CloseSession();
                }

            }
        }


        #region 原生sql方式
        public IList<T> ExecuteSql(string sqlCommand) 
        {
            try
            {
                OpenSession();
                ISQLQuery query = session.CreateSQLQuery(sqlCommand);
                return query.List<T>();
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                if(!IsTran || !IsKeepConnect)
                {
                    CloseSession();
                }
            }

        }

        public int ExecuteStoreProcedure(string procedureName, IDataParameter[] paramArr)
        {
            try
            {
                OpenSession();


                DataSet ds = new DataSet();
                IDbCommand command = session.Connection.CreateCommand();
                if (IsTran) transaction.Enlist(command);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = procedureName;
                foreach (IDataParameter dataParameter in paramArr)
                {
                    command.Parameters.Add(dataParameter);
                }
                return command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                return 0;
            }
            finally
            {
                if (!IsTran || !IsKeepConnect)
                {
                    CloseSession();
                }
            }   

        }

        public DataSet ExecuteForDataSet(string sqlCommand)
        {
            try
            {
                OpenSession();
                DataSet ds = new DataSet();
                IDbCommand command = session.Connection.CreateCommand();
                command.CommandText = sqlCommand;
                
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter((SqlCommand) command);
                sqlDataAdapter.Fill(ds);
                return ds;
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                if (!IsTran || !IsKeepConnect)
                {
                    CloseSession();
                }
            }            
        }

        #endregion

    }
}
