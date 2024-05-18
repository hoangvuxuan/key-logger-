using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Program
    {
        static void do_somthing(int a, ConsoleColor color)
        {
            for (int i = 0; i < a; i++)
            {
                lock(Console.Out)
                {
                    Console.ForegroundColor = color;
                    Console.WriteLine($"{color.ToString()} {i} ... ");
                    Console.ResetColor();
                }
            }
        }

        static  async Task task2()
        {
            Task t2 = new Task(
               (object ob) => {
                   int name = Convert.ToInt32(ob);
                   do_somthing(name, ConsoleColor.Red);
               }
               , "12");

            t2.Start();

            await t2;
            Console.WriteLine("t2 xong");

            

             
        }

        static Task task1()
        {
            Task t1 = new Task(
               () =>
               {
                   do_somthing(10, ConsoleColor.Green);
               });
            t1.Start();

            Console.WriteLine("t1 xong");
            return t1;
        }
        static void Main(string[] args)
        {
            Task t1 = task1();
            Task t2 = task2();
            
            
            do_somthing(5, ConsoleColor.Blue);

             
            Console.WriteLine("fggfgd");

            Task.WaitAll(t2, t1);

            Console.ReadKey();
        }
    }
}
