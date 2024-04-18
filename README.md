# Bachelor Thesis

This README provides instructions on how to run the application for the bachelor thesis project. There are two primary methods to run the application: using Docker Compose or running locally without Docker Compose.

## Prerequisites

Before running the application, ensure you have the following installed:

- Node.js and npm
- Docker and Docker Compose (for running with Docker)
- PostgreSQL (for running locally without Docker)
- RabbitMQ (for running locally without Docker)

## Running the Application

### Using Docker Compose

To-Do: Instructions for setting up and running the application using Docker Compose will be added here.

### Running Locally Without Docker Compose

To run the application locally without using Docker Compose, follow these steps for both the frontend and the backend services.

#### Running the Frontend

1. Navigate to the frontend directory:
   ```bash
   cd frontend
   ```
2. Install the necessary dependencies:
   ```bash
   npm install
   ```
3. Start the frontend application:
   ```bash
   npm start
   ```

#### Running the Backend Services

1. Ensure you have a `.env` file in the `API/Configuration` directory for the engine service. This file should contain the following environment variables:

   ```
   DB_HOST=localhost
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
   ```

## Additional Notes

- Make sure the PostgreSQL and RabbitMQ services are running before starting the backend services.
