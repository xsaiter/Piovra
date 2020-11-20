using System;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;
using Piovra.Demo.EfCoreTests;

namespace Piovra.Demo {
    class Program {        
        static async Task Main(string[] args) {
            await TestEf();
            Console.ReadKey();
        }

        static async Task TestEf() {
            using var dc = CreateStaffDc();
            var person = await dc.Persons.FirstOrDefaultAsync();
            var xml = person.Info;
            xml.Add(new XElement("test"));
            await dc.SaveChangesAsync();
        }

        static StaffDc CreateStaffDc() {
            var builder = new DbContextOptionsBuilder<StaffDc>();
            builder.UseNpgsql("");
            var dc = new StaffDc(builder.Options);
            return dc;
        }
    }
}