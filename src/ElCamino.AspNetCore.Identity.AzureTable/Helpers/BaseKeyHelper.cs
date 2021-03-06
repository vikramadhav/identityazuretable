﻿// MIT License Copyright 2020 (c) David Melendez. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ElCamino.AspNetCore.Identity.AzureTable.Model;
using Microsoft.AspNetCore.Identity;


namespace ElCamino.AspNetCore.Identity.AzureTable.Helpers
{
    public abstract class BaseKeyHelper : IKeyHelper
    {
        #region RowConstants
        public virtual string PreFixIdentityUserClaim => Constants.RowKeyConstants.PreFixIdentityUserClaim;

        public virtual string PreFixIdentityUserClaimUpperBound => Constants.RowKeyConstants.PreFixIdentityUserClaimUpperBound;

        public virtual string PreFixIdentityUserRole => Constants.RowKeyConstants.PreFixIdentityUserRole;

        public virtual string PreFixIdentityUserRoleUpperBound => Constants.RowKeyConstants.PreFixIdentityUserRoleUpperBound;

        public virtual string PreFixIdentityUserLogin => Constants.RowKeyConstants.PreFixIdentityUserLogin;

        public virtual string PreFixIdentityUserLoginUpperBound => Constants.RowKeyConstants.PreFixIdentityUserLoginUpperBound;

        public virtual string PreFixIdentityUserEmail => Constants.RowKeyConstants.PreFixIdentityUserEmail;

        public virtual string PreFixIdentityUserToken => Constants.RowKeyConstants.PreFixIdentityUserToken;

        public virtual string PreFixIdentityUserId => Constants.RowKeyConstants.PreFixIdentityUserId;

        public virtual string PreFixIdentityUserIdUpperBound => Constants.RowKeyConstants.PreFixIdentityUserIdUpperBound;

        public virtual string PreFixIdentityUserName => Constants.RowKeyConstants.PreFixIdentityUserName;

        public virtual string FormatterIdentityUserClaim => Constants.RowKeyConstants.FormatterIdentityUserClaim;

        public virtual string FormatterIdentityUserRole => Constants.RowKeyConstants.FormatterIdentityUserRole;

        public virtual string FormatterIdentityUserLogin => Constants.RowKeyConstants.FormatterIdentityUserLogin;

        public virtual string FormatterIdentityUserEmail => Constants.RowKeyConstants.FormatterIdentityUserEmail;

        public virtual string FormatterIdentityUserToken => Constants.RowKeyConstants.FormatterIdentityUserToken;

        public virtual string FormatterIdentityUserId => Constants.RowKeyConstants.FormatterIdentityUserId;

        public virtual string FormatterIdentityUserName => Constants.RowKeyConstants.FormatterIdentityUserName;

        public virtual string PreFixIdentityRole => Constants.RowKeyConstants.PreFixIdentityRole;

        public virtual string PreFixIdentityRoleUpperBound => Constants.RowKeyConstants.PreFixIdentityRoleUpperBound;

        public virtual string PreFixIdentityRoleClaim => Constants.RowKeyConstants.PreFixIdentityRoleClaim;

        public virtual string FormatterIdentityRole => Constants.RowKeyConstants.FormatterIdentityRole;

        public virtual string FormatterIdentityRoleClaim => Constants.RowKeyConstants.FormatterIdentityRoleClaim;

        #endregion

        public virtual string GeneratePartitionKeyIndexByLogin(string plainLoginProvider, string plainProviderKey)
        {
            string strTemp = string.Format("{0}_{1}", plainLoginProvider?.ToUpper(), plainProviderKey?.ToUpper());
            string hash = ConvertKeyToHash(strTemp);
            return string.Format(FormatterIdentityUserLogin, hash);
        }

        public virtual string GenerateRowKeyUserEmail(string plainEmail)
        {
            string hash = ConvertKeyToHash(plainEmail?.ToUpper());
            return string.Format(FormatterIdentityUserEmail, hash);
        }

        public virtual string GenerateUserId()
        {
            return Guid.NewGuid().ToString("N");
        }

        public virtual string GenerateRowKeyUserId(string plainUserId)
        {
            string hash = ConvertKeyToHash(plainUserId?.ToUpper());
            return string.Format(FormatterIdentityUserId, hash);
        }

        public virtual string GenerateRowKeyUserName(string plainUserName)
        {
            return GeneratePartitionKeyUserName(plainUserName);
        }

        public virtual string GeneratePartitionKeyUserName(string plainUserName)
        {
            string hash = ConvertKeyToHash(plainUserName?.ToUpper());
            return string.Format(FormatterIdentityUserName, hash);
        }

        public virtual string GenerateRowKeyIdentityUserRole(string plainRoleName)
        {
            string hash = ConvertKeyToHash(plainRoleName?.ToUpper());
            return string.Format(FormatterIdentityUserRole, hash);
        }

        public virtual string GenerateRowKeyIdentityRole(string plainRoleName)
        {
            string hash = ConvertKeyToHash(plainRoleName?.ToUpper());
            return string.Format(FormatterIdentityRole, hash);
        }

        public virtual string GeneratePartitionKeyIdentityRole(string plainRoleName)
        {
            string hash = ConvertKeyToHash(plainRoleName?.ToUpper());
            return hash.Substring(0, 1);
        }

        public virtual string GenerateRowKeyIdentityUserClaim(string claimType, string claimValue)
        {
            string strTemp = string.Format("{0}_{1}", claimType?.ToUpper(), claimValue?.ToUpper());
            string hash = ConvertKeyToHash(strTemp);
            return string.Format(FormatterIdentityUserClaim, hash);
        }

        public virtual string GenerateRowKeyIdentityRoleClaim(string claimType, string claimValue)
        {
            string strTemp = string.Format("{0}_{1}", claimType?.ToUpper(), claimValue?.ToUpper());
            string hash = ConvertKeyToHash(strTemp);
            return string.Format(FormatterIdentityRoleClaim, hash);
        }

        public virtual string GenerateRowKeyIdentityUserToken(string loginProvider, string name)
        {
            string strTemp = string.Format("{0}_{1}", loginProvider?.ToUpper(), name?.ToUpper());
            string hash = ConvertKeyToHash(strTemp);
            return string.Format(FormatterIdentityUserToken, hash);
        }

        public virtual string ParsePartitionKeyIdentityRoleFromRowKey(string rowKey)
        {
            return rowKey.Substring(PreFixIdentityRole.Length, 1);
        }

        public virtual string GenerateRowKeyIdentityUserLogin(string loginProvider, string providerKey)
        {
            string strTemp = string.Format("{0}_{1}", loginProvider?.ToUpper(), providerKey?.ToUpper());
            string hash = ConvertKeyToHash(strTemp);
            return string.Format(FormatterIdentityUserLogin, hash);
        }

        public double KeyVersion => 5.0;

        public abstract string ConvertKeyToHash(string input);

        protected virtual string GetHash(HashAlgorithm shaHash, string input, Encoding encoding, int hashHexLength)
        {
            // Convert the input string to a byte array and compute the hash. 
            byte[] data = shaHash.ComputeHash(encoding.GetBytes(input));
            Debug.WriteLine(string.Format("Key Size before hash: {0} bytes", encoding.GetBytes(input).Length));

            // Create a new StringBuilder to collect the bytes 
            // and create a string.
            StringBuilder sBuilder = new StringBuilder(hashHexLength);
            
            // Loop through each byte of the hashed data  
            // and format each one as a hexadecimal string. 
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            Debug.WriteLine(string.Format("Key Size after hash: {0} bytes", data.Length));

            // Return the hexadecimal string. 
            return sBuilder.ToString();
        }
    }
}
