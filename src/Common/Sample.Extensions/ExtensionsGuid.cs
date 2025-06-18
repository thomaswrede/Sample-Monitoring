namespace System
{
  /// <summary>
  /// Stellt Erweiterungsmethoden für <see cref="Guid"/> und <see cref="Guid?"/> bereit.
  /// </summary>
  public static class ExtensionsGuid
  {
    #region " öffentliche Methoden                        "

    #region " --> AsMemory                                "
    /// <summary>
    /// Gibt die Zeichenfolgendarstellung eines <see cref="Guid"/> als <see cref="ReadOnlyMemory{Char}"/> zurück.
    /// </summary>
    /// <param name="value">Der <see cref="Guid"/>, der konvertiert werden soll.</param>
    /// <returns>Die Zeichenfolgendarstellung des <see cref="Guid"/> als <see cref="ReadOnlyMemory{Char}"/>.</returns>
    public static ReadOnlyMemory<Char> AsMemory(this Guid value) => value.ToString().AsMemory();

    /// <summary>
    /// Gibt die Zeichenfolgendarstellung eines <see cref="Guid?"/> als <see cref="ReadOnlyMemory{Char}"/> zurück.
    /// Gibt <see cref="ReadOnlyMemory{Char}.Empty"/> zurück, wenn der Wert <c>null</c> ist.
    /// </summary>
    /// <param name="value">Der optionale <see cref="Guid"/>, der konvertiert werden soll.</param>
    /// <returns>Die Zeichenfolgendarstellung des <see cref="Guid"/> als <see cref="ReadOnlyMemory{Char}"/>, oder <see cref="ReadOnlyMemory{Char}.Empty"/> wenn <c>null</c>.</returns>
    public static ReadOnlyMemory<Char> AsMemory(this Guid? value) => !value.HasValue ? ReadOnlyMemory<Char>.Empty : value.ToString().AsMemory();
    #endregion

    #endregion
  }
}
