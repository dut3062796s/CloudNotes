﻿//  =======================================================================================================
// 
//     ,uEZGZX  LG                             Eu       iJ       vi                                              
//    BB7.  .:  uM                             8F       0BN      Bq             S:                               
//   @X         LO    rJLYi    :     i    iYLL XJ       Xu7@     Mu    7LjL;   rBOii   7LJ7    .vYUi             
//  ,@          LG  7Br...SB  vB     B   B1...7BL       0S i@,   OU  :@7. ,u@   @u.. :@:  ;B  LB. ::             
//  v@          LO  B      Z0 i@     @  BU     @Y       qq  .@L  Oj  @      5@  Oi   @.    MB U@                 
//  .@          JZ :@      :@ rB     B  @      5U       Eq    @0 Xj ,B      .B  Br  ,B:rv777i  :0ZU              
//   @M         LO  @      Mk :@    .@  BL     @J       EZ     GZML  @      XM  B;   @            Y@             
//    ZBFi::vu  1B  ;B7..:qO   BS..iGB   BJ..:vB2       BM      rBj  :@7,.:5B   qM.. i@r..i5. ir. UB             
//      iuU1vi   ,    ;LLv,     iYvi ,    ;LLr  .       .,       .     rvY7:     rLi   7LLr,  ,uvv:  
// 
// 
//  Copyright 2014-2015 daxnet
//  
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//  
//      http://www.apache.org/licenses/LICENSE-2.0
//  
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.
//  =======================================================================================================

namespace CloudNotes.Domain.Repositories.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using CloudNotes.DESecurity;
    using CloudNotes.Domain.Model;
    using CloudNotes.Infrastructure;

    /// <summary>
    ///     Represents the CloudNotes database initialization strategy.
    /// </summary>
    public class CloudNotesDatabaseInitializationStrategy : DropCreateDatabaseIfModelChanges<CloudNotesContext>
    {
        /// <summary>
        ///     Seeds the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        protected override void Seed(CloudNotesContext context)
        {
            #region Roles

            var administratorRole = new Role
            {
                Name = "Administrator",
                Description = "System administrator role that has the permission of all operations."
            };
            context.Roles.Add(administratorRole);

            var accountManagerRole = new Role
            {
                Name = "Account Manager",
                Description =
                    "Account Managers have the permission to create/update/delete the user accounts in CloudNotes system."
            };
            context.Roles.Add(accountManagerRole);

            var packageManagerRole = new Role
            {
                Name = "Package Manager",
                Description =
                    "Package Managers have the permission to post/get client packages."
            };
            context.Roles.Add(packageManagerRole);

            #endregion

            #region Privileges

            var createUserPrivilege = new Privilege
            {
                Name = Privileges.ApiCreateUser,
                Description = "The create user privilege."
            };
            context.Privileges.Add(createUserPrivilege);

            var updateUserPrivilege = new Privilege
            {
                Name = Privileges.ApiUpdateUser,
                Description = "The update user privilege."
            };
            context.Privileges.Add(updateUserPrivilege);

            var pingPrivilege = new Privilege
            {
                Name = Privileges.ApiPing,
                Description = "Ping privilege."
            };
            context.Privileges.Add(pingPrivilege);

            var getPackagePrivilege = new Privilege
            {
                Name = Privileges.ApiGetPackage,
                Description = "Get package privilege."
            };
            context.Privileges.Add(getPackagePrivilege);

            var postPackagePrivilege = new Privilege
            {
                Name = Privileges.ApiPostPackage,
                Description = "Post package privilege."
            };
            context.Privileges.Add(postPackagePrivilege);

            #endregion

            #region Permissions

            var adminCreateUserPermission = new Permission
            {
                Privilege = createUserPrivilege,
                Role = administratorRole,
                Value = PermissionValue.Allow
            };
            context.Permissions.Add(adminCreateUserPermission);

            var adminPingPermission = new Permission
            {
                Privilege = pingPrivilege,
                Role = administratorRole,
                Value = PermissionValue.Allow
            };
            context.Permissions.Add(adminPingPermission);

            var adminGetPackagePermission = new Permission
            {
                Privilege = getPackagePrivilege,
                Role = administratorRole,
                Value = PermissionValue.Allow
            };
            context.Permissions.Add(adminGetPackagePermission);

            var adminPostPackagePermission = new Permission
            {
                Privilege = postPackagePrivilege,
                Role = administratorRole,
                Value = PermissionValue.Allow
            };
            context.Permissions.Add(adminPostPackagePermission);

            var accountManagerCreateUserPermission = new Permission
            {
                Privilege = createUserPrivilege,
                Role = accountManagerRole,
                Value = PermissionValue.Allow
            };
            context.Permissions.Add(accountManagerCreateUserPermission);

            var pkgManagerGetPackagePermission = new Permission
            {
                Privilege = getPackagePrivilege,
                Role = packageManagerRole,
                Value = PermissionValue.Allow
            };
            context.Permissions.Add(pkgManagerGetPackagePermission);

            var pkgManagerPostPackagePermission = new Permission
            {
                Privilege = postPackagePrivilege,
                Role = packageManagerRole,
                Value = PermissionValue.Allow
            };
            context.Permissions.Add(pkgManagerPostPackagePermission);

            #endregion

            #region Users

            var proxyUser = new User
            {
                DateRegistered = DateTime.UtcNow,
                UserName = Crypto.ProxyUserName,
                Email = Crypto.ProxyUserEmail,
                Password = Crypto.ComputeHash(Crypto.ProxyUserPassword, Crypto.ProxyUserName),
                Locked = true,
                Roles = new List<Role> {accountManagerRole, packageManagerRole}
            };
            context.Users.Add(proxyUser);

            #endregion

            base.Seed(context);
        }
    }
}