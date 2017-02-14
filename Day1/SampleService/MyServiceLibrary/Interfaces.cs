using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageServiceLibrary
{
    public interface IIdGenerator
    {
        int GetId();
    }

    public interface IUnique
    {
        int Id { get; set; }
    }
}
