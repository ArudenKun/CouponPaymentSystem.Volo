using Abp.Mapperly;
using Riok.Mapperly.Abstractions;

namespace Application;

[Mapper]
public partial class UserMapper : MapperBase<User, UserDto>
{
    public override partial UserDto Map(User source);

    public override partial void Map(User source, UserDto destination);
}
