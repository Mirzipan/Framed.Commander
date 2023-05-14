[![openupm](https://img.shields.io/npm/v/net.mirzipan.heist?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/net.mirzipan.heist/) ![GitHub](https://img.shields.io/github/license/Mirzipan/Framed.Commander)

# Mirzipan.Heist

Slightly enriched command pattern, continuing the tradition of using words associated with murder mystery in the package names.

## Dependencies

The most notable dependency is [Reflex](https://github.com/gustavopsantos/Reflex), which is a super fast and super convenient library for dependency injection.

## Getting Started
A short guide to get you up-and-running.

1) Add [Reflex](https://github.com/gustavopsantos/Reflex) to your project and create `ProjectScope` prefab according to their instructions.
2) Add `SinglePlayerHeistInstaller` script to your `ProjectScope` prefab.
3) Create your custom actions and commands (see examples in [Sandbox.Heist](https://github.com/Mirzipan/Sandbox.Heist) for a quick reference).

In case you want more than the basics, refer to the documention below to learn how to extend Heist.

## The Flow
Similar to regular command pattern, but uses actions to trigger the commands.
There are two processors, `IClientProcessor` and `IServerProcessor`, which will be refered to as `client` and `server`.

1) Create action and tell client to process it
2) Client validates your action
3) Client sends the action to network
4) Server receives the action
5) Server creates command(s) based on action (usually atomic operations) and potentially more actions as well
6) Server sends the command(s) to network
7) Client receives and executes the command(s)

***!Important: Everything in the flow besides processor should be kept stateless!***

## Action

### Action Container
An empty class that implements `IActionContainer`.
It needs to include an `IAction` and an `IActionHandler` subtype.

### Action Data
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
    ValidationResult Validate(T action, ValidationOptions options);
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

## Command

### Command Container
An empty class that implements `ICommandContainer`.
It needs to include an `ICommand` and an `ICommandReceiver` subtype.

### Command Data
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
There is a client and a server processor.
Most of the time, you will interface with the client-side of things.

### Client
```csharp
public interface IClientProcessor
{
    ValidationResult Validate(IAction action);
    void Process(IAction action);
    void Tick();
    event Action<ICommand> OnCommandExecution;
    event Action<ICommand> OnCommandExecuted;
}
```
* `Validate` - this exists for when you want to do some external validation, such as in the UI, in order to disabled a button or some other element.
* `Process` - call this to have your action added to the execution queue (the default processor will also perform validation here).
* `Tick` - needs to be called manually in order for a processor to do its processing.
* `OnCommandExecution` - invoked immediatelly before execution of a command.
* `OnCommandExecuted` - invoked immediatelly after a command finished being executed.

### Server
```csharp
public interface IServerProcessor
{
    ValidationResult Validate(IAction action);
    ValidationResult Validate(IAction action, ValidationOptions options);
    void Process(IAction action);
    void ProcessFromClient(IAction action);
    void ProcessFromServer(IAction action);
    void Execute(ICommand command);
}
```
* `Validate` - same as `IClientProcessor`, includes an overload which have have further options specified.
* `Process` - same as calling `ProcessFromServer`.
* `ProcessFromClient` - validates and processes an action sent by client.
* `ProcessFromServer` - validates and processes an action sent by server/itself, such as when enqueing an `IAction` from `IActionHandler.Process()`.
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

## Future Plans
The following are likely further extensions of the package, in no particular order or priority

### Reactive Systems / Event Bus
Some form of registry for things that may want to respond to actions being processed and/or commands being executed.
This will mostly likely end up being generic, so that systems can simple declare what type of action/command they are interested in.
This role could also be served by an event bus, where the command would dispatch the relevant event(s) and subscribers could then execute whatever they see fit.

### Metadata Crawler
Separation of the type-crawling part (`IMetadataIndexer` and `MetadataContainer`) into a separate package, ideally reusable for indexing other types.
Not a priority until a need for indexers in other packages would arise.

### Debug View
A Debugger window where you can see all of the registered actions/commands. 
This would include some extra information, such as how many times an action was validated and processed, and a command was executed.
`Reflex` debugger already shows the count of resolutions for each action/command, but that tells use nothing about how the processing/execution went.

### Multiplayer Support
This is likely the lowest priority, because no project for which this package is primary intended, needs multiplayer at this stage.
It already has some preparation for it, such as separate processors.
The current architecture is already reasonably friendly for a client-server extension, though it would only work for single-player, as there is no way of identifying multiple clients or messaging between them.
It also lacks any form of "local simulation, until server tells us otherwise" when processing actions.

### Processor Ticking
Currently `IClientProcessor` needs to be ticked in a rather manual way.
Maybe an `ITickable` interface could be used by all things tickable, a central manager would do the ticking.
Something like this would likely end up in [Mirzipan.Scheduler](https://github.com/Mirzipan/Mirzipan.Scheduler), thus adding another dependency.
