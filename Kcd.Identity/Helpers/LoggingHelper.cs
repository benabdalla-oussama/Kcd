using Microsoft.AspNetCore.Identity;
using System.Text;

namespace Kcd.Identity.Helpers;

public static class LoggingHelper
{
    public static string GetFullIdentityError(IEnumerable<IdentityError> errors)
    {
        StringBuilder str = new StringBuilder();
        foreach (var err in errors)
        {
            str.AppendFormat("•{0}\n", err.Description);
        }
        return str.ToString();
    }
}
