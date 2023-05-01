# Changelog

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