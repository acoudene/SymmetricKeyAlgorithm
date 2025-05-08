# SymmetricKeyAlgorithm

# Cas MAUI utilisant une clé dynamique

## La solution :

-	1 assembly de library à utiliser telle quelle (CryptographyProvider)
-	1 application MAUI Blazor de test
-	1 assembly de test xUnit

## Le principe de la librairie :

-	Un contrat de gestion de la cryptographie : ICryptographyProvider qui a des méthodes Encrypt/Decrypt avec une chaîne et une clé en donnant une taille de clé à utiliser
-	Une implémentation de chiffrement symétrique basée sur AES : AeSCryptographyProvider
-	Un contrat de génération de clé en stateful : IStatefulKeyGenerator qui possède une méthode de génération de clé selon une taille de clé.
-	Un contrat stateful de crypto : IStatefulCryptographyProvider avec des méthodes Encrypt/Decrypt mais sans clé, associé à une implémentation StatefulCryptographyProvider qui aggrège un ICryptographyProvider et un IStatefulKeyGenerator.
-	Un exemple d'implémentation StateFul pour chiffrer ou déchiffrer du texte en incluant la génération de la clé à la volée.

## L’implémentation de la génération de clé dans l’application MAUI Blazor :

Cette dernière implémente IStatefulKeyGenerator.
Pour la génération de clé, l'implémnetation s’appuie sur toutes les infos courantes du device (d’où le stateful), qu’elle sérialize, puis de cette serialization, crée un hash SHA256 qui sera tronqué vers une chaîne de 256bits.

L’encryptage ou le décryptage regénère à chaque fois la clé contextuelle.

# Cas utilisant un coffre Azure Keyvault et une base Mongo

## L'idée générale :

Le principe :
- Chiffrement/déchiffrement d'un texte via un chiffrement symétrique AES combiné à une clé symétrique hashée en SHA256.
- Texte stocké dans un document mongo
- Clé stockée au sein d'un coffre Azure Keyvault.

## Solution développée :

Une solution avec une orchestration Aspire ayant : 
- 1 container mongo, 
- 1 montage dynamique de Azure Keyvault, 
- 1 application Blazor (Server+WASM)

Une appli web qui laisse l'utilisateur saisir une donnée et qui sera sous son action, écrite en bdd de manière chiffrée puis récupérée en clair pour l'afficher.

![Chiffrement Symétrique avec coffre pour clé](https://github.com/user-attachments/assets/3858b1b1-a665-46c9-a634-68406dd5ec28)

## Annexe :

```
title Chiffrement Symétrique avec coffre pour clé

User->BlazorClient:Navigate(Home)
User->BlazorClient: Input(clearText)
User->BlazorClient: Protect(clearText)
BlazorClient->BlazorClient: new CryptoDataDto
BlazorClient->BlazorClient: CryptoDataDto.Text=clearText
BlazorClient->CryptoController: POST(CryptoDataDto)
CryptoController->AzureKeyvault: Authenticate
CryptoController->AzureKeyvault: key=Get("aes-secret-key")
AzureKeyvault-->CryptoController:
CryptoController->AesProvider: CryptoDataDto.Text=Encrypt(CryptoDataDto.Text, key)
AesProvider->AesProvider: aesSecretKey=Hash(key)
AesProvider->AesProvider: AesEncrypt(CryptoDataDto.Text, aesSecretKey)
AesProvider-->CryptoController:
CryptoController->MongoDb: Connect
CryptoController->MongoDb: CreateOrUpdate(CryptoDataDto, collection:"CryptoData")
CryptoController-->BlazorClient:
BlazorClient->CryptoController: cryptoDataDto=GET
CryptoController->MongoDb: Get("CryptoData")
CryptoController-->BlazorClient:
BlazorClient-->User: Display(CryptoDataDto.Text)
```


