[![openupm](https://img.shields.io/npm/v/net.mirzipan.heist?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/net.mirzipan.heist/) ![GitHub](https://img.shields.io/github/license/Mirzipan/Framed.Commander)

# Mirzipan.Heist

Slightly enriched command pattern, continuing the tradition of using words associated with murder mystery in the package names.
It can support multiple clients with a single server.

## Dependencies

The most notable dependency is [Reflex](https://github.com/gustavopsantos/Reflex), which is a super fast and super convenient library for dependency injection.

## Getting Started
A short guide to get you up-and-running.

1) Add [Reflex](https://github.com/gustavopsantos/Reflex) to your project and create `ProjectScope` prefab according to their instructions.
2) Add `SinglePlayerHeistInstaller` script to your `ProjectScope` prefab or onto your `SceneScope` in the scene.
3) Add `ProcessorTicker` to a GameObject in your scene. 
4) Create your custom actions and commands (see examples in [Sandbox.Heist](https://github.com/Mirzipan/Sandbox.Heist) for a quick reference).
5) Have a separate copy of your data for client and for server.

In case you want more than the basics, refer to the documentation below to learn how to extend Heist.

## The Flow
Similar to regular command pattern, but uses actions to trigger the commands.
There are two processors, `IClientProcessor` and `IServerProcessor`, which will be refered to as `client` and `server`.

1) Create action and tell client to process it
2) Client validates your action
3) Client sends the action to network
4) Server receives the action
5) Server creates command(s) based on action (usually atomic operations) and potentially more actions as well
6) Server executes the command(s), if necessary
7) Server sends the command(s) to network, if necessary
8) Client receives and executes the command(s)

***!Important: Everything in the flow besides processors and queue should be kept stateless!***

## Action

### Action Container
An empty class that implements `IActionContainer`.
It needs to include an `IAction` and an `IActionHandler` subtype.
The container exists to enforce 1:1 pairing of actions and their handlers.

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
    ValidationResult Validate(T action, int clientId, ValidationOptions options);
    void Process(T action, int clientId);
}
```
`ValidationResult` holds a single `uint`, where `0` means successful validation.
The rest of the value range (`1 - 4,294,967,295`) can be used for your custom error codes.

The abstract class comes with a handful of convenience methods, so that you do not need to worry about flow, just about your game logic.
```csharp
static ValidationResult Pass();
static ValidationResult Fail(uint reason);
void Enqueue(IAction action);
void Enqueue(ICommand command, int[] clientIds, ExecuteOn target);
void Enqueue(ICommand command, int clientId, ExecuteOn target);
void Enqueue(ICommand command, ExecuteOn target);

```
* `Pass` - returns a `ValidationResult` with code `0`.
* `Fail` - returns a `ValidationResult` with whatever error code you find appropriate, except `0`.
* `Enqueue action`- adds an action to be processed on server.
* `Enqueue command` - adds command to be executed based on options and optionally sent to one or more clients.

## Command

### Command Container
An empty class that implements `ICommandContainer`.
It needs to include an `ICommand` and an `ICommandReceiver` subtype.
The container exists to enforce 1:1 pairing of commands and their receivers.

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
    event Action<ICommand> OnCommandExecution;
    event Action<ICommand> OnCommandExecuted;
    ValidationResult Validate(IAction action);
    void Process(IAction action);
    void Tick();
}
```
* `OnCommandExecution` - invoked immediately before execution of a command.
* `OnCommandExecuted` - invoked immediately after a command finished being executed.
* `Validate` - this exists for when you want to do some external validation, such as in the UI, in order to disabled a button or some other element.
* `Process` - call this to have your action added to the execution queue (the default processor will also perform validation here).
* `Tick` - needs to be called manually in order for a processor to do its processing.

### Server
```csharp
public interface IServerProcessor
{
    event Action<ICommand> OnCommandExecution;
    event Action<ICommand> OnCommandExecuted;
    ValidationResult Validate(IAction action);
    ValidationResult Validate(IAction action, int clientId, ValidationOptions options);
    void Process(IAction action);
    void ProcessClientAction(IAction action, int clientId);
    void ProcessServerAction(IAction action);
    void Execute(ICommand command, int[] clientIds, ExecuteOn target);
}
```
* `OnCommandExecution` - same as `IClientProcessor`.
* `OnCommandExecuted` - same as `IClientProcessor`.
* `Validate` - same as `IClientProcessor`, includes an overload which have have further options specified.
* `Process` - same as calling `ProcessServerAction`.
* `ProcessClientAction` - validates and processes an action sent by client.
* `ProcessServerAction` - validates and processes an action sent by server/itself, such as when enqueing an `IAction` from `IActionHandler.Process()`.
* `Execute` - executes the command and sends it to clients based on options.

### Queue
Currently there is only `LoopbackQueue`, that just passes its inputs along.
This means that any client(s)-server architecture will only work locally.
You may, however, implement your own version of queues, should you so desire.

#### For Client
```csharp
public interface IOutgoingActions : IDisposable
{
    void Send(IAction action);
}
```
* `Send` - invoked to send an `IAction` to server.

```csharp
public interface IIncomingCommands : IDisposable
{
    event CommandReceived OnCommandReceived;
}    
```
* `OnCommandReceived` - invoked when an `ICommand` was received.

#### For Server
```csharp
public interface IIncomingActions
{
    event ActionReceived OnActionReceived;
}
```
* `OnActionReceived` - invoked when an `IAction` was received from a `clientId`.

```csharp
public interface IOutgoingCommands : IDisposable
{
    void Send(ICommand command, int[] clientIds, bool sendToAll);
}
```
* `Send` - invoked to send an `ICommand` to one or more clients.

## Future Plans
The following are likely further extensions of the package, in no particular order or priority

### Debug View
A Debugger window where you can see all of the registered actions/commands. 
This would include some extra information, such as how many times an action was validated and processed, and a command was executed.
`Reflex` debugger already shows the count of resolutions for each action/command, but that tells use nothing about how the processing/execution went.

### Processor Ticking
Currently `IClientProcessor` needs to be ticked in a rather manual way.
Maybe an `ITickable` interface could be used by all things tickable, a central manager would do the ticking.
Something like this would likely end up in [Mirzipan.Scheduler](https://github.com/Mirzipan/Mirzipan.Scheduler), thus adding another dependency.

## Inspiration
There were many inspirations for this, most notably:
* experience from mobile games with server-side logic, and trying to reduce pointless code duplication 
* various comments by the dev of [Glorious Companions](https://store.steampowered.com/app/1001040/Glorious_Companions/)
* http://codensuch.com/trap-labs-series/part3