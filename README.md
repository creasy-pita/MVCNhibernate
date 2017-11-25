# MVCNhibernate

2017-11-25
参数（存储过程的输入输出参数 ，或增删改的条件参数） 外层传递的代码实现
1 原生odbc方式  CommonDataParameter 实现 System.Data.IDataParameter  
2 ICriteria 下
（1） ICriteria[] ICriteriaArr 见 CreateCriteria().Add(ICriteria Expression)
ICriteria crt = _session.CreateCriteria(typeof(Employees));

        crt.Add(Expression.Eq("City","London"));
（2）Restrictions

ICriteria crt = _session.CreateCriteria(typeof(Employees));

        crt.Add(Restrictions.Eq("City", "London"));
（3）通过实例来查询
Employees ee = new Employees { City = "London", BirthDate = Convert.ToDateTime("1955-03-04 00:00:00.000") };

         ICriteria crt = _session.CreateCriteria(typeof(Employees));

         crt.Add(Example.Create(ee));

     return crt.List<Employees>();

调用存储过程的代码实现主要难点
1 参数配置，各个方言使用统一参数类型 例如此种的 CommonDataParameter  
MySql.Data.MySqlClient.MySqlDbType
System.Data.DbType
OracleClient.OracleType
但特殊 oracleType.Cursor
2 返回类型配置
3 
a.如果原生odbc访问 datareader,然后自己组装


b. 如果通过映射文件配置， 例如

<sql-query name="persons">
    <return alias="person" class="eg.Person"/>
    SELECT person.NAME AS {person.Name},
           person.AGE AS {person.Age},
           person.SEX AS {person.Sex}
    FROM PERSON person
    WHERE person.NAME LIKE :namePattern
</sql-query>

var people = sess.GetNamedQuery("persons")
    .SetString("namePattern", namePattern)
    .SetMaxResults(50)
    .List<Person>();
2017-11-15
1 hibernate调用存储过程
1 session.getconnection  用odbc 的 CallableStatement
2 用 sqlquery的sql语句，如果有参数则封装参数
                   String procedureSql = "{call "+ procedureName +"()}";
                    Query query = session.createSQLQuery(procedureSql);
                    Integer num = query.executeUpdate();
3 session.GetNameQuery(procedureName).setInt32("paramaterName",val)  加  映射文件中的配置
