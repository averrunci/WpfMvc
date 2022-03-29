// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System.Collections;
using Carna;

namespace Charites.Windows.Mvc;

[Context("Attaches a controller when the IsEnabled property of the WpfController is set to true before the data context of the element is set")]
class WpfControllerSpec_AttachingAndDetachingController_IsEnabledBeforeDataContextIsSet : FixtureSteppable
{
    TestElement Element { get; set; } = default!;

    [Example("The WpfController for the element is enabled before a data context of the element is set")]
    [Sample(Source = typeof(IsEnabledBeforeDataContextIsSetSampleDataSource))]
    void Ex01(object dataContext, IEnumerable<Type> expectedControllerTypes)
    {
        Given("an element that does not contain the data context", () => Element = new TestElement());
        When("the WpfController is enabled for the element", () => WpfController.SetIsEnabled(Element, true));
        When("a data context of the element is set", () => Element.DataContext = dataContext);
        Then("the controller should be attached to the element", () =>
            WpfController.GetControllers(Element).Select(controller => controller.GetType()).SequenceEqual(expectedControllerTypes) &&
            WpfController.GetControllers(Element).OfType<TestWpfControllers.TestController>().All(controller => controller.DataContext == Element.DataContext)
        );
    }

    class IsEnabledBeforeDataContextIsSetSampleDataSource : ISampleDataSource
    {
        IEnumerable ISampleDataSource.GetData()
        {
            yield return new
            {
                Description = "When the key is the name of the data context type",
                DataContext = new TestDataContexts.AttachingTestDataContext(),
                ExpectedControllerTypes = new[] { typeof(TestWpfControllers.TestDataContextController) }
            };
            yield return new
            {
                Description = "When the key is the name of the data context base type",
                DataContext = new TestDataContexts.DerivedBaseAttachingTestDataContext(),
                ExpectedControllerTypes = new[] { typeof(TestWpfControllers.BaseTestDataContextController) }
            };
            yield return new
            {
                Description = "When the key is the full name of the data context type",
                DataContext = new TestDataContexts.AttachingTestDataContextFullName(),
                ExpectedControllerTypes = new[] { typeof(TestWpfControllers.TestDataContextFullNameController) }
            };
            yield return new
            {
                Description = "When the key is the full name of the data context base type",
                DataContext = new TestDataContexts.DerivedBaseAttachingTestDataContextFullName(),
                ExpectedControllerTypes = new[] { typeof(TestWpfControllers.BaseTestDataContextFullNameController) }
            };
            yield return new
            {
                Description = "When the key is the name of the data context type that is generic",
                DataContext = new TestDataContexts.GenericAttachingTestDataContext<string>(),
                ExpectedControllerTypes = new[] { typeof(TestWpfControllers.GenericTestDataContextController) }
            };
            yield return new
            {
                Description = "When the key is the full name of the data context type that is generic",
                DataContext = new TestDataContexts.GenericAttachingTestDataContextFullName<string>(),
                ExpectedControllerTypes = new[] { typeof(TestWpfControllers.GenericTestDataContextFullNameController), typeof(TestWpfControllers.GenericTestDataContextFullNameWithoutParametersController) }
            };
            yield return new
            {
                Description = "When the key is the name of the interface implemented by the data context",
                DataContext = new TestDataContexts.InterfaceImplementedAttachingTestDataContext(),
                ExpectedControllerTypes = new[] { typeof(TestWpfControllers.InterfaceImplementedTestDataContextController) }
            };
            yield return new
            {
                Description = "When the key is the full name of the interface implemented by the data context",
                DataContext = new TestDataContexts.InterfaceImplementedAttachingTestDataContextFullName(),
                ExpectedControllerTypes = new[] { typeof(TestWpfControllers.InterfaceImplementedTestDataContextFullNameController) }
            };
        }
    }
}