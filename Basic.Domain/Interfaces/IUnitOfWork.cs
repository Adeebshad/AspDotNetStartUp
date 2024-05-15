using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basic.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {        //IAdressRepository Address
        //{
        //    get;
        //}
        //IEmailRepository Email
        //{
        //    get;
        //}
        //IPersonRepository Person
        //{
        //    get;
        //}


        IItemRepository Item { get; }
        ICategoryRepository Category { get; }
        int Save();
    }
}
