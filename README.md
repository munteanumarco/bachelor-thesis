# bachelor-thesis

## Running the application

There are two ways to run this application: using Docker Compose or running locally without Docker Compose.

### Using Docker Compose

1. Ensure that you have a `.env` file at the same level as your `docker-compose.yml` file. This file should contain all the necessary environment variables for the application. Here is a mock `.env` file:

    ```env
    DB_HOST=db
    DB_PORT=5432
    DB_NAME=emergency
    DB_USER=user
    DB_PASSWORD=pass
    ASPNETCORE_ENVIRONMENT=Development
    ```

2. Run the application with Docker Compose:

    ```bash
    docker-compose up
    ```

### Running locally without Docker Compose

1. For the frontend, navigate to the frontend directory and start the application:

    ```bash
    cd frontend
    npm start
    ```

2. For the backend, ensure that you have a `.env.local` file with all the necessary environment variables. Then, source the `setupEnv.sh` script and start the application:

    ```bash
    source setupEnv.sh
    cd backend
    dotnet run --project API/API.csproj
    ```