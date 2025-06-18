namespace System.Threading.Tasks
{
  /// <summary>
  /// Stellt Erweiterungsmethoden für <see cref="Task"/> und <see cref="Task{TResult}"/> bereit,
  /// um das Awaiten und die Handhabung von Abbruch-Token zu erleichtern.
  /// </summary>
  public static class ExtensionsTask
  {
    #region " öffentliche Methoden                        "

    #region " --> GetAwaiterResult                        "
    /// <summary>
    /// Wartet synchron auf die Fertigstellung der angegebenen <see cref="Task"/> und gibt das Ergebnis zurück.
    /// </summary>
    /// <param name="task">Die zu wartende <see cref="Task"/>.</param>
    /// <exception cref="AggregateException">Wenn die Aufgabe mit einer Ausnahme abgeschlossen wird.</exception>
    public static void GetAwaiterResult(this Task task) => task.ConfigureAwait(true).GetAwaiter().GetResult();

    /// <summary>
    /// Wartet synchron auf die Fertigstellung der angegebenen <see cref="Task{TResult}"/> und gibt das Ergebnis zurück.
    /// </summary>
    /// <typeparam name="T">Der Typ des von der Aufgabe zurückgegebenen Ergebnisses.</typeparam>
    /// <param name="task">Die zu wartende <see cref="Task{TResult}"/>.</param>
    /// <returns>Das Ergebnis der Aufgabe.</returns>
    /// <exception cref="AggregateException">Wenn die Aufgabe mit einer Ausnahme abgeschlossen wird.</exception>
    public static T GetAwaiterResult<T>(this Task<T> task) => task.ConfigureAwait(true).GetAwaiter().GetResult();
    #endregion

    #region " --> WithCancellationTokenAsync              "
    /// <summary>
    /// Wartet asynchron auf die Fertigstellung der angegebenen <see cref="Task{TResult}"/>,
    /// kann jedoch durch ein <see cref="CancellationToken"/> abgebrochen werden.
    /// </summary>
    /// <typeparam name="T">Der Typ des von der Aufgabe zurückgegebenen Ergebnisses.</typeparam>
    /// <param name="task">Die zu wartende <see cref="Task{TResult}"/>.</param>
    /// <param name="cancellationToken">Das <see cref="CancellationToken"/>, das zum Abbrechen der Aufgabe verwendet werden kann.</param>
    /// <returns>Das Ergebnis der Aufgabe, sofern sie nicht abgebrochen wurde.</returns>
    /// <exception cref="OperationCanceledException">Wenn der Abbruch-Token ausgelöst wurde.</exception>
    public static async Task<T> WithCancellationTokenAsync<T>(this Task<T> task, CancellationToken cancellationToken)
    {
      TaskCompletionSource<T> tcs = new();
      _ = cancellationToken.Register(() => tcs.SetResult(default!));

      return task != await Task.WhenAny(task, tcs.Task).ConfigureAwait(false)
        ? throw new OperationCanceledException("The operation has timed out", cancellationToken)
        : await task.ConfigureAwait(false);
    }
    #endregion

    #endregion

  }
}
