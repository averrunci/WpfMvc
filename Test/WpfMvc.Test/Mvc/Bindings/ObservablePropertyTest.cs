// Copyright (C) 2016-2018 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using NUnit.Framework;

namespace Fievus.Windows.Mvc.Bindings.ObservablePropertyTest
{
    [TestFixture]
    public class InstanceCreation
    {
        [Test]
        public void CreatesWithDefaultInitialValue()
        {
            var property = new ObservableProperty<string>();

            Assert.That(property.Value, Is.Null);
        }

        [Test]
        public void CreatesWithSpecifiedInitialValue()
        {
            var property = new ObservableProperty<string>("Test");

            Assert.That(property.Value, Is.EqualTo("Test"));
        }

        [Test]
        public void CreatesWithSpecifiedInitialValueWithFactoryMethod()
        {
            Assert.That(ObservableProperty<string>.Of("Test").Value, Is.EqualTo("Test"));
        }
    }
        
    [TestFixture]
    public class PropertyChanged
    {
        [Test]
        public void RaisesPropertyChangedEventWhenNotNullValueIsSet()
        {
            var propertyChangedOccurred = false;
            var property = ObservableProperty<string>.Of("Test");
            property.PropertyChanged += (s, e) => propertyChangedOccurred = (e.PropertyName == "Value");

            property.Value = "Changed";

            Assert.That(propertyChangedOccurred, Is.True);
        }

        [Test]
        public void RaisesPropertyChangedEventWhenNullValueIsSet()
        {
            var propertyChangedOccurred = false;
            var property = ObservableProperty<string>.Of("Test");
            property.PropertyChanged += (s, e) => propertyChangedOccurred = (e.PropertyName == "Value");

            property.Value = null;

            Assert.That(propertyChangedOccurred, Is.True);
        }

        [Test]
        public void DoesNotRaisePropertyChangedEventWhenTheSameValueIsSet()
        {
            var propertyChangedOccurred = false;
            var property = ObservableProperty<string>.Of("Test");
            property.PropertyChanged += (s, e) => propertyChangedOccurred = true;

            property.Value = "Test";

            Assert.That(propertyChangedOccurred, Is.False);
        }

        [Test]
        public void DoesNotRaisePropertyChangedEventWhenNullIsSetToItWhoseValueIsNull()
        {
            var propertyChangedOccurred = false;
            var property = ObservableProperty<string>.Of(null);
            property.PropertyChanged += (s, e) => propertyChangedOccurred = true;

            property.Value = null;

            Assert.That(propertyChangedOccurred, Is.False);
        }

        [Test]
        public void RaisesPropertyChangedEventWhenNotNullValueIsSetToItWhoseValueIsNull()
        {
            var propertyChangedOccurred = false;
            var property = ObservableProperty<string>.Of(null);
            property.PropertyChanged += (s, e) => propertyChangedOccurred = (e.PropertyName == "Value");

            property.Value = "Changed";

            Assert.That(propertyChangedOccurred, Is.True);
        }
    }
    
    namespace Binding
    {
        [TestFixture]
        public class OneWay
        {
            [Test]
            public void BindsSpecifiedObservableProperty()
            {
                var property1 = ObservableProperty<string>.Of("Test1");
                var property2 = ObservableProperty<string>.Of("Test2");

                property1.Bind(property2);
                Assert.That(property1.Value, Is.EqualTo("Test2"));
                Assert.That(property2.Value, Is.EqualTo("Test2"));

                property2.Value = "Changed";
                Assert.That(property1.Value, Is.EqualTo("Changed"));
                Assert.That(property2.Value, Is.EqualTo("Changed"));

                property1.Value = "Test";
                Assert.That(property1.Value, Is.EqualTo("Test"));
                Assert.That(property2.Value, Is.EqualTo("Changed"));
            }

            [Test]
            public void UnbindsBoundObservableProperty()
            {
                var property1 = ObservableProperty<string>.Of("Test1");
                var property2 = ObservableProperty<string>.Of("Test2");

                property1.Bind(property2);
                Assert.That(property1.Value, Is.EqualTo("Test2"));
                Assert.That(property2.Value, Is.EqualTo("Test2"));

                property1.Unbind();

                property2.Value = "Changed";
                Assert.That(property1.Value, Is.EqualTo("Test2"));
                Assert.That(property2.Value, Is.EqualTo("Changed"));
            }

            [Test]
            public void ThrowsExceptionWhenObservablePropertyWhichIsBoundBind()
            {
                var property1 = ObservableProperty<string>.Of("Test1");
                var property2 = ObservableProperty<string>.Of("Test2");

                property1.Bind(property2);

                Assert.Throws<InvalidOperationException>(() => property1.Bind(property2));
            }

            [Test]
            public void ThrowsExceptionWhenObservablePropertyWhichIsNotBoundUnbind()
            {
                var property1 = ObservableProperty<string>.Of("Test1");

                Assert.Throws<InvalidOperationException>(() => property1.Unbind());
            }
        }

        [TestFixture]
        public class OneWayWithConverter
        {
            [Test]
            public void BindsSpecifiedObservablePropertyWithConverter()
            {
                var property1 = ObservableProperty<string>.Of("Test1");
                var property2 = ObservableProperty<int>.Of(3);

                property1.Bind(property2, t => t.ToString());
                Assert.That(property1.Value, Is.EqualTo("3"));
                Assert.That(property2.Value, Is.EqualTo(3));

                property2.Value = 7;
                Assert.That(property1.Value, Is.EqualTo("7"));
                Assert.That(property2.Value, Is.EqualTo(7));

                property1.Value = "Test";
                Assert.That(property1.Value, Is.EqualTo("Test"));
                Assert.That(property2.Value, Is.EqualTo(7));
            }

            [Test]
            public void UnbindsBoundObservablePropertyWithConverter()
            {
                var property1 = ObservableProperty<string>.Of("Test1");
                var property2 = ObservableProperty<int>.Of(3);

                property1.Bind(property2, t => t.ToString());
                Assert.That(property1.Value, Is.EqualTo("3"));
                Assert.That(property2.Value, Is.EqualTo(3));

                property1.Unbind();

                property2.Value = 7;
                Assert.That(property1.Value, Is.EqualTo("3"));
                Assert.That(property2.Value, Is.EqualTo(7));
            }

            [Test]
            public void ThrowsExceptionWhenObservablePropertyWhichIsBoundBindWithConverter()
            {
                var property1 = ObservableProperty<string>.Of("Test1");
                var property2 = ObservableProperty<int>.Of(3);

                property1.Bind(property2, t => t.ToString());

                Assert.Throws<InvalidOperationException>(() => property1.Bind(property2, t => t.ToString()));
            }
        }

        [TestFixture]
        public class MultiBinding
        {
            [Test]
            public void BindsSpecifiedObservableProperties()
            {
                var property = ObservableProperty<string>.Of("Test1");
                var property1 = ObservableProperty<int>.Of(1);
                var property2 = ObservableProperty<int>.Of(2);
                var property3 = ObservableProperty<int>.Of(3);

                property.Bind(
                    c => (c.GetValueAt<int>(0) + c.GetValueAt<int>(1) + c.GetValueAt<int>(2)).ToString(),
                    property1, property2, property3
                );
                Assert.That(property.Value, Is.EqualTo("6"));
                Assert.That(property1.Value, Is.EqualTo(1));
                Assert.That(property2.Value, Is.EqualTo(2));
                Assert.That(property3.Value, Is.EqualTo(3));

                property1.Value = 7;
                Assert.That(property.Value, Is.EqualTo("12"));
                Assert.That(property1.Value, Is.EqualTo(7));
                Assert.That(property2.Value, Is.EqualTo(2));
                Assert.That(property3.Value, Is.EqualTo(3));

                property2.Value = 8;
                Assert.That(property.Value, Is.EqualTo("18"));
                Assert.That(property1.Value, Is.EqualTo(7));
                Assert.That(property2.Value, Is.EqualTo(8));
                Assert.That(property3.Value, Is.EqualTo(3));

                property3.Value = 9;
                Assert.That(property.Value, Is.EqualTo("24"));
                Assert.That(property1.Value, Is.EqualTo(7));
                Assert.That(property2.Value, Is.EqualTo(8));
                Assert.That(property3.Value, Is.EqualTo(9));

                property.Value = "Test";
                Assert.That(property.Value, Is.EqualTo("Test"));
                Assert.That(property1.Value, Is.EqualTo(7));
                Assert.That(property2.Value, Is.EqualTo(8));
                Assert.That(property3.Value, Is.EqualTo(9));
            }

            [Test]
            public void UnbindsBoundObservableProperties()
            {
                var property = ObservableProperty<string>.Of("Test1");
                var property1 = ObservableProperty<int>.Of(1);
                var property2 = ObservableProperty<int>.Of(2);
                var property3 = ObservableProperty<int>.Of(3);

                property.Bind(
                    c => (c.GetValueAt<int>(0) + c.GetValueAt<int>(1) + c.GetValueAt<int>(2)).ToString(),
                    property1, property2, property3
                );
                Assert.That(property.Value, Is.EqualTo("6"));
                Assert.That(property1.Value, Is.EqualTo(1));
                Assert.That(property2.Value, Is.EqualTo(2));
                Assert.That(property3.Value, Is.EqualTo(3));

                property.Unbind();

                property1.Value = 7;
                Assert.That(property.Value, Is.EqualTo("6"));
                Assert.That(property1.Value, Is.EqualTo(7));
                Assert.That(property2.Value, Is.EqualTo(2));
                Assert.That(property3.Value, Is.EqualTo(3));

                property2.Value = 8;
                Assert.That(property.Value, Is.EqualTo("6"));
                Assert.That(property1.Value, Is.EqualTo(7));
                Assert.That(property2.Value, Is.EqualTo(8));
                Assert.That(property3.Value, Is.EqualTo(3));

                property3.Value = 9;
                Assert.That(property.Value, Is.EqualTo("6"));
                Assert.That(property1.Value, Is.EqualTo(7));
                Assert.That(property2.Value, Is.EqualTo(8));
                Assert.That(property3.Value, Is.EqualTo(9));
            }

            [Test]
            public void ThrowsExceptionWhenObservablePropertyWhichIsBoundBinds()
            {
                var property = ObservableProperty<string>.Of("Test1");
                var property1 = ObservableProperty<int>.Of(1);
                var property2 = ObservableProperty<int>.Of(2);
                var property3 = ObservableProperty<int>.Of(3);

                property.Bind(
                    c => (c.GetValueAt<int>(0) + c.GetValueAt<int>(1) + c.GetValueAt<int>(2)).ToString(),
                    property1, property2, property3
                );

                Assert.Throws<InvalidOperationException>(() => property.Bind(
                    c => (c.GetValueAt<int>(0) + c.GetValueAt<int>(1) + c.GetValueAt<int>(2)).ToString(),
                    property1, property2, property3
                ));
            }

            [Test]
            public void BindsSpecifiedObservablePropertiesWithDifferenceValueType()
            {
                var property = ObservableProperty<string>.Of("Test1");
                var property1 = ObservableProperty<int>.Of(1);
                var property2 = ObservableProperty<string>.Of("#");
                var property3 = ObservableProperty<bool>.Of(false);

                property.Bind(
                    c => c.GetValueAt<bool>(2) ? $"[{c.GetValueAt<string>(1)}{c.GetValueAt<int>(0)}]" : $"{c.GetValueAt<string>(1)}{c.GetValueAt<int>(0)}",
                    property1, property2, property3
                );
                Assert.That(property.Value, Is.EqualTo("#1"));
                Assert.That(property1.Value, Is.EqualTo(1));
                Assert.That(property2.Value, Is.EqualTo("#"));
                Assert.That(property3.Value, Is.EqualTo(false));

                property1.Value = 7;
                Assert.That(property.Value, Is.EqualTo("#7"));
                Assert.That(property1.Value, Is.EqualTo(7));
                Assert.That(property2.Value, Is.EqualTo("#"));
                Assert.That(property3.Value, Is.EqualTo(false));

                property2.Value = "## ";
                Assert.That(property.Value, Is.EqualTo("## 7"));
                Assert.That(property1.Value, Is.EqualTo(7));
                Assert.That(property2.Value, Is.EqualTo("## "));
                Assert.That(property3.Value, Is.EqualTo(false));

                property3.Value = true;
                Assert.That(property.Value, Is.EqualTo("[## 7]"));
                Assert.That(property1.Value, Is.EqualTo(7));
                Assert.That(property2.Value, Is.EqualTo("## "));
                Assert.That(property3.Value, Is.EqualTo(true));

                property.Value = "Test";
                Assert.That(property.Value, Is.EqualTo("Test"));
                Assert.That(property1.Value, Is.EqualTo(7));
                Assert.That(property2.Value, Is.EqualTo("## "));
                Assert.That(property3.Value, Is.EqualTo(true));
            }
        }

        [TestFixture]
        public class TwoWay
        {
            [Test]
            public void BindsSpecifiedObservablePropertyAsTwoWay()
            {
                var property1 = ObservableProperty<string>.Of("Test1");
                var property2 = ObservableProperty<string>.Of("Test2");

                property1.BindTwoWay(property2);
                Assert.That(property1.Value, Is.EqualTo("Test2"));
                Assert.That(property2.Value, Is.EqualTo("Test2"));

                property2.Value = "Changed";
                Assert.That(property1.Value, Is.EqualTo("Changed"));
                Assert.That(property2.Value, Is.EqualTo("Changed"));

                property1.Value = "Test";
                Assert.That(property1.Value, Is.EqualTo("Test"));
                Assert.That(property2.Value, Is.EqualTo("Test"));
            }

            [Test]
            public void UnbindsBoundObservablePropertyAsTwoWay()
            {
                var property1 = ObservableProperty<string>.Of("Test1");
                var property2 = ObservableProperty<string>.Of("Test2");

                property1.BindTwoWay(property2);
                Assert.That(property1.Value, Is.EqualTo("Test2"));
                Assert.That(property2.Value, Is.EqualTo("Test2"));

                property1.UnbindTwoWay(property2);

                property2.Value = "Changed";
                Assert.That(property1.Value, Is.EqualTo("Test2"));
                Assert.That(property2.Value, Is.EqualTo("Changed"));
            }

            [Test]
            public void ThrowsExceptionWhenObservablePropertyWhichIsBoundBindAsTwoWay()
            {
                var property1 = ObservableProperty<string>.Of("Test1");
                var property2 = ObservableProperty<string>.Of("Test2");

                property1.BindTwoWay(property2);

                Assert.Throws<InvalidOperationException>(() => property1.BindTwoWay(property2));
            }

            [Test]
            public void ThrowsExceptionWhenObservablePropertyWhichIsNotBoundUnbindAsTwoWay()
            {
                var property1 = ObservableProperty<string>.Of("Test1");
                var property2 = ObservableProperty<string>.Of("Test2");

                Assert.Throws<InvalidOperationException>(() => property1.UnbindTwoWay(property2));
            }
        }

        [TestFixture]
        public class TwoWayWithConverter
        {
            [Test]
            public void BindsSpecifiedObservablePropertyWithConverterAsTwoWay()
            {
                var property1 = ObservableProperty<string>.Of("8");
                var property2 = ObservableProperty<int>.Of(3);

                property1.BindTwoWay(property2, p => p.ToString(), p => int.Parse(p));
                Assert.That(property1.Value, Is.EqualTo("3"));
                Assert.That(property2.Value, Is.EqualTo(3));

                property2.Value = 7;
                Assert.That(property1.Value, Is.EqualTo("7"));
                Assert.That(property2.Value, Is.EqualTo(7));

                property1.Value = "10";
                Assert.That(property1.Value, Is.EqualTo("10"));
                Assert.That(property2.Value, Is.EqualTo(10));
            }

            [Test]
            public void UnbindsBoundObservablePropertyWithConverterAsTwoWay()
            {
                var property1 = ObservableProperty<string>.Of("8");
                var property2 = ObservableProperty<int>.Of(3);

                property1.BindTwoWay(property2, p => p.ToString(), p => int.Parse(p));
                Assert.That(property1.Value, Is.EqualTo("3"));
                Assert.That(property2.Value, Is.EqualTo(3));

                property1.UnbindTwoWay(property2);

                property2.Value = 7;
                Assert.That(property1.Value, Is.EqualTo("3"));
                Assert.That(property2.Value, Is.EqualTo(7));

                property1.Value = "Test";
                Assert.That(property1.Value, Is.EqualTo("Test"));
                Assert.That(property2.Value, Is.EqualTo(7));
            }

            [Test]
            public void ThrowsExceptionWhenObservablePropertyWhichIsBoundBindWithConverterAsTwoWay()
            {
                var property1 = ObservableProperty<string>.Of("8");
                var property2 = ObservableProperty<int>.Of(3);

                property1.BindTwoWay(property2, p => p.ToString(), p => int.Parse(p));

                Assert.Throws<InvalidOperationException>(() =>
                    property1.BindTwoWay(ObservableProperty<int>.Of(8), p => p.ToString(), p => int.Parse(p))
                );
            }

            [Test]
            public void ThrowsExceptionWhenObservablePropertyWhichIsNotBoundUnbindWithConverterAsTwoWay()
            {
                var property1 = ObservableProperty<string>.Of("8");
                var property2 = ObservableProperty<int>.Of(3);

                Assert.Throws<InvalidOperationException>(() => property1.UnbindTwoWay(property2));
            }
        }
    }

    namespace ChangingChangedHandler
    {
        [TestFixture]
        public class Changing
        {
            [Test]
            public void AddsChangingChangedHandler()
            {
                var property = new ObservableProperty<string>();
                property.PropertyValueChanging += (s, e) =>
                {
                    Assert.That(e.OldValue, Is.Null);
                    Assert.That(e.NewValue, Is.EqualTo("Changed"));
                };

                property.Value = "Changed";

                Assert.That(property.Value, Is.EqualTo("Changed"));
            }

            [Test]
            public void AddsChangingHandlerWithCancelingValueChanged()
            {
                var property = ObservableProperty<string>.Of("Test");
                property.PropertyValueChanging += (s, e) =>
                {
                    Assert.That(e.OldValue, Is.EqualTo("Test"));
                    Assert.That(e.NewValue, Is.EqualTo("Changed"));
                    e.Disable();
                };

                property.Value = "Changed";

                Assert.That(property.Value, Is.EqualTo("Test"));
            }
        }

        [TestFixture]
        public class Changed
        {
            [Test]
            public void AddsChangedHandler()
            {
                var property = new ObservableProperty<string>();
                property.PropertyValueChanged += (s, e) =>
                {
                    Assert.That(e.OldValue, Is.Null);
                    Assert.That(e.NewValue, Is.EqualTo("Changed"));
                };

                property.Value = "Changed";

                Assert.That(property.Value, Is.EqualTo("Changed"));
            }
        }
    }

    [TestFixture]
    public class Validation
    {
        public ObservableProperty<string> PropertyNotAnnotatedValidation { get { return propertyNotAnnotatedValidation; } }
        private ObservableProperty<string> propertyNotAnnotatedValidation;

        [Display(Name = "String Expression")]
        [StringLength(10, ErrorMessage = "Please enter {0} within {1} characters.")]
        public ObservableProperty<string> PropertyAnnotatedValidation { get { return propertyAnnotatedValidation; } }
        private ObservableProperty<string> propertyAnnotatedValidation;

        [StringLength(6, ErrorMessage = "Please enter within {1} characters.")]
        [RegularExpression("\\d+", ErrorMessage = "Please enter a digit only.")]
        public ObservableProperty<string> PropertyAnnotatedMultiValidations { get { return propertyAnnotatedMultiValidations; } }
        private ObservableProperty<string> propertyAnnotatedMultiValidations;

        [Required(ErrorMessage = "Please enter a value.")]
        public ObservableProperty<string> RequiredProperty { get { return requiredProperty; } }
        private ObservableProperty<string> requiredProperty;

        [Display(Name = nameof(Resources.LocalizablePropertyName), ResourceType = typeof(Resources))]
        [StringLength(10, ErrorMessageResourceName = nameof(Resources.StringLengthErrorMessage), ErrorMessageResourceType = typeof(Resources))]
        public ObservableProperty<string> LocalizablePropertyAnnotatedValidation { get { return localizablePropertyAnnotatedValidation; } }
        private ObservableProperty<string> localizablePropertyAnnotatedValidation;

        private readonly PropertyValueValidateEventHandler<string> propertyValueValidate = (s, e) =>
        {
            if (e.Value != "Correct")
            {
                e.Add("The value does not correct.");
            }
        };

        private bool errorsChangedOccurred;
        private readonly EventHandler<DataErrorsChangedEventArgs> errorsChangedVerificationHandler;

        public Validation()
        {
            errorsChangedVerificationHandler = (s, e) =>
            {
                Assert.That(e.PropertyName, Is.EqualTo("Value"));
                errorsChangedOccurred = true;
            };
        }

        [SetUp]
        public void EnableValidations()
        {
            propertyNotAnnotatedValidation = new ObservableProperty<string>();
            propertyAnnotatedValidation = new ObservableProperty<string>();
            propertyAnnotatedMultiValidations = new ObservableProperty<string>();
            requiredProperty = new ObservableProperty<string>();
            localizablePropertyAnnotatedValidation = new ObservableProperty<string>();

            PropertyNotAnnotatedValidation.EnableValidation(() => PropertyNotAnnotatedValidation);
            PropertyAnnotatedValidation.EnableValidation(() => PropertyAnnotatedValidation);
            PropertyAnnotatedMultiValidations.EnableValidation(() => PropertyAnnotatedMultiValidations);
            RequiredProperty.EnableValidation(() => RequiredProperty);
            LocalizablePropertyAnnotatedValidation.EnableValidation(() => LocalizablePropertyAnnotatedValidation);
        }

        [TearDown]
        public void DisableValidations()
        {
            PropertyNotAnnotatedValidation.DisableValidation();
            PropertyAnnotatedValidation.DisableValidation();
            PropertyAnnotatedMultiValidations.DisableValidation();
            RequiredProperty.DisableValidation();
            LocalizablePropertyAnnotatedValidation.DisableValidation();
        }

        private void SetValue<T>(T value, ObservableProperty<T> property)
        {
            errorsChangedOccurred = false;
            property.ErrorsChanged += errorsChangedVerificationHandler;
            property.Value = value;
            property.ErrorsChanged -= errorsChangedVerificationHandler;
        }

        private void AssertNoValidationError<T>(ObservableProperty<T> property)
        {
            Assert.That((property as IDataErrorInfo).Error, Is.Empty);
            Assert.That((property as IDataErrorInfo)["Value"], Is.Empty);
            Assert.That((property as INotifyDataErrorInfo).HasErrors, Is.False);
            Assert.That((property as INotifyDataErrorInfo).GetErrors("Value"), Is.Empty);
        }

        private void AssertValidationError<T>(ObservableProperty<T> property, params string[] messages)
        {
            var expectedMessage = string.Join(Environment.NewLine, messages);

            Assert.That((property as IDataErrorInfo).Error, Is.EqualTo(expectedMessage));
            Assert.That((property as IDataErrorInfo)["Value"], Is.EqualTo(expectedMessage));
            Assert.That((property as INotifyDataErrorInfo).HasErrors, Is.True);
            Assert.That((property as INotifyDataErrorInfo).GetErrors("Value"), Is.EquivalentTo(messages));
        }

        [Test]
        public void DoesNotGetErrorWhenValidationAttributeNotAnnotated()
        {
            SetValue("Changed", PropertyNotAnnotatedValidation);

            AssertNoValidationError(PropertyNotAnnotatedValidation);
            Assert.That(errorsChangedOccurred, Is.False);
        }

        [Test]
        public void GetsErrorForInvalidPropertyWhenValidationAttributeIsAnnotated()
        {
            SetValue("ABCDEFGHIJK", PropertyAnnotatedValidation);

            AssertValidationError(
                PropertyAnnotatedValidation,
                "Please enter String Expression within 10 characters."
            );
            Assert.That(errorsChangedOccurred, Is.True);
        }

        [Test]
        [SetUICulture("en-US")]
        public void GetsErrorMessageForInvalidPropertyWhenValidationAttributeThatIsLocalizableIsAnnotated()
        {
            SetValue("ABCDEFGHIJK", LocalizablePropertyAnnotatedValidation);

            AssertValidationError(
                LocalizablePropertyAnnotatedValidation,
                "Please enter Localizable Property within 10 characters."
            );
            Assert.That(errorsChangedOccurred, Is.True);
        }

        [Test]
        [SetUICulture("ja-JP")]
        public void GetsLocalizedErrorMessageForInvalidPropertyWhenValidationAttributeThatIsLocalizableIsAnnotated()
        {
            SetValue("ABCDEFGHIJK", LocalizablePropertyAnnotatedValidation);

            AssertValidationError(
                    LocalizablePropertyAnnotatedValidation,
                    "ローカライズ可能なプロパティは10文字以内で入力してください。"
                );
            Assert.That(errorsChangedOccurred, Is.True);
        }

        [Test]
        public void DoesNotGetErrorForValidPropertyWhenValidationAttributeIsAnnotated()
        {
            SetValue("ABCDEFGHIJK", PropertyAnnotatedValidation);

            AssertValidationError(
                PropertyAnnotatedValidation,
                "Please enter String Expression within 10 characters."
            );
            Assert.That(errorsChangedOccurred, Is.True);


            SetValue("ABCDEFGHIJ", PropertyAnnotatedValidation);

            Assert.That(errorsChangedOccurred, Is.True);
            AssertNoValidationError(PropertyAnnotatedValidation);
        }

        [Test]
        public void GetsMultiErrorsForInvalidPropertyWhenMultiValidationAttributesAreAnnotated()
        {
            SetValue("ABCDEFG", PropertyAnnotatedMultiValidations);

            AssertValidationError(
                PropertyAnnotatedMultiValidations,
                "Please enter within 6 characters.",
                "Please enter a digit only."
            );
            Assert.That(errorsChangedOccurred, Is.True);
        }

        [Test]
        public void DoesNotGetsErrorForValidPropertyWhenMultiValidationAttributesAreAnnotated()
        {
            SetValue("ABCDEFG", PropertyAnnotatedMultiValidations);

            AssertValidationError(
                PropertyAnnotatedMultiValidations,
                "Please enter within 6 characters.",
                "Please enter a digit only."
            );
            Assert.That(errorsChangedOccurred, Is.True);

            SetValue("123456", PropertyAnnotatedMultiValidations);

            AssertNoValidationError(PropertyAnnotatedMultiValidations);
            Assert.That(errorsChangedOccurred, Is.True);
        }

        [Test]
        public void GetsErrorForInvalidPropertyWhenValidationEventIsRegistered()
        {
            PropertyNotAnnotatedValidation.PropertyValueValidate += propertyValueValidate;
            SetValue("ABCDEFGHIJK", PropertyNotAnnotatedValidation);

            AssertValidationError(
                PropertyNotAnnotatedValidation,
                "The value does not correct."
            );
            Assert.That(errorsChangedOccurred, Is.True);
        }

        [Test]
        public void DoesNotGetErrorForValidPropertyWhenValidationEventIsRegistered()
        {
            PropertyNotAnnotatedValidation.PropertyValueValidate += propertyValueValidate;
            SetValue("ABCDEFGHIJK", PropertyNotAnnotatedValidation);

            AssertValidationError(
                PropertyNotAnnotatedValidation,
                "The value does not correct."
            );
            Assert.That(errorsChangedOccurred, Is.True);


            SetValue("Correct", PropertyNotAnnotatedValidation);

            Assert.That(errorsChangedOccurred, Is.True);
            AssertNoValidationError(PropertyNotAnnotatedValidation);
        }

        [Test]
        public void GetsErrorForInvalidPropertyWhenValidationAttributeIsAnnotatedAndValidationEventIsRegistered()
        {
            PropertyAnnotatedValidation.PropertyValueValidate += propertyValueValidate;
            SetValue("ABCDEFGHIJK", PropertyAnnotatedValidation);

            AssertValidationError(
                PropertyAnnotatedValidation,
                "Please enter String Expression within 10 characters.",
                "The value does not correct."
            );
            Assert.That(errorsChangedOccurred, Is.True);
        }

        [Test]
        public void DoesNotGetErrorForValidPropertyWhenValidationAttributeIsAnnotatedAndValidationEventIsRegistered()
        {
            PropertyAnnotatedValidation.PropertyValueValidate += propertyValueValidate;
            SetValue("ABCDEFGHIJK", PropertyAnnotatedValidation);

            AssertValidationError(
                PropertyAnnotatedValidation,
                "Please enter String Expression within 10 characters.",
                "The value does not correct."
            );
            Assert.That(errorsChangedOccurred, Is.True);


            SetValue("Correct", PropertyAnnotatedValidation);

            Assert.That(errorsChangedOccurred, Is.True);
            AssertNoValidationError(PropertyAnnotatedValidation);
        }

        [Test]
        public void CancelsChangeOfValueWhenValueIsInvalidBySettingTrueToMethodParameter()
        {
            PropertyAnnotatedValidation.EnableValidation(() => PropertyAnnotatedValidation, true);

            PropertyAnnotatedValidation.Value = "ABCDEFGHIJ";
            Assert.That(PropertyAnnotatedValidation.Value, Is.EqualTo("ABCDEFGHIJ"));

            PropertyAnnotatedValidation.Value = "ABCDEFGHIJK";
            Assert.That(PropertyAnnotatedValidation.Value, Is.EqualTo("ABCDEFGHIJ"));
        }

        [Test]
        public void EnsuresThatPropertyValueIsValidated()
        {
            RequiredProperty.EnsureValidation();

            AssertValidationError(RequiredProperty, "Please enter a value.");
        }
    }
}
