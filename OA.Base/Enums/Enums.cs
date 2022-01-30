using System;
using System.Collections.Generic;
using System.Text;

namespace OA.Base.Enums
{
    public enum EnumLang
    {
        Ar = 11,
        En = 22
    }
    public enum EnumServers
    {
        Windows = 1,
        Linux = 2,
    }
    public enum EnumMailer : int
    {
        ContactMail = 11,
        ResetPassword = 22,
        Adds = 33,
        Notifcation = 44
    }
    public enum EnumConsumer:long
    {
        cPanelConsumer = 143217789,
        DeviceConsumer = 254321889,
    }
    public enum EnumTypeRole : int
    {
        Admins = 1,
        Staff = 2,
        All = -1,
    }
    public enum EnumUserRole : long
    {
        User = 231542980,
        Employee = 650003211,
        Editor = 411880522,
        Customer = 533384120,
        Admin = 110579020,
        GeneralDirector = 120306380
    }

    public enum EnumWeekDays : int
    {
        Saturday = 1,
        Sunday = 2,
        Monday = 3,
        Tuesday = 4,
        Wednesday = 5,
        Thursday = 6,
        Friday = 7,
    }
    public enum EnumOprationType : int
    {
        New = 1,
        Update = 2,
        Delete = 3
    }
    public enum EnumFireBaseTopic : int
    {
        All = 1,
        Clients = 2,
    }
    public enum EnumTimeZones:int
    {
        Sudan = 2,
        SaudiArab = 3,
        Emarat = 4
    }

    public enum EnumEmojes : int
    {
        PissedMe = 44,
        VeryGood = 33,
        Good = 22,
        Like = 11,
    }
}
