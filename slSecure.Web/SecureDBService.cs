
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


    // 使用 SecureDB 內容實作應用程式邏輯。
    // TODO: 將應用程式邏輯加入至這些方法或其他方法。
    // TODO: 連接驗證 (Windows/ASP.NET Forms) 並取消下面的註解，以停用匿名存取
    // 視需要也考慮加入要限制存取的角色。
    // [RequiresAuthentication]
    [EnableClientAccess()]
    public class SecureDBService : LinqToEntitiesDomainService<SecureDB>
    {

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
            return this.ObjectContext.tblUserGroupMenu.Include("tblMenu").Include("tblUserGroup");
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


        public IQueryable<vwUserMenuAllow> GetVwUserMenuAllow()
        {
            return this.ObjectContext.vwUserMenuAllow;
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
    }
}


