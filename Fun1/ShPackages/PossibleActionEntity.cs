namespace ShPackages
{
    public class PossibleActionEntity
    {
        public PackageType PackageType { get; }
        public string Name { get; }

        public PossibleActionEntity(PackageType packageType, string name)
        {
            PackageType = packageType;
            Name = name;
        }
    }
}