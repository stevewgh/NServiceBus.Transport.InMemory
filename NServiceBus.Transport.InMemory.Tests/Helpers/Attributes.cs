namespace NServiceBus.Transport.InMemory.Tests.Helpers
{
    using System;

    [AttributeUsage(AttributeTargets.Property)]
    public class DataBusAttribute : Attribute
    {
    }
    [AttributeUsage(AttributeTargets.Property)]
    public class EncryptedAttribute : Attribute
    {
    }
}
