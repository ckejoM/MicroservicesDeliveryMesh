# 🚀 Microservices Delivery Mesh
### Project 7: Distributed Event-Driven Architecture with .NET 9 & Angular

[![.NET 9](https://img.shields.io/badge/.NET-9.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![Angular](https://img.shields.io/badge/Angular-18-DD0031?logo=angular)](https://angular.io/)
[![RabbitMQ](https://img.shields.io/badge/RabbitMQ-Messaging-FF6600?logo=rabbitmq)](https://www.rabbitmq.com/)
[![PostgreSQL](https://img.shields.io/badge/PostgreSQL-Database-4169E1?logo=postgresql)](https://www.postgresql.org/)

A production-grade demonstration of **Asynchronous Messaging**, **API Gateway Routing**, and **Network Isolation**. This project implements a decoupled delivery lifecycle where services communicate solely through a message broker, ensuring high availability and fault tolerance.

---

## 🏗 System Architecture

The mesh utilizes **SSL Termination** at the Gateway, allowing internal services to operate on a high-performance HTTP plane while maintaining a single secure entry point for the client. Internal microservices are isolated and hidden from the public internet, accessible only through the Gateway's routing rules.

### Key Components:
* **ApiGateway (YARP):** The single entry point (Port 5000). Orchestrates all incoming traffic, enforces CORS policies, and abstracts internal service ports (5010, 5020).
* **Order Service:** The producer service. Captures customer intent, persists state to PostgreSQL, and publishes agnostic message records to RabbitMQ.
* **Delivery Service:** The consumer service. An independent worker service that listens for events and processes logistics asynchronously.
* **Infrastructure:** RabbitMQ for the message bus and PostgreSQL for durable state storage, managed via Docker.

---

## 🛠 Tech Stack

| Category | Technology | Purpose |
| :--- | :--- | :--- |
| **Backend** | .NET 9 | High-performance core microservices. |
| **Frontend** | Angular (Standalone) | Modern, component-based user interface. |
| **Gateway** | YARP (Reverse Proxy) | Dynamic routing and SSL termination. |
| **Messaging** | Wolverine | Advanced mediator and messaging patterns. |
| **Persistence**| PostgreSQL | Relational data integrity. |
| **Observability**| Serilog | Structured JSON logging for distributed tracing. |

---

## 🚦 Getting Started

### 1. Prerequisites
* **Docker Desktop** (for infrastructure)
* **.NET 9 SDK**
* **Node.js & Angular CLI**

### 2. Infrastructure Setup
Spin up the backing services using Docker Compose:
```bash
docker-compose up -d postgres rabbitmq
```

### 3. Backend Execution
Open the solution in Visual Studio and set **Multiple Startup Projects** to run concurrently:
* `ApiGateway` (Port 5000)
* `Order.Service` (Port 5010)
* `Delivery.Service` (Port 5020)

### 4. Frontend Launch
```bash
cd src/UI/DeliveryMesh-UI
npm install
ng serve
```

---

## 📁 Directory Structure
```text
DeliveryMesh/
├── src/
│   ├── Services/
│   │   ├── ApiGateway/      # YARP Configuration & CORS Logic
│   │   ├── Order.Service/   # Domain Logic & Event Production
│   │   └── Delivery.Service/# Async Event Consumption
│   ├── Shared/
│   │   └── Contracts/       # Agnostic Message Schemas (Plain Records)
│   └── UI/
│       └── DeliveryMesh-UI/ # Angular Standalone Application
├── docker-compose.yml       # Infrastructure orchestration
└── README.md
```

---

## 📝 Tech Debt & Roadmap
* [ ] **Saga Implementation:** Re-introduce the state machine for complex distributed transactions using Wolverine Sagas.
* [ ] **Full Containerization:** Transition services from local run to full Docker Compose deployment with internal networking.
* [ ] **ELK Stack:** Integrate Seq or the ELK stack to aggregate structured Serilog outputs for cross-service tracing.

---

### **Created by Jovan Madzic | Software Engineer**
[LinkedIn Profile](https://www.linkedin.com/in/jovan-madzic/)
