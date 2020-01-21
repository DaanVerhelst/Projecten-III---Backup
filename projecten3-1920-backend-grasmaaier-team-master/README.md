# Kolveniershof Backend

De backend is een onderdeel van het software pakket, ontwikkeld voor het Kolveniershof, een dagcentrum voor kinderen en volwassenen met een beperking. Het platform dat hiervoor wordt gebruikt is ASP.NET Core. De bedoeling van deze applicatie is om het proces voor het opmaken en bekijken van de planning, makkelijker en sneller te maken.

### [Web applicatie](https://github.com/HoGent-Projecten3/projecten3-1920-angular-grasmaaier-team)

### [Android applicatie](https://github.com/HoGent-Projecten3/projecten3-1920-android-grasmaaier-team)



## Aan De Slag

Onderstaande instructies gaan over het opzetten van het project op basis van de broncode om het project te testen. 

### Vereisten

Om het project te kunnen gebruiken zal je <a href="https://dotnet.microsoft.com/download/dotnet-core/2.1">.NET Core 2.1</a> en [SQL Server](https://www.microsoft.com/nl-be/sql-server/sql-server-editions-express) nodig hebben.

### Installeren

Na het clonen van deze repository is het belangrijk om de user-secrets in te stellen. Ga daarvoor in de folder `KolveniershofAPI` en open vanuit deze map de command line. Voer dan hetvolgende commando uit:

```
dotnet user-secrets set "Tokens:Key" "KiesHierEenLangGenoegWachtwoord(16CharsOfMeer)"
```

Na het instellen van de user-secrets kan je best de connection-string van de databank controleren.  Bekijk hiervoor de `appsettings.json` en `Startup.cs`. Gebruik het volgende commando op de applicatie op te starten:

```
dotnet run --launch-profile 'KolveniershofAPI'
```

Als je Visual Studio gebruikt selecteer dan het launch profile `KolveniershofAPI` om de poort op `5000` te zetten en poort `5001` voor https.


### Gebruik

Om de documentatie van de API Calls en de Calls te kunnen testen gebruik onderstaande URL:

```
localhost:5001/swagger
```

Om de API te gebruiken om zelf een frontend applicatie te ontwikkelen gebruik

```
localhost:5001/api
```



### Login

De backend zal op voorhand al data gaan opvullen in de databank bij het opstarten. Gebruik het volgende administrator account om te JWT op het halen:

Email:

```
Tycho.Altink@mail.be
```

Paswoord:

```
admin123
```



Onderstaande gegevens zijn voor een ouder account:

Email:

```
Romein.Smit@mail.be
```

Passwoord:

```
test123
```



## Testen uitvoeren

De testen zijn geschreven in PostMan. En zijn te vinden in de `PostMan` map. Je kan deze importeren en uitvoeren.



## Deployment

Voor het deployen van onze applicatie heeft Michiel een uitgebreide [guide](https://github.com/michiel-schoofs/Deployment-Guide/blob/master/CentOS%20installation%20guide.md) geschreven.

### Hosted

De API wordt gehost op onderstaand adres tot en met 1 februari 2020:

API:

```
http://78.20.29.170:5000/api/
```

Swagger:

```
http://78.20.29.170:5000/swagger/
```



## Auteurs

* **Michiel Schoofs** - [michiel-schoofs](https://github.com/michiel-schoofs)
* **Wannes De Craene** - [WannesDC](https://github.com/WannesDC)
* **Daan Verhelst** - [DaanVerhelst](https://github.com/DaanVerhelst)
* **Nelson Horemans** - [gizmoanact](https://github.com/gizmoanact)
* **Laurens Ghekiere** - [LaurensGhekiere](https://github.com/LaurensGhekiere)
* **Lars Vandenberghe** - [LarsVandenberghe](https://github.com/LarsVandenberghe)

