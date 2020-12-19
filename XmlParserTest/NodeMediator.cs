using System;
using System.Collections.Generic;
using System.Linq;

namespace XmlParserTest
{
    class NodeMediator
    {
        private readonly Dictionary<string, Func<string, Node>> _templates;
       
        internal NodeMediator(IEnumerable<KeyValuePair<string, Func<string, Node>>> keyValues)
        {
            _templates = new Dictionary<string, Func<string, Node>>(keyValues);
        }
        internal Node GetNode(string line)
        {
            KeyValuePair<string, Func<string, Node>> creator = _templates
                .FirstOrDefault(k => line.StartsWith($"<{k.Key} "));

            return creator.Value?.Invoke(line);
        }

        
    }
}