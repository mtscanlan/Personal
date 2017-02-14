using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication14
{
    class Foo: IFoo
    {
        private IBar _bar;

        public Foo(IBar bar)
        {
            _bar = bar;
            ;
        }

        public void DoIt()
        {
            throw new NotImplementedException();
        }
    }
}
