using System;
using System.Collections.Generic;

namespace Netsoftware.Xanthos.Common.Resources;

public class UserIdsResource
{
    public UserIdsResource()
    {
        Ids = new List<Guid>();
    }

    public IList<Guid> Ids { get; set; }
}