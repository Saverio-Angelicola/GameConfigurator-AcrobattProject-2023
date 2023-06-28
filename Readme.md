# API Vegaflag

## Contexte

Le projet est une API afin de permettre la configuration de partie dans le cadre du projet Acrobatt de la licence pro CDAD lors de l'année 2023.

L'application permet de gérer son compte personnel ainsi que de créer des configurations de jeu téléchargeable pour pouvoir les lancer sur ses serveurs personnelles.

# Documentation

Lien de la documentation Open API : https://vegaflag.herokuapp.com/swagger

## Prérequis

- .Net 7
- Docker
- Editeur de code

## Configuration

- Dans le fichier AppSettings.json et AppSettings.Development.json renseignez la chaine de connexion de votre base de données PostgreSQL ainsi que les informations d'accès à votre bucket AWS S3 si vous l'avez déjà créer.
```json
{
   "Authentication": {
      "SecretToken": "keyforjwtauthentication"
   },
   "AWS": {
      "bucketName": "acrobatt",
      "accessKey": "accesskey",
      "secretKey": "secretkey"
   },
   "Logging": {
      "LogLevel": {
         "Default": "Information",
         "Microsoft.AspNetCore": "Warning"
      }
   },
   "AllowedHosts": "*",
   "ConnectionStrings": {
      "default": "<bdd conection string>"
   }
} 
```
<br>

## Architecture

1. Architecture et pattern : <br>

- Le projet est construit selon les principes de la clean architecture qui est un pattern architectural basé principalement
sur le principe d'inversion de dépendances issu des principes SOLID.
- Afin de respecter le principe de responsabilité unique de SOLID, le CQRS pattern a été implémenté d'un point de vue 
architecture afin d'améliorer la maintenance et la lisibilité du code. Le pattern consiste à séparer les actions en lectures et écritures de l'application.
<br> 

2. Source et explications :
- Clean Architecture : https://medium.com/idealo-tech-blog/hexagonal-ports-adapters-architecture-e3617bcf00a0
- CQRS Pattern : https://abdelmajid-baco.medium.com/cqrs-pattern-with-c-a9aff05aae3f

## Migration de la base de données
- Déplacez-vous via un terminal dans le dossier Src/Acrobatt.Infrastructure
### Installation de EF Core Tools 
`dotnet tool install --global dotnet-ef`
### Création des fichiers de migrations
`dotnet ef migrations add initialCreate --context PostgresContext --startup-project ../Acrobatt.API`
### Application des fichiers de migrations
`dotnet ef database update --context PostgresContext --startup-project ../Acrobatt.API`


## Build
Dotnet : `dotnet build` <br>

Docker pour la production :
1. Se déplacer dans le dossier parent du projet `cd ..`
2. Télécharger le [serveur de jeu](https://git.unistra.fr/lpacrobatlgsafetrhse/all-chimie-game-server-rust/-/tree/master/)
3. Extraire l'archive
4. Renommez le dossier game-server
5. Assurez-vous qu'à la racine du projet les fichiers sont bien présent
6. Exécutez la commande : `docker build -t acrobatt-api -f ./Acrobatt/Dockerfile .
   `

## Run
Dotnet : `dotnet run --project Src/External/Acrobatt.API` <br>
Docker : `docker run -p port:80 acrobatt-api` <br>
Entrez dans le navigateur : `http://localhost:port/swagger`

## Tests

Exécution de tout les tests : `dotnet test` <br>
Exécution des tests unitaires : `dotnet test Tests/Acrobatt.UnitTests` <br>
Exécution des tests d'intégrations : `dotnet test Tests/Acrobatt.IntegrationTests` <br>
Exécution des tests End-To-End : `dotnet test Tests/Acrobatt.E2E`

## Démarrage

Pour commencer à ajouter de nouvelles fonctionnalités dans l'application, il vous suffit d'aller dans Src/Core/Acrobatt.Application qui contient l'ensemble des fonctionnalités.

Dans Acrobatt.Application vous avez des cas d'utilisations de type Query ou Command, les queries sont les actions en lectures et les commands sont les actions en écritures.

Inspirez-vous des fonctionnlités existantes pour ajouter la votre.

Quand vous ajoutez une nouvelle fonctionnalité, rendez vous dans Src/External/Acrobatt.API pour exécuter la fonctionnalité dans un controller existant ou en ajoutant un nouveau controller.