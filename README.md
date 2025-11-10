## 🧑‍🍳 **Healthy Chef** 🧑‍🍳

Healthy Chef è un **Serious-Game** inspirato allo stile visual novel con lo scopo di insegnare al giocatore i principi di una'alimentazione sana e bilanciata.

Il gioco vede il giocatore come nuovo commis dello Chef Tony che da diverse nozioni istruttive che poi vanno "provate" tramite diversi semplici minigiochi educativi.

Il nostro progetto è stato interamente realizzato con:

- **.NET 9.0.9**
- **Unity 6000.2.9M**

---

## ⚙️ **Requisiti** ⚙️

Per far funzionare il gioco sul tuo PC avrai bisogno di installare:

- **Unity Editor 6000.2.9M** o versione compatibile
- **.NET SDK 9.0.9**
- (Opzionale) **Visual Studio 2022** o **VS Code** per modificare gli script C#

---

## 🚀 **Avviare il progetto** 🚀

### Unity

1. Aprire **Unity Hub**
2. Aggiungere il progetto all'Hub _(Open => Add project from disk)_
3. Selezionare la cartella principale del progetto
4. Avviare la scena principale _(Title Scene)_

### Build

1. Nello Unity Hub selezionare le Build Settings _(Open => Add project from disk)_
2. Selezionare la Piattaforma / Sistema Operativo desiderato
3. Cliccare su **Build and Run**

---

## 📁 **Struttura del progetto** 📁

## 📁 **Struttura del progetto**

```text
Healthy-Chef/
├─ .gitignore
├─ REDME.md
├─ bin/                     # Output compilazioni .NET
├─ obj/                     # File temporanei di compilazione
│
├─ Project/
│  ├─ Assets/               # Risorse di gioco principali
│  │  ├─ Animations/        # Controller e clip per animazioni (es. Chef Tony, DialogPanel)
│  │  ├─ Audio/             # Colonne sonore e musiche ambientali
│  │  ├─ Materials/         # Materiali utilizzati nei prefab e scene
│  │  ├─ Prefab/            # Oggetti prefab (cibo, UI, manager, ecc.)
│  │  ├─ Resources/
│  │  │  └─ Dialogues/      # Testi e script narrativi delle varie sezioni
│  │  ├─ Scenes/            # Scene principali (Menu, Quiz, Piramide, Minigame, ecc.)
│  │  ├─ Script/            # Script C# (GameManager, DialogManager, MusicPlayer, ecc.)
│  │  ├─ Sprite/            # Grafica (Tony, cibi, UI, sfondi)
│  │  ├─ TextMesh Pro/      # Font, materiali e risorse per UI testuale
│  │  ├─ Settings/          # Configurazioni URP e template di scena
│  │  └─ ToRemove/          # Script o file in disuso
│
│  ├─ Library/              # Cache e database Unity (generati automaticamente)
│  ├─ Packages/             # Dipendenze del progetto Unity
│  ├─ ProjectSettings/      # Impostazioni di progetto Unity
│  └─ .vscode/              # Configurazione editor VS Code
│
└─ repo-tree.txt            # Mappa generata della repo
```

---

## 👥 **Autori** 👥

- **Marcello Miranda** - *https://github.com/MarcyMiranda2004*
- **Alessandro Imbriani** - *https://github.com/Rela45*
- **Gabriele Frangiosa** - *https://github.com/DemonBloodstorm*

---

## 📸 **Immagini**

![Start](images\Start.png)

![Presentazione](images\Presentazione.png)

![Benvenuti](images/Benvenuti.png)

![Minigame](images/Minigame.png)
