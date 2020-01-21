# Kolveniershof Website

De website is een onderdeel van het software pakket, ontwikkeld voor het Kolveniershof, een dagcentrum voor kinderen en volwassenen met een beperking. Het platform dat hiervoor wordt gebruikt is Angular. De bedoeling van deze applicatie is om het proces voor het opmaken en bekijken van de planning, makkelijker en sneller te maken.

### [Android applicatie](https://github.com/HoGent-Projecten3/projecten3-1920-android-grasmaaier-team)

### [Web API](https://github.com/HoGent-Projecten3/projecten3-1920-backend-grasmaaier-team)



## Aan De Slag

Onderstaande instructies gaan over het opzetten van het project op basis van de broncode om het project te testen. 

### Vereisten

Om het project uit te voeren zal je [Node.js](https://nodejs.org/en/) en dan zal je Angular moeten installeren met het onderstaande commando:

```
npm install -g @angular/cli
```

Het is ook aangeraden om [Visual Studio Code](https://code.visualstudio.com/) te gebruiken om de code te bekijken.

Zorg er ook voor dat je een werkende versie hebt van de backend.

### Installeren

Als de repository is gedownload, ga in de map en open de command line. Voer het onderstaande commando uit:

```
npm install
```

Kijk eens na in `proxy.conf.json` de juiste connection string heeft naar de backend.

Om de code uit te voeren gebruik:

```
npm start
```



### Gebruik

De website is default terug te vinden op onderstaand adres.

```
localhost:4200
```



### Login

Gebruik onderstaand account om als administrator in te loggen.

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

De testen kan je uitvoeren door het onderstaand commando uit te voeren:

``` 
npx cypress open
```



## Deployment

Voor het deployen van onze applicatie heeft Michiel een uitgebreide [guide](https://github.com/michiel-schoofs/Deployment-Guide/blob/master/CentOS%20installation%20guide.md) geschreven.

Om daar nog even aan toe te voegen. De guide houdt geen rekening met het builden van de angular applicatie, maar maakt een docker container voor een test enviroment. Gebruik onderstaand commando om de applicatie te builden:

```
ng build --prod
```

De build kan je terug vinden in de ``dist` folder.



### Hosted

De API wordt gehost op onderstaand adres tot en met 1 februari 2020:

```
http://78.20.29.170:4200
```


## Auteurs

* **Michiel Schoofs** - [michiel-schoofs](https://github.com/michiel-schoofs)
* **Wannes De Craene** - [WannesDC](https://github.com/WannesDC)
* **Daan Verhelst** - [DaanVerhelst](https://github.com/DaanVerhelst)
* **Nelson Horemans** - [gizmoanact](https://github.com/gizmoanact)
* **Laurens Ghekiere** - [LaurensGhekiere](https://github.com/LaurensGhekiere)
* **Lars Vandenberghe** - [LarsVandenberghe](https://github.com/LarsVandenberghe)
