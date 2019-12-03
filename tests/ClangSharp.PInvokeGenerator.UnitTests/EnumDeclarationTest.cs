// Copyright (c) Microsoft and Contributors. All rights reserved. Licensed under the University of Illinois/NCSA Open Source License. See LICENSE.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ClangSharp.UnitTests
{
    public sealed class EnumDeclarationTest : PInvokeGeneratorTest
    {
        [Fact]
        public async Task BasicTest()
        {
            var inputContents = @"enum MyEnum : int
{
    MyEnum_Value0,
    MyEnum_Value1,
    MyEnum_Value2,
};
";

            var expectedOutputContents = @"namespace ClangSharp.Test
{
    public enum MyEnum
    {
        MyEnum_Value0,
        MyEnum_Value1,
        MyEnum_Value2,
    }
}
";

            await ValidateGeneratedBindings(inputContents, expectedOutputContents);
        }

        [Fact]
        public async Task BasicValueTest()
        {
            var inputContents = @"enum MyEnum : int
{
    MyEnum_Value1 = 1,
    MyEnum_Value2,
    MyEnum_Value3,
};
";

            var expectedOutputContents = @"namespace ClangSharp.Test
{
    public enum MyEnum
    {
        MyEnum_Value1 = 1,
        MyEnum_Value2,
        MyEnum_Value3,
    }
}
";

            await ValidateGeneratedBindings(inputContents, expectedOutputContents);
        }

        [Fact]
        public async Task FlagsTest()
        {
            var inputContents = @"enum MyEnum : int
{
    MyEnum_Value1 = 1,
    MyEnum_Value2,
    MyEnum_Value3,
};
";

            var expectedOutputContents = @"using System;

namespace ClangSharp.Test
{
    [Flags]
    public enum MyEnum
    {
        MyEnum_Value1 = 1,
        MyEnum_Value2,
        MyEnum_Value3,
    }
}
";

            var withAttributes = new Dictionary<string, IReadOnlyList<string>> {
                ["MyEnum"] = new List<string>() { "Flags" }
            };
            var withNamespaces = new Dictionary<string, IReadOnlyList<string>> {
                ["MyEnum"] = new List<string>() { "System" }
            };
            await ValidateGeneratedBindings(inputContents, expectedOutputContents, withAttributes: withAttributes, withNamespaces: withNamespaces);
        }

        [Fact]
        public async Task ExcludeTest()
        {
            var inputContents = @"enum MyEnum : int
{
    MyEnum_Value0,
    MyEnum_Value1,
    MyEnum_Value2,
};
";

            var expectedOutputContents = string.Empty;

            var excludedNames = new string[] { "MyEnum" };
            await ValidateGeneratedBindings(inputContents, expectedOutputContents, excludedNames);
        }

        [Theory]
        [InlineData("short", "short")]
        public async Task ExplicitTypedTest(string nativeType, string expectedManagedType)
        {
            var inputContents = $@"enum MyEnum : {nativeType}
{{
    MyEnum_Value0,
    MyEnum_Value1,
    MyEnum_Value2,
}};
";

            var expectedOutputContents = $@"namespace ClangSharp.Test
{{
    public enum MyEnum : {expectedManagedType}
    {{
        MyEnum_Value0,
        MyEnum_Value1,
        MyEnum_Value2,
    }}
}}
";

            await ValidateGeneratedBindings(inputContents, expectedOutputContents);
        }

        [Theory]
        [InlineData("unsigned char", "byte")]
        [InlineData("long long", "long")]
        [InlineData("signed char", "sbyte")]
        [InlineData("unsigned short", "ushort")]
        [InlineData("unsigned int", "uint")]
        [InlineData("unsigned long long", "ulong")]
        public async Task ExplicitTypedWithNativeTypeNameTest(string nativeType, string expectedManagedType)
        {
            var inputContents = $@"enum MyEnum : {nativeType}
{{
    MyEnum_Value0,
    MyEnum_Value1,
    MyEnum_Value2,
}};
";

            var expectedOutputContents = $@"namespace ClangSharp.Test
{{
    [NativeTypeName(""{nativeType}"")]
    public enum MyEnum : {expectedManagedType}
    {{
        MyEnum_Value0,
        MyEnum_Value1,
        MyEnum_Value2,
    }}
}}
";

            await ValidateGeneratedBindings(inputContents, expectedOutputContents);
        }

        [Fact]
        public async Task RemapTest()
        {
            var inputContents = @"typedef enum _MyEnum : int
{
    MyEnum_Value1,
    MyEnum_Value2,
    MyEnum_Value3,
} MyEnum;
";

            var expectedOutputContents = $@"namespace ClangSharp.Test
{{
    public enum MyEnum
    {{
        MyEnum_Value1,
        MyEnum_Value2,
        MyEnum_Value3,
    }}
}}
";

            var remappedNames = new Dictionary<string, string> { ["_MyEnum"] = "MyEnum" };
            await ValidateGeneratedBindings(inputContents, expectedOutputContents, excludedNames: null, remappedNames);
        }
    }
}
