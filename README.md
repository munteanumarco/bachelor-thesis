# bachelor-thesis

## Running the application

There are two ways to run this application: using Docker Compose or running locally without Docker Compose.

### Using Docker Compose

#TO-DO

### Running locally without Docker Compose

1. For the frontend, navigate to the frontend directory and start the application:

   ```bash
   cd frontend
   npm start
   ```

2. For the backend services ensure you have a .env file in the `API/Configuration` folder (eg. for engine service)
   `   DB_HOST=localhost
DB_PORT=5432
DB_NAME=name
DB_USER=user
DB_PASSWORD=carp04
JWT_ISSUER=issuer
JWT_AUDIENCE=audience
JWT_KEY=key
JWT_ALGORITHM=HS512
RABBITMQ_HOSTNAME=localhost
RABBITMQ_PORT=5672
RABBITMQ_USERNAME=guest
RABBITMQ_PASSWORD=guest
FRONTEND_BASE_URL=http://localhost:4200
  `
