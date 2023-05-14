# Changelog

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