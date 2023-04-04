# Release note

## v4.2.1

### Bug fix

- Fixed an issue that DataContextChanged event can't be handled on attaching a controller when a name of a root element is specified and a name of its event handler is not specified.

## v4.2.0

### Changes

- Change Charites version to 2.2.0.
- Change Charites.Binding version to 2.2.0.

### Bug fix

- Fixed an issure where events that are not a routed event can't be handled.

## v4.1.0

### Add

- Add the IWpfElementFinder interface that extends the IElementFinder&lt;FrameworkElement&gt; interface.
- Add the ElementFinder property to the WpfController class.

### Changes

- Change Charites version to 2.1.0.
- Change the WpfEventHandlerExtension and the CommandHandlerExtension so that event handlers that have parameters attributed by the following attribute can be injected . 
  - FromDIAttribute
  - FromElementAttribute
  - FromDataContextAttribute

## v4.0.0

### Add

- Add the Resolve method to the CommandHandlerBase.Executor class to be able to inject dependencies of parameters that are specified by the FromDI attribute.
- Add the Event property to the CommandHandlerAttribute class. This property must be set to handle command events.
- Add the following methods to the CommandHandlerBase.Executor class so that the PreviewExecuted and PreviewCanExecute event can be raised.
  - RaisePreviewExecuted
  - RaisePreviewCanExecute
  - RaisePreviewExecutedAsync
  - RaisePreviewCanExecuteAsync

### Changes

- Change the target framework version to .NET 6.0.
- Change Charites version to 2.0.0.
- Change Charites.Bindings version to 2.1.0.
- Enable Nullable reference types.
- Change to prefer to resolve parameters with the specified resolver when they are resolved by dependecies.
- Change the CommandHandlerExtension class and the CommandHandlerBase class so that the PreviewExecuted and PreviewCanExecute event can be handled.

## v3.1.1

### Changes

- Change Charites version to 1.3.2.
- Modify how to retrieve an event name of an event hadnler or a command handler from a method that represents an event handler using naming convention. If its name ends with "Async", it is ignored.

## v3.1.0

### Changes

- Change the framework version to .NET Core 3.1 and .NET 5.0.
- Change Charites version to 1.3.1.
- Change Charites.Bindings version to 1.2.1.

## v3.0.0

### Changes

- Change the framework version to .NET Core 3.0.

## v2.3.0

### Add

- Add the function to inject dependencies of parameters based on the FromDIAttribute attribute.

### Changes

- Change Charites version to 1.3.0.

## v2.2.0

### Add

- Add the function to set command handlers using a naming convention (ElementName_Executed or ElementName_CanExecute).

### Changes

- Change Charites version to 1.2.0.
- Change Charites.Bindings version to 1.2.0.

## v2.1.0

### Add

- Add the UnhandledException event to the WpfController.

## v2.0.1

### Bug fix

Fixed to be able to handle the DataContextChanged event.

## v2.0.0

### Changes

- Move bindings properties to the Charites.Bindings assembly.
- Move attributes and base implementations to the Charites assembly.
- Remove the Controllers attached property of the WpfController. The controller is specified using the ViewAttribute.
- Change the namespace from Fievus to Charites.
- Change the testing framework from NUnit to Carna.

## v1.4.0

### Add

- Add BindTwoWay method with converters that convert the value from/to the source value.
- Add IElementInjector that injects elements in a target element to the WPF controller.
- Add IDataContextInjector that injects a data context to the WPF controller.
- Add ElementInjectionException that is thrown when an element injection is failed.
- Add DataContextInjectionException that is thrown when a data context injection is failed.

### Changes

- Change RoutedEventHandlerAction class from a nested class to an independent class.

## v1.3.1

### Changes

- Change the way to find RoutedEvent so that an attached event can be handled.

### Bug fix

- Fixed the name value of DisplayNameAttribute when it is retrieved from a resource.
- Fixed the condition to find Command when CommandHandlerAttribute is specified.

## v1.3.0

### Add

- Add RaiseAsync method to RoutedEventHandlerBase.Executor class so that an async event handler can be awaited.
- Add RaiseExecutedAsync and RaiseCanExecuteAsync methods to CommandHandlerBase.Executor class so that an async event handler can be awaited.
- Add foundElementOnly parameter to SetElement method of the WpfController class. If its value is true, an element is not set to the WPF controller when it is not found in the specified element; otherwise, null is set. Its default value is false.
- Add the event call when an element is attached to a controller. The DataContextChanged event whose EventArgs is null is raised.
- Add the method that binds some observable properties to ObservableProperty class.

### Changes

- Change the type of the element specified by ElementAttribute. Its type is not only FrameworkElement but also any object.

## v1.2.1

### Changes

- Change event subscriptions of an element that is associated with a WPF controller.

## v1.2.0

### Add

- Add IWpfControllerFactory.

## v1.1.1

### Bug fix

- Fixed WpfMvc.nuspec

## v1.1.0

### Add

- Add CommandHandler.
- Add IWpfControllerExtension.