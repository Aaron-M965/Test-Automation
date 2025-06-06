# 🧪 Test Automation for BG Treasury System

This repository contains the **UI Test Automation Framework** for the BG Treasury System. It validates mission-critical workflows through automated browser testing using Selenium and NUnit, ensuring quality and reliability across software deployments.

---

## 📌 Project Overview

The BG Treasury System automates the processing, validation, enrichment, and tracking of **SWIFT payments** and financial transactions. Built on a **microservices** architecture with **Kafka topics** and **database-driven orchestration**, it enables seamless and accurate payment processing.

---

## ✅ Features of the Automation Framework

- Selenium WebDriver for UI browser automation
- NUnit as the test framework
- ExtentReports for rich HTML test reports with screenshots
- log4net for logging execution flow and issues
- Configurable environment setup (URLs, Chrome paths, etc.)
- Resilient interaction with flaky elements using safe retry logic
- Screenshot capture for passed and failed tests

---

## 🛠 Tech Stack

| Tool/Library       | Purpose                          |
|--------------------|----------------------------------|
| C# (.NET)          | Programming Language             |
| Selenium WebDriver | UI Automation                    |
| NUnit              | Test Framework                   |
| ExtentReports      | Test Reporting                   |
| log4net            | Logging                          |
| ChromeDriver       | Browser Automation Driver        |

---

## 💼 BG Treasury System

The BG Treasury System automates the processing, validation, enrichment, and tracking of **SWIFT payments and transactions**. It is built using **microservices**, **Kafka topics**, and **SQL-backed orchestration**, providing robust, scalable, and traceable financial transaction processing.

---

## 📊 System Overview

The system ingests **ISO 20022 pacs.008** messages and performs the following high-level tasks:

- 📥 Message ingestion from SWIFT via XML
- 🛡 File detection and screening via Embargo server
- 🔄 Kafka-based event processing pipeline
- 🧠 Microservices for validation, enrichment, and business logic
- 💾 Data persistence in Phoenix SQL DB
- 👤 User interaction and auditing via AppWorks portal

---

## 🧩 Architecture

### 🔑 Key Components

| Component          | Description                                                              |
|--------------------|--------------------------------------------------------------------------|
| Trustlink SWIFT    | Source of incoming `.xml` payment messages                               |
| Embargo Server     | Monitors folders, archives files, triggers Kafka events                  |
| Kafka Topics       | Event-driven transitions between processing stages                       |
| Microservices      | Handle parsing, validation, enrichment, routing                          |
| Kubernetes         | Orchestration of microservices                                           |
| Phoenix DB         | SQL database for persistence and lookups                                 |
| AppWorks           | Portal for user actions, reprocessing, audit trails                      |

---

## 🛰 Kafka Topics Flow

Each Kafka topic represents a **checkpoint** in the end-to-end processing lifecycle:

| Topic Name         | Description                                                              |
|--------------------|--------------------------------------------------------------------------|
| `NewPayment`       | Initial parsed payments from SWIFT files                                 |
| `ExchangeRate`     | Messages enriched with FX rate data from DB or APIs                      |
| `CorrespondentCheck` | Payments passing correspondent bank validation                          |
| `StpCheck`         | Validated against stop-list rules (e.g., sanctions/fraud)                |
| `SpecialRateCheck` | Checked for special/bulk transaction rates                               |
| `DealCheck`        | Ensures FX deals and rules compliance                                    |
| `Validation`       | Final business rule validations                                          |
| `FMC Submission`   | Specialized Deal ID/FMC submission stage                                 |

> ⚠️ Each topic is consumed/produced by dedicated microservices, ensuring asynchronous, decoupled processing.

---

## 🔁 Process Flow

1. **File Pickup**  
   - SWIFT files arrive in monitored folders

2. **Embargo Server**  
   - Archives the file and triggers Kafka processing

3. **Kafka-Driven Microservices**  
   - Parsing → Enrichment → Rule Checks → Routing

4. **Database Operations**  
   - Phoenix DB stores data and supports enrichment via lookups

5. **Downstream Systems**  
   - Data flows to Mainframe, validation layers, reconciliation

6. **User Interaction**  
   - AppWorks provides audit logs, retry features, and manual overrides
