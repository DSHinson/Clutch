# Detailed Specification for Phase 2: Indexes and Relationships

## Overview
Phase 2 builds upon the core database engine by adding support for indexing and managing relationships between tables. Indexes will allow for more efficient query execution on non-primary key columns, while relationships will enable the definition and enforcement of foreign key constraints. This phase also involves the integration of these features with the existing B-Tree and binary file storage mechanisms.

## Objectives
- Implement support for creating and managing indexes on columns other than the primary key.
- Develop mechanisms for defining and enforcing foreign key relationships between tables.
- Extend the B-Tree implementation to manage indexes and relationships.
- Modify the binary file storage system to handle additional files for indexes and relationships.
- Ensure that the database engine can perform efficient lookups and maintain data integrity through foreign key constraints.

## 1. Index Support

### 1.1 Index Creation and Management
#### 1.1.1 Supported Index Types
- **Single-Column Index**: An index on a single column to optimize queries involving that column.
- **Composite Index**: An index on multiple columns to optimize queries involving those columns in combination.

#### 1.1.2 SQL Syntax for Indexes
- **CREATE INDEX**: Define a new index on a table column or a set of columns.
  - Example: `CREATE INDEX idx_name ON table_name(column1, column2);`
- **DROP INDEX**: Remove an existing index.
  - Example: `DROP INDEX idx_name ON table_name;`

### 1.2 B-Tree Implementation for Indexes
- **Structure**: Extend the existing B-Tree implementation to manage indexes. Each index will have its own B-Tree.
  - **Nodes**: Similar to the primary key B-Tree, index nodes will store key values and pointers.
  - **Balancing**: Ensure that the B-Tree remains balanced for optimal performance.
  - **Split Operations**: Handle node splits as needed when inserting into the index B-Tree.

### 1.3 Integration with SQL Execution Engine
- **Query Optimization**: Modify the SQL execution engine to recognize and use indexes for optimizing query execution, especially for `SELECT`, `UPDATE`, and `DELETE` operations.
  - **WHERE Clauses**: Use indexes to quickly locate rows that match the conditions specified in WHERE clauses.
  - **ORDER BY**: Leverage indexes to efficiently sort results when executing queries with `ORDER BY`.

### 1.4 File Storage for Indexes
- **Index Files**: Store each index in its own binary file, similar to the primary key B-Tree file.
  - **File Structure**:
    - **Header**: Metadata about the index, including the columns it covers and the B-Tree root node.
    - **Node Storage**: Serialized B-Tree nodes representing the index structure.
- **File Operations**: Implement file I/O operations for reading and writing index data, ensuring consistency and durability.

### 1.5 Index Maintenance
- **Insertions and Updates**: Automatically update the index B-Trees when new records are inserted or existing records are updated.
- **Deletions**: Ensure that index entries are removed when corresponding records are deleted from the table.
- **Rebuilding Indexes**: Provide a mechanism for rebuilding indexes if they become corrupted or need optimization.

## 2. Foreign Key Relationships

### 2.1 Foreign Key Definition and Enforcement
#### 2.1.1 SQL Syntax for Foreign Keys
- **CREATE TABLE with Foreign Key**: Define foreign key constraints when creating a table.
  - Example:
    ```sql
    CREATE TABLE orders (
        order_id INT PRIMARY KEY,
        customer_id INT,
        FOREIGN KEY (customer_id) REFERENCES customers(customer_id)
    );
    ```
- **ALTER TABLE**: Add or drop foreign key constraints after a table has been created.
  - Example: `ALTER TABLE orders ADD CONSTRAINT fk_customer FOREIGN KEY (customer_id) REFERENCES customers(customer_id);`

### 2.2 B-Tree Implementation for Foreign Keys
- **Separate B-Trees**: Implement separate B-Trees to manage foreign key relationships, allowing efficient lookups and enforcement.
  - **Parent-Child Relationships**: The foreign key B-Tree will map foreign key values to their corresponding primary key values in the referenced table.
  - **Cascade Operations**: Implement cascade updates and deletions if specified in the foreign key constraints.

### 2.3 Enforcing Data Integrity
- **Referential Integrity**: Ensure that foreign key constraints are enforced during `INSERT`, `UPDATE`, and `DELETE` operations.
  - **INSERT**: Verify that a foreign key value exists in the referenced table before inserting a new record.
  - **UPDATE**: Prevent updates that would violate foreign key constraints, or propagate changes if cascading updates are enabled.
  - **DELETE**: Restrict deletions that would leave orphaned records or delete dependent records if cascading deletes are enabled.

### 2.4 File Storage for Relationships
- **Relationship Files**: Store each foreign key relationship in its own binary file.
  - **File Structure**:
    - **Header**: Metadata about the foreign key, including the tables and columns involved.
    - **B-Tree Storage**: Serialized B-Tree nodes representing the foreign key relationships.
- **File Operations**: Implement file I/O operations for reading and writing relationship data, ensuring data consistency and integrity.

### 2.5 Handling Complex Relationships
- **Composite Foreign Keys**: Support foreign keys that reference composite primary keys.
- **Self-Referencing Foreign Keys**: Handle scenarios where a table has a foreign key referencing itself.
- **Multi-Table Joins**: Prepare the system to efficiently execute multi-table joins involving foreign keys, though full JOIN support may be implemented in a later phase.

## 3. Integration with Existing Systems

### 3.1 Modification of Core Engine
- **SQL Parser**: Extend the SQL parser to recognize and correctly parse index and foreign key-related syntax.
- **Execution Engine**: Modify the execution engine to handle operations related to indexes and foreign keys.
- **Error Handling**: Improve error handling to provide meaningful messages related to index and foreign key constraints.

### 3.2 Compatibility with Binary File Storage
- **File Management**: Extend the binary file management system to handle the additional files created for indexes and relationships.
- **Consistency Checks**: Implement consistency checks to ensure that all related files (data, indexes, relationships) are synchronized and accurate.

### 3.3 Performance Considerations
- **Index Usage**: Optimize query execution paths to ensure that indexes are utilized effectively.
- **Relationship Enforcement**: Balance the enforcement of foreign key constraints with the performance impact on data modification operations.

## 4. Testing and Validation

### 4.1 Unit Testing
- **Index Operations**: Test the creation, usage, and deletion of indexes on single and multiple columns.
- **Foreign Key Constraints**: Test the enforcement of foreign key constraints during `INSERT`, `UPDATE`, and `DELETE` operations.

### 4.2 Integration Testing
- **End-to-End Scenarios**: Test scenarios that involve creating tables with indexes and foreign keys, inserting data, and executing queries.
- **Cascading Operations**: Validate the correct behavior of cascading updates and deletes.

### 4.3 Performance Testing
- **Index Performance**: Benchmark the performance improvements achieved by using indexes for query execution.
- **Relationship Integrity**: Measure the impact of foreign key enforcement on the performance of data modification operations.

### 4.4 Validation
- **Data Integrity**: Ensure that data integrity is maintained through the correct enforcement of foreign key constraints.
- **Index Accuracy**: Validate that indexes correctly represent the data and that queries return accurate results.

## 5. Documentation

### 5.1 Code Documentation
- **Inline Comments**: Document the logic and reasoning behind the implementation of indexes and relationships.
- **API Documentation**: Extend the API documentation to include methods and classes related to indexes and foreign keys.

### 5.2 User Documentation
- **Developer Guide**: Update the developer guide with detailed instructions on how to use and extend the indexing and relationship features.
- **Usage Examples**: Provide examples demonstrating the creation and usage of indexes and foreign key constraints.

## 6. Deliverables
- **Indexes and Relationships**: A fully integrated system supporting indexes and foreign key relationships.
- **Test Suite**: Expanded test suite covering all new features related to indexes and relationships.
- **Documentation**: Updated documentation covering the new features and their usage.