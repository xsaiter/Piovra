using Piovra.DapperExtensions;
using System;
using System.Threading.Tasks;

namespace Piovra.Demo;

class Program {
    static void Main(string[] args) {
        var n = Naming.ConvertFromCamelToSnakeCase("GetAgeForPerson");
        var n2 = Naming.ConvertFromCamelToSnakeCase("Get");
        var n3 = Naming.ConvertFromCamelToSnakeCase("GetA");
        var n4 = Naming.ConvertFromCamelToSnakeCase("GetATM");
        var n5 = Naming.ConvertFromCamelToSnakeCase("GetATMPerson");
        var n6 = Naming.ConvertFromCamelToSnakeCase("Get123Person");
        Console.ReadKey();
    }
}