// Copyright (c) .NET Foundation and Contributors. All Rights Reserved. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from https://github.com/llvm/llvm-project/tree/llvmorg-16.0.6/clang/include/clang-c
// Original source is Copyright (c) the LLVM Project and Contributors. Licensed under the Apache License v2.0 with LLVM Exceptions. See NOTICE.txt in the project root for license information.

using NUnit.Framework;
using System;
using System.Runtime.InteropServices;

namespace ClangSharp.Interop.UnitTests;

/// <summary>Provides validation of the <see cref="CXTUResourceUsage" /> struct.</summary>
public static unsafe partial class CXTUResourceUsageTests
{
    /// <summary>Validates that the <see cref="CXTUResourceUsage" /> struct is blittable.</summary>
    [Test]
    public static void IsBlittableTest()
    {
        Assert.That(Marshal.SizeOf<CXTUResourceUsage>(), Is.EqualTo(sizeof(CXTUResourceUsage)));
    }

    /// <summary>Validates that the <see cref="CXTUResourceUsage" /> struct has the right <see cref="LayoutKind" />.</summary>
    [Test]
    public static void IsLayoutSequentialTest()
    {
        Assert.That(typeof(CXTUResourceUsage).IsLayoutSequential, Is.True);
    }

    /// <summary>Validates that the <see cref="CXTUResourceUsage" /> struct has the correct size.</summary>
    [Test]
    public static void SizeOfTest()
    {
        if (Environment.Is64BitProcess)
        {
            Assert.That(sizeof(CXTUResourceUsage), Is.EqualTo(24));
        }
        else
        {
            Assert.That(sizeof(CXTUResourceUsage), Is.EqualTo(12));
        }
    }
}
