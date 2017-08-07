namespace DotCommon.Dependency
{
    public class IocManager
    {
        private static IIocContainer _iocContainer;

        public static void SetContainer(IIocContainer container)
        {
            if (_iocContainer == null)
            {
                _iocContainer = container;
            }
        }

        public static IIocContainer GetContainer()
        {
            return _iocContainer;
        }
    }
}
