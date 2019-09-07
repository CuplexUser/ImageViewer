using System.Collections.Generic;
using Fody;
using Mono.Cecil;



namespace ImageView.Library
{
    public class ModuleWeaver : BaseModuleWeaver
    {
        public override void Execute()
        {
            var objectType = FindType("System.Object");
            var objectImport = ModuleDefinition.ImportReference(objectType);
            ModuleDefinition.Types.Add(new TypeDefinition("MyNamespace", "MyType", TypeAttributes.Public, objectImport));
        }

        public override IEnumerable<string> GetAssembliesForScanning()
        {
            yield return "mscorlib";
            yield return "System";
        }
    }

}