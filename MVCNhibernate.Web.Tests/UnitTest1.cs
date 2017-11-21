using System;
using System.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MVCNhibernate.Domain.Entities;
using NHibernate;
using NHibernate.Cfg;
using System.Collections.Generic;
using System.Data;
using NHibernate.Criterion;

namespace MVCNhibernate.Web.Tests
{
    [TestClass]
    public class UnitTest1
    {


        [TestMethod]
        public void TestMethod1()
        {

            //IList<Student> list = CriteriaQueryWithRestrictions();
            //System.Console.Write("Count:"+ list.Count);

            //IList list =  IQueryGetNamedQuery();
            //System.Console.Write("Count:" + list.Count);

           // ExecuteStoreProcedureByIDbCommandIDataParameter();
            CommonDataParameter[] cp = new CommonDataParameter[2];
            int i = ExecuteStoreProcedureByIDbCommandIDataParameteCommonParameter(null, cp);
            System.Console.Write("square 10 =" + cp[1].Value);
            
        }

        public IList ScalarQueryList()
        {
            ISessionFactory sessionFactory = new Configuration().Configure().BuildSessionFactory();

            ISession session = sessionFactory.OpenSession();
            try
            {
                string sqlString = "select studentId, name from student";
                IQuery query = session.CreateQuery(sqlString);
                return query.List();
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                session.Close();
                sessionFactory.Close();
            }

            //query.List();
        }

        public IList<Student> QueryList()
        {
            ISessionFactory sessionFactory = new Configuration().Configure().BuildSessionFactory();

            ISession session = sessionFactory.OpenSession();
            try
            {
                string sqlString = "from student";
                IQuery query = session.CreateQuery(sqlString);
                return query.List<Student>();
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                session.Close();
                sessionFactory.Close();
            }
        }

        public IList<Student> QueryListWithFilter()
        {
            ISessionFactory sessionFactory = new Configuration().Configure().BuildSessionFactory();

            ISession session = sessionFactory.OpenSession();
            try
            {
                string sqlString = "from student where age = ?";
                IQuery query = session.CreateQuery(sqlString);
                query.SetInt32(0, 12);
                return query.List<Student>();
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                session.Close();
                sessionFactory.Close();
            }
        }

        public IList<Student> QueryPageList()
        {
            ISessionFactory sessionFactory = new Configuration().Configure().BuildSessionFactory();

            ISession session = sessionFactory.OpenSession();
            try
            {
                string sqlString = "from student";
                IQuery query = session.CreateQuery(sqlString);
               // query.SetInt32(0, 12);
                query.SetFetchSize(3).SetFirstResult(1).SetMaxResults(10);
                return query.List<Student>();
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                session.Close();
                sessionFactory.Close();
            }
        }

        public IList<Student> QueryPageListWithFilter()
        {
            ISessionFactory sessionFactory = new Configuration().Configure().BuildSessionFactory();

            ISession session = sessionFactory.OpenSession();
            try
            {
                string sqlString = "from student where age=? and nmae=?";
                IQuery query = session.CreateQuery(sqlString);
                query.SetInt32(0, 12);
                query.SetParameter(1, "");
                query.SetFetchSize(3).SetFirstResult(1).SetMaxResults(10);
                return query.List<Student>();
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                session.Close();
                sessionFactory.Close();
            }
        }

        public IList<Student> QueryPageListWithParameterList(IDataParameter[] paraArr)
        {
            ISessionFactory sessionFactory = new Configuration().Configure().BuildSessionFactory();

            ISession session = sessionFactory.OpenSession();
            try
            {
                string sqlString = "from student where age=? and name=?";
                IQuery query = session.CreateQuery(sqlString);
                query.SetInt32(0, 12);
                query.SetParameter(1, "");
                query.SetFetchSize(3).SetFirstResult(1).SetMaxResults(10);
                return query.List<Student>();
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                session.Close();
                sessionFactory.Close();
            }
        }

        /// <summary>
        /// 标准对象化 query
        /// </summary>
        /// <param name="conditionList"></param>
        /// <returns></returns>
        public IList<Student> CriteriaQueryWithRestrictions()
        {
            ISessionFactory sessionFactory = new Configuration().Configure().BuildSessionFactory();

            ISession session = sessionFactory.OpenSession();
            try
            {
               // ICriteria criteria = Restrictions.And( Restrictions.Eq("age", 12),Restrictions.Eq("name",""));
                SimpleExpression[] rArr = new SimpleExpression[2];

                ICriteria query = session.CreateCriteria(typeof(Student));
                query.Add(Restrictions.And(Restrictions.Eq("Age", 24), Restrictions.Like("Name", "%2")));
                return query.List<Student>();
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                session.Close();
                sessionFactory.Close();
            }
        }

        /// <summary>
        /// 标准对象化 query
        /// </summary>
        /// <param name="conditionList"></param>
        /// <returns></returns>
        public IList<Student> CriteriaQueryWithICriterions(IList<ICriterion> conditionList)
        {
            ISessionFactory sessionFactory = new Configuration().Configure().BuildSessionFactory();

            ISession session = sessionFactory.OpenSession();
            try
            {
                // ICriteria criteria = Restrictions.And( Restrictions.Eq("age", 12),Restrictions.Eq("name",""));
                SimpleExpression[] rArr = new SimpleExpression[2];

                ICriteria query = session.CreateCriteria(typeof(Student));
                foreach (var queryItem in conditionList)
                {
                    query.Add(queryItem);
                }
                
                return query.List<Student>();
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                session.Close();
                sessionFactory.Close();
            }
        }

        /// <summary>
        /// mysql 创建InputPramWithNameAndReturnScalar 
        /// 
        /// </summary>
        /// <returns></returns>
        public IList IQueryGetNamedQuery()
        {
            ISessionFactory sessionFactory = new Configuration().Configure().BuildSessionFactory();
            ISession session = sessionFactory.OpenSession();
            /* procedure in mysql 
                DELIMITER //
                create procedure InputPramWithNameAndReturnScalar
                (in number1 int, out square int)
                BEGIN
	                #select number1+1 into square;
	                set square =number1*number1;
                END;
                //
                DELIMITER 
             调用
             SET @p_inout=1;
#CALL InputPramWithNameAndReturnScalar(10,@p_inout) ;
 CALL `test`.`InputPramWithNameAndReturnScalar`(10, @p_inout);
SELECT @p_inout;
             */
            IQuery query = session.GetNamedQuery("InputPramWithNameAndReturnScalar");
            return query.SetInt32("number1", 10).List();
            
        }

        public void SqlQuery()
        {
            ISessionFactory sessionFactory =  new Configuration().Configure().BuildSessionFactory();

            ISession session = sessionFactory.OpenSession();

            string sqlString = "select * from student";
            ISQLQuery query = session.CreateSQLQuery(sqlString);

        }


        public void SqlQuery1()
        {
            ISessionFactory sessionFactory = new Configuration().Configure().BuildSessionFactory();

            ISession session = sessionFactory.OpenSession();
            //session.CreateSQLQuery()
        }

        /// <summary>
        /// 可以传入
        /// </summary>
        /// <returns></returns>
        public int ExecuteStoreProcedureByIDbCommandIDataParameter()
        {

            ISessionFactory sessionFactory = new Configuration().Configure().BuildSessionFactory();
            ISession session = sessionFactory.OpenSession();
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
                return cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                session.Close();

            }
        }

        /// <summary>
        /// 可以传入
        /// </summary>
        /// <returns></returns>
        public int ExecuteStoreProcedureByIDbCommandIDataParameteCommonParameter(string procName, CommonDataParameter[] dataParaArr)
        {

            ISessionFactory sessionFactory = new Configuration().Configure().BuildSessionFactory();
            ISession session = sessionFactory.OpenSession(new SQLWatcher());
            int value = 1;
            try
            {
                IDbCommand cmd = session.Connection.CreateCommand();
                cmd.CommandText = "InputPramWithNameAndReturnScalar";
                cmd.CommandType = CommandType.StoredProcedure;
                dataParaArr[0] = new CommonDataParameter
                {
                    DbType = DbType.Int32
                    ,
                    ParameterName = "number1"
                    ,
                    Direction = ParameterDirection.Input
                    ,
                    Value = 10
                };
                dataParaArr[1] = new CommonDataParameter
                {
                    DbType = DbType.Int32
                    ,
                    ParameterName = "square"
                    ,
                    Direction = ParameterDirection.Output
                    ,
                    Value = 0
                };

                IDataParameter dp = cmd.CreateParameter();
                dp.DbType = dataParaArr[0].DbType;
                dp.ParameterName = dataParaArr[0].ParameterName;
                dp.Direction = dataParaArr[0].Direction;
                dp.Value = dataParaArr[0].Value;
                cmd.Parameters.Add(dp);

                IDataParameter dp2 = cmd.CreateParameter();
                dp2.DbType = dataParaArr[1].DbType;
                dp2.ParameterName = dataParaArr[1].ParameterName;
                dp2.Direction = dataParaArr[1].Direction;
                dp2.Value = dataParaArr[1].Value;
                cmd.Parameters.Add(dp2);
                

                cmd.ExecuteNonQuery();
                session.Flush();
                System.Console.Write("square 10 =" + dp2.Value);
                return 0;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                session.Close();

            }
        }
    }
}
