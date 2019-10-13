module Fint.Enums

open System

type TargetArchitecture =
    | I386 = 0x014c
    | AMD64 = 0x8664
    | IA64 = 0x0200
    | ARM = 0x01c0
    | ARMv7 = 0x01c4
    | ARM64 = 0xaa64
    | AC020 = 0xc020

type ModuleKind =
    | Dll = 0
    | Windows = 1
    | Console = 2

[<FlagsAttribute>]
type ModuleAttributes =
    | ILOnly = 1
    | Required32Bit = 2
    | StrongNameSigned = 8
    | Preferred32Bit = 0x00020000

[<FlagsAttribute>]
type ModuleCharacteristics =
    | HighEntropyVA = 0x0020
    | DynamicBase = 0x0040
    | NoSEH = 0x0400
    | NXCompat = 0x0100
    | AppContainer = 0x1000
    | TerminalServerAware = 0x8000

// Method metadata attributes
[<FlagsAttribute>]
type MethodAttributes =
    // member access mask - Use this mask to retrieve accessibility information.
    | MemberAccessMask = 0x0007
    // Indicates that the member cannot be referenced.
    | PrivateScope = 0x0000
    // Indicates that the method is accessible only to the current class.  
    | Private = 0x0001
    // Indicates that the method is accessible to members of this type and its derived types that are in this assembly only.
    | FamANDAssem = 0x0002
    // Indicates that the method is accessible to any class of this assembly
    | Assembly = 0x0003
    // Indicates that the method is accessible only to members of this class and its derived classes.    
    | Family = 0x0004
    // Indicates that the method is accessible to derived classes anywhere, as well as to any class in the assembly.
    | FamORAssem = 0x0005
    // Indicates that the method is accessible to any object for which this object is in scope.   
    | Public = 0x0006
    // Indicates that the method is defined on the type; otherwise, it is defined per instance.
    | Static = 0x0010
    // Indicates that the method cannot be overridden.
    | Final = 0x0020
    // Indicates that the method is virtual.
    | Virtual = 0x0040
    // Indicates that the method hides by name and signature; otherwise, by name only.
    | HideBySig = 0x0080
    // vtable layout mask - Use this mask to retrieve vtable attributes.
    | VtableLayoutMask = 0x0100
    // Indicates that the method will reuse an existing slot in the vtable. This is the default behavior.
    | ReuseSlot = 0x0000
    // Indicates that the method always gets a new slot in the vtable.
    | NewSlot = 0x0100
    // Indicates that the class does not provide an implementation of this method.
    | Abstract = 0x0400
    // Indicates that the method is special. The name describes how this method is special.
    | SpecialName = 0x0800
    // Indicates that the method implementation is forwarded through PInvoke (Platform Invocation Services).
    | PinvokeImpl = 0x2000
    // Indicates that the managed method is exported by thunk to unmanaged code. 
    | UnmanagedExport = 0x0008
    // Indicates that the common language runtime checks the name encoding.
    | RTSpecialName = 0x1000
    // Reserved flags for runtime use only. 
    | ReservedMask = 0xd000
    // Indicates that the method has security associated with it. Reserved flag for runtime use only.
    | HasSecurity = 0x4000
    // Indicates that the method calls another method containing security code. Reserved flag for runtime use only.
    | RequireSecObject = 0x8000

// Defines metadata table codes
type TableId =
    | Assembly = 0x20
    | AssemblyOS = 0x22
    | AssemblyProcessor = 0x21
    | AssemblyRef = 0x23
    | AssemblyRefOS = 0x25
    | AssemblyRefProcessor = 0x24
    | ClassLayout = 0x0F
    | Constant = 0x0B
    | CustomAttribute = 0x0C
    | DeclSecurity = 0x0E
    | EventMap = 0x12
    | Event = 0x14
    | ExportedType = 0x27
    | Field = 0x04
    | FieldLayout = 0x10
    | FieldMarshal = 0x0D
    | FieldRVA = 0x1D
    | File = 0x26
    | GenericParam = 0x2A
    | GenericParamConstraint = 0x2C
    | ImplMap = 0x1C
    | InterfaceImpl = 0x09
    | ManifestResource = 0x28
    | MemberRef = 0x0A
    | MethodDef = 0x06
    | MethodImpl = 0x19
    | MethodSemantics = 0x18
    | MethodSpec = 0x2B
    | Module = 0x00
    | ModuleRef = 0x1A
    | NestedClass = 0x29
    | Param = 0x08
    | Property = 0x17
    | PropertyMap = 0x15
    | StandAloneSig = 0x11
    | TypeDef = 0x02
    | TypeRef = 0x01
    | TypeSpec = 0x1B
    | FieldPtr = 3
    | MethodPtr = 5
    | ParamPtr = 7
    | EventPtr = 19
    | PropertyPtr = 22
    | EncodingLog = 30
    | EncodingMap = 31

type BranchOp =
    | False = 0
    | True = 1
    | Null = 2
    | NotNull = 3
    | Equal = 4 
    | NotEqual = 5
    | NotEqualUnsigned = 6
    | LessThan = 7
    | LessThanUnsigned = 8
    | LessThanOrEqual = 9
    | LessThanOrEqualUnsigned = 10
    | GreaterThan = 11
    | GreaterThanUnsigned = 12
    | GreaterThanOrEqual = 13
    | GreaterThanOrEqualUnsigned = 14

type CalcOp =
    | Add = 0
    | AddUnsigned = 1
    | Sub = 2
    | SubUnsigned = 3
    | Mul = 4
    | MulUnsigned = 5
    | Div = 6
    | DivUnsigned = 7
    | Rem = 8
    | RemUnsigned = 9
    | BitwiseAnd = 10
    | BitwiseOr = 11
    | Xor = 12
    | ShiftLeft = 13
    | ShiftRight = 14
    | ShiftRightUnsigned = 15
    | Neg = 16
    | Not = 17
    | Equal = 18
    | LessThan = 19
    | LessThanUnsigned = 20
    | GreaterThan = 21
    | GreaterThanUnsigned = 22

// Enumerates CIL instruction codes
type InstructionCode =
    // Fills space if opcodes are patched. No meaningful operation is performed although a processing cycle can be consumed.
    | Nop = 0
    // Signals the Common Language Infrastructure (CLI) to inform the debugger that a break point has been tripped.
    | Break = 1
    // Loads the argument at index 0 onto the evaluation stack.
    | Ldarg_0 = 2
    // Loads the argument at index 1 onto the evaluation stack.
    | Ldarg_1 = 3
    // Loads the argument at index 2 onto the evaluation stack.
    | Ldarg_2 = 4
    // Loads the argument at index 3 onto the evaluation stack.
    | Ldarg_3 = 5
    // Loads the local variable at index 0 onto the evaluation stack.
    | Ldloc_0 = 6
    // Loads the local variable at index 1 onto the evaluation stack.
    | Ldloc_1 = 7
    // Loads the local variable at index 2 onto the evaluation stack.
    | Ldloc_2 = 8
    // Loads the local variable at index 3 onto the evaluation stack.
    | Ldloc_3 = 9
    // Pops the current value from the top of the evaluation stack and stores it in a the local variable list at index 0.
    | Stloc_0 = 10
    // Pops the current value from the top of the evaluation stack and stores it in a the local variable list at index 1.
    | Stloc_1 = 11
    // Pops the current value from the top of the evaluation stack and stores it in a the local variable list at index 2.
    | Stloc_2 = 12
    // Pops the current value from the top of the evaluation stack and stores it in a the local variable list at index 3.
    | Stloc_3 = 13
    // Loads the argument (referenced by a specified short form index) onto the evaluation stack.
    | Ldarg_S = 14
    // Load an argument address, in short form, onto the evaluation stack.
    | Ldarga_S = 15
    // Stores the value on top of the evaluation stack in the argument slot at a specified index, short form.
    | Starg_S = 16
    // Loads the local variable at a specific index onto the evaluation stack, short form.
    | Ldloc_S = 17
    // Loads the address of the local variable at a specific index onto the evaluation stack, short form.
    | Ldloca_S = 18
    // Pops the current value from the top of the evaluation stack and stores it in a the local variable list at index (short form).
    | Stloc_S = 19
    // Pushes a null reference (type O) onto the evaluation stack.
    | Ldnull = 20
    // Pushes the integer value of -1 onto the evaluation stack as an int32.
    | Ldc_I4_M1 = 21
    // Pushes the integer value of 0 onto the evaluation stack as an int32.
    | Ldc_I4_0 = 22
    // Pushes the integer value of 1 onto the evaluation stack as an int32.
    | Ldc_I4_1 = 23
    // Pushes the integer value of 2 onto the evaluation stack as an int32.
    | Ldc_I4_2 = 24
    // Pushes the integer value of 3 onto the evaluation stack as an int32.
    | Ldc_I4_3 = 25
    // Pushes the integer value of 4 onto the evaluation stack as an int32.
    | Ldc_I4_4 = 26
    // Pushes the integer value of 5 onto the evaluation stack as an int32.
    | Ldc_I4_5 = 27
    // Pushes the integer value of 6 onto the evaluation stack as an int32.
    | Ldc_I4_6 = 28
    // Pushes the integer value of 7 onto the evaluation stack as an int32.
    | Ldc_I4_7 = 29
    // Pushes the integer value of 8 onto the evaluation stack as an int32.
    | Ldc_I4_8 = 30
    // Pushes the supplied int8 value onto the evaluation stack as an int32 short form.
    | Ldc_I4_S = 31
    // Pushes a supplied value of type int32 onto the evaluation stack as an int32.
    | Ldc_I4 = 32
    // Pushes a supplied value of type int64 onto the evaluation stack as an int64.
    | Ldc_I8 = 33
    // Pushes a supplied value of type float32 onto the evaluation stack as type F (float).
    | Ldc_R4 = 34
    // Pushes a supplied value of type float64 onto the evaluation stack as type F (float).
    | Ldc_R8 = 35
    // Copies the current topmost value on the evaluation stack, and then pushes the copy onto the evaluation stack.
    | Dup = 37
    // Removes the value currently on top of the evaluation stack.
    | Pop = 38
    // Exits current method and jumps to specified method.
    | Jmp = 39
    // Calls the method indicated by the passed method descriptor.
    | Call = 40
    // Calls the method indicated on the evaluation stack (as a pointer to an entry point) with arguments described by a calling convention.
    | Calli = 41
    // Returns from the current method, pushing a return value (if present) from the callee&apos;s evaluation stack onto the caller&apos;s evaluation stack.
    | Ret = 42
    // Unconditionally transfers control to a target instruction (short form).
    | Br_S = 43
    // Transfers control to a target instruction if value is false, a null reference, or zero.
    | Brfalse_S = 44
    // Transfers control to a target instruction (short form) if value is true, not null, or non-zero.
    | Brtrue_S = 45
    // Transfers control to a target instruction (short form) if two values are equal.
    | Beq_S = 46
    // Transfers control to a target instruction (short form) if the first value is greater than or equal to the second value.
    | Bge_S = 47
    // Transfers control to a target instruction (short form) if the first value is greater than the second value.
    | Bgt_S = 48
    // Transfers control to a target instruction (short form) if the first value is less than or equal to the second value.
    | Ble_S = 49
    // Transfers control to a target instruction (short form) if the first value is less than the second value.
    | Blt_S = 50
    // Transfers control to a target instruction (short form) when two unsigned integer values or unordered float values are not equal.
    | Bne_Un_S = 51
    // Transfers control to a target instruction (short form) if the first value is greater than the second value, when comparing unsigned integer values or unordered float values.
    | Bge_Un_S = 52
    // Transfers control to a target instruction (short form) if the first value is greater than the second value, when comparing unsigned integer values or unordered float values.
    | Bgt_Un_S = 53
    // Transfers control to a target instruction (short form) if the first value is less than or equal to the second value, when comparing unsigned integer values or unordered float values.
    | Ble_Un_S = 54
    // Transfers control to a target instruction (short form) if the first value is less than the second value, when comparing unsigned integer values or unordered float values.
    | Blt_Un_S = 55
    // Unconditionally transfers control to a target instruction.
    | Br = 56
    // Transfers control to a target instruction if value is false, a null reference (Nothing in Visual Basic), or zero.
    | Brfalse = 57
    // Transfers control to a target instruction if value is true, not null, or non-zero.
    | Brtrue = 58
    // Transfers control to a target instruction if two values are equal.
    | Beq = 59
    // Transfers control to a target instruction if the first value is greater than or equal to the second value.
    | Bge = 60
    // Transfers control to a target instruction if the first value is greater than the second value.
    | Bgt = 61
    // Transfers control to a target instruction if the first value is less than or equal to the second value.
    | Ble = 62
    // Transfers control to a target instruction if the first value is less than the second value.
    | Blt = 63
    // Transfers control to a target instruction when two unsigned integer values or unordered float values are not equal.
    | Bne_Un = 64
    // Transfers control to a target instruction if the first value is greater than the second value, when comparing unsigned integer values or unordered float values.
    | Bge_Un = 65
    // Transfers control to a target instruction if the first value is greater than the second value, when comparing unsigned integer values or unordered float values.
    | Bgt_Un = 66
    // Transfers control to a target instruction if the first value is less than or equal to the second value, when comparing unsigned integer values or unordered float values.
    | Ble_Un = 67
    // Transfers control to a target instruction if the first value is less than the second value, when comparing unsigned integer values or unordered float values.
    | Blt_Un = 68
    // Implements a jump table.
    | Switch = 69
    // Loads a value of type int8 as an int32 onto the evaluation stack indirectly.
    | Ldind_I1 = 70
    // Loads a value of type unsigned int8 as an int32 onto the evaluation stack indirectly.
    | Ldind_U1 = 71
    // Loads a value of type int16 as an int32 onto the evaluation stack indirectly.
    | Ldind_I2 = 72
    // Loads a value of type unsigned int16 as an int32 onto the evaluation stack indirectly.
    | Ldind_U2 = 73
    // Loads a value of type int32 as an int32 onto the evaluation stack indirectly.
    | Ldind_I4 = 74
    // Loads a value of type unsigned int32 as an int32 onto the evaluation stack indirectly.
    | Ldind_U4 = 75
    // Loads a value of type int64 as an int64 onto the evaluation stack indirectly.
    | Ldind_I8 = 76
    // Loads a value of type natural int as a natural int onto the evaluation stack indirectly.
    | Ldind_I = 77
    // Loads a value of type float32 as a type F (float) onto the evaluation stack indirectly.
    | Ldind_R4 = 78
    // Loads a value of type float64 as a type F (float) onto the evaluation stack indirectly.
    | Ldind_R8 = 79
    // Loads an object reference as a type O (object reference) onto the evaluation stack indirectly.
    | Ldind_Ref = 80
    // Stores a object reference value at a supplied address.
    | Stind_Ref = 81
    // Stores a value of type int8 at a supplied address.
    | Stind_I1 = 82
    // Stores a value of type int16 at a supplied address.
    | Stind_I2 = 83
    // Stores a value of type int32 at a supplied address.
    | Stind_I4 = 84
    // Stores a value of type int64 at a supplied address.
    | Stind_I8 = 85
    // Stores a value of type float32 at a supplied address.
    | Stind_R4 = 86
    // Stores a value of type float64 at a supplied address.
    | Stind_R8 = 87
    // Adds two values and pushes the result onto the evaluation stack.
    | Add = 88
    // Subtracts one value from another and pushes the result onto the evaluation stack.
    | Sub = 89
    // Multiplies two values and pushes the result on the evaluation stack.
    | Mul = 90
    // Divides two values and pushes the result as a floating-point (type F) or quotient (type int32) onto the evaluation stack.
    | Div = 91
    // Divides two unsigned integer values and pushes the result (int32) onto the evaluation stack.
    | Div_Un = 92
    // Divides two values and pushes the remainder onto the evaluation stack.
    | Rem = 93
    // Divides two unsigned values and pushes the remainder onto the evaluation stack.
    | Rem_Un = 94
    // Computes the bitwise AND of two values and pushes the result onto the evaluation stack.
    | And = 95
    // Compute the bitwise complement of the two integer values on top of the stack and pushes the result onto the evaluation stack.
    | Or = 96
    // Computes the bitwise XOR of the top two values on the evaluation stack, pushing the result onto the evaluation stack.
    | Xor = 97
    // Shifts an integer value to the left (in zeroes) by a specified number of bits, pushing the result onto the evaluation stack.
    | Shl = 98
    // Shifts an integer value (in sign) to the right by a specified number of bits, pushing the result onto the evaluation stack.
    | Shr = 99
    // Shifts an unsigned integer value (in zeroes) to the right by a specified number of bits, pushing the result onto the evaluation stack.
    | Shr_Un = 100
    // Negates a value and pushes the result onto the evaluation stack.
    | Neg = 101
    // Computes the bitwise complement of the integer value on top of the stack and pushes the result onto the evaluation stack as the same type.
    | Not = 102
    // Converts the value on top of the evaluation stack to int8 then extends (pads) it to int32.
    | Conv_I1 = 103
    // Converts the value on top of the evaluation stack to int16 then extends (pads) it to int32.
    | Conv_I2 = 104
    // Converts the value on top of the evaluation stack to int32.
    | Conv_I4 = 105
    // Converts the value on top of the evaluation stack to int64.
    | Conv_I8 = 106
    // Converts the value on top of the evaluation stack to float32.
    | Conv_R4 = 107
    // Converts the value on top of the evaluation stack to float64.
    | Conv_R8 = 108
    // Converts the value on top of the evaluation stack to unsigned int32 and extends it to int32.
    | Conv_U4 = 109
    // Converts the value on top of the evaluation stack to unsigned int64 and extends it to int64.
    | Conv_U8 = 110
    // Calls a late-bound method on an object, pushing the return value onto the evaluation stack.
    | Callvirt = 111
    // Copies the value type located at the address of an object (type &amp;, * or natural int) to the address of the destination object (type &amp;, * or natural int).
    | Cpobj = 112
    // Copies the value type object pointed to by an address to the top of the evaluation stack.
    | Ldobj = 113
    // Pushes a new object reference to a string literal stored in the metadata.
    | Ldstr = 114
    // Creates a new object or a new instance of a value type, pushing an object reference (type O) onto the evaluation stack.
    | Newobj = 115
    // Attempts to cast an object passed by reference to the specified class.
    | Castclass = 116
    // Tests whether an object reference (type O) is an instance of a particular class.
    | Isinst = 117
    // Converts the unsigned integer value on top of the evaluation stack to float32.
    | Conv_R_Un = 118
    // Converts the boxed representation of a value type to its unboxed form.
    | Unbox = 121
    // Throws the exception object currently on the evaluation stack.
    | Throw = 122
    // Finds the value of a field in the object whose reference is currently on the evaluation stack.
    | Ldfld = 123
    // Finds the address of a field in the object whose reference is currently on the evaluation stack.
    | Ldflda = 124
    // Replaces the value stored in the field of an object reference or pointer with a new value.
    | Stfld = 125
    // Pushes the value of a static field onto the evaluation stack.
    | Ldsfld = 126
    // Pushes the address of a static field onto the evaluation stack.
    | Ldsflda = 127
    // Replaces the value of a static field with a value from the evaluation stack.
    | Stsfld = 128
    // Copies a value of a specified type from the evaluation stack into a supplied memory address.
    | Stobj = 129
    // Converts the unsigned value on top of the evaluation stack to signed int8 and extends it to int32 throwing  on overflow.
    | Conv_Ovf_I1_Un = 130
    // Converts the unsigned value on top of the evaluation stack to signed int16 and extends it to int32 throwing  on overflow.
    | Conv_Ovf_I2_Un = 131
    // Converts the unsigned value on top of the evaluation stack to signed int32 throwing  on overflow.
    | Conv_Ovf_I4_Un = 132
    // Converts the unsigned value on top of the evaluation stack to signed int64 throwing  on overflow.
    | Conv_Ovf_I8_Un = 133
    // Converts the unsigned value on top of the evaluation stack to unsigned int8 and extends it to int32 throwing  on overflow.
    | Conv_Ovf_U1_Un = 134
    // Converts the unsigned value on top of the evaluation stack to unsigned int16 and extends it to int32 throwing  on overflow.
    | Conv_Ovf_U2_Un = 135
    // Converts the unsigned value on top of the evaluation stack to unsigned int32 throwing  on overflow.
    | Conv_Ovf_U4_Un = 136
    // Converts the unsigned value on top of the evaluation stack to unsigned int64 throwing  on overflow.
    | Conv_Ovf_U8_Un = 137
    // Converts the unsigned value on top of the evaluation stack to signed natural int, throwing  on overflow.
    | Conv_Ovf_I_Un = 138
    // Converts the unsigned value on top of the evaluation stack to unsigned natural int, throwing  on overflow.
    | Conv_Ovf_U_Un = 139
    // Converts a value type to an object reference (type O).
    | Box = 140
    // Pushes an object reference to a new zero-based, one-dimensional array whose elements are of a specific type onto the evaluation stack.
    | Newarr = 141
    // Pushes the number of elements of a zero-based, one-dimensional array onto the evaluation stack.
    | Ldlen = 142
    // Loads the address of the array element at a specified array index onto the top of the evaluation stack as type &amp; (managed pointer).
    | Ldelema = 143
    // Loads the element with type int8 at a specified array index onto the top of the evaluation stack as an int32.
    | Ldelem_I1 = 144
    // Loads the element with type unsigned int8 at a specified array index onto the top of the evaluation stack as an int32.
    | Ldelem_U1 = 145
    // Loads the element with type int16 at a specified array index onto the top of the evaluation stack as an int32.
    | Ldelem_I2 = 146
    // Loads the element with type unsigned int16 at a specified array index onto the top of the evaluation stack as an int32.
    | Ldelem_U2 = 147
    // Loads the element with type int32 at a specified array index onto the top of the evaluation stack as an int32.
    | Ldelem_I4 = 148
    // Loads the element with type unsigned int32 at a specified array index onto the top of the evaluation stack as an int32.
    | Ldelem_U4 = 149
    // Loads the element with type int64 at a specified array index onto the top of the evaluation stack as an int64.
    | Ldelem_I8 = 150
    // Loads the element with type natural int at a specified array index onto the top of the evaluation stack as a natural int.
    | Ldelem_I = 151
    // Loads the element with type float32 at a specified array index onto the top of the evaluation stack as type F (float).
    | Ldelem_R4 = 152
    // Loads the element with type float64 at a specified array index onto the top of the evaluation stack as type F (float).
    | Ldelem_R8 = 153
    // Loads the element containing an object reference at a specified array index onto the top of the evaluation stack as type O (object reference).
    | Ldelem_Ref = 154
    // Replaces the array element at a given index with the natural int value on the evaluation stack.
    | Stelem_I = 155
    // Replaces the array element at a given index with the int8 value on the evaluation stack.
    | Stelem_I1 = 156
    // Replaces the array element at a given index with the int16 value on the evaluation stack.
    | Stelem_I2 = 157
    // Replaces the array element at a given index with the int32 value on the evaluation stack.
    | Stelem_I4 = 158
    // Replaces the array element at a given index with the int64 value on the evaluation stack.
    | Stelem_I8 = 159
    // Replaces the array element at a given index with the float32 value on the evaluation stack.
    | Stelem_R4 = 160
    // Replaces the array element at a given index with the float64 value on the evaluation stack.
    | Stelem_R8 = 161
    // Replaces the array element at a given index with the object ref value (type O) on the evaluation stack.
    | Stelem_Ref = 162
    // Loads the element at a specified array index onto the top of the evaluation stack as the type specified in the instruction.
    | Ldelem = 163
    // Replaces the array element at a given index with the value on the evaluation stack, whose type is specified in the instruction.
    | Stelem = 164
    // Converts the boxed representation of a type specified in the instruction to its unboxed form.
    | Unbox_Any = 165
    // Converts the signed value on top of the evaluation stack to signed int8 and extends it to int32 throwing  on overflow.
    | Conv_Ovf_I1 = 179
    // Converts the signed value on top of the evaluation stack to unsigned int8 and extends it to int32 throwing  on overflow.
    | Conv_Ovf_U1 = 180
    // Converts the signed value on top of the evaluation stack to signed int16 and extending it to int32 throwing  on overflow.
    | Conv_Ovf_I2 = 181
    // Converts the signed value on top of the evaluation stack to unsigned int16 and extends it to int32 throwing  on overflow.
    | Conv_Ovf_U2 = 182
    // Converts the signed value on top of the evaluation stack to signed int32 throwing  on overflow.
    | Conv_Ovf_I4 = 183
    // Converts the signed value on top of the evaluation stack to unsigned int32 throwing  on overflow.
    | Conv_Ovf_U4 = 184
    // Converts the signed value on top of the evaluation stack to signed int64 throwing  on overflow.
    | Conv_Ovf_I8 = 185
    // Converts the signed value on top of the evaluation stack to unsigned int64 throwing  on overflow.
    | Conv_Ovf_U8 = 186
    // Retrieves the address (type &amp;) embedded in a typed reference.
    | Refanyval = 194
    // Throws  if value is not a finite number.
    | Ckfinite = 195
    // Pushes a typed reference to an instance of a specific type onto the evaluation stack.
    | Mkrefany = 198
    // Converts a metadata token to its runtime representation, pushing it onto the evaluation stack.
    | Ldtoken = 208
    // Converts the value on top of the evaluation stack to unsigned int16 and extends it to int32.
    | Conv_U2 = 209
    // Converts the value on top of the evaluation stack to unsigned int8 and extends it to int32.
    | Conv_U1 = 210
    // Converts the value on top of the evaluation stack to natural int.
    | Conv_I = 211
    // Converts the signed value on top of the evaluation stack to signed natural int, throwing  on overflow.
    | Conv_Ovf_I = 212
    // Converts the signed value on top of the evaluation stack to unsigned natural int, throwing  on overflow.
    | Conv_Ovf_U = 213
    // Adds two integers, performs an overflow check, and pushes the result onto the evaluation stack.
    | Add_Ovf = 214
    // Adds two unsigned integer values, performs an overflow check, and pushes the result onto the evaluation stack.
    | Add_Ovf_Un = 215
    // Multiplies two integer values, performs an overflow check, and pushes the result onto the evaluation stack.
    | Mul_Ovf = 216
    // Multiplies two unsigned integer values, performs an overflow check, and pushes the result onto the evaluation stack.
    | Mul_Ovf_Un = 217
    // Subtracts one integer value from another, performs an overflow check, and pushes the result onto the evaluation stack.
    | Sub_Ovf = 218
    // Subtracts one unsigned integer value from another, performs an overflow check, and pushes the result onto the evaluation stack.
    | Sub_Ovf_Un = 219
    // Transfers control from the fault or finally clause of an exception block back to the Common Language Infrastructure (CLI) exception handler.
    | Endfinally = 220
    // Exits a protected region of code, unconditionally transferring control to a specific target instruction.
    | Leave = 221
    // Exits a protected region of code, unconditionally transferring control to a target instruction (short form).
    | Leave_S = 222
    // Stores a value of type natural int at a supplied address.
    | Stind_I = 223
    // Converts the value on top of the evaluation stack to unsigned natural int, and extends it to natural int.
    | Conv_U = 224
    // This is a reserved instruction.
    | Prefix7 = 248
    // This is a reserved instruction.
    | Prefix6 = 249
    // This is a reserved instruction.
    | Prefix5 = 250
    // This is a reserved instruction.
    | Prefix4 = 251
    // This is a reserved instruction.
    | Prefix3 = 252
    // This is a reserved instruction.
    | Prefix2 = 253
    // This is a reserved instruction.
    | Prefix1 = 254
    // This is a reserved instruction.
    | Prefixref = 255
    // Returns an unmanaged pointer to the argument list of the current method.
    | Arglist = -512
    // Compares two values. If they are equal, the integer value 1 (int32) is pushed onto the evaluation stack; otherwise 0 (int32) is pushed onto the evaluation stack.
    | Ceq = -511
    // Compares two values. If the first value is greater than the second, the integer value 1 (int32) is pushed onto the evaluation stack; otherwise 0 (int32) is pushed onto the evaluation stack.
    | Cgt = -510
    // Compares two unsigned or unordered values. If the first value is greater than the second, the integer value 1 (int32) is pushed onto the evaluation stack; otherwise 0 (int32) is pushed onto the evaluation stack.
    | Cgt_Un = -509
    // Compares two values. If the first value is less than the second, the integer value 1 (int32) is pushed onto the evaluation stack; otherwise 0 (int32) is pushed onto the evaluation stack.
    | Clt = -508
    // Compares the unsigned or unordered values value1 and value2. If value1 is less than value2 then the integer value 1 (int32) is pushed onto the evaluation stack; otherwise 0 (int32) is pushed onto the evaluation stack.
    | Clt_Un = -507
    // Pushes an unmanaged pointer (type natural int) to the native code implementing a specific method onto the evaluation stack.
    | Ldftn = -506
    // Pushes an unmanaged pointer (type natural int) to the native code implementing a particular virtual method associated with a specified object onto the evaluation stack.
    | Ldvirtftn = -505
    // Loads an argument (referenced by a specified index value) onto the stack.
    | Ldarg = -503
    // Load an argument address onto the evaluation stack.
    | Ldarga = -502
    // Stores the value on top of the evaluation stack in the argument slot at a specified index.
    | Starg = -501
    // Loads the local variable at a specific index onto the evaluation stack.
    | Ldloc = -500
    // Loads the address of the local variable at a specific index onto the evaluation stack.
    | Ldloca = -499
    // Pops the current value from the top of the evaluation stack and stores it in a the local variable list at a specified index.
    | Stloc = -498
    // Allocates a certain number of bytes from the local dynamic memory pool and pushes the address (a transient pointer, type *) of the first allocated byte onto the evaluation stack.
    | Localloc = -497
    // Transfers control from the filter clause of an exception back to the Common Language Infrastructure (CLI) exception handler.
    | Endfilter = -495
    // Indicates that an address currently atop the evaluation stack might not be aligned to the natural size of the immediately following ldind, stind, ldfld, stfld, ldobj, stobj, initblk, or cpblk instruction.
    | Unaligned = -494
    // Specifies that an address currently atop the evaluation stack might be volatile, and the results of reading that location cannot be cached or that multiple stores to that location cannot be suppressed.
    | Volatile = -493
    // Performs a postfixed method call instruction such that the current method&apos;s stack frame is removed before the actual call instruction is executed.
    | Tailcall = -492
    // Initializes all the fields of the object at a specific address to a null reference or a 0 of the appropriate primitive type.
    | Initobj = -491
    // Constrains the type on which a virtual method call is made.
    | Constrained = -490
    // Copies a specified number bytes from a source address to a destination address.
    | Cpblk = -489
    // Initializes a specified block of memory at a specific address to a given size and initial value.
    | Initblk = -488
    // Rethrows the current exception.
    | Rethrow = -486
    // Pushes the size, in bytes, of a supplied value type onto the evaluation stack.
    | Sizeof = -484
    // Retrieves the type token embedded in a typed reference.
    | Refanytype = -483
    // Specifies that the subsequent array address operation performs no type check at run time, and that it returns a managed pointer whose mutability is restricted.
    | Readonly = -482
