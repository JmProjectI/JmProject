using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace JMProject.Common
{
    public class DataBack
    {
        public DataBack()
        {

        }

        /*  
            通过调用MSSQL的SQLDMO.DLL文件来实现备份数据库
            1.首先在在项目中引用SQLDMO.DLL文件。
            2.在引用中的SQLDMO.DLL文件右击-->属性-->设置[嵌入互操作类型]为flase*/
        #region SQL数据库备份
        /// <summary>
        /// SQL数据库备份
        /// </summary>
        /// <param name="ServerIP">SQL服务器IP或(Localhost)</param>
        /// <param name="LoginName">数据库登录名</param>
        /// <param name="LoginPass">数据库登录密码</param>
        /// <param name="DBName">数据库名</param>
        /// <param name="BackPath">备份到的路径</param>
        public static bool BakSql(string ServerIP, string LoginName, string LoginPass, string DBName, string BackPath)
        {
            SQLDMO.Backup oBackup = new SQLDMO.BackupClass();
            SQLDMO.SQLServer oSQLServer = new SQLDMO.SQLServerClass();
            try
            {
                oSQLServer.LoginSecure = false;
                oSQLServer.Connect(ServerIP, LoginName, LoginPass);
                oBackup.Action = SQLDMO.SQLDMO_BACKUP_TYPE.SQLDMOBackup_Database;
                oBackup.Database = DBName;
                oBackup.Files = BackPath;
                oBackup.BackupSetName = DBName;
                oBackup.BackupSetDescription = "数据库备份";
                oBackup.Initialize = true;
                oBackup.SQLBackup(oSQLServer);
                return true;
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
                return false;
            }
            finally
            {
                oSQLServer.DisConnect();
            }
        }

        public static bool BakSql(string DBName, string BackPath)
        {
            try
            {
                string ds = ConfigurationManager.AppSettings["ds"].ToString();
                string uid = ConfigurationManager.AppSettings["uid"].ToString();
                string pwd = ConfigurationManager.AppSettings["pwd"].ToString();
                return BakSql(ds, uid, pwd, DBName, BackPath);
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
        }
        #endregion


        #region 还原SQL数据库
        public static bool ResSql(string dbname, string backpath)
        {
            string ds = ConfigurationManager.AppSettings["ds"].ToString();
            string uid = ConfigurationManager.AppSettings["uid"].ToString();
            string pwd = ConfigurationManager.AppSettings["pwd"].ToString();
            try
            {
                ///   杀死和要恢复的数据库有关的进程 
                SQLDMO.SQLServer svr = new SQLDMO.SQLServerClass();
                svr.Connect(ds, uid, pwd);
                SQLDMO.QueryResults qr = svr.EnumProcesses(-1);
                int iColPIDNum = -1;
                int iColDbName = -1;
                for (int i = 1; i <= qr.Columns; i++)
                {
                    string strName = qr.get_ColumnName(i);
                    if (strName.ToUpper().Trim() == "SPID")
                    {
                        iColPIDNum = i;
                    }
                    else if (strName.ToUpper().Trim() == "DBNAME")
                    {
                        iColDbName = i;
                    }
                    if (iColPIDNum != -1 && iColDbName != -1)
                        break;
                }
                for (int i = 1; i <= qr.Rows; i++)
                {
                    int lPID = qr.GetColumnLong(i, iColPIDNum);
                    string strDBName = qr.GetColumnString(i, iColDbName);
                    if (strDBName.ToUpper() == dbname.ToUpper())
                        svr.KillProcess(lPID);
                }
                svr.DisConnect();

                ///   恢复数据库 
                SQLDMO.Restore oRestore = new SQLDMO.RestoreClass();
                SQLDMO.SQLServer oSQLServer = new SQLDMO.SQLServerClass();

                oSQLServer.LoginSecure = false;
                oSQLServer.Connect(ds, uid, pwd);
                oRestore.Action = SQLDMO.SQLDMO_RESTORE_TYPE.SQLDMORestore_Database;
                oRestore.Database = dbname;
                string strFile = backpath;
                oRestore.Files = strFile;
                oRestore.FileNumber = 1;
                oRestore.ReplaceDatabase = true;
                oRestore.SQLRestore(oSQLServer);
                oSQLServer.DisConnect();
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw new Exception(ex.ToString());
            }
        }
        #endregion
    }
}