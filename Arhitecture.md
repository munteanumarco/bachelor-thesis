# Project Arhitecture

Document the arhitecture of the bachelor thesis project.

[TOC]

#Services
Presenting the microservices that make up the application.

##1. UserService

- Responsibilities: Manages user accounts, authentication, and role assignments. Handles account recovery and user profile management.

- Communication: Synchronous calls for user information retrieval and user actions. Publishes events to a message queue when user profiles are created or updated for other services that need to react to these changes.

##2. EmergencyService

- Responsibilities: Manages the lifecycle of emergency events, including reporting, updating, and closing events. Stores details like location, type, and severity.

- Communication: Receives reports of new events and updates to existing ones through synchronous calls. Publishes event-related changes to a message queue for services like the `NotificationService` and the `ResourceService`.

##3. ResourceService

- Responsibilities: Coordinates and allocates resources such as personnel, equipment, and supplies in response to emergency events. Manages requests from users for specific resources.
- Communication: Listens to events from the `EmergencyService` for resource allocation needs. Interacts with the User Service to verify user permissions for resource requests.

##4. CommunicationService

- Responsibilities: Manages real-time chat and communication channels for each ongoing emergency event.

- Communication: Works closely with the `EmergencyService` to create communication channels for new events and uses synchronous calls for delivering messages and managing chat sessions.

##5. NotificationService

- Responsibilities: Sends notifications, alerts, and communications to users based on event activity or system alerts.

- Communication: Subscribes to relevant topics in the message queue, such as new events, updates, or resource requests, to trigger notifications to users.

##6. LandcoverAnalysisService

- Responsibilities:
- Fetch satellite imagery from Google Earth Engine API based on specified parameters such as geographic coordinates and time frame. Process the retrieved images using AI models for semantic segmentation to identify different land cover types (e.g., water, vegetation, urban).

- Return detailed analysis results, including classifications and percentages of land cover types.

- Communication:

- Receives image analysis requests from other services within the system, like `EmergencyService`.

- Sends back analysis results to the requesting service, either directly through API responses or via a message queue for asynchronous processing.

##7. EmergencyUI

- Responsibilities:

- Serves as the user interface for the emergency response application, built with Angular.

- Enables users to interact with various features of the application.

- Integrates with backend services to fetch and display data, submit user requests, and update information in real-time based on user interactions and system updates.

- Communication:

- Communicates with backend microservices via RESTful APIs or WebSockets for real-time features. This includes sending requests to and receiving responses from services like `UserService`, `EmergencyService`, `ResourceService`, and others.

- Subscribes to updates from backend services to reflect changes in the UI dynamically, ensuring that the users have access to the most current information and can interact with the system effectively.

# Message Queues

##1. UserEventsQueue

- Purpose: To handle events related to user account changes, such as creation, updates, or role changes.

- Publishers: `UserService` publishes to this queue when there are significant changes to user accounts.

- Subscribers: Services that need to react to user account changes. For example, `NotificationService` might subscribe to send welcome emails to new users or notifications about account changes.

- Possible events:
  - UserCreated: Published when a new user account is successfully created.
  - UserProfileUpdated: When a user updates their profile information.
  - UserPasswordChanged: Notifies the system of a user password change, distinct from a reset request, indicating an active user action.
  - UserRoleChanged: When a user's role or access level is updated.
  - UserAccountDeactivated: When a user deactivates their account, potentially reversible.
  - UserAccountDeleted: Signifies the permanent deletion of a user account.
  - PasswordResetRequested: When a user requests a password reset link or code.
  - PasswordResetCompleted: Marks the completion of the password reset process.

##2. EmergencyEventsQueue

- Purpose: For broadcasting updates or new emergency events reported by users.
- Publishers: `EmergencyEventService` publishes new or updated emergency events to this queue.
- Subscribers:

  - `NotificationService` can subscribe to alert users about new or updated emergencies.
  - `ResourceService` might also subscribe to manage and allocate resources based on the nature and location of the emergency.
  - `LandcoverAnalysisService` could listen for new events that require fetching and analyzing satellite imagery.

- Possible events:
  - EmergencyReported: A new emergency event is reported by a user.
  - EmergencyUpdated: Updates to an existing emergency event, which could include changes to its severity, location, description, or status.
  - EmergencyClosed: When an emergency event is resolved or closed.
  - ResourceRequestForEmergency: Requests for resources specific to handling an emergency event.
  - EmergencyCommentAdded: For systems that support comments or notes on events, indicates a new comment has been added.

##3. LandcoverAnalysisRequestsQueue

- Purpose: To handle requests for landcover analysis.
- Publishers: `EventService` (or any other service that needs to initiate a landcover analysis).
- Subscribers: `LandcoverAnalysisService` (receives the requests and performs the analysis).

##4. LandcoverAnalysisResultsQueue

- Purpose: To communicate the results of landcover analyses back to other services.
- Publishers: `LandcoverAnalysisService` publishes the outcomes of the image analyses, including classifications and metrics.
- Subscribers: `EventService` could subscribe to incorporate analysis results into emergency event data, enriching the information available for decision-making and response planning.
