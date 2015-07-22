
namespace slSecure.Web
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Data;
    using System.Linq;
    using System.ServiceModel.DomainServices.EntityFramework;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;


    // 使用 SecureDBEntities 內容實作應用程式邏輯。
    // TODO: 將應用程式邏輯加入至這些方法或其他方法。
    // TODO: 連接驗證 (Windows/ASP.NET Forms) 並取消下面的註解，以停用匿名存取
    // 視需要也考慮加入要限制存取的角色。
    // [RequiresAuthentication]
    [EnableClientAccess()]
    public class DomainService1 : LinqToEntitiesDomainService<SecureDBEntities>
    {

        // TODO:
        // 考慮限制查詢方法的結果。如果需要其他輸入，可以將
        // 參數加入至這個中繼資料，或建立其他不同名稱的其他查詢方法。
        // 為支援分頁，您必須將排序加入至 'vwPasswordEveryDayDifference' 查詢。
        public IQueryable<vwPasswordEveryDayDifference> GetVwPasswordEveryDayDifference()
        {
            return this.ObjectContext.vwPasswordEveryDayDifference;
        }

        // TODO:
        // 考慮限制查詢方法的結果。如果需要其他輸入，可以將
        // 參數加入至這個中繼資料，或建立其他不同名稱的其他查詢方法。
        // 為支援分頁，您必須將排序加入至 'vwR23DeviceStateLog' 查詢。
        public IQueryable<vwR23DeviceStateLog> GetVwR23DeviceStateLog()
        {
            return this.ObjectContext.vwR23DeviceStateLog;
        }

        // TODO:
        // 考慮限制查詢方法的結果。如果需要其他輸入，可以將
        // 參數加入至這個中繼資料，或建立其他不同名稱的其他查詢方法。
        // 為支援分頁，您必須將排序加入至 'vwR23EngineRoomLog' 查詢。
        public IQueryable<vwR23EngineRoomLog> GetVwR23EngineRoomLog()
        {
            return this.ObjectContext.vwR23EngineRoomLog;
        }

        // TODO:
        // 考慮限制查詢方法的結果。如果需要其他輸入，可以將
        // 參數加入至這個中繼資料，或建立其他不同名稱的其他查詢方法。
        // 為支援分頁，您必須將排序加入至 'vwR23HardwareState' 查詢。
        public IQueryable<vwR23HardwareState> GetVwR23HardwareState()
        {
            return this.ObjectContext.vwR23HardwareState;
        }

        // TODO:
        // 考慮限制查詢方法的結果。如果需要其他輸入，可以將
        // 參數加入至這個中繼資料，或建立其他不同名稱的其他查詢方法。
        // 為支援分頁，您必須將排序加入至 'vwR23ReaderLog' 查詢。
        public IQueryable<vwR23ReaderLog> GetVwR23ReaderLog()
        {
            return this.ObjectContext.vwR23ReaderLog;
        }
    }
}


