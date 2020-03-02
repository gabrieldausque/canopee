using Canopee.Common.Events;

namespace Canopee.StandardLibrary.Triggers.Cron
{
    public class CronTriggerEventArgs : TriggerEventArgs
    {
        public CronTriggerEventArgs()
        {
            
        }
        public CronTriggerEventArgs(string ownerName, string ownerId): base(ownerName, ownerId)
        {
            
        }

        public string FireTimeInUtc { get; set; }
    }
}