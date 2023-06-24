# Changelog

## [5.2.1] - 2023-06-24

### Changed
- Reflex 4.3.2

## [5.2.0] - 2023-05-24

### Added
- OnActionProcessing and OnActionProcessed events in IServerProcessor

## [5.1.0] - 2023-05-21

### Changed
- ContainerDescriptor extensions are now chainable

## [5.0.0] - 2023-05-21

### Added
- ExecutionOptions for ICommandReceiver (currently empty, but aimed at avoiding API changes in the future)

### Changed
- ProcessorTicker now expects ClientProcessor and ServerProcessor instead of their interfaces
- ClientProcessor is now also registered in container as its concrete type
- ServerProcessor is now also registered in container as its concrete type

### Removed
- Tick from IClientProcessor, because it should not be mandatory

## [4.0.0] - 2023-05-20

### Added
- IIncomingActions and IOutgoingActions
- IIncomingCommands and IOutgoingCommands
- OnCommandExecution and OnCommandExecuted events in IServerProcessor
- ContainerDescriptorExtensions for easy adding of necessary components
- Support for multi-client operation
- Execution options for commands (client, all clients, server)

### Changed
- ClientProcessor now requires IOutgoingActions and IIncomingCommands instead of INetwork
- ServerProcessor now requires IIncomingActions and IOutgoingCommands instead of INetwork
- IActionHandler now requires clientId in all of its methods

### Removed
- HeistInstaller (superseded by ContainerDescriptorExtensions)
- NullNetwork (superseded by LoopbackQueue)

## [3.0.0] - 2023-05-14

### Added
- Action validation can now utilize ValidationOptions
- SinglePlayerHeistInstaller that contains all basics
- OnCommandExecution and OnCommandExecuted events in IClientProcessor
- Validate with options, ProcessFromClient and ProcessFromServer in IServerProcessor  

### Changed
- Reworked metadata indexing and processing to avoid potential stack overflows in installer
- HeistInstaller is now a static class containing common methods for binding install
- Resolver is now public instead of internal

## [2.0.0] - 2023-05-01

### Added
- IClientProcessor and IServerProcessor to differentiate client and server
- INetwork and its basics implementation, NullNetwork
- ProcessorTicker behaviour, so projects do not need to implement their own

### Changed
- Split LocalProcessor into ClientProcessor and ServerProcessor
- Renamed LocalProcessorInstaller to HeistInstaller
- AActionHandler now use IServerProcessor instead of IProcessor
- Classes not indented for extension are now sealed

### Removed
- LocalProcessor is no longer a thing

## [1.0.0] - 2023-04-23

### Added
- Initial release