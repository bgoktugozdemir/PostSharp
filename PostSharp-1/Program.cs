using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using PostSharp.Aspects;

namespace PostSharp_1
{
    class Program
    {
        static void Main(string[] args)
        {
            MyClass my = new MyClass();
        }
    }

    [Serializable]
    public class MyAspect : OnMethodBoundaryAspect
    {
        public override void OnEntry(MethodExecutionArgs args)
        {
            Console.WriteLine("Before the method");
        }

        public override void OnExit(MethodExecutionArgs args)
        {
            Console.WriteLine("After the method");
        }
    }
    public class MyClass
    {
        [MyAspect]
        public void Write()
        {
            Console.WriteLine("Hello World");
        }
    }
}
