using Microsoft.AspNet.SignalR;
using Model.Dao;
using Model.DataModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;


namespace avSVAW
{
    public class Monitoring
    {
        string Message2Monitoring = "";
        string Message2ListLoss = "";
        string Message3MonitoringLine = "";
        DateTime CheckDateTime = DateTime.Now;
        //Here we will add a function for register monitoring (will add sql dependency)
        private DateTime _refDate = DateTime.MinValue;
        public void RegisterListLoss()
        {
            string conStr = System.Configuration.ConfigurationManager.ConnectionStrings["ConStr"].ConnectionString;
            string sqlCommand = " SELECT [Id],[NodeId],[EventDefId],[T3],[T2],[T1],[Reason]" +
                               " FROM [dbo].[tblNodeEvent];";
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand(sqlCommand, con);

                if (con.State != System.Data.ConnectionState.Open)
                {
                    con.Open();
                }
                cmd.Notification = null;

                SqlDependency sqlDep = new SqlDependency(cmd);

                //sqlDep.OnChange -= new OnChangeEventHandler(sqlDep_OnChange);
                sqlDep.OnChange += new OnChangeEventHandler(sqlDep_OnChange_listLoss);
                cmd.ExecuteNonQuery();
               
            }
        }

        public void RegisterMonitorLine()
        {
            string conStr = System.Configuration.ConfigurationManager.ConnectionStrings["ConStr"].ConnectionString;
            string sqlCommand = "SELECT '0', [LineId], [ProductModel], [Plan],[Actual],[StopDuration],[RunningStatus],[OEE],[LineName],[Target],[Status],[WorkingPlanId]" +
                           " FROM [dbo].[tblLineOnline] ;" +
                           "SELECT '1',[NodeId], [NodeName], [LineId],[LineName],[Description],[Status],[UpdateTime],'0','0','',''" +
                                " FROM [dbo].[tblNodeOnline];  ";
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand(sqlCommand, con);

                if (con.State != System.Data.ConnectionState.Open)
                {
                    con.Open();
                }
                cmd.Notification = null;

                SqlDependency sqlDep = new SqlDependency(cmd);

                sqlDep.OnChange += new OnChangeEventHandler(sqlDep_OnChange_MonitorLine);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string iStatus = "";
                            if (reader[0] != DBNull.Value)
                            {
                                iStatus = reader.GetString(0);
                            }

                            int LineId = 0;
                            if (reader[1] != DBNull.Value)
                            {
                                LineId = reader.GetInt32(1);
                            }
                            string ProductModel = "";
                            if (reader[2] != DBNull.Value)
                            {
                                ProductModel = reader.GetString(2);
                            }
                            int Plan = 0;
                            if (reader[3] != DBNull.Value)
                            {
                                Plan = reader.GetInt32(3);
                            }
                            int Actual = 0;
                            if (reader[4] != DBNull.Value)
                            {
                                Actual = reader.GetInt32(4);
                            }
                            long StopDuration = 0;
                            if (reader[5] != DBNull.Value)
                            {
                                StopDuration = reader.GetInt64(5);
                            }

                            string RunningStatus = "";
                            if (reader[6] != DBNull.Value)
                            {
                                RunningStatus = reader.GetString(6);
                            }
                            double Oee = 0;
                            if (reader[7] != DBNull.Value)
                            {
                                Oee = reader.GetDouble(7);
                            }
                            string LineName = "";
                            if (reader[8] != DBNull.Value)
                            {
                                LineName = reader.GetString(8);
                            }
                            int target = 0;
                            if (reader[9] != DBNull.Value)
                            {
                                target = reader.GetInt32(9);
                            }
                            string Status = "";
                            if (reader[10] != DBNull.Value)
                            {
                                Status = reader.GetString(10);
                            }
                            Int64 WorkingPlanId = 0;
                            if (reader[11] != DBNull.Value)
                            {
                                WorkingPlanId = reader.GetInt64(11);
                            }
                            Message3MonitoringLine += iStatus + "|" + LineId + "|" + ProductModel+ "|" + Plan+ 
                                "|" + Actual + "|" + StopDuration+ "|" + RunningStatus + "|" +Math.Round(Oee)+ "|" + LineName+ "|" + target+"|" + Status + "|" + WorkingPlanId + ";";

                        }
                    }

                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            string iStatus = "";
                            if (reader[0] != DBNull.Value)
                            {
                                iStatus = reader.GetString(0);
                            }

                            int NodeId = 0;
                            if (reader[1] != DBNull.Value)
                            {
                                NodeId = reader.GetInt32(1);
                            }
                            string NodeName = "";
                            if (reader[2] != DBNull.Value)
                            {
                                NodeName = reader.GetString(2);
                            }
                            int LineId = 0;
                            if (reader[3] != DBNull.Value)
                            {
                                LineId = reader.GetInt32(3);
                            }
                            string LineName = "";
                            if (reader[4] != DBNull.Value)
                            {
                                LineName = reader.GetString(4);
                            }
                            string Description = "";
                            if (reader[5] != DBNull.Value)
                            {
                                Description = reader.GetString(5);
                            }
                            string Status = "";
                            if (reader[6] != DBNull.Value)
                            {
                                Status = reader.GetString(6);
                            }
                            DateTime UpdateTime= DateTime.MinValue;
                            if (reader[7] != DBNull.Value)
                            {
                                UpdateTime = reader.GetDateTime(7);
                            }
                           
                            Message3MonitoringLine += iStatus + "|" + NodeId + "|" + NodeName + "|" + LineId + "|" + LineName + "|"+ Description + "|" 
                                + Status + "|" + UpdateTime.ToString("yyyy/MM/dd HH:mm:ss") + "|0|0||;";
                        }
                    }


                }
            }
        }

        public void RegisterMonitoring()
        {
            string conStr = System.Configuration.ConfigurationManager.ConnectionStrings["ConStr"].ConnectionString;
            // huantn add signalr

            string sqlCommand = " SELECT [NodeId], [NodeName], [LineId],[Status],'0',[Description],'0','0'" +
                                " FROM [dbo].[tblNodeOnline]; " +
                                " SELECT [LineId], [LineName], [ShiftId],[Status],'1',[NumberOfStop],[StopDuration],[Quality],[Target],[Actual],[OEE],[BreakTime],[Plan],[ProductName],[TakeTime],[HeadCount],[RunningStatus]" +
                                " FROM [dbo].[tblLineOnline] " ;
            //you can notice here I have added table name like this [dbo].[Contacts] with [dbo], its mendatory when you use Sql Dependency
            using (SqlConnection con = new SqlConnection(conStr))
            {
                SqlCommand cmd = new SqlCommand(sqlCommand, con);
              
                if (con.State != System.Data.ConnectionState.Open)
                {
                    con.Open();
                }
                cmd.Notification = null;

                SqlDependency sqlDep = new SqlDependency(cmd);

                //sqlDep.OnChange -= new OnChangeEventHandler(sqlDep_OnChange);
                sqlDep.OnChange += new OnChangeEventHandler(sqlDep_OnChange);
                ////we must have to execute the command here
                //cmd.ExecuteReader();
                //cmdTrigger.ExecuteNonQuery();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            int _id = reader.GetInt32(0);
                            string _name = reader.GetString(1);

                            int _val1 = 0;
                            if (reader[3] != DBNull.Value)
                            {
                                _val1 = reader.GetInt32(2);
                            }
                            string _status = "" ;
                            if (reader[3] != DBNull.Value)
                            {
                                _status = reader.GetString(3);
                            }
                            string _type = "";
                            if (reader[4] != DBNull.Value)
                            {
                                _type = reader.GetString(4);
                            }

                            string Description ="" ;
                            if (reader[5] != DBNull.Value)
                            {
                                Description = reader.GetString(5);
                            }
                            Message2Monitoring += _id.ToString() + "|" + _name.ToString() 
                                + "|" + _val1.ToString() + "|" + _status +"|" + _type + "|"+ Description + "|-1|-1|-1|-1|-1|-1|-1|-1|-1|-1|-1;";

                            //break;
                            //Thread.Sleep(30);
                        }
                    }
                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            
                                int _id = reader[0]!=null?Convert.ToInt32(reader[0]):0;
                                string _name = reader.GetString(1);
                                int _val1 = reader.GetInt32(2);
                                string _status = reader.GetString(3);
                                string _type = reader.GetString(4);
                                int NumberOfStop = 0;
                                long StopDuration = 0;
                                if (!reader.IsDBNull(5))
                                {
                                    NumberOfStop = reader.GetInt32(5); 
                                }
                                if (!reader.IsDBNull(6))
                                {
                                    StopDuration = reader.GetInt64(6); 
                                }
                            double Quality = 0;
                                if (!reader.IsDBNull(7))
                                {
                                     Quality  = reader.GetDouble(7);
                                }
                            int target = 0;
                            int actual = 0;
                            if (reader[8] != DBNull.Value)
                            {
                                target = reader.GetInt32(8);
                            }
                            if (reader[9] != DBNull.Value)
                            {
                                actual = reader.GetInt32(9);
                            }
                            double oee = 0;
                            if (reader[10] != DBNull.Value)
                            {
                                oee = reader.GetDouble(10);
                            }
                            string BreakTime = "";
                            if (reader[11] != DBNull.Value)
                            {
                                BreakTime = reader.GetString(11);
                            }
                            int Plan = 0;
                            if (reader[12] != DBNull.Value)
                            {
                                Plan = reader.GetInt32(12);
                            }
                            string ProductName = "";
                            if (reader[13] != DBNull.Value)
                            {
                                ProductName = reader.GetString(13);
                            }
                            int TaktTime = 0;
                            if (reader[14] != DBNull.Value)
                            {
                                TaktTime = reader.GetInt32(14);
                            }
                            int HeadCount = 0;
                            if (reader[15] != DBNull.Value)
                            {
                                HeadCount = reader.GetInt32(15);
                            }
                            string RunningStatus = "";
                            if (reader[16] != DBNull.Value)
                            {
                                RunningStatus = reader.GetString(16);
                            }
                            Message2Monitoring += _id.ToString() + "|" + _name.ToString()
                                    + "|" + _val1.ToString() + "|" + _status.ToString() + "|" + _type+ "|" + 
                                    NumberOfStop + "|" + StopDuration + "|" + Math.Round(Quality) + "|" + target + "|" + actual +"|" 
                                    + Math.Round(oee) +"|" + BreakTime +"|" + Plan.ToString() + "|"+ ProductName+ "|"+ TaktTime + "|"+ HeadCount +"|"+ RunningStatus + ";";
                            
                        }
                    }
                }

            }
        }

    

        void sqlDep_OnChange(object sender, SqlNotificationEventArgs e)
        {
            //or you can also check => if (e.Info == SqlNotificationInfo.Insert) , if you want notification only for inserted record
            if (e.Type == SqlNotificationType.Change)
            {
                SqlDependency sqlDep = sender as SqlDependency;
                sqlDep.OnChange -= sqlDep_OnChange;

                RegisterMonitoring();

               
                //from here we will send notification message to client
                var monitoringHub = GlobalHost.ConnectionManager.GetHubContext<MonitoringHub>();

                monitoringHub.Clients.All.notify(Message2Monitoring);
                
                //Reset
                //Clear Message
                Message2Monitoring = "";

            }
        }

        void sqlDep_OnChange_listLoss(object sender, SqlNotificationEventArgs e)
        {
            
            //or you can also check => if (e.Info == SqlNotificationInfo.Insert) , if you want notification only for inserted record
            if (e.Type == SqlNotificationType.Change)
            {
              

                RegisterListLoss();


                //from here we will send notification message to client
                var listlossHub = GlobalHost.ConnectionManager.GetHubContext<ListLossHub>();
                Message2ListLoss = e.Type.ToString();
                listlossHub.Clients.All.notify(Message2ListLoss);
                _refDate = DateTime.MinValue;
                //Reset
                //Clear Message
                Message2ListLoss = "";

            }
        }
        void sqlDep_OnChange_MonitorLine(object sender, SqlNotificationEventArgs e)
        {
            
            //or you can also check => if (e.Info == SqlNotificationInfo.Insert) , if you want notification only for inserted record
            if (e.Type == SqlNotificationType.Change)
            {
              
                RegisterMonitorLine();

                //from here we will send notification message to client
                var monitoringLine = GlobalHost.ConnectionManager.GetHubContext<MonitorLineHub>();
                monitoringLine.Clients.All.getdata(Message3MonitoringLine);
                Message3MonitoringLine = "";

            }
        }

        //public List<tblEmployee> GetData(DateTime afterDate)
        //{
        //    using (SignalRDBEntities dc = new SignalRDBEntities())
        //    {
        //        return dc.tblEmployees.Where(a => a.AddedOn > afterDate).OrderByDescending(a => a.AddedOn).ToList();
        //    }
        //}
    }
}