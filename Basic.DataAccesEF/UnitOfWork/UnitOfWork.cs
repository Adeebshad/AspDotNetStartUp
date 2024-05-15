using Basic.DataAccesEF.TypeRepository;
using Basic.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basic.DataAccesEF.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private ItemContext itemContext;
        //public UnitOfWork(PeopleContext context)
        //{
        //    this.context = context;
        //    Address = new AddressRepository(this.context);
        //    Email = new EmailRepository(this.context);
        //    Person = new PersonRepository(this.context);
        //    Item = new ItemRepository(this.context);
        //    Category = new CategoryRepository(this.context);
        //}

        public UnitOfWork(ItemContext item)
        {
            this.itemContext = item;
            //Address = new AddressRepository(this.context);
            //Email = new EmailRepository(this.context);
            //Person = new PersonRepository(this.context);
            Item = new ItemRepository(this.itemContext);
            Category = new CategoryRepository(this.itemContext);
        }
        //public IAdressRepository Address
        //{
        //    get;
        //    private set;
        //}
        //public IEmailRepository Email
        //{
        //    get;
        //    private set;
        //}
        //public IPersonRepository Person
        //{
        //    get;
        //    private set;
        //}

        public IItemRepository Item
        {
            get;
            private set;
        }

        public ICategoryRepository Category
        {
            get;
            private set;
        }
        public void Dispose()
        {
            itemContext.Dispose();
        }
        public int Save()
        {
            return itemContext.SaveChanges();
        }
    }
}
