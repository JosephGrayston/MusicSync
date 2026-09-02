namespace MusicSync.Menu
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class MenuItemAttribute(string description) : Attribute
    {
        public string Description => description;
    }
}
