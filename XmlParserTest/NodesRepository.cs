using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace XmlParserTest
{
    class NodesRepository
    {
        private readonly StreamReader _reader;
        private readonly NodeMediator _mediator;
        private readonly Dictionary<string, Node> _rootNodes;

        public NodesRepository(StreamReader reader, NodeMediator mediator)
        {
            _reader = reader;
            
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));

            _mediator = mediator;
            
            if (mediator == null)
                throw new ArgumentNullException(nameof(mediator));

            _rootNodes = new Dictionary<string, Node>();
        }

        public void CollectNodes()
        {
            while (!_reader.EndOfStream)
            {
                var line = _reader.ReadLine().TrimStart();

                var node = _mediator.GetNode(line);

                if (node == null) continue;

                node.Parse(_reader, this);
            }
        }

        public string ToJson()
        {
            var nodes = _rootNodes
                .Where(k => string.IsNullOrEmpty(k.Value.Parent))
                .Select(kv => kv.Value)
                .ToList();

            StringBuilder result = new StringBuilder();
            
            result
                .Append("[")
                .AppendJoin(',', nodes)
                .Append("]");

            return result.ToString();
        }

        public void AddNode(Node node)
        {
            _rootNodes.Add(node.Key, node);

            if (string.IsNullOrEmpty(node.Parent)) return;

            if(_rootNodes.TryGetValue(node.Parent, out Node parent))
            {
                if(parent is ParentNode pn)
                {
                    pn.Chldren.Add(node);
                }
            }
        }

    }

}
