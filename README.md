# CampiAgricoli API

**CampiAgricoli_API** è il backend del sistema CampiAgricoli, progettato per gestire i dati provenienti dai sensori agricoli, archiviare le informazioni in un database MSSQL e fornire dati strutturati al frontend tramite API RESTful. Il backend è implementato in **C#** utilizzando **.NET Core 8** per garantire prestazioni elevate e scalabilità.

## Caratteristiche principali

- **Gestione dati sensori**: Riceve, valida e memorizza i dati raccolti dai sensori agricoli.
- **Database relazionale MSSQL**: Organizzazione dei dati tramite tabelle e viste ottimizzate per operazioni rapide e affidabili.
- **API RESTful**: Endpoint per invio e recupero dei dati in formato JSON.
- **Entity Framework Core**: Per una gestione fluida del database e query tramite LINQ.

## Architettura

### Backend
Il backend è responsabile della comunicazione tra le stazioni, il database e la dashboard web. È progettato per:
- **Memorizzare i dati**: Tabelle ottimizzate per garantire letture veloci e scritture affidabili.
- **Interagire con il database**: Query SQL dinamiche e performanti tramite **Entity Framework Core**.
- **Fornire dati al frontend**: Risposte strutturate in formato JSON per una facile integrazione con il frontend React.

### Database
Il sistema utilizza un database **MSSQL**, che ospita:
- **Tabelle**: Per la memorizzazione delle misurazioni e altre informazioni correlate.
- **Viste**: Per generare report pre-elaborati, ottimizzati per il frontend.

## API

### Endpoint principali

#### **POST /Misurazioni/PostMisurazione**
- **Descrizione**: Consente di inviare nuove misurazioni al sistema.
- **Payload richiesto**:
  ```json
  {
    "IdSensore": 1,
    "ValoreMisurazione": 35.5,
    "DataOraMisurazione": "2024-12-01T10:30:00Z"
  }
