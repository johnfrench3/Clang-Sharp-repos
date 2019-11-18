// Copyright (c) Microsoft and Contributors. All rights reserved. Licensed under the University of Illinois/NCSA Open Source License. See LICENSE.txt in the project root for license information.
// Ported from https://github.com/microsoft/ClangSharp/blob/master/sources/libClangSharp

namespace ClangSharp.Interop
{
    public enum CX_TypeClass
    {
        CX_TypeClass_Invalid,
        CX_TypeClass_Builtin,
        CX_TypeClass_Complex,
        CX_TypeClass_Pointer,
        CX_TypeClass_BlockPointer,
        CX_TypeClass_LValueReference,
        CX_TypeClass_RValueReference,
        CX_TypeClass_MemberPointer,
        CX_TypeClass_ConstantArray,
        CX_TypeClass_IncompleteArray,
        CX_TypeClass_VariableArray,
        CX_TypeClass_DependentSizedArray,
        CX_TypeClass_DependentSizedExtVector,
        CX_TypeClass_DependentAddressSpace,
        CX_TypeClass_Vector,
        CX_TypeClass_DependentVector,
        CX_TypeClass_ExtVector,
        CX_TypeClass_FunctionProto,
        CX_TypeClass_FunctionNoProto,
        CX_TypeClass_UnresolvedUsing,
        CX_TypeClass_Paren,
        CX_TypeClass_Typedef,
        CX_TypeClass_MacroQualified,
        CX_TypeClass_Adjusted,
        CX_TypeClass_Decayed,
        CX_TypeClass_TypeOfExpr,
        CX_TypeClass_TypeOf,
        CX_TypeClass_Decltype,
        CX_TypeClass_UnaryTransform,
        CX_TypeClass_Record,
        CX_TypeClass_Enum,
        CX_TypeClass_Elaborated,
        CX_TypeClass_Attributed,
        CX_TypeClass_TemplateTypeParm,
        CX_TypeClass_SubstTemplateTypeParm,
        CX_TypeClass_SubstTemplateTypeParmPack,
        CX_TypeClass_TemplateSpecialization,
        CX_TypeClass_Auto,
        CX_TypeClass_DeducedTemplateSpecialization,
        CX_TypeClass_InjectedClassName,
        CX_TypeClass_DependentName,
        CX_TypeClass_DependentTemplateSpecialization,
        CX_TypeClass_PackExpansion,
        CX_TypeClass_ObjCTypeParam,
        CX_TypeClass_ObjCObject,
        CX_TypeClass_ObjCInterface,
        CX_TypeClass_ObjCObjectPointer,
        CX_TypeClass_Pipe,
        CX_TypeClass_Atomic,
        CX_TypeClass_TypeLast = CX_TypeClass_Atomic,
        CX_TypeClass_TagFirst = CX_TypeClass_Record,
        CX_TypeClass_TagLast = CX_TypeClass_Enum,
    }
}
