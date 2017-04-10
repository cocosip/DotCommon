namespace DotCommon.Dependency
{
    public class IocManager
    {
        public static IIocContainer IocContainer { get; private set; }

        public static void SetContainer(IIocContainer iocContainer)
        {
            IocContainer = iocContainer;
        }

    }
}
