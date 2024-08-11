# Detailed Specification for Phase 1: Core Database Engine

## Overview
Phase 1 focuses on the foundational components of the relational database system, namely the core database engine. This phase will involve implementing the SQL parsing and execution logic, the B-Tree data structures for primary key management, and the basic mechanisms for binary file storage. The goal of this phase is to establish a functioning database engine capable of handling basic SQL operations on tables stored as binary files.

## Objectives
- Implement a basic SQL parser capable of interpreting simple SQL commands.
- Develop a B-Tree data structure to efficiently manage primary key lookups.
- Create a file storage system that stores table structures and data in binary format.
- Ensure that the core engine can execute basic SQL operations such as `CREATE TABLE`, `INSERT`, `SELECT`, `UPDATE`, and `DELETE`.

## 1. SQL Parsing and Execution

### 1.1 SQL Syntax Support
#### 1.1.1 Supported SQL Commands
- **CREATE TABLE**: Define a new table structure.
- **INSERT INTO**: Insert new records into a table.
- **SELECT**: Retrieve records from a table.
- **UPDATE**: Modify existing records in a table.
- **DELETE**: Remove records from a table.

#### 1.1.2 SQL Parser
- **Implementation**: Develop a simple recursive descent parser or utilize a parser generator (e.g., ANTLR) to interpret SQL commands.
- **Components**:
  - **Lexer**: Tokenizes the SQL input string, identifying keywords, identifiers, literals, and operators.
  - **Parser**: Constructs an abstract syntax tree (AST) from the tokens, representing the structure of the SQL query.
  - **Error Handling**: Implement basic syntax error detection, with clear messages for the user.

#### 1.1.3 SQL Execution Engine
- **Query Execution**: Implement a query execution engine that interprets the AST and performs the corresponding operations on the binary files.
- **Command Handling**:
  - **CREATE TABLE**: Generate a new binary file to store the table’s structure and another for its data.
  - **INSERT INTO**: Parse the input data and insert it into the appropriate binary file, updating the B-Tree for primary key indexing.
  - **SELECT**: Retrieve and display data based on simple WHERE clauses, with support for primary key lookups.
  - **UPDATE**: Modify records in the binary file, ensuring the B-Tree remains consistent.
  - **DELETE**: Remove records and update the binary file, also maintaining the B-Tree structure.

## 2. B-Tree Implementation for Primary Keys

### 2.1 B-Tree Basics
- **Structure**: Implement a B-Tree data structure to manage the primary key index for each table.
  - **Nodes**: Each node will store keys and pointers to child nodes.
  - **Balancing**: Ensure that the tree remains balanced, with all leaf nodes at the same level.
  - **Split Operations**: When a node overflows, split the node and propagate changes upwards.

### 2.2 B-Tree Operations
- **Insert**: Insert a new key into the B-Tree, ensuring the tree remains balanced.
- **Search**: Efficiently search for a key in the B-Tree, used for quick lookups during `SELECT`, `UPDATE`, and `DELETE` operations.
- **Delete**: Remove a key from the B-Tree, adjusting the structure to maintain balance.

### 2.3 Integration with File Storage
- **Persistent Storage**: Store the B-Tree on disk in a binary file associated with the table.
  - **Node Serialization**: Serialize and deserialize nodes to/from the binary file.
  - **Caching**: Implement a caching mechanism to keep frequently accessed nodes in memory, reducing disk I/O.
- **Transaction Safety**: Ensure that B-Tree modifications are atomic and recoverable in case of failure.

## 3. Basic Binary File Storage

### 3.1 File Structure Design
#### 3.1.1 Table Structure File
- **Purpose**: Define the schema of the table, including column names, data types, and constraints (e.g., primary keys).
- **Format**:
  - **Header**: Metadata about the table (e.g., number of columns, primary key information).
  - **Column Definitions**: Detailed information about each column (name, type, size).

#### 3.1.2 Data File
- **Purpose**: Store the actual records of the table in binary format.
- **Format**:
  - **Header**: Metadata such as the number of records and size of each record.
  - **Record Storage**: Each record will be stored in a binary format, with fixed or variable-length fields depending on the column definitions.

#### 3.1.3 B-Tree File
- **Purpose**: Store the B-Tree structure for primary key indexing.
- **Format**:
  - **Header**: Information about the tree (e.g., root node location, node size).
  - **Node Storage**: Serialized nodes stored sequentially or in a structured format.

### 3.2 File Operations
- **File Creation**: Implement methods for creating new binary files for tables and indexes.
- **File Access**: Provide read/write access to the binary files, ensuring efficient data retrieval and modification.
- **File Integrity**: Implement mechanisms to detect and recover from file corruption.

### 3.3 Record Storage and Retrieval
- **Insertion**: Develop a method to insert new records into the data file, updating any associated indexes (e.g., B-Tree).
- **Retrieval**: Efficiently retrieve records based on primary key lookups or other criteria, with support for simple WHERE clauses.
- **Updating Records**: Modify existing records in the data file, ensuring the changes are reflected in the B-Tree.
- **Deleting Records**: Remove records from the data file, ensuring the B-Tree and file structure remain consistent.

## 4. Testing and Validation

### 4.1 Unit Testing
- **SQL Parsing**: Test the SQL parser with a variety of valid and invalid SQL statements.
- **B-Tree Operations**: Test the B-Tree implementation with different scenarios, including edge cases like node splits and merges.
- **File Storage**: Test the binary file operations, ensuring data is correctly stored, retrieved, and modified.

### 4.2 Integration Testing
- **End-to-End Operations**: Test the full cycle of operations from `CREATE TABLE` to `INSERT`, `SELECT`, `UPDATE`, and `DELETE`, ensuring all components work together seamlessly.
- **Concurrency Testing**: Although concurrency is a focus in later phases, begin testing with simple concurrent operations to identify potential issues early.

### 4.3 Performance Testing
- **Benchmarking**: Measure the performance of key operations (e.g., insertions, lookups) with varying amounts of data.
- **Optimization**: Identify bottlenecks and optimize the B-Tree and file access mechanisms for better performance.

### 4.4 Validation
- **Data Integrity**: Ensure that all data is accurately stored and retrieved, with no corruption or loss.
- **Consistency**: Validate that the B-Tree remains consistent after operations like insertion, deletion, and updates.

## 5. Documentation

### 5.1 Code Documentation
- **Inline Comments**: Provide detailed comments within the code to explain complex logic and decisions.
- **API Documentation**: Document the public interfaces and methods, describing their purpose, parameters, and return values.

### 5.2 User Documentation
- **Developer Guide**: Create a guide explaining the architecture, components, and how to extend or modify the core engine.
- **Usage Examples**: Provide examples of how to use the core engine to create tables, insert data, and execute queries.

## 6. Deliverables
- **Core Engine**: A functioning database engine with the ability to parse and execute basic SQL commands, manage primary keys with B-Trees, and store data in binary files.
- **Test Suite**: A comprehensive set of unit and integration tests covering all major components.
- **Documentation**: Detailed documentation covering code, architecture, and usage.