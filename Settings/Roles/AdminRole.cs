﻿using FlatsAPI.Settings.Permissions;
using System.Collections.Generic;
using System.Reflection;

namespace FlatsAPI.Settings.Roles;

public class AdminRole : IRole
{
    private static ICollection<string> _permissions = new List<string>();

    static AdminRole()
    {
        var permissionsFields = new List<IPermissions>()
        {
            new AccountPermissions(),
            new BlockOfFlatsPermissions(),
            new FlatPermissions(),
            new InvoicePermissions(),
        };

        var fields = new List<FieldInfo>();

        foreach (var type in permissionsFields)
        {
            var properties = type.GetType().GetFields();
            fields.AddRange(properties);
        }

        foreach (var property in fields)
        {
            _permissions.Add(property.GetValue(null).ToString());
        }
    }
    public static string Name { get; } = "Admin";

    public static ICollection<string> Permissions { get; } = _permissions;
}
