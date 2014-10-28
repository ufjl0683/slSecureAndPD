
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

        // TODO:
        // 考慮限制查詢方法的結果。如果需要其他輸入，可以將
        // 參數加入至這個中繼資料，或建立其他不同名稱的其他查詢方法。
        // 為支援分頁，您必須將排序加入至 'tblAlarmLog' 查詢。
        public IQueryable<tblAlarmLog> GetTblAlarmLog()
        {
            return this.ObjectContext.tblAlarmLog;
        }

        // TODO:
        // 考慮限制查詢方法的結果。如果需要其他輸入，可以將
        // 參數加入至這個中繼資料，或建立其他不同名稱的其他查詢方法。
        // 為支援分頁，您必須將排序加入至 'tblCardReaderConfig' 查詢。
        public IQueryable<tblCardReaderConfig> GetTblCardReaderConfig()
        {
            return this.ObjectContext.tblCardReaderConfig;
        }

        // TODO:
        // 考慮限制查詢方法的結果。如果需要其他輸入，可以將
        // 參數加入至這個中繼資料，或建立其他不同名稱的其他查詢方法。
        // 為支援分頁，您必須將排序加入至 'tblCCTVConfig' 查詢。
        public IQueryable<tblCCTVConfig> GetTblCCTVConfig()
        {
            return this.ObjectContext.tblCCTVConfig;
        }

        // TODO:
        // 考慮限制查詢方法的結果。如果需要其他輸入，可以將
        // 參數加入至這個中繼資料，或建立其他不同名稱的其他查詢方法。
        // 為支援分頁，您必須將排序加入至 'tblControllerCard' 查詢。
        public IQueryable<tblControllerCard> GetTblControllerCard()
        {
            return this.ObjectContext.tblControllerCard;
        }

        // TODO:
        // 考慮限制查詢方法的結果。如果需要其他輸入，可以將
        // 參數加入至這個中繼資料，或建立其他不同名稱的其他查詢方法。
        // 為支援分頁，您必須將排序加入至 'tblControllerConfig' 查詢。
        public IQueryable<tblControllerConfig> GetTblControllerConfig()
        {
            return this.ObjectContext.tblControllerConfig;
        }

        // TODO:
        // 考慮限制查詢方法的結果。如果需要其他輸入，可以將
        // 參數加入至這個中繼資料，或建立其他不同名稱的其他查詢方法。
        // 為支援分頁，您必須將排序加入至 'tblControlSetting' 查詢。
        public IQueryable<tblControlSetting> GetTblControlSetting()
        {
            return this.ObjectContext.tblControlSetting;
        }

        // TODO:
        // 考慮限制查詢方法的結果。如果需要其他輸入，可以將
        // 參數加入至這個中繼資料，或建立其他不同名稱的其他查詢方法。
        // 為支援分頁，您必須將排序加入至 'tblDeviceStateLog' 查詢。
        public IQueryable<tblDeviceStateLog> GetTblDeviceStateLog()
        {
            return this.ObjectContext.tblDeviceStateLog;
        }

        // TODO:
        // 考慮限制查詢方法的結果。如果需要其他輸入，可以將
        // 參數加入至這個中繼資料，或建立其他不同名稱的其他查詢方法。
        // 為支援分頁，您必須將排序加入至 'tblEngineRoomConfig' 查詢。
        public IQueryable<tblEngineRoomConfig> GetTblEngineRoomConfig()
        {
            return this.ObjectContext.tblEngineRoomConfig;
        }

        // TODO:
        // 考慮限制查詢方法的結果。如果需要其他輸入，可以將
        // 參數加入至這個中繼資料，或建立其他不同名稱的其他查詢方法。
        // 為支援分頁，您必須將排序加入至 'tblEngineRoomLog' 查詢。
        public IQueryable<tblEngineRoomLog> GetTblEngineRoomLog()
        {
            return this.ObjectContext.tblEngineRoomLog;
        }

        // TODO:
        // 考慮限制查詢方法的結果。如果需要其他輸入，可以將
        // 參數加入至這個中繼資料，或建立其他不同名稱的其他查詢方法。
        // 為支援分頁，您必須將排序加入至 'tblEntranceGuardConfig' 查詢。
        public IQueryable<tblEntranceGuardConfig> GetTblEntranceGuardConfig()
        {
            return this.ObjectContext.tblEntranceGuardConfig;
        }

        // TODO:
        // 考慮限制查詢方法的結果。如果需要其他輸入，可以將
        // 參數加入至這個中繼資料，或建立其他不同名稱的其他查詢方法。
        // 為支援分頁，您必須將排序加入至 'tblERDoorPasscode' 查詢。
        public IQueryable<tblERDoorPasscode> GetTblERDoorPasscode()
        {
            return this.ObjectContext.tblERDoorPasscode;
        }

        // TODO:
        // 考慮限制查詢方法的結果。如果需要其他輸入，可以將
        // 參數加入至這個中繼資料，或建立其他不同名稱的其他查詢方法。
        // 為支援分頁，您必須將排序加入至 'tblERDoorPassword' 查詢。
        public IQueryable<tblERDoorPassword> GetTblERDoorPassword()
        {
            return this.ObjectContext.tblERDoorPassword;
        }

        // TODO:
        // 考慮限制查詢方法的結果。如果需要其他輸入，可以將
        // 參數加入至這個中繼資料，或建立其他不同名稱的其他查詢方法。
        // 為支援分頁，您必須將排序加入至 'tblERPlane' 查詢。
        public IQueryable<tblERPlane> GetTblERPlane()
        {
            return this.ObjectContext.tblERPlane;
        }

        // TODO:
        // 考慮限制查詢方法的結果。如果需要其他輸入，可以將
        // 參數加入至這個中繼資料，或建立其他不同名稱的其他查詢方法。
        // 為支援分頁，您必須將排序加入至 'tblHostConfig' 查詢。
        public IQueryable<tblHostConfig> GetTblHostConfig()
        {
            return this.ObjectContext.tblHostConfig;
        }

        // TODO:
        // 考慮限制查詢方法的結果。如果需要其他輸入，可以將
        // 參數加入至這個中繼資料，或建立其他不同名稱的其他查詢方法。
        // 為支援分頁，您必須將排序加入至 'tblItemConfig' 查詢。
        public IQueryable<tblItemConfig> GetTblItemConfig()
        {
            return this.ObjectContext.tblItemConfig;
        }

        // TODO:
        // 考慮限制查詢方法的結果。如果需要其他輸入，可以將
        // 參數加入至這個中繼資料，或建立其他不同名稱的其他查詢方法。
        // 為支援分頁，您必須將排序加入至 'tblMagneticCard' 查詢。
        public IQueryable<tblMagneticCard> GetTblMagneticCard()
        {
            return this.ObjectContext.tblMagneticCard;
        }

        // TODO:
        // 考慮限制查詢方法的結果。如果需要其他輸入，可以將
        // 參數加入至這個中繼資料，或建立其他不同名稱的其他查詢方法。
        // 為支援分頁，您必須將排序加入至 'tblMagneticCardNormalGroup' 查詢。
        public IQueryable<tblMagneticCardNormalGroup> GetTblMagneticCardNormalGroup()
        {
            return this.ObjectContext.tblMagneticCardNormalGroup;
        }

        // TODO:
        // 考慮限制查詢方法的結果。如果需要其他輸入，可以將
        // 參數加入至這個中繼資料，或建立其他不同名稱的其他查詢方法。
        // 為支援分頁，您必須將排序加入至 'tblMenu' 查詢。
        public IQueryable<tblMenu> GetTblMenu()
        {
            return this.ObjectContext.tblMenu;
        }

        // TODO:
        // 考慮限制查詢方法的結果。如果需要其他輸入，可以將
        // 參數加入至這個中繼資料，或建立其他不同名稱的其他查詢方法。
        // 為支援分頁，您必須將排序加入至 'tblMenuGroup' 查詢。
        public IQueryable<tblMenuGroup> GetTblMenuGroup()
        {
            return this.ObjectContext.tblMenuGroup;
        }

       

        // TODO:
        // 考慮限制查詢方法的結果。如果需要其他輸入，可以將
        // 參數加入至這個中繼資料，或建立其他不同名稱的其他查詢方法。
        // 為支援分頁，您必須將排序加入至 'tblSchConfig' 查詢。
        public IQueryable<tblSchConfig> GetTblSchConfig()
        {
            return this.ObjectContext.tblSchConfig;
        }

        // TODO:
        // 考慮限制查詢方法的結果。如果需要其他輸入，可以將
        // 參數加入至這個中繼資料，或建立其他不同名稱的其他查詢方法。
        // 為支援分頁，您必須將排序加入至 'tblSchLog' 查詢。
        public IQueryable<tblSchLog> GetTblSchLog()
        {
            return this.ObjectContext.tblSchLog;
        }

        // TODO:
        // 考慮限制查詢方法的結果。如果需要其他輸入，可以將
        // 參數加入至這個中繼資料，或建立其他不同名稱的其他查詢方法。
        // 為支援分頁，您必須將排序加入至 'tblSingalIO' 查詢。
        public IQueryable<tblSingalIO> GetTblSingalIO()
        {
            return this.ObjectContext.tblSingalIO;
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
            return this.ObjectContext.tblSysRole;
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


