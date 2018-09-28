namespace DotCommon.Test.Dependency.Dto
{
    public class GetServiceByInjectAndArgsTestClass
    {
        private readonly GetServiceByInjectAndArgsTestInjectClass _getServiceByInjectAndArgsTestInjectClass;
        private int _id;
        private string _name;
        public GetServiceByInjectAndArgsTestClass(GetServiceByInjectAndArgsTestInjectClass getServiceByInjectAndArgsTestInjectClass, int id, string name)
        {
            _getServiceByInjectAndArgsTestInjectClass = getServiceByInjectAndArgsTestInjectClass;
            _id = id;
            _name = name;
        }

        public string GetInfo()
        {
            return $"{_getServiceByInjectAndArgsTestInjectClass.Hello()},Id:{_id},Name:{_name}";
        }


    }
}
