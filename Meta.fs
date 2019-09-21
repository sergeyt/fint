module Fint.Meta

open Fint.Enums

type CodedIndexId =
    | CustomAttributeType
    | HasConstant
    | HasCustomAttribute
    | HasDeclSecurity
    | HasFieldMarshal
    | HasSemantics
    | Implementation
    | MemberForwarded
    | MemberRefParent
    | MethodDefOrRef
    | ResolutionScope
    | TypeDefOrRef
    | TypeOrMethodDef

type CodedIndex =
    { bits : int
      tables : TableId list }

let customAttributeType : CodedIndex =
    { bits = 3
      tables =
          [ TableId.TypeRef; TableId.TypeRef; TableId.MethodDef;
            TableId.MemberRef; TableId.TypeDef ] }

let codedIndexMap =
    dict [ (CustomAttributeType, customAttributeType)
           (HasConstant,
            { bits = 2
              tables = [ TableId.Field; TableId.Param; TableId.Property ] })

           (//NOTE FROM SPEC:
            //[Note: HasCustomAttributes only has values for tables that are �externally visible�; that is, that correspond to items
            //in a user source program. For example, an attribute can be attached to a TypeDef table and a Field table, but not a
            //ClassLayout table. As a result, some table types are missing from the enum above.]
            HasCustomAttribute,
            { bits = 5
              tables =
                  [ TableId.MethodDef; TableId.Field; TableId.TypeRef;
                    TableId.TypeDef; TableId.Param; TableId.InterfaceImpl;
                    TableId.MemberRef; TableId.Module; TableId.DeclSecurity;
                    TableId.Property; TableId.Event; TableId.StandAloneSig;
                    TableId.ModuleRef; TableId.TypeSpec; TableId.Assembly;
                    TableId.AssemblyRef; TableId.File; TableId.ExportedType;
                    TableId.ManifestResource; TableId.GenericParam ] })

           (HasDeclSecurity,
            { bits = 2
              tables = [ TableId.TypeDef; TableId.MethodDef; TableId.Assembly ] })
           (HasFieldMarshal,
            { bits = 1
              tables = [ TableId.Field; TableId.Param ] })
           (HasSemantics,
            { bits = 1
              tables = [ TableId.Event; TableId.Property ] })

           (Implementation,
            { bits = 2
              tables =
                  [ TableId.File; TableId.AssemblyRef; TableId.ExportedType ] })
           (MemberForwarded,
            { bits = 1
              tables = [ TableId.Field; TableId.MethodDef ] })

           (MemberRefParent,
            { bits = 3
              tables =
                  [ TableId.TypeDef; TableId.TypeRef; TableId.ModuleRef;
                    TableId.MethodDef; TableId.TypeSpec ] })
           (MethodDefOrRef,
            { bits = 1
              tables = [ TableId.MethodDef; TableId.MemberRef ] })

           (ResolutionScope,
            { bits = 2
              tables =
                  [ TableId.Module; TableId.ModuleRef; TableId.AssemblyRef;
                    TableId.TypeRef ] })
           (TypeDefOrRef,
            { bits = 2
              tables = [ TableId.TypeDef; TableId.TypeRef; TableId.TypeSpec ] })
           (TypeOrMethodDef,
            { bits = 1
              tables = [ TableId.TypeDef; TableId.MethodDef ] }) ]
