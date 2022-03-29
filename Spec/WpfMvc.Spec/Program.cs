﻿// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using Carna;
using Carna.ConsoleRunner;

[assembly: AssemblyFixture(RequiresSta = true)]

namespace Charites.Windows;

internal static class Program
{
    private static void Main(string[] args) => CarnaConsoleRunner.Run(args);
}