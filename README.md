# Crypto Project - Project .NET Framework

* Naam: Fabio Di Nota
* Studentennummer: 0173810-83
* Academiejaar: 25-26
* Klasgroep: INF202B
* Onderwerp: Cryptocurrency * - * Exchange 1 - * UserReview

````mermaid
classDiagram
class Cryptocurrency {
+int Id
+string Name
+string Symbol
+double CurrentPrice
+CryptoType Type
+long? MaxSupply
+List~Exchange~ Exchanges
+ToString() string
}

class Exchange {
+int Id
+string Name
+string Website
+int TrustScore
+List~Cryptocurrency~ Cryptocurrencies
+List~UserReview~ Reviews
+ToString() string
}

class UserReview {
+int Id
+string UserName
+string Comment
+double Rating
+DateTime DatePosted
+Exchange Exchange
+ToString() string
}

class CryptoType {
<<enumeration>>
Coin
Token
Stablecoin
MemeCoin
}

Cryptocurrency "0..*" -- "0..*" Exchange : IsListedOn
Exchange "1" -- "0..*" UserReview : Has
Cryptocurrency ..> CryptoType : Uses
````
