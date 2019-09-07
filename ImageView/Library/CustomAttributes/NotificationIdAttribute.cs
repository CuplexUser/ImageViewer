using System;

namespace ImageViewer.Library.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class NotificationIdAttribute : Attribute
    {
        public NotificationIdAttribute(string name) : this(name, Guid.NewGuid())
        {
            Name = name;
        }

        public NotificationIdAttribute(string name, Guid notificationId)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Name must be specified");
            }


            Name = name;
            NotificationId = notificationId;
        }

        public override string ToString()
        {
            return $"Name: {Name}, NotificationId: {NotificationId}";
        }

        public string Name { get; }

        public Guid NotificationId { get; }
    }
}
