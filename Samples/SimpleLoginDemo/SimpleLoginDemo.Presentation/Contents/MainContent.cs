// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using Charites.Windows.Mvc.Bindings;

namespace Charites.Windows.Samples.SimpleLoginDemo.Presentation.Contents;

public class MainContent
{
    public ObservableProperty<object?> Content { get; } = new(null);
}