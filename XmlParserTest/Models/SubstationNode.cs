namespace XmlParserTest
{
    class SubstationNode : ParentNode
    {
        public SubstationNode(string line)
            : base(line)
        {
        }
        protected override string Template => "cim:Substation";
    }
}
