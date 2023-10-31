namespace Domain.Enums
{
    public static class Permissions
    {
        public static List<string> generatePermissionsList(string module)
        {
            return new List<string>
            {
                $"Permissions.{module}.Read",
                $"Permissions.{module}.Create",
                $"Permissions.{module}.Edit",
                $"Permissions.{module}.Delete"
            };
        }
    }
}
