using Microsoft.VisualStudio.TestTools.UnitTesting;
using ОТК_ЗАО__Русская_косметика_.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ОТК_ЗАО__Русская_косметика_.Classes;
using System.Configuration;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Data;

namespace ОТК_ЗАО__Русская_косметика_.Views.Tests
{
    [TestClass()]
    public class OrderFormationWindowTests
    {
        CheckingWorker checking = new CheckingWorker();

        [TestMethod()]
        public void CheckWorkerIsFit()
        {
            int workerPost;

            string connectionString = "Data Source=LAPTOP-1BQG2FKL\\SQLEXPRESS;initial catalog=demoTesting;integrated security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "select PostFK from Workers where FIO=@name";
                    command.Parameters.Add("@name", SqlDbType.NVarChar).Value = "Игнатов Кассиан Васильевич";
                    workerPost = Convert.ToInt32(command.ExecuteScalarAsync().Result);
                    connection.Close();
                }
            }

            var actual = checking.CheckWorker(workerPost);
            Assert.IsTrue(actual);
        }


        [TestMethod()]
        public void CheckWorkerIsNotFit()
        {
            int workerPost;

            string connectionString = "Data Source=LAPTOP-1BQG2FKL\\SQLEXPRESS;initial catalog=demoTesting;integrated security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "select PostFK from Workers where FIO=@name";
                    command.Parameters.Add("@name", SqlDbType.NVarChar).Value = "Федоров Федор Федорович";
                    workerPost = Convert.ToInt32(command.ExecuteScalarAsync().Result);
                    connection.Close();
                }
            }

            var actual = checking.CheckWorker(workerPost);
            Assert.IsFalse(actual);
        }
    }
}