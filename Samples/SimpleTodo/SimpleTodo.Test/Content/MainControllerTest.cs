// Copyright (C) 2017 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System.Windows;
using System.Windows.Input;

using Fievus.Windows.Samples.SimpleTodo.Contents;

using NUnit.Framework;

using NSubstitute;

using Fievus.Windows.Mvc;
using Fievus.Windows.Runners;

namespace Fievus.Windows.Samples.SimpleTodo.Test.Content
{
    [TestFixture]
    public class MainControllerTest
    {
        [Test]
        public void ShouldAddTodoItemWhenEnterKeyIsPressedOnTodoContentTextBox()
        {
            WpfApplicationRunner.Start<Application>().Run(application =>
            {
                var controller = new MainContentController
                {
                    Content = new MainContent()
                };

                controller.Content.TodoContent.Value = "Todo Item";
                WpfController.RetrieveRoutedEventHandlers(controller)
                    .GetBy("TodoContentTextBox")
                    .With(new KeyEventArgs(Keyboard.PrimaryDevice, Substitute.For<PresentationSource>(), 0, Key.Enter))
                    .Raise(UIElement.KeyDownEvent.Name);

                Assert.That(controller.Content.TodoItems, Has.Count.EqualTo(1));
            }).Shutdown();
        }

        [Test]
        public void ShouldNotAddTodoItemWhenEnterKeyIsNotPressedOnTodoContentTextBox()
        {
            WpfApplicationRunner.Start<Application>().Run(application =>
            {
                var controller = new MainContentController
                {
                    Content = new MainContent()
                };

                controller.Content.TodoContent.Value = "Todo Item";
                WpfController.RetrieveRoutedEventHandlers(controller)
                    .GetBy("TodoContentTextBox")
                    .With(new KeyEventArgs(Keyboard.PrimaryDevice, Substitute.For<PresentationSource>(), 0, Key.Tab))
                    .Raise(UIElement.KeyDownEvent.Name);

                Assert.That(controller.Content.TodoItems, Is.Empty);
            }).Shutdown();
        }
    }
}
