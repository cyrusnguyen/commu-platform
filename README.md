
# Communication Platform 

Welcome to our Communication App, a dynamic platform designed to facilitate seamless interaction and connection among users. Our application allows individuals to create accounts, set up profiles, exchange messages in real-time, and a variety of features aimed at enhancing communication experiences. Crafted with Angular 16 and .NET Core 7, our app adopts advanced patterns such as the repository and unit of work to ensure efficient data management and abstraction layers.


## Demo

https://commu-platform.fly.dev


## Features
Our application boasts a range of features to elevate your communication experience:

- Exchange messages with other users in real time.
- Ability to send friend requests.
- Ability to edit profiles.
- User profiles with seamless photo upload capabilities via Cloudinary and intuitive drag-and-drop functionality.
- Presence tracker to check if your friends are online.
- Robust user authentication and authorization using ASP.net Identity and JWT tokens.
- Effortless pagination, filtering, and sorting of data coupled with comprehensive error handling.
- Fully responsive web application with cache for better performance.
- Error interceptors to seamlessly handle WebAPI errors on the client end.
- Toast notifications to keep you updated on important events.
- Tabbed interface for efficient organization and navigation across different sections of the app.
- HTTPS certificate implementation ensures secure communication channels.
- Admin Role Editing and Adding: Admins can edit user roles and add roles for flexible management.
- Moderator and Admin Photo Control: Moderators and Admins can approve or decline user photo uploads



## Screenshots

![image](https://github.com/cyrusnguyen/commu-platform/assets/52537523/b5436c95-4cc9-4438-9268-fd7e57a2cd84)
![image](https://github.com/cyrusnguyen/commu-platform/assets/52537523/65bbbdd3-a60d-4ff2-9943-a9e55feaacfa)
![image](https://github.com/cyrusnguyen/commu-platform/assets/52537523/198d7bf9-66b0-418f-b1de-4eca7e4aad35)




## Tech Stack

**Client:** Angular 16, Typescript, Bootstrap

**Server:** C#, .NET Core 7, Docker, PostgreSQL


## Deployment

To run this project using docker

```bash
  docker build -t commapp .
```
```bash
  docker run -d -p 8080:80 datingapp
```

## Run Locally

Clone the project

```bash
  git clone https://github.com/cyrusnguyen/commu-platform
```

Go to the project directory

```bash
  cd commu-platform
```
Go to server side (cd API) and start the server
```bash
  dotnet run
```

Another terminal to run client side (cd client) and install dependencies:

```bash
  npm install
```

Start the client

```bash
  ng serve
```

