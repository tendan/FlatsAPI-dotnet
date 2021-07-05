namespace FlatsAPI.Settings.Permissions
{
    public class FlatPermissions : IPermissions
    {
        public const string Create = "Flat.Create";
        public const string Read = "Flat.Read";
        public const string Update = "Flat.Update";
        public const string UpdateOthers = "Flat.Update.Others";
        public const string Delete = "Flat.Delete";
        public const string DeleteOthers = "Flat.Delete.Others";
        public const string ApplyTenant = "Flat.ApplyTenant";
    }
}
