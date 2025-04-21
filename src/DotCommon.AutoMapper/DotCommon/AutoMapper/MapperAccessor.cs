using AutoMapper;

namespace DotCommon.AutoMapper
{
    public class MapperAccessor : IMapperAccessor
    {
        public IMapper Mapper { get; set; } = default!;
    }
}
