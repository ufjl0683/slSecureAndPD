using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoomDoorControlServer
{
    class DatabaseAccess
    {
        static Queue<string> cmdQueue = new Queue<string>();
        static System.Threading.AutoResetEvent wait = new System.Threading.AutoResetEvent(false);
        static bool End = false;
        static System.Threading.Thread t;

        internal static void StartDatabaseAcces()
        {
            if (t == null)
            {
                End = false;
                t = new System.Threading.Thread(DatabaseAccess.DatabaseAcces);
                t.Start();
            }
        }

        internal static void EndDatabaseAcces()
        {
            End = true;
        }

        static void DatabaseAcces()
        {
            dbroomEntities dbroom = new dbroomEntities();

            while(!End || cmdQueue.Count > 0) {
                if (cmdQueue.Count > 0)
                {
                    string cmd = cmdQueue.Dequeue();
                    try
                    {
                        dbroom.Database.ExecuteSqlCommand(cmd);
                    }
                    catch(Exception ex)
                    {
                        TCommon.SaveLog(ex.Message + "\r\n" + cmd);
                    }
                }
                if (cmdQueue.Count == 0)
                {
                    wait.WaitOne(1000);
                }
            }
        }

        internal static void DatabaseAcces(string cmd)
        {
            cmdQueue.Enqueue(cmd);
        }
    }
}
