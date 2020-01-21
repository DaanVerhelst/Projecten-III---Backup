# Kolveniershof App

De app is een onderdeel van het software pakket, ontwikkeld voor het Kolveniershof, een dagcentrum voor kinderen en volwassenen met een beperking. De bedoeling van deze applicatie is om het proces voor het opmaken en bekijken van de planning, makkelijker en sneller te maken.

### [Web applicatie](https://github.com/HoGent-Projecten3/projecten3-1920-angular-grasmaaier-team)

### [Web API](https://github.com/HoGent-Projecten3/projecten3-1920-backend-grasmaaier-team)



## Aan De Slag

Onderstaande instructies gaan over het opzetten van het project op basis van de broncode.

### Vereisten

Om het project te kunnen gebruiken zal je <a href="https://developer.android.com/studio">Android Studio</a> nodig hebben.

### Installeren

Na het openen van het project in Android Studio, open je AVD Manager, bij Tools. Maak een nieuw Virtual Device. Kies voor Tablet, Pixel C. Download indien nodig de laatste Android versie (Huidige versie Q, API level 29) en ga door. Selecteer het device boven in het dropdown menu en voer het project uit. 

### Login

Na het opstarten van de applicatie is het belangrijk dat er internet is! Er zal een scherm tevoorschijn komen dat vraagt om je in te loggen met een administrator account. Dat doen we omdat de app wijzigingen mag uitvoeren op de planningen van alle cliënten. Voor test doeleinden geven we het paswoord hier mee. Uiteraard bevat de finale versie van het project dit account niet.

Email:

```
Tycho.Altink@mail.be
```

Paswoord:

```
admin123
```

In theorie zou het inloggen slechts enkel moeten gebeuren bij de eerste keer dat de applicatie wordt opgestart. Het device onthoudt dan je login gegevens. Als het inloggen is gelukt, krijg je een overzicht van alle cliënten. 



## Testen uitvoeren

We hebben gekozen om end-to-end testen te maken. Dit om de simpele reden dat de app zelf weinig domein logica bevat. Om de testen uit te voeren, open je de `java` folder en vervolgens klik je rechtermuisknop op de `be.grasmaaier.kolveniershof ( androidTest )` folder. Kies `Run 'Tests in be.grasmaa...'`.



## Deployment

De [app](https://play.google.com/apps/testing/be.grasmaaier.kolveniershof) is te downloaden via Google Play. Deze app maakt gebruik van een <a href="http://78.20.29.170:5000/swagger/">.NET Core backend</a> en is onderdeel van een software pakket. De <a href="http://78.20.29.170:4200/">website</a> is hier ook onderdeel van.



## References

* [SpanningLinearLayoutManager](https://gist.github.com/heinrichreimer/39f9d2f9023a184d96f8)



## Auteurs

* **Michiel Schoofs** - [michiel-schoofs](https://github.com/michiel-schoofs)
* **Wannes De Craene** - [WannesDC](https://github.com/WannesDC)
* **Daan Verhelst** - [DaanVerhelst](https://github.com/DaanVerhelst)
* **Nelson Horemans** - [gizmoanact](https://github.com/gizmoanact)
* **Laurens Ghekiere** - [LaurensGhekiere](https://github.com/LaurensGhekiere)
* **Lars Vandenberghe** - [LarsVandenberghe](https://github.com/LarsVandenberghe)
