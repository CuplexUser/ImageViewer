﻿using System.Security.AccessControl;
using System.Security.Principal;

namespace ImageViewer.Storage;

public class AccessHelper
{
    private readonly WindowsIdentity _winId;

    public AccessHelper()
    {
        _winId = WindowsIdentity.GetCurrent();
    }

    public bool UserHasReadAccessToDirectory(DirectoryInfo directoryInfo)
    {
        try
        {
            var dSecurity = directoryInfo.GetAccessControl();
            var authorizarionRuleCollecion = dSecurity.GetAccessRules(true, true, typeof(SecurityIdentifier));

            foreach (FileSystemAccessRule fsAccessRules in authorizarionRuleCollecion)
                if (_winId.UserClaims.Any(c => c.Value == fsAccessRules.IdentityReference.Value) &&
                    fsAccessRules.FileSystemRights.HasFlag(FileSystemRights.ReadData) &&
                    fsAccessRules.AccessControlType == AccessControlType.Allow)
                {
                    return true;
                }

            return false;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Errorr in DirectoryData.UserHasReadAccessToDirectory {directoryInfo}", directoryInfo);
        }

        return false;
    }
}