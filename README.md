# 🎮 Little Great Heroes (Pequeños Grandes Héroes)

**Little Great Heroes** is a serious game project developed in **Unity** as part of a Computer Science undergraduate thesis (2019-2).  

The project is designed as a **tool to create and share stories of Colombian social leaders as videogames**. Each story can be customized in a structured **JSON format**, which the system interprets and loads to automatically generate a playable game.  

The work includes a **test case based on the real story of Kevin Julián León (2002–2018)**, a young social leader assassinated at the age of 16, whose life was turned into an interactive narrative to raise awareness and empathy.

---

## 📖 Project Overview

- **Objective**: Provide a platform to **create videogames that narrate the lives and struggles of social leaders**, aiming to raise awareness of the violence they face in Colombia.  
- **Gameplay**: Players live the daily routine of a leader, interact with their environment, read letters and newspapers, maintain wellbeing (energy, food, sleep), and witness events that reveal their social work and the risks they face.  
- **Customization**: Stories are defined in **JSON files** (characters, dialogues, sounds, environments, events) and interpreted by the Unity system.  

---

## 🛠️ Development Details

- **Engine**: Unity  
- **Architecture**:
  - **Interpreter**: Reads JSON story files and generates the playable game.  
  - **Story Creator (future work)**: Planned graphical editor to simplify story creation.  
- **Database**: Firebase Realtime Database (JSON-based).  
- **Methodology**: Agile (SCRUM), 16 weeks of sprints.  
- **Sources**: [Datasketch – Líderes Sociales](http://lideres-sociales.datasketch.co/) and [Postales para la Memoria](http://postalesparalamemoria.com/).  

---

## 🎮 Features

- 🏡 **Interactive scenes**: bedroom, living room, kitchen, street, and community spaces.  
- 📜 **Narrative system**: letters, newspapers, dialogues with family and community.  
- 🍞 **Gameplay mechanics**: manage the leader’s wellbeing through food, sleep, and activities.  
- 🎨 **Custom content**: personalized images, animations, and sounds defined in JSON and stored in Firebase.  
- 🎵 **Audio-visual immersion**: background music, ambient effects, and symbolic endings.  

---

## 🧪 Test Case: *Kevin Julián León*

As proof of concept, the project developed the story of **Kevin Julián León (RIP, 2002–2018)**, a young leader murdered at the age of 16.  
The game lets players experience a few days in Kevin’s life, his community work, and concludes with a powerful final message.

<img width="1643" height="359" alt="image" src="https://github.com/user-attachments/assets/d470c0cc-c3d7-4185-976b-5cacec4c7cb6" />
Video gameplay available here: [https://youtu.be/4ISS7MHfs3g]([url](https://youtu.be/4ISS7MHfs3g))


This case validated the potential of the platform to transform real stories into impactful interactive experiences.

---

## 🧪 User Testing & Results

- **Participants**: 41 users (students and young professionals).  
- **Findings**:
  - 90% showed interest in serious games.  
  - 77% would play another leader’s story.  
  - 82% preferred playing an interactive story over reading it in newspapers.  
  - Users reported emotions such as sadness, empathy, and curiosity.  

---

## 📈 Outcomes

- Demonstrated that videogames can serve as **educational and social awareness tools**.  
- Validated that interactive narratives engage young audiences more than traditional media.  
- Opened research opportunities in **serious games for social impact** in Colombia.  

This work was published as our Undergraduate Thesis Project and it is available here: [https://hdl.handle.net/1992/44845](https://hdl.handle.net/1992/44845)

---

## 🔮 Future Work

- Development of a **graphical Story Creator editor** (instead of manually editing JSON).  
- Ability to create new, non-generic scenes at runtime.  
- Optimization of multimedia loading and database backups.  
- Improved sprite cutting and runtime animation generation.  
- User authentication for story creators to save, edit, and publish games.  

---

## 👩‍💻 Authors

- **Vivian Gómez Cubillos**  
- **Kelly Peñaranda Rivera**  

Advisor: **Pablo Figueroa Forero**  

---

## 📜 License

This project was developed for academic purposes and is shared under a **Creative Commons BY-SA 4.0 License** (acknowledging original sources like Datasketch and Postales para la Memoria).
