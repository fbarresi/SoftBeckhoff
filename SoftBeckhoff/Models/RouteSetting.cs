namespace SoftBeckhoff.Models
{
    public class RouteSetting
    {
        public string Name { get; set; }
        public string IpAddress { get; set; }
        public string AmsNetId { get; set; }
        public override string ToString()
        {
            return $"{Name} [{AmsNetId} over {IpAddress}]";
        }
    }
}