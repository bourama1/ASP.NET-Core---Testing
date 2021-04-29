using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    public class DbInitializer
    {
        public static void Initialize(FactoryContext context)
        {
            context.Database.EnsureCreated();

            if (context.Lines.Any())
            {
                return;
            }

            var lines = new LineModel[]
            {
            new LineModel{Name="A"},
            new LineModel{Name="B"},
            new LineModel{Name="C"},
            new LineModel{Name="D"},
            new LineModel{Name="E"},
            new LineModel{Name="F"}
            };
            foreach (LineModel l in lines)
            {
                context.Lines.Add(l);
            }
            context.SaveChanges();

            var buggies = new BuggyModel[]
            {
            new BuggyModel{BuggyID=150,Name="A1"},
            new BuggyModel{BuggyID=250,Name="B1"},
            new BuggyModel{BuggyID=350,Name="A2"},
            new BuggyModel{BuggyID=450,Name="A3"},
            new BuggyModel{BuggyID=550,Name="A5"},
            new BuggyModel{BuggyID=650,Name="B3"}
            };
            foreach (BuggyModel b in buggies)
            {
                context.Buggies.Add(b);
            }
            context.SaveChanges();

            var routes = new RouteModel[]
            {
            new RouteModel{LineID=1,BuggyID=150},
            new RouteModel{LineID=1,BuggyID=250},
            new RouteModel{LineID=1,BuggyID=350},
            new RouteModel{LineID=4,BuggyID=450},
            new RouteModel{LineID=5,BuggyID=550},
            new RouteModel{LineID=6,BuggyID=650},
            new RouteModel{LineID=2,BuggyID=250},
            new RouteModel{LineID=3,BuggyID=350},
            new RouteModel{LineID=6,BuggyID=150},
            };
            foreach (RouteModel r in routes)
            {
                context.Routes.Add(r);
            }
            context.SaveChanges();
        }
    }
}
