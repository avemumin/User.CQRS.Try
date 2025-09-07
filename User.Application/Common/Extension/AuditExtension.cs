namespace User.Application.Common.Extension;

public static class AuditExtension
{
  public static string GetAuditActionName(this object command)
  {
    var typeName = command.GetType().Name;
    return typeName.Replace("Command", "")
      .Replace("Query", "")
      .Replace("Cmd", "");
  }
}
