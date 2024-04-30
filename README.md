# AI Based Microservice

This project consists of four main components:

1. **Auth Service**: A .NET-based microservice responsible for user authentication. It uses JWT for authentication and MongoDB as a database. The service secrets are stored in a .env file.

2. **Ollama Bridge**: A Node.js service that uses Express and TypeScript. It appears to be a bridge or middleware service, possibly handling communication between different parts of the application.

3. **Vue UI**: A Vue.js frontend application. It uses Vite for a build tool, and Tailwind CSS for styling. The application also uses Axios for HTTP requests and JWT-decode for handling JWTs.

4. **Ollama**: A tool that helps run large language models locally, making experimentation more accessible. It exposes an endpoint for interacting with a model via its API, serving as a fourth service in this application.

The project uses Docker for containerization, with a `docker-compose.yml` file provided for easy setup. **IMPORTANT NOT FINISHED**