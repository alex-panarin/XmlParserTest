using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task CollectNodes()
        {
            while (!_reader.EndOfStream)
            {
                var line = await _reader.ReadLineAsync();

                var node = _mediator.GetNode(line.TrimStart());

                if (node == null) continue;

                await node.Parse(_reader, this);
            }
        }

        public string ToJson()
        {
            var nodes = _rootNodes
                .Where(k => !k.Value.HasParent)
                .Select(kv => kv.Value);
               

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
