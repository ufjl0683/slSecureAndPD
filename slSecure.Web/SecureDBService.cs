
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
    public class SecureDBService : LinqToEntitiesDomainService<SecureDBEntities>
    {

        // TODO:
        // 考慮限制查詢方法的結果。如果需要其他輸入，可以將
        // 參數加入至這個中繼資料，或建立其他不同名稱的其他查詢方法。
        // 為支援分頁，您必須將排序加入至 'tblAIItem1HourLog' 查詢。
        public IQueryable<tblAIItem1HourLog> GetTblAIItem1HourLog()
        {
            return this.ObjectContext.tblAIItem1HourLog;
        }

        public void InsertTblAIItem1HourLog(tblAIItem1HourLog tblAIItem1HourLog)
        {
            if ((tblAIItem1HourLog.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblAIItem1HourLog, EntityState.Added);
            }
            else
            {
                this.ObjectContext.tblAIItem1HourLog.AddObject(tblAIItem1HourLog);
            }
        }

        public void UpdateTblAIItem1HourLog(tblAIItem1HourLog currenttblAIItem1HourLog)
        {
            this.ObjectContext.tblAIItem1HourLog.AttachAsModified(currenttblAIItem1HourLog, this.ChangeSet.GetOriginal(currenttblAIItem1HourLog));
        }

        public void DeleteTblAIItem1HourLog(tblAIItem1HourLog tblAIItem1HourLog)
        {
            if ((tblAIItem1HourLog.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblAIItem1HourLog, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.tblAIItem1HourLog.Attach(tblAIItem1HourLog);
                this.ObjectContext.tblAIItem1HourLog.DeleteObject(tblAIItem1HourLog);
            }
        }

        // TODO:
        // 考慮限制查詢方法的結果。如果需要其他輸入，可以將
        // 參數加入至這個中繼資料，或建立其他不同名稱的其他查詢方法。
        // 為支援分頁，您必須將排序加入至 'tblAlarmLog' 查詢。
        public IQueryable<tblAlarmLog> GetTblAlarmLog()
        {
            return this.ObjectContext.tblAlarmLog;
        }

        public void InsertTblAlarmLog(tblAlarmLog tblAlarmLog)
        {
            if ((tblAlarmLog.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblAlarmLog, EntityState.Added);
            }
            else
            {
                this.ObjectContext.tblAlarmLog.AddObject(tblAlarmLog);
            }
        }

        public void UpdateTblAlarmLog(tblAlarmLog currenttblAlarmLog)
        {
            this.ObjectContext.tblAlarmLog.AttachAsModified(currenttblAlarmLog, this.ChangeSet.GetOriginal(currenttblAlarmLog));
        }

        public void DeleteTblAlarmLog(tblAlarmLog tblAlarmLog)
        {
            if ((tblAlarmLog.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblAlarmLog, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.tblAlarmLog.Attach(tblAlarmLog);
                this.ObjectContext.tblAlarmLog.DeleteObject(tblAlarmLog);
            }
        }

        // TODO:
        // 考慮限制查詢方法的結果。如果需要其他輸入，可以將
        // 參數加入至這個中繼資料，或建立其他不同名稱的其他查詢方法。
        // 為支援分頁，您必須將排序加入至 'tblCardCommandLog' 查詢。
        public IQueryable<tblCardCommandLog> GetTblCardCommandLog()
        {
            return this.ObjectContext.tblCardCommandLog;
        }

        public void InsertTblCardCommandLog(tblCardCommandLog tblCardCommandLog)
        {
            if ((tblCardCommandLog.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblCardCommandLog, EntityState.Added);
            }
            else
            {
                this.ObjectContext.tblCardCommandLog.AddObject(tblCardCommandLog);
            }
        }

        public void UpdateTblCardCommandLog(tblCardCommandLog currenttblCardCommandLog)
        {
            this.ObjectContext.tblCardCommandLog.AttachAsModified(currenttblCardCommandLog, this.ChangeSet.GetOriginal(currenttblCardCommandLog));
        }

        public void DeleteTblCardCommandLog(tblCardCommandLog tblCardCommandLog)
        {
            if ((tblCardCommandLog.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblCardCommandLog, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.tblCardCommandLog.Attach(tblCardCommandLog);
                this.ObjectContext.tblCardCommandLog.DeleteObject(tblCardCommandLog);
            }
        }

        // TODO:
        // 考慮限制查詢方法的結果。如果需要其他輸入，可以將
        // 參數加入至這個中繼資料，或建立其他不同名稱的其他查詢方法。
        // 為支援分頁，您必須將排序加入至 'tblCardReaderConfig' 查詢。
        public IQueryable<tblCardReaderConfig> GetTblCardReaderConfig()
        {
            return this.ObjectContext.tblCardReaderConfig;
        }

        public void InsertTblCardReaderConfig(tblCardReaderConfig tblCardReaderConfig)
        {
            if ((tblCardReaderConfig.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblCardReaderConfig, EntityState.Added);
            }
            else
            {
                this.ObjectContext.tblCardReaderConfig.AddObject(tblCardReaderConfig);
            }
        }

        public void UpdateTblCardReaderConfig(tblCardReaderConfig currenttblCardReaderConfig)
        {
            this.ObjectContext.tblCardReaderConfig.AttachAsModified(currenttblCardReaderConfig, this.ChangeSet.GetOriginal(currenttblCardReaderConfig));
        }

        public void DeleteTblCardReaderConfig(tblCardReaderConfig tblCardReaderConfig)
        {
            if ((tblCardReaderConfig.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblCardReaderConfig, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.tblCardReaderConfig.Attach(tblCardReaderConfig);
                this.ObjectContext.tblCardReaderConfig.DeleteObject(tblCardReaderConfig);
            }
        }

        // TODO:
        // 考慮限制查詢方法的結果。如果需要其他輸入，可以將
        // 參數加入至這個中繼資料，或建立其他不同名稱的其他查詢方法。
        // 為支援分頁，您必須將排序加入至 'tblCCTVConfig' 查詢。
        public IQueryable<tblCCTVConfig> GetTblCCTVConfig()
        {
            return this.ObjectContext.tblCCTVConfig;
        }

        public void InsertTblCCTVConfig(tblCCTVConfig tblCCTVConfig)
        {
            if ((tblCCTVConfig.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblCCTVConfig, EntityState.Added);
            }
            else
            {
                this.ObjectContext.tblCCTVConfig.AddObject(tblCCTVConfig);
            }
        }

        public void UpdateTblCCTVConfig(tblCCTVConfig currenttblCCTVConfig)
        {
            this.ObjectContext.tblCCTVConfig.AttachAsModified(currenttblCCTVConfig, this.ChangeSet.GetOriginal(currenttblCCTVConfig));
        }

        public void DeleteTblCCTVConfig(tblCCTVConfig tblCCTVConfig)
        {
            if ((tblCCTVConfig.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblCCTVConfig, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.tblCCTVConfig.Attach(tblCCTVConfig);
                this.ObjectContext.tblCCTVConfig.DeleteObject(tblCCTVConfig);
            }
        }

        // TODO:
        // 考慮限制查詢方法的結果。如果需要其他輸入，可以將
        // 參數加入至這個中繼資料，或建立其他不同名稱的其他查詢方法。
        // 為支援分頁，您必須將排序加入至 'tblControllerCard' 查詢。
        public IQueryable<tblControllerCard> GetTblControllerCard()
        {
            return this.ObjectContext.tblControllerCard;
        }

        public void InsertTblControllerCard(tblControllerCard tblControllerCard)
        {
            if ((tblControllerCard.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblControllerCard, EntityState.Added);
            }
            else
            {
                this.ObjectContext.tblControllerCard.AddObject(tblControllerCard);
            }
        }

        public void UpdateTblControllerCard(tblControllerCard currenttblControllerCard)
        {
            this.ObjectContext.tblControllerCard.AttachAsModified(currenttblControllerCard, this.ChangeSet.GetOriginal(currenttblControllerCard));
        }

        public void DeleteTblControllerCard(tblControllerCard tblControllerCard)
        {
            if ((tblControllerCard.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblControllerCard, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.tblControllerCard.Attach(tblControllerCard);
                this.ObjectContext.tblControllerCard.DeleteObject(tblControllerCard);
            }
        }

        // TODO:
        // 考慮限制查詢方法的結果。如果需要其他輸入，可以將
        // 參數加入至這個中繼資料，或建立其他不同名稱的其他查詢方法。
        // 為支援分頁，您必須將排序加入至 'tblControllerConfig' 查詢。
        public IQueryable<tblControllerConfig> GetTblControllerConfig()
        {
            return this.ObjectContext.tblControllerConfig;
        }

        public void InsertTblControllerConfig(tblControllerConfig tblControllerConfig)
        {
            if ((tblControllerConfig.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblControllerConfig, EntityState.Added);
            }
            else
            {
                this.ObjectContext.tblControllerConfig.AddObject(tblControllerConfig);
            }
        }

        public void UpdateTblControllerConfig(tblControllerConfig currenttblControllerConfig)
        {
            this.ObjectContext.tblControllerConfig.AttachAsModified(currenttblControllerConfig, this.ChangeSet.GetOriginal(currenttblControllerConfig));
        }

        public void DeleteTblControllerConfig(tblControllerConfig tblControllerConfig)
        {
            if ((tblControllerConfig.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblControllerConfig, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.tblControllerConfig.Attach(tblControllerConfig);
                this.ObjectContext.tblControllerConfig.DeleteObject(tblControllerConfig);
            }
        }

        // TODO:
        // 考慮限制查詢方法的結果。如果需要其他輸入，可以將
        // 參數加入至這個中繼資料，或建立其他不同名稱的其他查詢方法。
        // 為支援分頁，您必須將排序加入至 'tblControlSetting' 查詢。
        public IQueryable<tblControlSetting> GetTblControlSetting()
        {
            return this.ObjectContext.tblControlSetting;
        }

        public void InsertTblControlSetting(tblControlSetting tblControlSetting)
        {
            if ((tblControlSetting.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblControlSetting, EntityState.Added);
            }
            else
            {
                this.ObjectContext.tblControlSetting.AddObject(tblControlSetting);
            }
        }

        public void UpdateTblControlSetting(tblControlSetting currenttblControlSetting)
        {
            this.ObjectContext.tblControlSetting.AttachAsModified(currenttblControlSetting, this.ChangeSet.GetOriginal(currenttblControlSetting));
        }

        public void DeleteTblControlSetting(tblControlSetting tblControlSetting)
        {
            if ((tblControlSetting.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblControlSetting, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.tblControlSetting.Attach(tblControlSetting);
                this.ObjectContext.tblControlSetting.DeleteObject(tblControlSetting);
            }
        }

        // TODO:
        // 考慮限制查詢方法的結果。如果需要其他輸入，可以將
        // 參數加入至這個中繼資料，或建立其他不同名稱的其他查詢方法。
        // 為支援分頁，您必須將排序加入至 'tblDeviceStateLog' 查詢。
        public IQueryable<tblDeviceStateLog> GetTblDeviceStateLog()
        {
            return this.ObjectContext.tblDeviceStateLog;
        }

        public void InsertTblDeviceStateLog(tblDeviceStateLog tblDeviceStateLog)
        {
            if ((tblDeviceStateLog.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblDeviceStateLog, EntityState.Added);
            }
            else
            {
                this.ObjectContext.tblDeviceStateLog.AddObject(tblDeviceStateLog);
            }
        }

        public void UpdateTblDeviceStateLog(tblDeviceStateLog currenttblDeviceStateLog)
        {
            this.ObjectContext.tblDeviceStateLog.AttachAsModified(currenttblDeviceStateLog, this.ChangeSet.GetOriginal(currenttblDeviceStateLog));
        }

        public void DeleteTblDeviceStateLog(tblDeviceStateLog tblDeviceStateLog)
        {
            if ((tblDeviceStateLog.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblDeviceStateLog, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.tblDeviceStateLog.Attach(tblDeviceStateLog);
                this.ObjectContext.tblDeviceStateLog.DeleteObject(tblDeviceStateLog);
            }
        }

        // TODO:
        // 考慮限制查詢方法的結果。如果需要其他輸入，可以將
        // 參數加入至這個中繼資料，或建立其他不同名稱的其他查詢方法。
        // 為支援分頁，您必須將排序加入至 'tblEngineRoomConfig' 查詢。
        public IQueryable<tblEngineRoomConfig> GetTblEngineRoomConfig()
        {
            return this.ObjectContext.tblEngineRoomConfig.Include("tblControllerConfig").Include("tblEntranceGuardConfig") ;
        }

        public void InsertTblEngineRoomConfig(tblEngineRoomConfig tblEngineRoomConfig)
        {
            if ((tblEngineRoomConfig.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblEngineRoomConfig, EntityState.Added);
            }
            else
            {
                this.ObjectContext.tblEngineRoomConfig.AddObject(tblEngineRoomConfig);
            }
        }

        public void UpdateTblEngineRoomConfig(tblEngineRoomConfig currenttblEngineRoomConfig)
        {
            this.ObjectContext.tblEngineRoomConfig.AttachAsModified(currenttblEngineRoomConfig, this.ChangeSet.GetOriginal(currenttblEngineRoomConfig));
        }

        public void DeleteTblEngineRoomConfig(tblEngineRoomConfig tblEngineRoomConfig)
        {
            if ((tblEngineRoomConfig.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblEngineRoomConfig, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.tblEngineRoomConfig.Attach(tblEngineRoomConfig);
                this.ObjectContext.tblEngineRoomConfig.DeleteObject(tblEngineRoomConfig);
            }
        }

        // TODO:
        // 考慮限制查詢方法的結果。如果需要其他輸入，可以將
        // 參數加入至這個中繼資料，或建立其他不同名稱的其他查詢方法。
        // 為支援分頁，您必須將排序加入至 'tblEngineRoomLog' 查詢。
        public IQueryable<tblEngineRoomLog> GetTblEngineRoomLog()
        {
            return this.ObjectContext.tblEngineRoomLog;
        }

        public void InsertTblEngineRoomLog(tblEngineRoomLog tblEngineRoomLog)
        {
            if ((tblEngineRoomLog.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblEngineRoomLog, EntityState.Added);
            }
            else
            {
                this.ObjectContext.tblEngineRoomLog.AddObject(tblEngineRoomLog);
            }
        }

        public void UpdateTblEngineRoomLog(tblEngineRoomLog currenttblEngineRoomLog)
        {
            this.ObjectContext.tblEngineRoomLog.AttachAsModified(currenttblEngineRoomLog, this.ChangeSet.GetOriginal(currenttblEngineRoomLog));
        }

        public void DeleteTblEngineRoomLog(tblEngineRoomLog tblEngineRoomLog)
        {
            if ((tblEngineRoomLog.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblEngineRoomLog, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.tblEngineRoomLog.Attach(tblEngineRoomLog);
                this.ObjectContext.tblEngineRoomLog.DeleteObject(tblEngineRoomLog);
            }
        }

        // TODO:
        // 考慮限制查詢方法的結果。如果需要其他輸入，可以將
        // 參數加入至這個中繼資料，或建立其他不同名稱的其他查詢方法。
        // 為支援分頁，您必須將排序加入至 'tblEntranceGuardConfig' 查詢。
        public IQueryable<tblEntranceGuardConfig> GetTblEntranceGuardConfig()
        {
            return this.ObjectContext.tblEntranceGuardConfig;
        }

        public void InsertTblEntranceGuardConfig(tblEntranceGuardConfig tblEntranceGuardConfig)
        {
            if ((tblEntranceGuardConfig.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblEntranceGuardConfig, EntityState.Added);
            }
            else
            {
                this.ObjectContext.tblEntranceGuardConfig.AddObject(tblEntranceGuardConfig);
            }
        }

        public void UpdateTblEntranceGuardConfig(tblEntranceGuardConfig currenttblEntranceGuardConfig)
        {
            this.ObjectContext.tblEntranceGuardConfig.AttachAsModified(currenttblEntranceGuardConfig, this.ChangeSet.GetOriginal(currenttblEntranceGuardConfig));
        }

        public void DeleteTblEntranceGuardConfig(tblEntranceGuardConfig tblEntranceGuardConfig)
        {
            if ((tblEntranceGuardConfig.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblEntranceGuardConfig, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.tblEntranceGuardConfig.Attach(tblEntranceGuardConfig);
                this.ObjectContext.tblEntranceGuardConfig.DeleteObject(tblEntranceGuardConfig);
            }
        }

        // TODO:
        // 考慮限制查詢方法的結果。如果需要其他輸入，可以將
        // 參數加入至這個中繼資料，或建立其他不同名稱的其他查詢方法。
        // 為支援分頁，您必須將排序加入至 'tblERDoorPassword' 查詢。
        public IQueryable<tblERDoorPassword> GetTblERDoorPassword()
        {
            return this.ObjectContext.tblERDoorPassword;
        }

        public void InsertTblERDoorPassword(tblERDoorPassword tblERDoorPassword)
        {
            if ((tblERDoorPassword.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblERDoorPassword, EntityState.Added);
            }
            else
            {
                this.ObjectContext.tblERDoorPassword.AddObject(tblERDoorPassword);
            }
        }

        public void UpdateTblERDoorPassword(tblERDoorPassword currenttblERDoorPassword)
        {
            this.ObjectContext.tblERDoorPassword.AttachAsModified(currenttblERDoorPassword, this.ChangeSet.GetOriginal(currenttblERDoorPassword));
        }

        public void DeleteTblERDoorPassword(tblERDoorPassword tblERDoorPassword)
        {
            if ((tblERDoorPassword.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblERDoorPassword, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.tblERDoorPassword.Attach(tblERDoorPassword);
                this.ObjectContext.tblERDoorPassword.DeleteObject(tblERDoorPassword);
            }
        }

        // TODO:
        // 考慮限制查詢方法的結果。如果需要其他輸入，可以將
        // 參數加入至這個中繼資料，或建立其他不同名稱的其他查詢方法。
        // 為支援分頁，您必須將排序加入至 'tblERPlane' 查詢。
        public IQueryable<tblERPlane> GetTblERPlane()
        {
            return this.ObjectContext.tblERPlane;
        }

        public void InsertTblERPlane(tblERPlane tblERPlane)
        {
            if ((tblERPlane.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblERPlane, EntityState.Added);
            }
            else
            {
                this.ObjectContext.tblERPlane.AddObject(tblERPlane);
            }
        }

        public void UpdateTblERPlane(tblERPlane currenttblERPlane)
        {
            this.ObjectContext.tblERPlane.AttachAsModified(currenttblERPlane, this.ChangeSet.GetOriginal(currenttblERPlane));
        }

        public void DeleteTblERPlane(tblERPlane tblERPlane)
        {
            if ((tblERPlane.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblERPlane, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.tblERPlane.Attach(tblERPlane);
                this.ObjectContext.tblERPlane.DeleteObject(tblERPlane);
            }
        }

        // TODO:
        // 考慮限制查詢方法的結果。如果需要其他輸入，可以將
        // 參數加入至這個中繼資料，或建立其他不同名稱的其他查詢方法。
        // 為支援分頁，您必須將排序加入至 'tblHostConfig' 查詢。
        public IQueryable<tblHostConfig> GetTblHostConfig()
        {
            return this.ObjectContext.tblHostConfig;
        }

        public void InsertTblHostConfig(tblHostConfig tblHostConfig)
        {
            if ((tblHostConfig.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblHostConfig, EntityState.Added);
            }
            else
            {
                this.ObjectContext.tblHostConfig.AddObject(tblHostConfig);
            }
        }

        public void UpdateTblHostConfig(tblHostConfig currenttblHostConfig)
        {
            this.ObjectContext.tblHostConfig.AttachAsModified(currenttblHostConfig, this.ChangeSet.GetOriginal(currenttblHostConfig));
        }

        public void DeleteTblHostConfig(tblHostConfig tblHostConfig)
        {
            if ((tblHostConfig.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblHostConfig, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.tblHostConfig.Attach(tblHostConfig);
                this.ObjectContext.tblHostConfig.DeleteObject(tblHostConfig);
            }
        }

         //TODO:
         //考慮限制查詢方法的結果。如果需要其他輸入，可以將
         //參數加入至這個中繼資料，或建立其他不同名稱的其他查詢方法。
         //為支援分頁，您必須將排序加入至 'tblItemConfig' 查詢。
        public IQueryable<tblItemConfig> GetTblItemConfig()
        {
            return this.ObjectContext.tblItemConfig;
        }

        public void InsertTblItemConfig(tblItemConfig tblItemConfig)
        {
            if ((tblItemConfig.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblItemConfig, EntityState.Added);
            }
            else
            {
                this.ObjectContext.tblItemConfig.AddObject(tblItemConfig);
            }
        }

        public void UpdateTblItemConfig(tblItemConfig currenttblItemConfig)
        {
            this.ObjectContext.tblItemConfig.AttachAsModified(currenttblItemConfig, this.ChangeSet.GetOriginal(currenttblItemConfig));
        }

        public void DeleteTblItemConfig(tblItemConfig tblItemConfig)
        {
            if ((tblItemConfig.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblItemConfig, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.tblItemConfig.Attach(tblItemConfig);
                this.ObjectContext.tblItemConfig.DeleteObject(tblItemConfig);
            }
        }

        // TODO:
        // 考慮限制查詢方法的結果。如果需要其他輸入，可以將
        // 參數加入至這個中繼資料，或建立其他不同名稱的其他查詢方法。
        // 為支援分頁，您必須將排序加入至 'tblMagneticCard' 查詢。
        public IQueryable<tblMagneticCard> GetTblMagneticCard()
        {
            return this.ObjectContext.tblMagneticCard;
        }

        public void InsertTblMagneticCard(tblMagneticCard tblMagneticCard)
        {
            if ((tblMagneticCard.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblMagneticCard, EntityState.Added);
            }
            else
            {
                this.ObjectContext.tblMagneticCard.AddObject(tblMagneticCard);
            }
        }

        public void UpdateTblMagneticCard(tblMagneticCard currenttblMagneticCard)
        {
            this.ObjectContext.tblMagneticCard.AttachAsModified(currenttblMagneticCard, this.ChangeSet.GetOriginal(currenttblMagneticCard));
        }

        public void DeleteTblMagneticCard(tblMagneticCard tblMagneticCard)
        {
            if ((tblMagneticCard.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblMagneticCard, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.tblMagneticCard.Attach(tblMagneticCard);
                this.ObjectContext.tblMagneticCard.DeleteObject(tblMagneticCard);
            }
        }

        // TODO:
        // 考慮限制查詢方法的結果。如果需要其他輸入，可以將
        // 參數加入至這個中繼資料，或建立其他不同名稱的其他查詢方法。
        // 為支援分頁，您必須將排序加入至 'tblMagneticCardNormalGroup' 查詢。
        public IQueryable<tblMagneticCardNormalGroup> GetTblMagneticCardNormalGroup()
        {
            return this.ObjectContext.tblMagneticCardNormalGroup;
        }

        public void InsertTblMagneticCardNormalGroup(tblMagneticCardNormalGroup tblMagneticCardNormalGroup)
        {
            if ((tblMagneticCardNormalGroup.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblMagneticCardNormalGroup, EntityState.Added);
            }
            else
            {
                this.ObjectContext.tblMagneticCardNormalGroup.AddObject(tblMagneticCardNormalGroup);
            }
        }

        public void UpdateTblMagneticCardNormalGroup(tblMagneticCardNormalGroup currenttblMagneticCardNormalGroup)
        {
            this.ObjectContext.tblMagneticCardNormalGroup.AttachAsModified(currenttblMagneticCardNormalGroup, this.ChangeSet.GetOriginal(currenttblMagneticCardNormalGroup));
        }

        public void DeleteTblMagneticCardNormalGroup(tblMagneticCardNormalGroup tblMagneticCardNormalGroup)
        {
            if ((tblMagneticCardNormalGroup.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblMagneticCardNormalGroup, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.tblMagneticCardNormalGroup.Attach(tblMagneticCardNormalGroup);
                this.ObjectContext.tblMagneticCardNormalGroup.DeleteObject(tblMagneticCardNormalGroup);
            }
        }

        // TODO:
        // 考慮限制查詢方法的結果。如果需要其他輸入，可以將
        // 參數加入至這個中繼資料，或建立其他不同名稱的其他查詢方法。
        // 為支援分頁，您必須將排序加入至 'tblMenu' 查詢。
        public IQueryable<tblMenu> GetTblMenu()
        {
            return this.ObjectContext.tblMenu;
        }

        public void InsertTblMenu(tblMenu tblMenu)
        {
            if ((tblMenu.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblMenu, EntityState.Added);
            }
            else
            {
                this.ObjectContext.tblMenu.AddObject(tblMenu);
            }
        }

        public void UpdateTblMenu(tblMenu currenttblMenu)
        {
            this.ObjectContext.tblMenu.AttachAsModified(currenttblMenu, this.ChangeSet.GetOriginal(currenttblMenu));
        }

        public void DeleteTblMenu(tblMenu tblMenu)
        {
            if ((tblMenu.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblMenu, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.tblMenu.Attach(tblMenu);
                this.ObjectContext.tblMenu.DeleteObject(tblMenu);
            }
        }

        // TODO:
        // 考慮限制查詢方法的結果。如果需要其他輸入，可以將
        // 參數加入至這個中繼資料，或建立其他不同名稱的其他查詢方法。
        // 為支援分頁，您必須將排序加入至 'tblMenuGroup' 查詢。
        public IQueryable<tblMenuGroup> GetTblMenuGroup()
        {
            return this.ObjectContext.tblMenuGroup;
        }

        public void InsertTblMenuGroup(tblMenuGroup tblMenuGroup)
        {
            if ((tblMenuGroup.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblMenuGroup, EntityState.Added);
            }
            else
            {
                this.ObjectContext.tblMenuGroup.AddObject(tblMenuGroup);
            }
        }

        public void UpdateTblMenuGroup(tblMenuGroup currenttblMenuGroup)
        {
            this.ObjectContext.tblMenuGroup.AttachAsModified(currenttblMenuGroup, this.ChangeSet.GetOriginal(currenttblMenuGroup));
        }

        public void DeleteTblMenuGroup(tblMenuGroup tblMenuGroup)
        {
            if ((tblMenuGroup.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblMenuGroup, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.tblMenuGroup.Attach(tblMenuGroup);
                this.ObjectContext.tblMenuGroup.DeleteObject(tblMenuGroup);
            }
        }

        // TODO:
        // 考慮限制查詢方法的結果。如果需要其他輸入，可以將
        // 參數加入至這個中繼資料，或建立其他不同名稱的其他查詢方法。
        // 為支援分頁，您必須將排序加入至 'tblSchConfig' 查詢。
        public IQueryable<tblSchConfig> GetTblSchConfig()
        {
            return this.ObjectContext.tblSchConfig;
        }

        public void InsertTblSchConfig(tblSchConfig tblSchConfig)
        {
            if ((tblSchConfig.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblSchConfig, EntityState.Added);
            }
            else
            {
                this.ObjectContext.tblSchConfig.AddObject(tblSchConfig);
            }
        }

        public void UpdateTblSchConfig(tblSchConfig currenttblSchConfig)
        {
            this.ObjectContext.tblSchConfig.AttachAsModified(currenttblSchConfig, this.ChangeSet.GetOriginal(currenttblSchConfig));
        }

        public void DeleteTblSchConfig(tblSchConfig tblSchConfig)
        {
            if ((tblSchConfig.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblSchConfig, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.tblSchConfig.Attach(tblSchConfig);
                this.ObjectContext.tblSchConfig.DeleteObject(tblSchConfig);
            }
        }

        // TODO:
        // 考慮限制查詢方法的結果。如果需要其他輸入，可以將
        // 參數加入至這個中繼資料，或建立其他不同名稱的其他查詢方法。
        // 為支援分頁，您必須將排序加入至 'tblSchLog' 查詢。
        public IQueryable<tblSchLog> GetTblSchLog()
        {
            return this.ObjectContext.tblSchLog;
        }

        public void InsertTblSchLog(tblSchLog tblSchLog)
        {
            if ((tblSchLog.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblSchLog, EntityState.Added);
            }
            else
            {
                this.ObjectContext.tblSchLog.AddObject(tblSchLog);
            }
        }

        public void UpdateTblSchLog(tblSchLog currenttblSchLog)
        {
            this.ObjectContext.tblSchLog.AttachAsModified(currenttblSchLog, this.ChangeSet.GetOriginal(currenttblSchLog));
        }

        public void DeleteTblSchLog(tblSchLog tblSchLog)
        {
            if ((tblSchLog.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblSchLog, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.tblSchLog.Attach(tblSchLog);
                this.ObjectContext.tblSchLog.DeleteObject(tblSchLog);
            }
        }

        // TODO:
        // 考慮限制查詢方法的結果。如果需要其他輸入，可以將
        // 參數加入至這個中繼資料，或建立其他不同名稱的其他查詢方法。
        // 為支援分頁，您必須將排序加入至 'tblSingalIO' 查詢。
        public IQueryable<tblSingalIO> GetTblSingalIO()
        {
            return this.ObjectContext.tblSingalIO;
        }

        public void InsertTblSingalIO(tblSingalIO tblSingalIO)
        {
            if ((tblSingalIO.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblSingalIO, EntityState.Added);
            }
            else
            {
                this.ObjectContext.tblSingalIO.AddObject(tblSingalIO);
            }
        }

        public void UpdateTblSingalIO(tblSingalIO currenttblSingalIO)
        {
            this.ObjectContext.tblSingalIO.AttachAsModified(currenttblSingalIO, this.ChangeSet.GetOriginal(currenttblSingalIO));
        }

        public void DeleteTblSingalIO(tblSingalIO tblSingalIO)
        {
            if ((tblSingalIO.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblSingalIO, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.tblSingalIO.Attach(tblSingalIO);
                this.ObjectContext.tblSingalIO.DeleteObject(tblSingalIO);
            }
        }

        // TODO:
        // 考慮限制查詢方法的結果。如果需要其他輸入，可以將
        // 參數加入至這個中繼資料，或建立其他不同名稱的其他查詢方法。
        // 為支援分頁，您必須將排序加入至 'tblSingalIOLog' 查詢。
        public IQueryable<tblSingalIOLog> GetTblSingalIOLog()
        {
            return this.ObjectContext.tblSingalIOLog;
        }

        public void InsertTblSingalIOLog(tblSingalIOLog tblSingalIOLog)
        {
            if ((tblSingalIOLog.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblSingalIOLog, EntityState.Added);
            }
            else
            {
                this.ObjectContext.tblSingalIOLog.AddObject(tblSingalIOLog);
            }
        }

        public void UpdateTblSingalIOLog(tblSingalIOLog currenttblSingalIOLog)
        {
            this.ObjectContext.tblSingalIOLog.AttachAsModified(currenttblSingalIOLog, this.ChangeSet.GetOriginal(currenttblSingalIOLog));
        }

        public void DeleteTblSingalIOLog(tblSingalIOLog tblSingalIOLog)
        {
            if ((tblSingalIOLog.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblSingalIOLog, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.tblSingalIOLog.Attach(tblSingalIOLog);
                this.ObjectContext.tblSingalIOLog.DeleteObject(tblSingalIOLog);
            }
        }

        // TODO:
        // 考慮限制查詢方法的結果。如果需要其他輸入，可以將
        // 參數加入至這個中繼資料，或建立其他不同名稱的其他查詢方法。
        // 為支援分頁，您必須將排序加入至 'tblSysOperation' 查詢。
        public IQueryable<tblSysOperation> GetTblSysOperation()
        {
            return this.ObjectContext.tblSysOperation;
        }

        public void InsertTblSysOperation(tblSysOperation tblSysOperation)
        {
            if ((tblSysOperation.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblSysOperation, EntityState.Added);
            }
            else
            {
                this.ObjectContext.tblSysOperation.AddObject(tblSysOperation);
            }
        }

        public void UpdateTblSysOperation(tblSysOperation currenttblSysOperation)
        {
            this.ObjectContext.tblSysOperation.AttachAsModified(currenttblSysOperation, this.ChangeSet.GetOriginal(currenttblSysOperation));
        }

        public void DeleteTblSysOperation(tblSysOperation tblSysOperation)
        {
            if ((tblSysOperation.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblSysOperation, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.tblSysOperation.Attach(tblSysOperation);
                this.ObjectContext.tblSysOperation.DeleteObject(tblSysOperation);
            }
        }

        // TODO:
        // 考慮限制查詢方法的結果。如果需要其他輸入，可以將
        // 參數加入至這個中繼資料，或建立其他不同名稱的其他查詢方法。
        // 為支援分頁，您必須將排序加入至 'tblSysParameter' 查詢。
        public IQueryable<tblSysParameter> GetTblSysParameter()
        {
            return this.ObjectContext.tblSysParameter;
        }

        public void InsertTblSysParameter(tblSysParameter tblSysParameter)
        {
            if ((tblSysParameter.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblSysParameter, EntityState.Added);
            }
            else
            {
                this.ObjectContext.tblSysParameter.AddObject(tblSysParameter);
            }
        }

        public void UpdateTblSysParameter(tblSysParameter currenttblSysParameter)
        {
            this.ObjectContext.tblSysParameter.AttachAsModified(currenttblSysParameter, this.ChangeSet.GetOriginal(currenttblSysParameter));
        }

        public void DeleteTblSysParameter(tblSysParameter tblSysParameter)
        {
            if ((tblSysParameter.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblSysParameter, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.tblSysParameter.Attach(tblSysParameter);
                this.ObjectContext.tblSysParameter.DeleteObject(tblSysParameter);
            }
        }

        // TODO:
        // 考慮限制查詢方法的結果。如果需要其他輸入，可以將
        // 參數加入至這個中繼資料，或建立其他不同名稱的其他查詢方法。
        // 為支援分頁，您必須將排序加入至 'tblSysRole' 查詢。
        public IQueryable<tblSysRole> GetTblSysRole()
        {
            return this.ObjectContext.tblSysRole.Include("tblMagneticCard");
        }

        public void InsertTblSysRole(tblSysRole tblSysRole)
        {
            if ((tblSysRole.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblSysRole, EntityState.Added);
            }
            else
            {
                this.ObjectContext.tblSysRole.AddObject(tblSysRole);
            }
        }

        public void UpdateTblSysRole(tblSysRole currenttblSysRole)
        {
            this.ObjectContext.tblSysRole.AttachAsModified(currenttblSysRole, this.ChangeSet.GetOriginal(currenttblSysRole));
        }

        public void DeleteTblSysRole(tblSysRole tblSysRole)
        {
            if ((tblSysRole.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblSysRole, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.tblSysRole.Attach(tblSysRole);
                this.ObjectContext.tblSysRole.DeleteObject(tblSysRole);
            }
        }

        // TODO:
        // 考慮限制查詢方法的結果。如果需要其他輸入，可以將
        // 參數加入至這個中繼資料，或建立其他不同名稱的其他查詢方法。
        // 為支援分頁，您必須將排序加入至 'tblSysRoleAuthority' 查詢。
        public IQueryable<tblSysRoleAuthority> GetTblSysRoleAuthority()
        {
            return this.ObjectContext.tblSysRoleAuthority;
        }

        public void InsertTblSysRoleAuthority(tblSysRoleAuthority tblSysRoleAuthority)
        {
            if ((tblSysRoleAuthority.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblSysRoleAuthority, EntityState.Added);
            }
            else
            {
                this.ObjectContext.tblSysRoleAuthority.AddObject(tblSysRoleAuthority);
            }
        }

        public void UpdateTblSysRoleAuthority(tblSysRoleAuthority currenttblSysRoleAuthority)
        {
            this.ObjectContext.tblSysRoleAuthority.AttachAsModified(currenttblSysRoleAuthority, this.ChangeSet.GetOriginal(currenttblSysRoleAuthority));
        }

        public void DeleteTblSysRoleAuthority(tblSysRoleAuthority tblSysRoleAuthority)
        {
            if ((tblSysRoleAuthority.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblSysRoleAuthority, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.tblSysRoleAuthority.Attach(tblSysRoleAuthority);
                this.ObjectContext.tblSysRoleAuthority.DeleteObject(tblSysRoleAuthority);
            }
        }

        // TODO:
        // 考慮限制查詢方法的結果。如果需要其他輸入，可以將
        // 參數加入至這個中繼資料，或建立其他不同名稱的其他查詢方法。
        // 為支援分頁，您必須將排序加入至 'tblTypeDetail' 查詢。
        public IQueryable<tblTypeDetail> GetTblTypeDetail()
        {
            return this.ObjectContext.tblTypeDetail;
        }

        public void InsertTblTypeDetail(tblTypeDetail tblTypeDetail)
        {
            if ((tblTypeDetail.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblTypeDetail, EntityState.Added);
            }
            else
            {
                this.ObjectContext.tblTypeDetail.AddObject(tblTypeDetail);
            }
        }

        public void UpdateTblTypeDetail(tblTypeDetail currenttblTypeDetail)
        {
            this.ObjectContext.tblTypeDetail.AttachAsModified(currenttblTypeDetail, this.ChangeSet.GetOriginal(currenttblTypeDetail));
        }

        public void DeleteTblTypeDetail(tblTypeDetail tblTypeDetail)
        {
            if ((tblTypeDetail.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblTypeDetail, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.tblTypeDetail.Attach(tblTypeDetail);
                this.ObjectContext.tblTypeDetail.DeleteObject(tblTypeDetail);
            }
        }

        // TODO:
        // 考慮限制查詢方法的結果。如果需要其他輸入，可以將
        // 參數加入至這個中繼資料，或建立其他不同名稱的其他查詢方法。
        // 為支援分頁，您必須將排序加入至 'tblUser' 查詢。
        public IQueryable<tblUser> GetTblUser()
        {
            return this.ObjectContext.tblUser;
        }

        public void InsertTblUser(tblUser tblUser)
        {
            if ((tblUser.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblUser, EntityState.Added);
            }
            else
            {
                this.ObjectContext.tblUser.AddObject(tblUser);
            }
        }

        public void UpdateTblUser(tblUser currenttblUser)
        {
            this.ObjectContext.tblUser.AttachAsModified(currenttblUser, this.ChangeSet.GetOriginal(currenttblUser));
        }

        public void DeleteTblUser(tblUser tblUser)
        {
            if ((tblUser.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblUser, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.tblUser.Attach(tblUser);
                this.ObjectContext.tblUser.DeleteObject(tblUser);
            }
        }

        // TODO:
        // 考慮限制查詢方法的結果。如果需要其他輸入，可以將
        // 參數加入至這個中繼資料，或建立其他不同名稱的其他查詢方法。
        // 為支援分頁，您必須將排序加入至 'tblUserGroup' 查詢。
        public IQueryable<tblUserGroup> GetTblUserGroup()
        {
            return this.ObjectContext.tblUserGroup;
        }

        public void InsertTblUserGroup(tblUserGroup tblUserGroup)
        {
            if ((tblUserGroup.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblUserGroup, EntityState.Added);
            }
            else
            {
                this.ObjectContext.tblUserGroup.AddObject(tblUserGroup);
            }
        }

        public void UpdateTblUserGroup(tblUserGroup currenttblUserGroup)
        {
            this.ObjectContext.tblUserGroup.AttachAsModified(currenttblUserGroup, this.ChangeSet.GetOriginal(currenttblUserGroup));
        }

        public void DeleteTblUserGroup(tblUserGroup tblUserGroup)
        {
            if ((tblUserGroup.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblUserGroup, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.tblUserGroup.Attach(tblUserGroup);
                this.ObjectContext.tblUserGroup.DeleteObject(tblUserGroup);
            }
        }

        // TODO:
        // 考慮限制查詢方法的結果。如果需要其他輸入，可以將
        // 參數加入至這個中繼資料，或建立其他不同名稱的其他查詢方法。
        // 為支援分頁，您必須將排序加入至 'tblUserGroupMenu' 查詢。
        public IQueryable<tblUserGroupMenu> GetTblUserGroupMenu()
        {
            return this.ObjectContext.tblUserGroupMenu;
        }
        //錯誤	13	'slSecure.Web.SecureDBContext' 不包含 'GetTblUserGroupMenuIncludeMenuQuery' 的定義，也找不到擴充方法 'GetTblUserGroupMenuIncludeMenuQuery' 來接受類型 'slSecure.Web.SecureDBContext' 的第一個引數 (您是否遺漏 using 指示詞或組件參考?)	D:\backd\hwacom\機房門禁\slSecureAndPD\slSecure\Forms\UserGroupAuth.xaml.cs	47	65	slSecure

        
        public IQueryable<tblUserGroupMenu> GetTblUserGroupMenuIncludeMenu()
        {
            return this.ObjectContext.tblUserGroupMenu.Include("tblMenu");
        }

        public void InsertTblUserGroupMenu(tblUserGroupMenu tblUserGroupMenu)
        {
            if ((tblUserGroupMenu.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblUserGroupMenu, EntityState.Added);
            }
            else
            {
                this.ObjectContext.tblUserGroupMenu.AddObject(tblUserGroupMenu);
            }
        }

        public void UpdateTblUserGroupMenu(tblUserGroupMenu currenttblUserGroupMenu)
        {
            this.ObjectContext.tblUserGroupMenu.AttachAsModified(currenttblUserGroupMenu, this.ChangeSet.GetOriginal(currenttblUserGroupMenu));
        }

        public void DeleteTblUserGroupMenu(tblUserGroupMenu tblUserGroupMenu)
        {
            if ((tblUserGroupMenu.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(tblUserGroupMenu, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.tblUserGroupMenu.Attach(tblUserGroupMenu);
                this.ObjectContext.tblUserGroupMenu.DeleteObject(tblUserGroupMenu);
            }
        }

        // TODO:
        // 考慮限制查詢方法的結果。如果需要其他輸入，可以將
        // 參數加入至這個中繼資料，或建立其他不同名稱的其他查詢方法。
        // 為支援分頁，您必須將排序加入至 'vwUserGroupMenuAllow' 查詢。
        public IQueryable<vwUserGroupMenuAllow> GetVwUserGroupMenuAllow()
        {
            return this.ObjectContext.vwUserGroupMenuAllow;
        }

        public void InsertVwUserGroupMenuAllow(vwUserGroupMenuAllow vwUserGroupMenuAllow)
        {
            if ((vwUserGroupMenuAllow.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(vwUserGroupMenuAllow, EntityState.Added);
            }
            else
            {
                this.ObjectContext.vwUserGroupMenuAllow.AddObject(vwUserGroupMenuAllow);
            }
        }

        public void UpdateVwUserGroupMenuAllow(vwUserGroupMenuAllow currentvwUserGroupMenuAllow)
        {
            this.ObjectContext.vwUserGroupMenuAllow.AttachAsModified(currentvwUserGroupMenuAllow, this.ChangeSet.GetOriginal(currentvwUserGroupMenuAllow));
        }

        public void DeleteVwUserGroupMenuAllow(vwUserGroupMenuAllow vwUserGroupMenuAllow)
        {
            if ((vwUserGroupMenuAllow.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(vwUserGroupMenuAllow, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.vwUserGroupMenuAllow.Attach(vwUserGroupMenuAllow);
                this.ObjectContext.vwUserGroupMenuAllow.DeleteObject(vwUserGroupMenuAllow);
            }
        }

        // TODO:
        // 考慮限制查詢方法的結果。如果需要其他輸入，可以將
        // 參數加入至這個中繼資料，或建立其他不同名稱的其他查詢方法。
        // 為支援分頁，您必須將排序加入至 'vwUserMenuAllow' 查詢。
        public IQueryable<vwUserMenuAllow> GetVwUserMenuAllow()
        {
            return this.ObjectContext.vwUserMenuAllow;
        }

        public void InsertVwUserMenuAllow(vwUserMenuAllow vwUserMenuAllow)
        {
            if ((vwUserMenuAllow.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(vwUserMenuAllow, EntityState.Added);
            }
            else
            {
                this.ObjectContext.vwUserMenuAllow.AddObject(vwUserMenuAllow);
            }
        }

        public void UpdateVwUserMenuAllow(vwUserMenuAllow currentvwUserMenuAllow)
        {
            this.ObjectContext.vwUserMenuAllow.AttachAsModified(currentvwUserMenuAllow, this.ChangeSet.GetOriginal(currentvwUserMenuAllow));
        }

        public void DeleteVwUserMenuAllow(vwUserMenuAllow vwUserMenuAllow)
        {
            if ((vwUserMenuAllow.EntityState != EntityState.Detached))
            {
                this.ObjectContext.ObjectStateManager.ChangeObjectState(vwUserMenuAllow, EntityState.Deleted);
            }
            else
            {
                this.ObjectContext.vwUserMenuAllow.Attach(vwUserMenuAllow);
                this.ObjectContext.vwUserMenuAllow.DeleteObject(vwUserMenuAllow);
            }
        }
    }
}


