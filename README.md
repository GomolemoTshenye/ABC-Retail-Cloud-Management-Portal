# ABC Retail Cloud Management Portal 

## Project Overview
This project extends the ABC Retail web application (a cloud-based retail management system) by implementing a modern, scalable, and cost-efficient architecture using Azure serverless technologies.
Building on the POE(which used Azure Storage services directly from the web app), the POE offloads all major storage operations to four dedicated Azure Functions hosted in a single Azure Function App (ABCRetailFunctions). This decoupling improves performance, enables automatic scaling during peak retail periods (e.g., sales events), reduces costs through pay-per-execution billing, and enhances system resilience with proper error handling and retry logic.

**Student:** Gomolemo Tshenye 

The project extends the **ABC Retail** web application — a cloud-based retail management portal — by integrating **Azure serverless technologies** for improved scalability, cost-efficiency, and robustness.

### Key Objectives
- Offload storage operations from the main web app to dedicated **Azure Functions**.
- Demonstrate decoupling of concerns using serverless architecture.
- Explore advanced Azure services (Event Hubs and Service Bus) to enhance customer experience.
- Provide full evidence through code, screenshots, and architectural diagrams.

## Features Implemented

### Section A: Four Azure Functions
All functions are hosted in a single **Azure Function App** (`ABCRetailFunctions` - Consumption Plan, .NET 8 Isolated runtime).

1. **Table Storage Function**  
   - Persists customer and product data to Azure Table Storage.  
   - HTTP trigger for seamless integration.

2. **Blob Storage Function**  
   - Handles product image uploads to Azure Blob Storage.

3. **Queue Storage Function**  
   - Queues order details for asynchronous processing.

4. **File Storage Function**  
   - Uploads contract documents and invoices to Azure File Shares.

### Section B: Advanced Azure Services
- **Azure Event Hubs**: Real-time event streaming for live inventory updates, personalized recommendations, and fraud detection.
- **Azure Service Bus**: Reliable messaging for order processing, status notifications, and system decoupling with dead-letter queues.

## Technologies Used
- **Framework**: ASP.NET Core MVC (.NET 8)
- **Cloud Services**: 
  - Azure Functions (HTTP triggers)
  - Azure Storage (Table, Blob, Queue, File)
  - Azure Event Hubs
  - Azure Service Bus
- **Tools**: Visual Studio, Azure Functions Core Tools, Azurite (local emulator), Storage Explorer
- **Deployment**: Azure App Service + Function App

## Repository Structure
├── ABC Retail Web App/          # Main ASP.NET Core MVC project
├── Azure Functions/             # Four function projects
├── Documentation/               # POE report (Word/PDF), diagrams
├── Screenshots/                 # Evidence from Azure Portal
└── README.md
text## Live Deployment
- **Web Application**: [https://abccloudretailapp-gehuataqfnh4dyff.southafricanorth-01.azurewebsites.ne/](https://abccloudretailapp-gehuataqfnh4dyff.southafricanorth-01.azurewebsites.ne/)
- **Function App**: ABCRetailFunctions (deployed via Azure Functions Core Tools)

## Setup & Running Locally
1. Clone the repository
2. Install Azure Functions Core Tools
3. Run Azurite for local storage emulation
4. Open solution in Visual Studio
5. Start the web app and functions

## POE Documentation
Full report with:
- Code snippets (with proper attributions)
- Azure Portal screenshots
- Architecture diagrams (draw.io)
- In-depth analysis of Event Hubs & Service Bus
- IIE Harvard referencing

## Lessons Learned
- Benefits of serverless architecture for retail applications
- Importance of async processing and retry policies
- Real-time capabilities using Azure messaging services

---

**This project demonstrates mastery of Azure cloud-native patterns and how they can be applied to build scalable, user-centric e-commerce solutions.**

For any questions, contact: Gomolemo Tshenye 
