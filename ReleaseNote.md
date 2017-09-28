# Release note

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