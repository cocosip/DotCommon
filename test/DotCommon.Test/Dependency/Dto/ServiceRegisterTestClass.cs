namespace DotCommon.Test.Dependency.Dto
{
    public interface IDependencyTestService
    {
        string GetName();
    }

    public class DependencyTestService : IDependencyTestService
    {
        public string GetName()
        {
            return "123";
        }
    }

     public interface IDependencyTest2Service
    {
        string GetId();
    }

    public class DependencyTest2Service : IDependencyTest2Service
    {
        public string GetId()
        {
            return "1000";
        }
    }
}
