# Monitoring Sample App mit REST API zur Speicherung und Auswertung von Metriken

Das Projekt implementiert eine Health-Check Überwachung auf Anwendungsebe. 
Mit Hilfe der Erfassung und Auswertung der verschiedenen Metriken soll der Zustand der Anwendung überwacht werden. 
Das WfMS (Workflow Management System), aus dem die Module stammen, ist ein über mehrere Standorte verteilte Host-Anwendung für die Ausführung modellierter Geschäftsprozesse, welche selbst über verschiedene Schnittstellen mit anderen Systemen und Anwendungen verbunden sind.
Um die Ausführung der Geschäftsprozesse zu gewährleisten, ist es notwendig, den Zustand der Anwendung zu überwachen und sicherzustellen, dass alle erforderlichen Dienste erreichbar sind.

## Abgrenzung/ Einschränkungen
  Der hier abgebildete Code ist ein Ausschnitt aus der Real-Implementierung und ist um einige Aspekte gekürzt. 
  - Die Überwachung der Metriken und die damit verbundene Alamierung bei Problemen ist nicht in diesem Beispiel implementiert. 
  - Ursprünglich wurden die Metriken in eine Sql-Datenbank gespeichert, der lokalen Verwendbarkeit der Beispiel-Anwendung aber durch eine Datei-basierte Sqlite-Datenbank ersetzt.
  - Als Programierkonzept ist nach der Vermeidung von Exceptions, diese beim Auftreten unmittelbar abzufangen, zu loggen und als Methoden-Rückgabe ein Ergebnis-Objekt zu liefern, das den erfolg und das Ergebnis, oder eine Fehlermeldung enthält. Das Logging selbst ist in der Beispielanwendung nicht implementiert, der Aufruf erfolgt lediglich auf einen leeren Dummy.

## Features
- REST-API zur Speicherung von Metriken
- Sqlite-Datenbank zur Speicherung der Metriken

## Technologien
- Net 9.0
- C#
- Entity Framework Core
- Blazor Fluent UI
- REST-API
- Sqlite

## Projektstruktur
<table>
  <tr>
    <td>Sample-Client-App</td>
    <td>Host-Anwendung für die System- und Anwendungsprüfungen, die den Zustand der Anwendung anzeigt</td>
  </tr>
  <tr>
    <td colspan="2">Common</td>
  </tr>
  <tr>
    <td>Sample.Extensions</td>
    <td>Enthält allgemeine Erweiterungen und Erweiterungsmethoden, die zur Vereinfachung der Programmierung genutzt werden</td>
  </tr>
  <tr>
    <td>Sample.UI.Extensions</td>
    <td>Enthält UI-spezifische Erweiterungen für die Anwendung</td>
  </tr>
  <tr>
    <td colspan="2">Logging</td>
  </tr>
  <tr>
    <td>Sample.Logging</td>
    <td>Erweiterungen für das Logging innerhalb der Anwendung</td>
  </tr>
  <tr>
    <td colspan="2">Monitoring</td>
  </tr>
  <tr>
    <td>Sample.Monitoring</td>
    <td>Enthält die Logik für die Health-Checks und die Speicherung der Metriken</td>
  </tr>
  <tr>
    <td>Sample.Monitoring.API</td>
    <td>REST-API zur Speicherung und Abfrage von Metriken</td>
  </tr>
  <tr>
    <td>Sample.Monitoring.Infrastructure</td>
    <td>Datenbank-spezifische Logik zum Entity-Framework, wie z.B. Migrationen</td>
  </tr>
  <tr>
    <td>Sample.Monitoríng.Model</td>
    <td>Enthält die Model-Klassen für die Health-Checks und Metriken</td>
  </tr>
  <tr>
    <td>Sample.Monitoring.UI</td>
    <td>Blazor-Komponenten und Seiten zur Anzeige des Anwendungszustands in der Client-App mit Zugriff der Metriken über die REST-API</td>
  </tr>
<table>

### Verwendung
Die Anwendung kann in Visual Studio gestartet werden, die erforderlichen Startprojekte sind 'Sample.Monitoring.API' und 'Sample-Client-App'.
Alternativ kann die Anwendung auch mit Ausführung der Batch-Datei `run.bat` gestartet werden, die die erforderlichen Projekte baut und startet.

### Konfiguration
Die Konfiguration der Health-Checks erfolgt in der Datei `appsettings.json` der 'Sample-Client-App'.
```json
"Health": {
  "StorageApi": "https://localhost:7127", // URL der REST-API
  "ApplicationId": "0C73A5C4-A28A-49E2-8752-ED71D181B2C1", // Eindeutige ID der Anwendung, die überwacht wird
  "ServerName": "MyServer", // Name des Servers auf dem die Anwendung läuft, wird der Knoten weggelassen ist der Servername = System.Environment.MachineName
  "CheckInterval": 30, // Intervall in Sekunden, in dem die Health-Checks durchgeführt werden
  "HealthChecks": [ ] // Liste der Health-Checks, die durchgeführt werden sollen
}
```
Die einzelnen Health-Check können wie folgt zur Liste "Health-Checks" hinzugefügt und konfiguriert werden:
#### Basis-Konfiguration aller Heath-Checks:
```json
{
  "Name": "Name", // eigener Name des Health-Checks (in der Anzeige verwendet)
  "Type": "HealthCheckType", // der HealthCheck, welcher durchgeführt werden soll, verfügbar sind ["Application State", "Allocated Memory", "DNS", "Disk Storage", "REST", "SMTP"]
  "Tags": [ "Tag1", "Tag2" ], // Tags zur Kategorisierung des Health-Checks"
}
```
#### Typ "Application State"
Prüft den Zustand der Anwendung, ob sie läuft und erreichbar ist. Der Anwendungszustand hat keine individuellen Konfigurationseinstellungen.
#### Typ "Allocated Memory"
Prüft den aktuell verwendeten Arbeitsspeicher der Anwendung.
```json
{
  "MaximumMemory": 24576, // der maximal erlaubte Arbeitsspeicherverbrauch (in MByte) der Anwendung
}
```
#### Typ "DNS"
Prüft die Verfügbarkeit der DNS-Auflösung.
```json
{
  "Hosts": [
    "www.google.de",
    "www.elanet.biz"
  ] // Liste der Hostnamen, die aufgelöst werden sollen
}
```
#### Typ "Disk Storage"
Prüft den verfügbaren Speicherplatz auf dem Server.
```json
{
  "Drives": [
    {
      "DriveName": "C:\\", // der Datenträger, der geprüft werden soll
      "MinimumFreeSpace": 16384 // der minimal erlaubte freie Speicherplatz (in MByte) auf dem Datenträger
    }
}
```
#### Typ "REST"
Prüft die Erreichbarkeit der angegebenen REST-APIs.
```json
{
  "Hosts": [
    {
      "Host": "https://dummy.restapiexample.com/api/v1/", // die URL der REST-API, die geprüft werden soll
      "Service": "employees" // der spezifische Service-Endpunkt, der geprüft werden soll
    }
  ]
}
```
#### Typ "SMTP"
Prüft die Erreichbarkeit des SMTP-Servers.
```json
{
  "Host": "smtp.example.com", // der Hostname des SMTP-Servers
  "Port": 587, // der Port des SMTP-Servers
  "EnableSsl": true // ob SSL für die Verbindung verwendet werden soll
}
```
