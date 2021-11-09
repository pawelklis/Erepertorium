using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.Protocols;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Threading;


public class A_Directory
{
    public static String ADAdminUser = "SVC_Aktyw_Skaner_Wro";

    public static String ADAdminPassword = "da{rQ,KgC$pTt[!2eGZ6e$Q7=Gs?oX";

    public static String ADFullPath = "net.pp";
    //= "LDAP://192.168.0.3"; 

    public static String ADServer = "net.pp";
    //= "sakura.com";
    // public static string ADPath= ADFullPath ; 
    // public static string ADUser = ADAdminUser ;
    // public static string ADPassword = ADAdminPassword ;

    public enum ADAccountOptions
    {
        UF_TEMP_DUPLICATE_ACCOUNT = 256,
        UF_NORMAL_ACCOUNT = 512,
        UF_INTERDOMAIN_TRUST_ACCOUNT = 2048,
        UF_WORKSTATION_TRUST_ACCOUNT = 4096,
        UF_SERVER_TRUST_ACCOUNT = 8192,
        UF_DONT_EXPIRE_PASSWD = 65536,
        UF_SCRIPT = 1,
        UF_ACCOUNTDISABLE = 2,
        UF_HOMEDIR_REQUIRED = 8,
        UF_LOCKOUT = 16,
        UF_PASSWD_NOTREQD = 32,
        UF_PASSWD_CANT_CHANGE = 64,
        UF_ACCOUNT_LOCKOUT = 16,
        UF_ENCRYPTED_TEXT_PASSWORD_ALLOWED = 128
    }


    public enum LoginResult
    {
        LOGIN_OK = 0,
        LOGIN_USER_DOESNT_EXIST,
        LOGIN_USER_ACCOUNT_INACTIVE
    }

    public static bool IsAccountActive(String userLogin, int idlok)
    {
        try
        {
            DirectoryEntry us = getUser(userLogin, idlok);
            if(us == null)
                return false;
            int userAccountControl = System.Convert.ToInt32(us.Properties["userAccountControl"].Value);

            int accountDisabled = Convert.ToInt32(ADAccountOptions.UF_ACCOUNTDISABLE);
            int flagExists = userAccountControl & accountDisabled;
            // if a match Is found, then the disabled flag exists within the control flags
            if(flagExists > 0)
                 return false;
            else
                return true;
        }
        catch(Exception ex)
        {
        }
        return false;
    }

    public static DirectoryEntry getDirectoryEntry()
    {
        ADAdminUser = Erepertorium.Properties.Settings.Default.ADuser;// "SVC_Aktyw_Skaner_Wro";
        ADAdminPassword = Erepertorium.Properties.Settings.Default.ADpwd; // "da{rQ,KgC$pTt[!2eGZ6e$Q7=Gs?oX";
        string ldap = Erepertorium.Properties.Settings.Default.LDAPPath; //"LDAP://10.32.30.25/OU=Wroclaw,OU=SkaneryRadiowe,DC=net,DC=pp"

            DirectoryEntry dirEntry = new DirectoryEntry(ldap, ADAdminUser, ADAdminPassword, AuthenticationTypes.Secure); // getDirectoryObject("/" + getLDAPDomain())

            return dirEntry;    

        
    }
    public class DirEntryProps
    {
        internal AuthenticationTypes Secure;

        public string LDAP { get; set; }
        public string UserLogin { get; set; }
        public string UserPass { get; set; }
        //public System.Security Secure { get; set; }
    }
    public static DirEntryProps getDirectoryEntryString()
    {

        ADAdminUser = Erepertorium.Properties.Settings.Default.ADuser; // "SVC_Aktyw_Skaner_Wro";
        ADAdminPassword = Erepertorium.Properties.Settings.Default.ADpwd; // "da{rQ,KgC$pTt[!2eGZ6e$Q7=Gs?oX";
        string ldap = Erepertorium.Properties.Settings.Default.LDAPPath; //"LDAP://10.32.30.25/OU=Wroclaw,OU=SkaneryRadiowe,DC=net,DC=pp"


        DirEntryProps p = new DirEntryProps();
        p.LDAP = ldap; // "LDAP://10.32.30.25/OU=Wroclaw,OU=SkaneryRadiowe,DC=net,DC=pp";
            p.UserLogin = ADAdminUser;
            p.UserPass = ADAdminPassword;
            p.Secure = AuthenticationTypes.Secure;

            return p;
              
    }
    public static DirectoryEntry getUser(String UserName, int idlok)
    {
        if (UserName==null)
        return null /* TODO Change To Default(_) if this Is Not a reference type */;
        if (String.IsNullOrEmpty(UserName))
        return null /* TODO Change To Default(_) if this Is Not a reference type */;
        // create an instance of the DirectoryEntry
        // Dim dirEntry As DirectoryEntry = getDirectoryEntry(idlok)
        DirectoryEntry dirEntry = new DirectoryEntry("LDAP://10.32.30.25/OU=Wroclaw,DC=net,DC=pp", ADAdminUser, ADAdminPassword, AuthenticationTypes.Secure); // getDirectoryObject("/" + getLDAPDomain())

        // create instance fo the direcory searcher
        DirectorySearcher dirSearch = new DirectorySearcher(dirEntry);

        dirSearch.SearchRoot = dirEntry;
        // set the search filter

        dirSearch.Filter = "(&(objectCategory=*)(cn=" + UserName + "))";
        dirSearch.SearchScope = System.DirectoryServices.SearchScope.Subtree;

        // find the first instance
        SearchResult searchResults = dirSearch.FindOne();


        if (searchResults==null)
        {
            dirEntry = getDirectoryEntry(); // new DirectoryEntry("LDAP://10.32.30.25/OU=SkaneryRadiowe,DC=net,DC=pp", ADAdminUser, ADAdminPassword, AuthenticationTypes.Secure) 'getDirectoryObject("/" + getLDAPDomain())
            dirSearch = new DirectorySearcher(dirEntry);
            dirSearch.SearchRoot = dirEntry;


            dirSearch.Filter = "(&(objectCategory=*)(cn=" + UserName + "))";
            dirSearch.SearchScope = System.DirectoryServices.SearchScope.OneLevel;

            // find the first instance
            searchResults = dirSearch.FindOne();
        }

        // if found then return, otherwise return Null
        if (searchResults == null)
        {
            // de= new DirectoryEntry(results.Path,ADAdminUser,ADAdminPassword,AuthenticationTypes.Secure);
            // if so then return the DirectoryEntry object
            var a = searchResults.GetDirectoryEntry();

            return searchResults.GetDirectoryEntry();
        }
        else
        return null /* TODO Change To Default(_) if this Is Not a reference type */;
    }
    public enum UserInfoProps
    {
        objectClass,
        cn,
        sn,
        l,
        title,
        description,
        postalCode,
        telephoneNumber,
        givenName,
        distinguishedName,
        instanceType,
        whenCreated,
        whenChanged,
        displayName,
        uSNCreated,
        memberOf,
        uSNChanged,
        department,
        proxyAddresses,
        streetAddress,
        nTSecurityDescriptor,
        employeeNumber,
        name,
        objectGUID,
        userAccountControl,
        badPwdCount,
        codePage,
        countryCode,
        employeeID,
        badPasswordTime,
        lastLogon,
        pwdLastset,
        primaryGroupID,
        objectSid,
        accountExpires,
        logonCount,
        sAMAccountName,
        sAMAccountType,
        showInAddressBook,
        legacyExchangeDN,
        userPrincipalName,
        lockoutTime,
        objectCategory,
        dSCorePropagationData,
        lastLogonTimestamp,
        mail,
        mobile,
        manager,
        pager,
        msExchRecipientTypeDetails,
        msRTCSIP_UserRoutingGroupId,
        msRTCSIP_UserPolicies,
        extensionAttribute13,
        extensionAttribute14,
        msExchRemoteRecipientType,
        msExchPoliciesExcluded,
        msRTCSIP_OptionFlags,
        msRTCSIP_DeploymentLocator,
        msRTCSIP_PrimaryHomeServer,
        msRTCSIP_FederationEnabled,
        msRTCSIP_InternetAccessEnabled,
        msExchUMDtmfMap,
        targetAddress,
        msExchRecipientDisplayType,
        msRTCSIP_PrimaryUserAddress,
        msExchVersion,
        msRTCSIP_UserEnabled,
        mailNickname
    }



    public static List<String> getuserAdInformation(String userName, List<UserInfoProps> propNames)
    {
        var DE = A_Directory.getUser(userName, 8);
        List<string> sb = new List<string>();
        try
        {
            foreach (System.DirectoryServices.PropertyValueCollection p in DE.Properties)
            {
                foreach (var propName in propNames)
                {
                    if (p.PropertyName == propName.ToString().Replace("_", "-"))
                        sb.Add(p.Value.ToString());
                }
            }
        }
        catch (Exception ex)
        {
        }

        return sb;
    }
    public static bool ValidateActiveDirectoryLogin(String Username, String Password)
    {
        bool Success = false;

        DirEntryProps ld = getDirectoryEntryString();

        System.DirectoryServices.DirectoryEntry Entry = new System.DirectoryServices.DirectoryEntry(ld.LDAP, Username, Password);
        System.DirectoryServices.DirectorySearcher Searcher = new System.DirectoryServices.DirectorySearcher(Entry);
        Searcher.SearchScope = System.DirectoryServices.SearchScope.OneLevel;
        try
        {
            System.DirectoryServices.SearchResult Results = Searcher.FindOne();
            Success = !(Results == null);
        }
        catch
        {
            Success = false;
        }
        return Success;
    }

    public static void DisableAccount(String sLogin, int idlok)
    {
        //String strError;
        //try
        //{
        //    System.DirectoryServices.DirectoryEntry child = new System.DirectoryServices.DirectoryEntry(getDirectoryEntryString().LDAP, getDirectoryEntryString().UserLogin, getDirectoryEntryString().UserPass, getDirectoryEntryString().Secure);
        //    DirectorySearcher searcher = new DirectorySearcher(child);
        //    SearchResult result;
        //    DirectoryEntry userEntry;
        //    searcher.Filter = "(SAMAccountName=" + sLogin + ")";
        //    searcher.CacheResults = false;
        //    result = searcher.FindOne();
        //    userEntry = result.GetDirectoryEntry();
        //    {
        //        var withBlock = userEntry;
        //        userEntry.NativeObject.accountdisabled = true;
        //    }
        //    userEntry.CommitChanges();
        //}
        //catch (Exception ex)
        //{
        //}
    }


    public static void EnableAccount(DirectoryEntry de, int idlok)
    {
        try
        {


            // UF_DONT_EXPIRE_PASSWD 0x10000
            int exp = System.Convert.ToInt32(de.Properties["userAccountControl"].Value);
            de.Properties["userAccountControl"].Value = exp | 0x1;
            de.CommitChanges();
            // UF_ACCOUNTDISABLE 0x0002
            int val = System.Convert.ToInt32(de.Properties["userAccountControl"].Value);
            de.Properties["userAccountControl"].Value = val & 0x2;
            de.CommitChanges();
        }

        catch (Exception ex)
        {
        }
    }
}
