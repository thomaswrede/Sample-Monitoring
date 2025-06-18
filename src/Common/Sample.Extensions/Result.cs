using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace System
{
  /// <summary>
  /// Basisklasse für alle Ergebnisse einer Operation.
  /// </summary>
  public class Result : IResult
  {
    #region " Variablen/ Properties                       "

    #region " --> State                                   "
    /// <summary>
    /// Liefert den Erfolgsstatus des Ergebnisses.
    /// </summary>
    public ResultState State
    {
      get => this._State;
      private set
      {
        this._State = value;
        if (this._Parent != default
          && this._Parent.State < value)
        {
          _ = this._Parent._State = value;
        }
      }
    }
    private ResultState _State;
    #endregion

    #region " --> Message                                 "
    /// <summary>
    /// Liefert die Nachricht des Ergebnisses.
    /// </summary>
    public String Message => this._Message.ToString();
    private ReadOnlyMemory<Char> _Message;
    #endregion

    #region " --> Exception                               "
    /// <summary>
    /// Liefert die von der Operation geworfene Exception.
    /// </summary>
    public Exception Exception => this._Exception == default
          && this._Results?.Count > 0
          ? this._Results
            .Select(e => e.Exception)
            .FirstOrDefault(e => e != default)
          : this._Exception;
    private readonly Exception _Exception;
    #endregion

    #region " --> Sink                                    "
    /// <summary>
    /// Liefert das Objekt auf dem die Operation ausgeführt wurde.
    /// </summary>
    public Object Sink => this._Sink;
    private Object _Sink;
    #endregion

    #region " --> Results                                 "
    /// <summary>
    /// Liefert die Liste aller Operationsergebnisse.
    /// </summary>
    public ReadOnlyCollection<IResult> Results => new(this._Results);
    private readonly List<IResult> _Results = [];
    #endregion

    #region " --> Parent                                  "
    public IResult Parent
    {
      get => this._Parent;
      set => this._Parent = value as Result;
    }
    private Result _Parent;
    #endregion

    #region " --> AdditionalData                          "
    public IDictionary<String, ReadOnlyMemory<Char>> AdditionalData { get; }
    #endregion

    #endregion

    #region " Konstruktor/ Destruktor                     "

    #region " --> New                                     "    
    protected Result(ResultState state, ReadOnlyMemory<Char> message, Exception exception, Object sink, IResult parent, IDictionary<String, ReadOnlyMemory<Char>> additionalData)
    {
      this._Results = [];
      this.State = state;
      this._Message = message;
      this._Sink = sink;
      this._Exception = exception;
      this._Parent = parent as Result;
      this.AdditionalData = additionalData;
    }
    #endregion

    #endregion

    #region " öffentliche Methoden                        "

    #region " --> Add                                     "
    public void Add(Object sink, ResultState state, ReadOnlyMemory<Char> message = default, IDictionary<String, ReadOnlyMemory<Char>> additionalData = default)
    {
      Result result = new(state, message, null, sink, this, additionalData);
      if (result.State > this._State) { this.State = result.State; }

      this.Add(result);
    }
    public void Add(Object sink, Exception exception, ReadOnlyMemory<Char> message = default, IDictionary<String, ReadOnlyMemory<Char>> additionalData = default)
    {
      Result opResult = new(ResultState.Error, message, exception, sink, this, additionalData);
      if (opResult.State > this._State) { this.State = opResult.State; }

      this.Add(opResult);
    }
    public void Add(IResult result)
    {
      if (result == default) { return; }
      result.Parent = this;

      _ = this._Results.TryAdd(result);

      if (this._State == ResultState.None)
      {
        this.State = result.State;
        this._Message = result.Message.AsMemory();
        this._Sink = result.Sink;
      }

      if (result.State > this._State) { this.State = result.State; }
    }
    #endregion

    #region " --> Create                                  "
    public static Result Create(ResultState state = ResultState.Ok, ReadOnlyMemory<Char> message = default, Exception exception = default, Object sink = default, IDictionary<String, ReadOnlyMemory<Char>> additionalData = default) => new(
        state: state,
        message: message,
        exception: exception,
        sink: sink,
        parent: default,
        additionalData: additionalData);
    public static async Task<Result> CreateAsync(ResultState state = ResultState.Ok, ReadOnlyMemory<Char> message = default, Exception exception = default, Object sink = default, IDictionary<String, ReadOnlyMemory<Char>> additionalData = default) =>
      await Task.FromResult(Create(state, message, exception, sink, additionalData));
    #endregion

    #region " --> Warning                                 "
    public static Result Warning(ReadOnlyMemory<Char> message = default, Exception exception = default, Object sink = default, IDictionary<String, ReadOnlyMemory<Char>> additionalData = default) => new(
        state: ResultState.Warning,
        message: message.IsEmpty ? "Warnung".AsMemory() : message,
        exception: exception,
        sink: sink,
        parent: default,
        additionalData: additionalData);
    public static async Task<Result> WarningAsync(ReadOnlyMemory<Char> message = default, Exception exception = default, Object sink = default, IDictionary<String, ReadOnlyMemory<Char>> additionalData = default) =>
        await Task.FromResult(Warning(message, exception, sink, additionalData));
    #endregion

    #region " --> Error                                   "
    public static Result Error(ReadOnlyMemory<Char> message = default, Exception exception = default, Object sink = default, IDictionary<String, ReadOnlyMemory<Char>> additionalData = default) => new(
        state: ResultState.Error,
        message: message.IsEmpty ? "Fehler".AsMemory() : message,
        exception: exception,
        sink: sink,
        parent: default,
        additionalData: additionalData);
    public static async Task<Result> ErrorAsync(ReadOnlyMemory<Char> message = default, Exception exception = default, Object sink = default, IDictionary<String, ReadOnlyMemory<Char>> additionalData = default) =>
      await Task.FromResult(Error(message, exception, sink, additionalData));
    #endregion

    #region " --> ArgumentNull                            "
    public static Result ArgumentNull(String argumentName, Object sink = default, IDictionary<String, ReadOnlyMemory<Char>> additionalData = default) => new(
        state: ResultState.Error,
        message: String.Format(Globalization.CultureInfo.CurrentCulture, Constants.ERR_MSG_ARGUMENT_NULL, argumentName).AsMemory(),
        exception: null,
        sink: sink,
        parent: default,
        additionalData: additionalData);
    public static async Task<Result> ArgumentNullAsync(String argumentName, Object sink = default, IDictionary<String, ReadOnlyMemory<Char>> additionalData = default) =>
      await Task.FromResult(ArgumentNull(argumentName, sink, additionalData));
    #endregion

    #region " --> InvalidArgument                         "
    public static Result InvalidArgument(String argumentName, Object sink = default, IDictionary<String, ReadOnlyMemory<Char>> additionalData = default) => new(
        state: ResultState.Error,
        message: String.Format(Globalization.CultureInfo.CurrentCulture, Constants.ERR_MSG_INVALID_ARGUMENT, argumentName).AsMemory(),
        exception: null,
        sink: sink,
        parent: default,
        additionalData: additionalData);
    public static async Task<Result> InvalidArgumentAsnyc(String argumentName, Object sink = default, IDictionary<String, ReadOnlyMemory<Char>> additionalData = default) =>
      await Task.FromResult(InvalidArgument(argumentName, sink, additionalData));
    #endregion

    #region " --> Timeout                                 "
    public static Result Timeout(Object sink = default, IDictionary<String, ReadOnlyMemory<Char>> additionalData = default) => new(
        state: ResultState.Error,
        message: Constants.ERR_MSG_TIMEOUT.AsMemory(),
        exception: null,
        sink: sink,
        parent: default,
        additionalData: additionalData);
    public static async Task<Result> TimeoutAsync(Object sink = default, IDictionary<String, ReadOnlyMemory<Char>> additionalData = default) =>
      await Task.FromResult(Timeout(sink, additionalData));
    #endregion

    #region " --> Security                                "
    public static Result Security(Object sink = default, IDictionary<String, ReadOnlyMemory<Char>> additionalData = default) => new(
        state: ResultState.Error,
        message: Constants.ERR_MSG_MISSING_RIGHTS.AsMemory(),
        exception: null,
        sink: sink,
        parent: default,
        additionalData: additionalData);
    public static async Task<Result> SecurityAsync(Object sink = default, IDictionary<String, ReadOnlyMemory<Char>> additionalData = default) =>
      await Task.FromResult(Security(sink, additionalData));
    #endregion

    #region " --> Success                                 "
    public static Result Success(Object sink = default, IDictionary<String, ReadOnlyMemory<Char>> additionalData = default) =>
      new(
        state: ResultState.Ok,
        message: ReadOnlyMemory<Char>.Empty,
        exception: default,
        sink: sink,
        parent: default,
        additionalData: additionalData);
    public static async Task<Result> SuccessAsync(Object sink = default, IDictionary<String, ReadOnlyMemory<Char>> additionalData = default) =>
      await Task.FromResult(Success(sink, additionalData));
    #endregion

    #endregion
  }

  /// <summary>
  /// Basisklasse für alle Ergebnisse einer Operation.
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public class Result<T> : Result, IResult<T>
  {
    #region " Variablen/ Properties                       "

    #region " --> Data                              "
    /// <summary>
    /// Liefert / Setzt die Daten des Ergebnisses.
    /// </summary>
    public T Data { get; set; }
    #endregion

    #endregion

    #region " Konstruktor                                 "

    #region " --> New                                     "
    protected Result(ResultState state, ReadOnlyMemory<Char> message, Exception exception, Object sink, IResult parent, IDictionary<String, ReadOnlyMemory<Char>> additionalData, T data)
      : base(state, message, exception, sink, parent, additionalData) => this.Data = data;
    #endregion

    #endregion

    #region " öffentliche Methoden                        "

    #region " --> Create                                  "
    public static Result<T> Create(ReadOnlyMemory<Char> message = default, Object sink = default, IDictionary<String, ReadOnlyMemory<Char>> additionalData = default, T data = default) => new(state: ResultState.Ok, message: message, exception: default, sink: sink, parent: default, additionalData: additionalData, data: data);
    #endregion

    #region " --> Error                                   "
    public static new Result<T> Error(ReadOnlyMemory<Char> message = default, Exception exception = default, Object sink = default, IDictionary<String, ReadOnlyMemory<Char>> additionalData = default) => new(state: ResultState.Error, message: message, exception: exception, sink: sink, parent: default, additionalData: additionalData, data: default);
    public static async new Task<Result<T>> ErrorAsync(ReadOnlyMemory<Char> message = default, Exception exception = default, Object sink = default, IDictionary<String, ReadOnlyMemory<Char>> additionalData = default) => await Task.FromResult<Result<T>>(new(state: ResultState.Error, message: message, exception: exception, sink: sink, parent: default, additionalData: additionalData, data: default));
    #endregion

    #region " --> ArgumentNull                            "
    public static new Result<T> ArgumentNull(String argumentName, Object sink = default, IDictionary<String, ReadOnlyMemory<Char>> additionalData = default) => new(
        state: ResultState.Error,
        message: String.Format(Globalization.CultureInfo.CurrentCulture, Constants.ERR_MSG_ARGUMENT_NULL, argumentName).AsMemory(),
        exception: null,
        sink: sink,
        parent: default,
        additionalData: additionalData,
        data: default);
    #endregion

    #region " --> InvalidArgument                         "
    public static new Result<T> InvalidArgument(String argumentName, Object sink = default, IDictionary<String, ReadOnlyMemory<Char>> additionalData = default) => new(
        state: ResultState.Error,
        message: String.Format(Globalization.CultureInfo.CurrentCulture, Constants.ERR_MSG_INVALID_ARGUMENT, argumentName).AsMemory(),
        exception: null,
        sink: sink,
        parent: default,
        additionalData: additionalData,
        data: default);
    #endregion

    #region " --> Timeout                                 "
    public static new Result<T> Timeout(Object sink = default, IDictionary<String, ReadOnlyMemory<Char>> additionalData = default) => new(
        state: ResultState.Error,
        message: Constants.ERR_MSG_TIMEOUT.AsMemory(),
        exception: null,
        sink: sink,
        parent: default,
        additionalData: additionalData,
        data: default);
    #endregion

    #region " --> Security                                "
    public static new Result<T> Security(Object sink = default, IDictionary<String, ReadOnlyMemory<Char>> additionalData = default) => new(
        state: ResultState.Error,
        message: Constants.ERR_MSG_MISSING_RIGHTS.AsMemory(),
        exception: null,
        sink: sink,
        parent: default,
        additionalData: additionalData,
        data: default);
    #endregion

    #region " --> Success                                 "
    public static Result<T> Success(T data = default, Object sink = default, IDictionary<String, ReadOnlyMemory<Char>> additionalData = default) => new(state: ResultState.Ok, message: ReadOnlyMemory<Char>.Empty, exception: default, sink: sink, parent: default, additionalData: additionalData, data: data);
    public static async Task<Result<T>> SuccessAsync(T data = default, Object sink = default, IDictionary<String, ReadOnlyMemory<Char>> additionalData = default) => await Task.FromResult<Result<T>>(new(state: ResultState.Ok, message: ReadOnlyMemory<Char>.Empty, exception: default, sink: sink, parent: default, additionalData: additionalData, data: data));
    #endregion

    #endregion

  }
}
