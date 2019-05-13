Architectural guidance for developing Xamarin.Forms enterprise apps that are easier to test, maintain, and evolve. Guidance is provided on how to implement the Model-View-ViewModel (MVVM) pattern, dependency injection, navigation, validation, and configuration management, while maintaining loose coupling. 

# Mobile app solution
The Architecture mobile app solution organizes the source code and other resources into projects. All of the projects use folders to organize the source code and other resources into categories. The following table outlines the projects that make up the Architecture mobile app:

|Project   |Description   |
|---|---|
|Architecture   |This project is the .NET Standard project that contains the shared code and shared UI.   |
|Architecture.Droid   |This project holds Android specific code and is the entry point for the Android app.   |
|Architecture.iOS   |This project holds iOS specific code and is the entry point for the iOS app.   |
|Architecture.UITests   |This project contains UI tests for the Architecture project.   |
|Architecture.NUnitTests|This project contains unit tests for the Architecture project.|

The classes from the Architecture mobile app can be re-used in any Xamarin.Forms app with little or no modification.

# Architecture project
The Architecture project contains the following folders:

|Folder   |Description   |
|---|---|
|Animations   | Contains classes that enable animations to be consumed in XAML.   |
|Behaviors   |Contains behaviors that are exposed to view classes.   |
|Controls   |Contains custom controls used by the app.   |
|Converters   |Contains value converters that apply custom logic to a binding.   |
|Effects   |Contains effects.  |
|Exceptions   |Contains the custom Exceptions.   |
|Extensions   |Contains extension methods for classes.   |
|Helpers   |Contains helper classes for the app.   |
|Models   |Contains the model classes for the app.   |
|Properties   |Contains AssemblyInfo.cs, a .NET assembly metadata file.   |
|Services   |Contains interfaces and classes that implement services that are provided to the app.   |
|Triggers   |Contains triggers, which are used to invoke an animation in XAML.   |
|Validations   |Contains classes involved in validating data input.   |
|ViewModels   |Contains the application logic that's exposed to pages.   |
|Views   |Contains the pages for the app.   |

# Xamarin solution structure

 ```
 .
├── Architecture.sln
├── artifacts
├── docs
│   ├── MobileProjectStructure.md
│   ├── NUnitTests.md
│   └── UITests.md
├── lib
├── packages
├── src
│   ├── Architecture
│   │   ├── App.xaml
│   │   ├── App.xaml.cs
│   │   ├── Cells
│   │   ├── Constants.cs
│   │   ├── Controls
│   │   ├── Exceptions
│   │   ├── Architecture.csproj
│   │   ├── Enums
│   │   ├── EventArguments
│   │   ├── Extensions
│   │   ├── Helpers
│   │   ├── Hubs
│   │   ├── Managers
│   │   ├── Models
│   │   ├── Resources
│   │   ├── Services
│   │   ├── ViewModels
│   │   └── Views
│   ├── Architecture.Android
│   │   ├── AndroidInitializer.cs
│   │   ├── Assets
│   │   ├── Architecture.Android.csproj
│   │   ├── Keys
│   │   ├── MainActivity.cs
│   │   ├── MainApplication.cs
│   │   ├── Properties
│   │   ├── Renderers
│   │   ├── Resources
│   │   └── Services
│   └── Architecture.iOS
│       ├── AppDelegate.cs
│       ├── Assets.xcassets
│       ├── Entitlements.plist
│       ├── Architecture.iOS.csproj
│       ├── Info.plist
│       ├── Main.cs
│       ├── Properties
│       ├── Renderers
│       ├── Resources
│       ├── Services
│       └── iOSInitializer.cs
├── stylecop
│   └── StyleCopRuleSet.ruleset
└── tests
    └── Architecture.UITests
        ├── AppInitializer.cs
        ├── Architecture.UITests.csproj
        ├── Tests.cs
        └── packages.config
 ```
  
# Platform projects

The platform projects contain effect implementations, custom renderer implementations, and other platform-specific resources.

# Dependency injection

Dependency injection is a specialized version of the Inversion of Control (IoC) pattern, where the concern being inverted is the process of obtaining the required dependency. With dependency injection, another class is responsible for injecting dependencies into an object at runtime. The following code example shows how the ProfileViewModel class is structured when using dependency injection:
 
 ```
public class ProfileViewModel : ViewModelBase 
{
    private IOrderService _orderService;
    public ProfileViewModel(IOrderService orderService)
{
        _orderService = orderService;
}
... }
 ```
 
 The ```ProfileViewModel``` constructor receives an ```IOrderService``` instance as an argument, injected by another class. The only dependency in the ```ProfileViewModel``` class is on the interface type. Therefore, the ```ProfileViewModel``` class doesn't have any knowledge of the class that's responsible for instantiating the ```IOrderService``` object. The class that's responsible for instantiating the ```IOrderService``` object, and inserting it into the ProfileViewModel class, is known as the dependency injection container.
 
 
 Dependency injection containers reduce the coupling between objects by providing a facility to instantiate class instances and manage their lifetime based on the configuration of the container. During the objects creation, the container injects any dependencies that the object requires into it. If those dependencies have not yet been created, the container creates and resolves their dependencies first.
 

