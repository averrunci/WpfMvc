// Copyright (C) 2017 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using NUnit.Framework;

using Rhino.Mocks;

namespace Fievus.Windows.Mvc
{
    [TestFixture]
    public class WpfControllerCollectionTest
    {
        [Test]
        public void AddWpfControllerWhenIWpfControllerFactoryIsSpecified()
        {
            var controller = new TestWpfControllers.TestWpfController();
            var controllerType = controller.GetType();

            WpfController.Factory = MockRepository.GenerateMock<IWpfControllerFactory>();
            WpfController.Factory.Expect(f => f.Create(controllerType)).Return(controller);

            var wpfController = new WpfController
            {
                ControllerType = controllerType
            };
            var controllers = new WpfControllerCollection();
            controllers.Add(wpfController);

            Assert.That(controllers.Count, Is.EqualTo(1));
            Assert.That(controllers[0], Is.EqualTo(controller));
        }

        [Test]
        public void AddWpfControllerWhenIWpfControllerFactoryIsNotSpecified()
        {
            var controllerType = typeof(TestWpfControllers.TestWpfController);

            var wpfController = new WpfController
            {
                ControllerType = controllerType
            };
            var controllers = new WpfControllerCollection();
            controllers.Add(wpfController);

            Assert.That(controllers.Count, Is.EqualTo(1));
            Assert.That(controllers[0], Is.TypeOf<TestWpfControllers.TestWpfController>());
        }

        [Test]
        public void AddWpfControllerWithSpecifiedIWpfControllerFactory()
        {
            var controller = new TestWpfControllers.TestWpfController();

            var factory = MockRepository.GenerateMock<IWpfControllerFactory>();
            factory.Expect(f => f.Create(null)).Return(controller);

            var controllers = new WpfControllerCollection();
            controllers.Add(factory);

            Assert.That(controllers.Count, Is.EqualTo(1));
            Assert.That(controllers[0], Is.EqualTo(controller));
        }
    }
}
