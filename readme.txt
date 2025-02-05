# Dolphins Sunset Resort - Database Scripts

## 📌 Descriere
Acest proiect utilizează un fișier de configurare pentru a gestiona conexiunea la baza de date și oferă două scripturi SQL pentru inițializarea bazei de date.

## 🔧 Configurarea Conexiunii la Baza de Date
Stringul de conexiune la baza de date este definit în fișierul `appsettings.json`.

## 📜 Scripturi SQL Disponibile

1. **`script_Create_DolphinsSunsetResortDb.sql`**  
   - Acest script conține doar schema bazei de date (tabele, constrângeri, proceduri stocate etc.).
   - Utilizați-l dacă doriți să creați structura bazei de date fără a insera date inițiale.

2. **`script_CreateAndData_DolphinsSunsetResortDb.sql`**  
   - Acest script include atât schema bazei de date, cât și datele existente.
   - Folosiți-l dacă doriți să populați baza de date cu informațiile deja existente.

## 📧 Configurarea Serverului SMTP Local
Aplicația folosește un server SMTP local pentru a trimite e-mailuri. Pentru a testa funcționalitatea de trimitere a e-mailurilor, utilizați **smtp4dev**, un server SMTP local de testare.

- Descărcați **smtp4dev** de aici: [rnwood/smtp4dev](https://github.com/rnwood/smtp4dev/releases)
- Rulați aplicația **smtp4dev** pe sistemul local.


