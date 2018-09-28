namespace DotCommon.Test.Dependency.Dto
{
    public class GetServiceByArgsTestClass
    {
        public int Id { get; }
        public string Name { get; }
        public GetServiceByArgsTestClass(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
