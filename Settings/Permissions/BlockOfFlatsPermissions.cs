namespace FlatsAPI.Settings.Permissions;

public class BlockOfFlatsPermissions : IPermissions
{
    public const string Create = "BlockOfFlats.Create";
    public const string CreateAnonymously = "BlockOfFlats.Create.Anonymously";
    public const string Read = "BlockOfFlats.Read";
    public const string Update = "BlockOfFlats.Update";
    public const string UpdateOthers = "BlockOfFlats.Update.Others";
    public const string Delete = "BlockOfFlats.Delete";
    public const string DeleteOthers = "BlockOfFlats.Delete.Others";
    public const string ApplyOwner = "BlockOfFlats.ApplyOwner";
}
