using System;
using Cosmos.IL2CPU.X86;
using XSharp.Compiler;
using CPUx86 = Cosmos.Assembler.x86;

namespace Cosmos.IL2CPU.X86.IL
{
	[Cosmos.IL2CPU.OpCode( ILOpCode.Code.And )]
	public class And : ILOp
	{
		public And( Cosmos.Assembler.Assembler aAsmblr )
			: base( aAsmblr )
		{
		}

		public override void Execute( MethodInfo aMethod, ILOpCode aOpCode )
		{
		    var xStackContent = aOpCode.StackPopTypes[0];
		    var xStackContentSize = SizeOfType(xStackContent);
            var xStackContent2 = aOpCode.StackPopTypes[1];
            var xStackContent2Size = SizeOfType(xStackContent2);


			var xSize = Math.Max( xStackContentSize, xStackContent2Size);
			if (ILOp.Align(xStackContentSize, 4u) != ILOp.Align(xStackContent2Size, 4u))
			{
				throw new NotSupportedException("Cosmos.IL2CPU.x86->IL->And.cs->Error: Operands have different size!");
			}
			if( xSize > 8 )
			{
				throw new NotImplementedException("Cosmos.IL2CPU.x86->IL->And.cs->Error: StackSize > 8 not supported");
			}

			if( xSize > 4 )
			{
				// [ESP] is low part
				// [ESP + 4] is high part
				// [ESP + 8] is low part
				// [ESP + 12] is high part
				XS.Pop(XSRegisters.OldToNewRegister(CPUx86.RegistersEnum.EAX));
				XS.Pop(XSRegisters.OldToNewRegister(CPUx86.RegistersEnum.EDX));
				// [ESP] is low part
				// [ESP + 4] is high part
				XS.And(XSRegisters.ESP, XSRegisters.EAX, destinationIsIndirect: true);
				new CPUx86.And { DestinationReg = CPUx86.RegistersEnum.ESP, DestinationIsIndirect = true, DestinationDisplacement = 4, SourceReg = CPUx86.RegistersEnum.EDX };
			}
			else
			{
                XS.Pop(XSRegisters.OldToNewRegister(CPUx86.RegistersEnum.EAX));
                XS.And(XSRegisters.ESP, XSRegisters.EAX, destinationIsIndirect: true);
			}
		}
	}
}
