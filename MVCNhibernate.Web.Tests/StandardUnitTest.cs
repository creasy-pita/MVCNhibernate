using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MVCNhibernate.Domain.Entities;
using MVCNhibernate.Data;
using System.Data;

namespace MVCNhibernate.Web.Tests
{
    /// <summary>
    /// StandardUnitTest 的摘要说明
    /// </summary>
    [TestClass]
    public class StandardUnitTest
    {
        public StandardUnitTest()
        {
            //
            //TODO:  在此处添加构造函数逻辑
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///获取或设置测试上下文，该上下文提供
        ///有关当前测试运行及其功能的信息。
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region 附加测试特性
        //
        // 编写测试时，可以使用以下附加特性: 
        //
        // 在运行类中的第一个测试之前使用 ClassInitialize 运行代码
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // 在类中的所有测试都已运行之后使用 ClassCleanup 运行代码
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // 在运行每个测试之前，使用 TestInitialize 来运行代码
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // 在每个测试运行完之后，使用 TestCleanup 来运行代码
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void AddStudent()
        {
            NHibernateHelper dataHelper = new NHibernateHelper();
            Student s = new Student { StudentId = System.Guid.NewGuid().ToString(), Name = "wweess", Age = 43 };
            if (dataHelper.Add<Student>(s))
                Console.Write("add success");
            
            //Student s1 = new Student { StudentId = System.Guid.NewGuid().ToString(), Name = "s2ews1", Age = 13 };

            //List<Student> list = new List<Student>();
            //list.Add(s);
            //list.Add(s1);
            //if (dataHelper.AddList<Student>(list))
            //    Console.Write("add success");

        }

        [TestMethod]
        public void ExecuteSqlReturnEntityList()
        {
            NHibernateHelper dataHelper = new NHibernateHelper();
            IList<Student> list = dataHelper.ExecuteSql<Student>("select * from student");
            foreach (Student s in list)
            {
                Console.Write(s.StudentId);
            }
            //Student s1 = new Student { StudentId = System.Guid.NewGuid().ToString(), Name = "s2ews1", Age = 13 };

            //List<Student> list = new List<Student>();
            //list.Add(s);
            //list.Add(s1);
            //if (dataHelper.AddList<Student>(list))
            //    Console.Write("add success");

        }
        [TestMethod]

        public void ExecuteStoreProcedureDataParameter()
        {
            NHibernateHelper dataHelper = new NHibernateHelper();
            CommonDataParameter[] dataParaArr = new CommonDataParameter[2];
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
                dataHelper.ExecuteStoreProcedure("InputPramWithNameAndReturnScalar", dataParaArr);

                System.Console.Write("square 10 =" + dataParaArr[1].Value);
        }

        [TestMethod]

        public void GetPageList()
        {
            NHibernateHelper dataHelper = new NHibernateHelper();
            int totalCount = 0;
            IList<Student> list = dataHelper.GetPageList<Student>(6, 0, out totalCount);
            Console.WriteLine("totalCount=" + totalCount);
            foreach (Student s in list)
            {
                Console.WriteLine(s.StudentId);
            }
        }
    }
}
