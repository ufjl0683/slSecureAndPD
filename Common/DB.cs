using slSecure.Web;
using System;
using System.Collections.Generic;
using System.Net;
using System.ServiceModel.DomainServices.Client;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;




namespace Common
{
    public  static  class DB
    {    public static SecureDBContext GetDB()
        {
            return new SecureDBContext();
        }


    public static void Test()
    {
        //SecureDBContext db = new SecureDBContext();
        // EntityQuery <tblUser> q=  from n in db.GetTblUserQuery() where n.
    }

    public static Task<IEnumerable<T>> LoadAsync<T>(this DomainContext source, EntityQuery<T> query) where T : Entity
    {
        return source.LoadAsync(   query, LoadBehavior.KeepCurrent);
    }


    public static Task<IEnumerable<T>> LoadAsync<T>(this DomainContext source, EntityQuery<T> query, LoadBehavior loadBehavior) where T : Entity
    {
        TaskCompletionSource<IEnumerable<T>> taskCompletionSource = new TaskCompletionSource<IEnumerable<T>>();

        source.Load(
            query,
            loadBehavior,
            loadOperation =>
            {
                if (loadOperation.HasError && !loadOperation.IsErrorHandled)
                {
                    taskCompletionSource.TrySetException(loadOperation.Error);
                    loadOperation.MarkErrorAsHandled();
                }
                else if (loadOperation.IsCanceled)
                {
                    taskCompletionSource.TrySetCanceled();
                }
                else
                {
                    taskCompletionSource.TrySetResult(loadOperation.Entities);
                }
            },
            null);

        return taskCompletionSource.Task;
    }


 


    }

    
    
}
