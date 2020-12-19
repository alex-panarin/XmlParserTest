namespace XmlParserTest
{
    class VoltageNode : ParentNode
    {
        public VoltageNode(string line)
            : base(line)
        {
            
        }

        protected override string ParentTemplate => "cim:VoltageLevel.Substation";
        protected override string Template => "cim:VoltageLevel";
       
    }
}
