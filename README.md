[![openupm](https://img.shields.io/npm/v/net.mirzipan.heist?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/net.mirzipan.heist/) ![GitHub](https://img.shields.io/github/license/Mirzipan/Framed.Commander)

# Mirzipan.Heist

Slightly enriched command pattern, continuing the tradition of using words associated with murder mystery in the package names.

## Dependencies

The most notable dependency is [Reflex](https://github.com/gustavopsantos/Reflex), which is a super fast and super convenient library for dependency injection.

## The Flow
Similar to regular command pattern, but uses actions to trigger the commands.
There are two processors, client and server.

1) Create action and send it to your client
2) Client validates your action
3) Client sends the action to network
4) Server receives the action
5) Server creates command(s) based on action (usually atomic operations) and potentially more actions as well
6) Server sends the command(s) to network
7) Client receives the command(s)
8) Client executes the command(s)

***!Important: Everything in the flow besides processor is stateless!***

## User Types
These are the types you will mostly come into contact with when implementing your game logic.

### Action Container
An empty class that implements `IActionContainer`.
It needs to include an `IAction` and an `IActionHandler` subtype.

### Action
Simple data type that implements `IAction` to hold all information that is necessary for validation and processing.
Action means anything that could be a player action.

**Examples:**
* purchase item from shop
* spawn an enemy
* summon an ally
* cast a spell
* etc..

Additionally, you may use it for actions that player might do while not in-game.

**Examples:**
* load game
* save game
* change game settings
* connect to server
* host a game
* etc..

### Handler
Handles validation and processing of actions.
The base interface is `IActionHandler`, but ideally, you want to use `AActionHandler<T>` for convenience.
Relevant part of the API:
```csharp
abstract class AActionHandler<T> : IActionHandler where T : IAction
{
    ValidationResult Validate(T action);
    void Process(T action);
}
```
`ValidationResult` holds a single `uint`, where `0` means successful validation.
The rest of the value range (`1 - 4,294,967,295`) can be used for your custom error codes.

The abstract class comes with a handful of convenience methods, so that you do not need to worry about flow, just about your game logic.
```csharp
static ValidationResult Pass();
static ValidationResult Fail(uint reason);
void Enqueue(IProcessable processable);
```
* `Pass` - returns a `ValidationResult` with code `0`.
* `Fail` - returns a `ValidationResult` with whatever error code you find appropriate.
* `Enqueue(action / command)`- adds an action to be processed or command to be executed.

### Command Container
An empty class that implements `ICommandContainer`.
It needs to include an `ICommand` and an `ICommandReceiver` subtype.

### Command
Simple data type that implements `ICommand` to hold all information necessary for command execution.
These can have any granularity you may see fit, but it ideally, those would be atomic operations.

**Examples:**
* add item to inventory
* remove item from inventory
* change health
* add buff
* etc..

### Receiver
Handles execution of commands.
The base interface is `ICommandReceiver`, but ideally, you want to use `ACommandReceiver<T>` for convenience.
Relevant part of the API:
```csharp
abstract class ACommandReceiver<T> : ICommandReceiver where T : ICommand
{
    void Execute(T command);
}
```
At this point, no validation checks are necessary, because any issues should have been caught by Handler.
If you want to use some kind of event bus to let the rest of your game know what happened, this is a good place to it.

## Processing

### Processors
There is client and server processor.
Basically, there is no reason for you to use the server side, it is really only necessary for action handlers.

#### Client
```csharp
public interface IClientProcessor
{
    ValidationResult Validate(IAction action);
    void Process(IAction action);
    void Tick();
}
```
* `Validate` - this exists for when you want to do some external validation, such as in the UI, in order to disabled a button or some other element.
* `Process` - call this to have your action added to the execution queue (the default processor will also perform validation here).
* `Tick` - needs to be called manually in order for a processor to do its processing.

#### Server
```csharp
public interface IServerProcessor
{
    ValidationResult Validate(IAction action);
    void Process(IAction action);
    void Execute(ICommand command);
}
```
* `Validate` - same as `IClientProcessor`.
* `Process` - same as `IClientProcessor`.
* `Execute` - this should only ever be called internally or for debug purposes.

### Network
Currently there is only `NullNetwork`, that just passes its input along. This means that any client-server architecture will only work locally.
You may, however, implement your own version of `INetwork`, should you so desire.
```csharp
public interface INetwork
{
    event ProcessableReceived OnReceived;
    void Send(IProcessable processable);
}
```
* `OnReceived` - invoked when an `IProcessable` was received.
* `Send` - invoked to send an `IProcessable`.

### Installer
If you wish to use the default one, just add `HeistInstaller` to your `ProjectScope`.
It comes with basic versions of client, server and null network.

# Examples
There are a couple of examples in [Sandbox.Heist](https://github.com/Mirzipan/Sandbox.Heist). It is not available as a package, but feel free to clone it into your project and play around.

# Future Plans
The following are likely further extensions of the package, in no particular order or priority

## Reactive Systems / Event Bus
Some form of registry for things that may want to respond to actions being processed and/or commands being executed.
This will mostly likely end up being generic, so that systems can simple declare what type of action/command they are interested in.
This role could also be served by an event bus, where the command would dispatch the relevant event(s) and subscribers could then execute whatever they see fit.

## Metadata Crawler
Separation of the type-crawling part (`IMetadataIndexer` and `MetadataContainer`) into a separate package, ideally reusable for indexing other types.
Not a priority until a need for indexers in other packages would arise.

## Debug View
A Debugger window where you can see all of the registered actions/commands. 
This would include some extra information, such as how many times an action was validated and processed, and a command was executed.
`Reflex` debugger already shows the count of resolutions for each action/command, but that tells use nothing about how the processing/execution went.

## Multiplayer Support
This is likely the lowest priority, because no project for which this package is primary intended, needs multiplayer at this stage.
It already has some preparation for it, such as separate processors.
The current architecture is already reasonably friendly for a client-server extension, though it would only work for single-player, as there is no way of identifying multiple clients or messaging between them.
It also lacks any form of "local simulation, until server tells us otherwise" when processing actions.

## Processor Ticking
Currently `IClientProcessor` needs to be ticked in a rather manual way.
Maybe an `ITickable` interface could be used by all things tickable, a central manager would do the ticking.
Something like this would likely end up in [Mirzipan.Scheduler](https://github.com/Mirzipan/Mirzipan.Scheduler), thus adding another dependency.
