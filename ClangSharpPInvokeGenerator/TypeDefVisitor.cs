﻿namespace ClangSharpPInvokeGenerator
{
    using System;
    using System.IO;
    using System.Collections.Generic;
    using ClangSharp;

    internal sealed class TypeDefVisitor : ICXCursorVisitor
    {
        private readonly TextWriter tw;

        private readonly HashSet<string> visitedTypeDefs = new HashSet<string>();

        public TypeDefVisitor(TextWriter tw)
        {
            this.tw = tw;
        }

        public CXChildVisitResult Visit(CXCursor cursor, CXCursor parent, IntPtr data)
        {
            CXCursorKind curKind = Methods.clang_getCursorKind(cursor);
            if (curKind == CXCursorKind.CXCursor_TypedefDecl)
            {
                var spelling = Methods.clang_getCursorSpelling(cursor).ToString();

                if (this.visitedTypeDefs.Contains(spelling))
                {
                    return CXChildVisitResult.CXChildVisit_Continue;
                }

                this.visitedTypeDefs.Add(spelling);

                CXType type = Methods.clang_getCanonicalType(Methods.clang_getTypedefDeclUnderlyingType(cursor));

                // we handle enums and records in struct and enum visitors with forward declarations also
                if (type.kind == CXTypeKind.CXType_Record || type.kind == CXTypeKind.CXType_Enum)
                {
                    return CXChildVisitResult.CXChildVisit_Continue;
                }

                // no idea what this is? -- template stuff?
                if (type.kind == CXTypeKind.CXType_Unexposed)
                {
                    var canonical = Methods.clang_getCanonicalType(type);
                    if (canonical.kind == CXTypeKind.CXType_Unexposed)
                    {
                        return CXChildVisitResult.CXChildVisit_Continue; 
                    }
                }

                if (type.kind == CXTypeKind.CXType_Pointer)
                {
                    var pointee = Methods.clang_getPointeeType(type);
                    if (pointee.kind == CXTypeKind.CXType_Record || pointee.kind == CXTypeKind.CXType_Void)
                    {
                        this.tw.WriteLine("    public partial struct " + spelling);
                        this.tw.WriteLine("    {");
                        this.tw.WriteLine("        public " + spelling + "(IntPtr pointer)");
                        this.tw.WriteLine("        {");
                        this.tw.WriteLine("            this.Pointer = pointer;");
                        this.tw.WriteLine("        }");
                        this.tw.WriteLine();
                        this.tw.WriteLine("        public IntPtr Pointer;");
                        this.tw.WriteLine("    }");
                        this.tw.WriteLine();

                        return CXChildVisitResult.CXChildVisit_Continue;
                    }

                    if (pointee.kind == CXTypeKind.CXType_FunctionProto)
                    {
                        this.tw.WriteLine("    [UnmanagedFunctionPointer(" + pointee.CallingConventionSpelling() + ")]");
                        this.tw.Write("    public delegate ");
                        Extensions.ReturnTypeHelper(Methods.clang_getResultType(pointee), tw);
                        this.tw.Write(" ");
                        this.tw.Write(spelling);
                        this.tw.Write("(");

                        uint argumentCounter = 0;

                        Methods.clang_visitChildren(cursor, delegate(CXCursor cxCursor, CXCursor parent1, IntPtr ptr)
                        {
                            if (cxCursor.kind == CXCursorKind.CXCursor_ParmDecl)
                            {
                                Extensions.ArgumentHelper(pointee, cxCursor, tw, argumentCounter++);
                            }

                            return CXChildVisitResult.CXChildVisit_Continue;
                        }, new CXClientData(IntPtr.Zero));

                        this.tw.WriteLine(");");
                        this.tw.WriteLine();

                        return CXChildVisitResult.CXChildVisit_Continue;
                    }
                }

                if (Methods.clang_isPODType(type) != 0)
                {
                    this.tw.WriteLine("    public partial struct " + spelling);
                    this.tw.WriteLine("    {");
                    this.tw.WriteLine("        public " + type.ToPlainTypeString() + " Value;");
                    this.tw.WriteLine("    }");
                    this.tw.WriteLine();
                }

                return CXChildVisitResult.CXChildVisit_Continue;
            }

            return CXChildVisitResult.CXChildVisit_Recurse;
        }
    }
}