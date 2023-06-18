# Deliveries by Drones

## Table of Contents

- [Introduction](#introduction)
- [User Interfaces](#user-interfaces)
- [Project Goals](#project-goals)
- [Structure](#structure)
- [Delivery Management System Simulator](#delivery-management-system-simulator)
- [Usage](#usage)
- [Configuration](#configuration)
- [Contributing](#contributing)
- [License](#license)

## Introduction

The Deliveries by Drones project focuses on developing an information management system for delivery services carried out by drones. The system facilitates deliveries between various customers, such as deliveries from different stores and businesses to consumers. It includes base stations for drone maintenance and recharging, as well as managing information about customers and packages shipped using the service.

## User Interfaces

The system consists of two interfaces for different users:

- **Operational Interface (Company Management):** This interface allows management and updating of information about base stations for drone battery charging, drones used for deliveries, customers who send and receive packages, and packages being delivered.

- **Customer Interface (Bonus):** This interface provides features such as registration for new customers, profile updates for managing customer details, adding new packages for delivery, and tracking packages sent by and to the customer.

## Project Goals

The main goals of this project include:

- Implementing the principles of the C# programming language.
- Utilizing software design patterns:
  - Architectural patterns (Layered Model, Contract Design).
  - Creational patterns (Singleton, Simple Factory).
  - Behavioral patterns (Observer, Iterator).
- Creating a user interface using the modern WPF framework.
- Fostering teamwork.
- Enhancing problem-solving and self-learning skills.

## Structure

This project follows a three-layered architecture:

1. **Presentation Layer (PL):** This layer is responsible for the user interface and handling user interactions. It includes the operational and customer interfaces described earlier.

2. **Business Logic Layer (BL):** The BL layer contains the core logic and rules of the delivery management system. It implements functionalities such as package prioritization, drone operations, and delivery tracking. The BL layer communicates with the DAL layer for data access and updates.

3. **Data Access Layer (DAL):** The DAL layer handles the retrieval and persistence of data. It interacts with the underlying database or data storage system to perform operations such as reading and writing data related to base stations, drones, customers, and packages. The BL layer communicates with the DAL layer to access and update relevant data.

This layered architecture promotes modularity, scalability, and separation of concerns. Each layer has specific responsibilities, enabling code reusability and maintainability.

## Delivery Management System Simulator

The Delivery Management System Simulator is designed to operate a drone in the BL layer. Its primary function is to autonomously manage drone operations for package delivery until the battery is depleted. After that, the drone returns to the nearest base station for recharging.

### Features

- Automatic selection of the highest priority package for delivery, based on the solution implemented in the BL exercise.
- Package delivery functionality if the battery allows reaching the sender, delivering the package to the destination, and returning to the nearest charging station.
- Support for additional deliveries after completing a package delivery.
- Handling scenarios where no available charging stations are present at the nearest base station.
- Battery status updates at each stage of movement and charging.
- Real-time location updates for the drone, including sender, receiver, and base station locations.

### Usage

1. Start the simulator.
2. The simulator will automatically select the highest priority package for delivery.
3. The drone will execute the delivery, including reaching the sender, delivering the package to the receiver, and returning to the nearest charging station.
4. If there are additional deliveries, the drone will attempt to perform them.
5. Once all deliveries are completed or no further deliveries are possible, the drone will periodically check for the availability of new deliveries while remaining at the charging station.
6. The battery status, movement, and charging information will be displayed in relevant windows.

### Configuration

The simulator provides configuration options for adjusting its behavior. The following settings can be modified:

- **Timer Interval:** The delay interval between each simulator step, measured in milliseconds.
- **Drone Speed:** The speed at which the drone moves, measured in kilometers per second.
- **Battery Charging Rate:** The rate at which the drone's battery charges, represented as a percentage per minute or second.
- **Battery Consumption Rate:** The rate at which the drone consumes battery power during different movement scenarios (no load, light/medium/heavy packages).

### Contributing

Contributions to the Deliveries by Drones project are welcome!

### License



