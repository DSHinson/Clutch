grammar Tsql;

// Parser Rules
sql_script: statement+ EOF;

statement
    : select_stmt
    | insert_stmt
    | update_stmt
    | delete_stmt
    ;

select_stmt
    : SELECT select_list FROM table_source (where_clause)? (order_by_clause)?
    ;

insert_stmt
    : INSERT INTO table_name (column_list)? VALUES value_list
    ;

update_stmt
    : UPDATE table_name SET set_clause (where_clause)?
    ;

delete_stmt
    : DELETE FROM table_name (where_clause)?
    ;

select_list
    : '*'
    | column_name (COMMA column_name)*
    ;

table_source
    : table_name (alias=ID)?
    | table_source JOIN table_source ON condition
    ;

where_clause
    : WHERE condition
    ;

order_by_clause
    : ORDER BY order_by_expression (COMMA order_by_expression)*
    ;

order_by_expression
    : column_name (ASC | DESC)?
    ;

set_clause
    : column_name '=' value (COMMA column_name '=' value)*
    ;

value_list
    : '(' value (COMMA value)* ')'
    ;

condition
    : expression
    ;

expression
    : column_name operator value
    | value operator column_name
    | value operator value
    ;

value
    : STRING
    | NUMBER
    | NULL
    | column_name
    ;

operator
    : '=' | '<>' | '<' | '>' | '<=' | '>=' | 'LIKE' | 'IN' | 'BETWEEN'
    ;

column_name
    : ID
    ;

table_name
    : ID
    ;

// Lexer Rules
SELECT  : 'SELECT';
INSERT  : 'INSERT';
INTO    : 'INTO';
UPDATE  : 'UPDATE';
DELETE  : 'DELETE';
FROM    : 'FROM';
WHERE   : 'WHERE';
ORDER   : 'ORDER';
BY      : 'BY';
JOIN    : 'JOIN';
ON      : 'ON';
SET     : 'SET';
VALUES  : 'VALUES';
NULL    : 'NULL';
ASC     : 'ASC';
DESC    : 'DESC';
AND     : 'AND';
OR      : 'OR';
LIKE    : 'LIKE';
IN      : 'IN';
BETWEEN : 'BETWEEN';

COMMA   : ',';
SEMI    : ';';
LPAREN  : '(';
RPAREN  : ')';
EQUAL   : '=';

// Identifiers and Literals
ID      : [a-zA-Z_][a-zA-Z_0-9]*;
STRING  : '\'' ( ~('\'' | '\r' | '\n') | '\'\'' )* '\'';
NUMBER  : [0-9]+ ('.' [0-9]+)?;
WS      : [ \t\r\n]+ -> skip;
