namespace XmlParserTest
{
    class MachineNode : Node
    {
        public MachineNode(string line)
            :base(line)
        {
            
        }

        protected override string Template => "cim:SynchronousMachine";
        protected override string ParentTemplate => "cim:Equipment.EquipmentContainer";
        protected override string ToJson()
        {
            return $"\"{Name}\"";
        }
    }
}
