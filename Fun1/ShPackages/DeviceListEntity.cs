namespace ShPackages
{
    public class DeviceListEntity
    {
        public string Name { get; }
        public string Tag { get; }
        public PossibleActionEntity[] PossibleActions { get; }

        public DeviceListEntity(string name, string tag, PossibleActionEntity[] possibleActions)
        {
            Name = name;
            Tag = tag;
            PossibleActions = possibleActions;
        }
    }
}