using System;
using System.Diagnostics;
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
                Stopwatch watch = new Stopwatch();

                NodesRepository node;

                watch.Start();

                /*
                 * Для работы с файлом большого объема лучше использовать MemoryMappedFile
                 * И делать параллельную сборку Нод используя Parallel и CreateViewAccessor
                */
                using (var reader = new StreamReader("Example.xml"))
                {
                    List<KeyValuePair<string, Func<string, Node>>> templates = new List<KeyValuePair<string, Func<string, Node>>>
                    {
                         new KeyValuePair<string, Func<string, Node>>("cim:Substation", (s) => new SubstationNode(s)),
                         new KeyValuePair<string, Func<string, Node>>("cim:VoltageLevel", (s) => new VoltageNode(s)),
                         new KeyValuePair<string, Func<string, Node>>("cim:SynchronousMachine", (s) => new MachineNode(s)),
                         //new KeyValuePair<string, Func<string, Node>>("cim:OperationalLimitSet", (s) => new LimitSetNode(s)), // Можно подключать и отключать Ноды
                    };

                    node = new NodesRepository(reader, new NodeMediator(templates));
                    
                    node.CollectNodes();
                }
                
                var s = node?.ToJson();
                
                watch.Stop();
                
                var time = $"Время выполенения: {watch.ElapsedMilliseconds} мс";

                Console.WriteLine(s);
                Console.WriteLine();
                Console.WriteLine(time);

                Console.ReadLine();

            }
            catch(FileNotFoundException fx)
            {
                Console.WriteLine($"Отсутствует файл: {fx.Message}");
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
