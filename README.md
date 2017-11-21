# MVCNhibernate
1 hibernate调用存储过程
1 session.getconnection  用odbc 的 CallableStatement
2 用 sqlquery的sql语句，如果有参数则封装参数
                   String procedureSql = "{call "+ procedureName +"()}";
                    Query query = session.createSQLQuery(procedureSql);
                    Integer num = query.executeUpdate();
3 session.GetNameQuery(procedureName).setInt32("paramaterName",val)  加  映射文件中的配置
