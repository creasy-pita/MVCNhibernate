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

            IList<Student> list = CriteriaQueryWithRestrictions();
            System.Console.Write("Count:"+ list.Count);
            
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

        public void SqlQuery()
        {
            ISessionFactory sessionFactory =  new Configuration().Configure().BuildSessionFactory();

            ISession session = sessionFactory.OpenSession();

            string sqlString = "select * from student";
            ISQLQuery query = session.CreateSQLQuery(sqlString);

        }
    }
}
