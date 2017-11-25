using NHibernate;
using NHibernate.Cfg;
using NHibernate.Criterion;
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
    public class NHibernateHelper
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

        public T Get<T>(string keyId)
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

        #region 增加
        public bool Add<T>(T item)
        {
            try
            {
                OpenSession();

                session.Save(item);
                session.Flush();
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
            finally
            {
                if (!IsTran || !IsKeepConnect)
                {
                    CloseSession();
                }
            }
        }
        /// <summary>
        /// ？？？事物上的处理 ,默认开启，完成后关闭事务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="itemList"></param>
        /// <returns></returns>
        public bool AddList<T>(List<T> itemList)
        {
            try
            {
                OpenSession();
                if (!IsTran) BeginTransaction();
                
                foreach (T item in itemList)
                {
                    session.Save(item);
                }
                Commit();
                return true;
            }
            catch (Exception e)
            {
                RollBack();
                return false;
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
        #region
        public IList<T> GetPageList<T>(int pageSize,int pageIndex, out int totalCount)
        {
            totalCount = 0;
            try
            {
                OpenSession();
                IList<T> list = session.CreateCriteria(typeof(T))
                    .SetFirstResult(pageSize * (pageIndex))
                    .SetMaxResults(pageSize).List<T>();
                totalCount = int.Parse( session.CreateCriteria(typeof(T)).SetProjection(Projections.RowCount()).UniqueResult().ToString());
                session.Flush();
                return list;
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

        /// <summary>
        /// 参数 需要 对Restrictions 进行改写CommonRestrictions，改写后在外层调用 CommonRestrictions
        /// 或 ICriteria[] ICriteriaArr 见 CreateCriteria().Add(ICriteria Expression)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="paramArr"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public IList<T> GetPageList<T>(int pageSize, int pageIndex, CommonDataParameter[] paramArr, out int totalCount)
        {
            totalCount = 0;
            try
            {
                OpenSession();
                

                IList<T> list = session.CreateCriteria(typeof(T))
                    .SetFirstResult(pageSize * (pageIndex))
                    .SetMaxResults(pageSize).List<T>();
                totalCount = int.Parse(session.CreateCriteria(typeof(T)).SetProjection(Projections.RowCount()).UniqueResult().ToString());
                session.Flush();
                return list;
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


        #region 原生sql方式
        public IList<T> ExecuteSql<T>(string sqlCommand) 
        {
            try
            {
                OpenSession();
                ISQLQuery query = session.CreateSQLQuery(sqlCommand);
                return query.AddEntity(typeof(T)).List<T>();//需要先AddEntity 注册对象T
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

        public int ExecuteStoreProcedure(string procedureName, CommonDataParameter[] paramArr)
        {
            try
            {
                OpenSession();
                IDbCommand command = session.Connection.CreateCommand();
                if (IsTran) transaction.Enlist(command);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = procedureName;

                foreach (IDataParameter dataParameter in paramArr)
                {
                    IDataParameter dp = command.CreateParameter();
                    dp.DbType = dataParameter.DbType;
                    dp.ParameterName = dataParameter.ParameterName;
                    dp.Direction = dataParameter.Direction;
                    dp.Value = dataParameter.Value;
                    command.Parameters.Add(dp);
                }
                int re = command.ExecuteNonQuery();
                for (int i = 0; i < command.Parameters.Count; i++)
                {
                    IDataParameter dataParameter = (IDataParameter)command.Parameters[i];
                    IDataParameter dp = paramArr[i];
                    dp.Value = dataParameter.Value;
                }

                return re;
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

        public int ExecuteStoreProcedureForIList(string procedureName, CommonDataParameter[] paramArr)
        {
            try
            {
                OpenSession();

                IDbCommand command = session.Connection.CreateCommand();
                if (IsTran) transaction.Enlist(command);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = procedureName;

                foreach (IDataParameter dataParameter in paramArr)
                {
                    IDataParameter dp = command.CreateParameter();
                    dp.DbType = dataParameter.DbType;
                    dp.ParameterName = dataParameter.ParameterName;
                    dp.Direction = dataParameter.Direction;
                    dp.Value = dataParameter.Value;
                    command.Parameters.Add(dp);
                }
                int re = command.ExecuteNonQuery();
                for (int i = 0; i < command.Parameters.Count; i++)
                {
                    IDataParameter dataParameter = (IDataParameter)command.Parameters[i];
                    IDataParameter dp = paramArr[i];
                    dp.Value = dataParameter.Value;
                }

                return re;
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
