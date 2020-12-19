using System;
using System.Collections.Generic;
using System.IO;

namespace XmlParserTest
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                using var reader = new StreamReader("Example.xml");

                List<KeyValuePair<string, Func<string, Node>>> templates = new List<KeyValuePair<string, Func<string, Node>>>
                {
                     new KeyValuePair<string, Func<string, Node>>("cim:Substation", (s) => new SubstationNode(s)),
                     new KeyValuePair<string, Func<string, Node>>("cim:VoltageLevel", (s) => new VoltageNode(s)),
                     new KeyValuePair<string, Func<string, Node>>("cim:SynchronousMachine", (s) => new MachineNode(s)),
                };

                var node = new NodesRepository(reader, new NodeMediator(templates));

                node.CollectNodes();

                var s = node.ToJson();

                Console.WriteLine(s);
                
                Console.ReadLine();

            }
            catch(FileNotFoundException fx)
            {
                Console.WriteLine($"Ошибка файла: {fx.Message}");
            }
            catch(IOException ix)
            {
                Console.WriteLine($"Ошибка чтения: {ix.Message}");
            }
            catch(FormatException fx)
            {
                Console.WriteLine($"Ошибка форматирования: {fx.Message}");
            }
            catch(Exception x)
            {
                Console.WriteLine($"Ошибка: {x.Message}");
            }
        }
    }
}
