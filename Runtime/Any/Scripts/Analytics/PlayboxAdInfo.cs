namespace Playbox
{
    public class PlayboxAdInfo
    {
        public string AdUnitIdentifier { get; set; }
        public string AdFormat { get; set; }
        public string NetworkName { get;  set; }
        public string NetworkPlacement { get;  set; }
        public string Placement { get;  set; }
        public string CreativeIdentifier { get;  set; }
        public double Revenue { get;  set; }
        public string RevenuePrecision { get;  set; }
        public long LatencyMillis { get;  set; }
        public string DspName { get;  set; }
    }
}