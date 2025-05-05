# SymmetricKeyAlgorithm

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

 


