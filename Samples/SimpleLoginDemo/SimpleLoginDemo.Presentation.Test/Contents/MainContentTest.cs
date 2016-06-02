// Copyright (C) 2016 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;

using NUnit.Framework;

using Rhino.Mocks;

using Fievus.Windows.Samples.SimpleLoginDemo.Presentation.Contents.Login;

namespace Fievus.Windows.Samples.SimpleLoginDemo.Presentation.Contents
{
    [TestFixture]
    public class MainContentTest
    {
        [Test]
        public void SetsSpecifiedContentAtInitializing()
        {
            var initialContent = MockRepository.GenerateStub<ILoginDemoContent>();

            var mainContent = new MainContent(initialContent);

            Assert.That(mainContent.Content.Value, Is.EqualTo(initialContent));
        }
        
        [Test]
        public void AddsHandlerThatSetsNextContentWhenContentChangingEventIsOccurred()
        {
            var content = MockRepository.GenerateStub<ILoginDemoContent>();
            var mainContent = new MainContent(content);

            var nextContent = MockRepository.GenerateStub<ILoginDemoContent>();
            content.Raise(x => x.ContentChanging += null, this, new ContentChangingEventArgs(nextContent));

            Assert.That(mainContent.Content.Value, Is.EqualTo(nextContent));
        }

        [Test]
        public void AddsHandlerThatSetsLoginContentWhenLoggedOutEventIsOccurred()
        {
            var content = MockRepository.GenerateStub<ILoginDemoContent>();
            var mainContent = new MainContent(content);

            content.Raise(x => x.LoggedOut += null, this, EventArgs.Empty);

            Assert.That(mainContent.Content.Value, Is.TypeOf<LoginContent>());
        }

        [Test]
        public void RemovesHandlerForContentChangingEventFromOldContentWhenContentIsChanged()
        {
            var content = MockRepository.GenerateStub<ILoginDemoContent>();
            var mainContent = new MainContent(content);

            var newContent = MockRepository.GenerateStub<ILoginDemoContent>();
            mainContent.Content.Value = newContent;

            var nextContent = MockRepository.GenerateStub<ILoginDemoContent>();
            content.Raise(x => x.ContentChanging += null, this, new ContentChangingEventArgs(nextContent));
            Assert.That(mainContent.Content.Value, Is.Not.EqualTo(nextContent));
            Assert.That(mainContent.Content.Value, Is.EqualTo(newContent));
        }

        [Test]
        public void RemovesHandlerForLoggedOutEventFromOldContentWhenContentIsChanged()
        {
            var content = MockRepository.GenerateStub<ILoginDemoContent>();
            var mainContent = new MainContent(content);

            var newContent = MockRepository.GenerateStub<ILoginDemoContent>();
            mainContent.Content.Value = newContent;

            content.Raise(x => x.LoggedOut += null, this, EventArgs.Empty);
            Assert.That(mainContent.Content.Value, Is.Not.TypeOf<LoginContent>());
            Assert.That(mainContent.Content.Value, Is.EqualTo(newContent));
        }
    }
}
