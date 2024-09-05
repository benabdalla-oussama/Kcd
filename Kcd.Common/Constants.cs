namespace Kcd.Common;

public class Constants
{
    /// <summary>
    /// Default password used for development and testing environments.
    /// <remarks>
    /// <para>TODO: This default password is intended for development and testing purposes only. 
    /// It should be removed or replaced with a secure password policy in production environments.</para>
    /// </remarks>
    /// </summary>
    public static string DefaultPassword = "Azerty123456789!.";
    public static string DefaultAdminEmail = "admin@admin.com";

    //Claims
    public static string Country_Claim_Type = "country";
    public static string Company_Claim_Type = "company";
    public static string Avatar_Id_Claim_Type = "avatar_id";
    public static string Uid_Claim_Type = "uid";
}
