namespace FlatsAPI.Settings.Permissions
{
    public class AccountPermissions : IPermissions
    {
        public const string Create = "Account.Create";
        public const string Read = "Account.Read";
        public const string Update = "Account.Update";
        public const string UpdateOthers = "Account.Update.Others";
        public const string Delete = "Account.Delete";
        public const string DeleteOthers = "Account.Delete.Others";
    }
}
